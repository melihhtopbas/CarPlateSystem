using AracPlakaSistemi.Data;
using AracPlakaSistemi.ViewModels.Admin;
using AracPlakaSistemi.ViewModels.Common;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AracPlakaSistemi.Service.Admin
{
    public class MisafirAracService
    {
        private readonly AracPlakaSistemiEntities _context;
        public MisafirAracService(AracPlakaSistemiEntities context)
        {
            _context = context;
        }
        private IQueryable<MisafirAracListViewModel> _getMisafirAracListIQueryable(Expression<Func<Data.MisafirAraclar, bool>> expr)
        {
            return (from b in _context.MisafirAraclar.AsExpandable().Where(expr)
                    
                    select new MisafirAracListViewModel()
                    {
                        Id = b.Id,
                        Ad = b.Ad + " " + b.Soyad,
                        Arac_Marka = b.arac_marka,
                        Arac_Model = b.arac_model,
                        Plaka = b.plaka,
                         
                        



                    });
        }
        public IQueryable<MisafirAracListViewModel> GetMisafirAracListIQueryable(MisafirAracSearchViewModel misafirAracSearchViewModel)
        {
            var predicate = PredicateBuilder.New<Data.MisafirAraclar>(true);/*AND*/

            if (!string.IsNullOrWhiteSpace(misafirAracSearchViewModel.Plaka))
            {
                predicate.And(a => a.plaka.Contains(misafirAracSearchViewModel.Plaka));
            }


            return _getMisafirAracListIQueryable(predicate);
        }
        public async Task<MisafirAracListViewModel> GetMisafirAracListViewAsync(long carId)
        {

            var predicate = PredicateBuilder.New<Data.MisafirAraclar>(true);/*AND*/
            predicate.And(a => a.Id == carId);
            var arac = await _getMisafirAracListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return arac;
        }
        public async Task<ServiceCallResult> AddCarAsync(MisafirAracAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.MisafirAraclar.AnyAsync(a => a.plaka == model.Plaka).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu plakada Misafir Araç bulunmaktadır.");
                return callResult;
            }
            bool plakaExist = await _context.KayitliAraclar.AnyAsync(a => a.plaka == model.Plaka).ConfigureAwait(false);
            if (plakaExist)
            {
                callResult.ErrorMessages.Add("Bu araç plakası sistemde zaten kayıtlı olarak mevcuttur. Misafir araç olarak eklenemez!");
                return callResult;
            }

            var arac = new MisafirAraclar()
            {
                Ad = model.Ad,
                Soyad = model.Soyad,
                plaka = model.Plaka,
                arac_marka = model.Arac_Marka,
                arac_model = model.Arac_Model,
                tc_no = model.TC_No
                




            };
            
        
            



            _context.MisafirAraclar.Add(arac);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetMisafirAracListViewAsync(arac.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<MisafirAracEditViewModel> GetMisafirAracEditViewModelAsync(int aracId)
        {
            var car = await (from p in _context.MisafirAraclar
                             
                                
                                 where p.Id == aracId
                                 select new MisafirAracEditViewModel()
                                 {
                                     Ad = p.Ad,
                                     Soyad = p.Soyad,
                                     TC_No = p.tc_no,
                                     Arac_Marka = p.arac_marka,
                                     Arac_Model = p.arac_model,
                                     Id = p.Id,
                                     Plaka = p.plaka


                                 }).FirstOrDefaultAsync();
            return car;
        }
        public async Task<ServiceCallResult> MisafirAracEditAracAsync(MisafirAracEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.MisafirAraclar.AnyAsync(a => a.Id != model.Id && a.plaka == model.Plaka).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu plakada Misafir Araç bulunmaktadır.");
                return callResult;
            }
            bool plakaExist = await _context.KayitliAraclar.AnyAsync(a => a.plaka == model.Plaka).ConfigureAwait(false);
            if (plakaExist)
            {
                callResult.ErrorMessages.Add("Bu araç plakası sistemde zaten kayıtlı olarak mevcuttur. Misafir araç olarak eklenemez!");
                return callResult;
            }

            var arac = await _context.MisafirAraclar.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (arac == null)
            {
                callResult.ErrorMessages.Add("Böyle bir araç bulunamadı.");
                return callResult;
            }


            arac.Ad = model.Ad;
            arac.Soyad =  model.Soyad;
            arac.plaka = model.Plaka;
            arac.arac_marka = model.Arac_Marka;
            arac.arac_model = model.Arac_Model;
            arac.tc_no = model.TC_No;
           
            
             
            






            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetMisafirAracListViewAsync(arac.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeleteMisafirAracAsync(int id)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var car = await _context.MisafirAraclar.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
            if (car == null)
            {
                callResult.ErrorMessages.Add("Böyle bir araba kaydı bulunamadı.");
                return callResult;
            }

            
            

            _context.MisafirAraclar.Remove(car);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetMisafirAracListViewAsync(car.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }

        public async Task<ServiceCallResult> AracKaydıYapAsync(int id)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var car = await _context.MisafirAraclar.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
            if (car == null)
            {
                callResult.ErrorMessages.Add("Böyle bir araba kaydı bulunamadı.");
                return callResult;
            }

            var kayitliArac = new KayitliAraclar
            {
                ad_soyad = car.Ad + " " + car.Soyad,
                blacklist = false,
                datetime = DateTime.Now,
                marka = car.arac_marka,
                model = car.arac_model,
                tc_no = car.tc_no,
                plaka = car.plaka,
            };
            kayitliArac.PlakaGorsel.Add(new PlakaGorsel());

            _context.KayitliAraclar.Add(kayitliArac);
            _context.MisafirAraclar.Remove(car);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetMisafirAracListViewAsync(car.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
    }
}
