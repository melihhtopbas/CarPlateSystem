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
    public class KayitliAracService
    {
        private readonly AracPlakaSistemiEntities _context;
        public KayitliAracService(AracPlakaSistemiEntities context)
        {
            _context = context;
        }
        private IQueryable<KayitliAracListViewModel> _getKayitliAracListIQueryable(Expression<Func<Data.KayitliAraclar, bool>> expr)
        {
            return (from b in _context.KayitliAraclar.AsExpandable().Where(expr)
                    
                    select new KayitliAracListViewModel()
                    {
                        Id = b.Id,
                        Ad = b.ad_soyad,
                        Arac_Marka = b.marka,
                        Arac_Model = b.model,
                        Plaka = b.plaka,
                        blacklist = b.blacklist,
                         
                        



                    });
        }
        public IQueryable<KayitliAracListViewModel> GetKayitliAracListIQueryable(KayitliAracSearchViewModel kayitliAracSearchViewModel)
        {
            var predicate = PredicateBuilder.New<Data.KayitliAraclar>(true);/*AND*/

            if (!string.IsNullOrWhiteSpace(kayitliAracSearchViewModel.Plaka))
            {
                predicate.And(a => a.plaka.Contains(kayitliAracSearchViewModel.Plaka));
            }


            return _getKayitliAracListIQueryable(predicate);
        }
        public async Task<KayitliAracListViewModel> GetKayitliAracListViewAsync(long carId)
        {

            var predicate = PredicateBuilder.New<Data.KayitliAraclar>(true);/*AND*/
            predicate.And(a => a.Id == carId);
            var arac = await _getKayitliAracListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return arac;
        }
        public async Task<ServiceCallResult> AddCarAsync(KayitliAracAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.KayitliAraclar.AnyAsync(a => a.plaka == model.Plaka).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu plaka zaten sistemde bulunmaktadır.");
                return callResult;
            }
            bool plakaExist = await _context.MisafirAraclar.AnyAsync(x => x.plaka == model.Plaka).ConfigureAwait(false);
            if (plakaExist)
            {
                callResult.WarningMessages.Add("Bu plaka misafir bir araca aittir. Lütfen Misafir Araçlar sekmesinden aracı kayıt ettiriniz.");
                return callResult;
            }

            var arac = new KayitliAraclar()
            {

                ad_soyad = model.Ad,
                model = model.Arac_Model,
                marka = model.Arac_Marka,
                plaka = model.Plaka,
                tc_no = model.Tc_No,
               
                datetime = DateTime.Now,
                




            };
            
            arac.PlakaGorsel.Add(new PlakaGorsel
            {
                PathName =  model.FileName,
            });
           
            



            _context.KayitliAraclar.Add(arac);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetKayitliAracListViewAsync(arac.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<KayitliAracAEditViewModel> GetAracEditViewModelAsync(int aracId)
        {
            var car = await (from p in _context.KayitliAraclar
                             
                                
                                 where p.Id == aracId
                                 select new KayitliAracAEditViewModel()
                                 {
                                     Ad = p.ad_soyad,
                                     Arac_Marka = p.marka,
                                     Arac_Model = p.model,
                                     Plaka = p.plaka,
                                     Id = p.Id,
                                     BlackList = p.blacklist,
                                     Tc_No = p.tc_no,
                                     Date = p.datetime,


                                 }).FirstOrDefaultAsync();
            return car;
        }
        public async Task<ServiceCallResult> EditAracAsync(KayitliAracAEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.KayitliAraclar.AnyAsync(a => a.Id != model.Id && a.plaka == model.Plaka).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu plaka bulunmaktadır.");
                return callResult;
            }

            var arac = await _context.KayitliAraclar.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (arac == null)
            {
                callResult.ErrorMessages.Add("Böyle bir araç bulunamadı.");
                return callResult;
            }


            arac.tc_no = model.Tc_No;
            arac.plaka = model.Plaka;
            arac.ad_soyad = model.Ad;
            arac.model = model.Arac_Model;
            arac.marka = model.Arac_Marka;
            arac.blacklist = model.BlackList;
            
             
            






            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetKayitliAracListViewAsync(arac.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeleteKayitliAracAsync(int kayitliAracId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var car = await _context.KayitliAraclar.FirstOrDefaultAsync(a => a.Id == kayitliAracId).ConfigureAwait(false);
            if (car == null)
            {
                callResult.ErrorMessages.Add("Böyle bir araba kaydı bulunamadı.");
                return callResult;
            }

            var plakaGorsel = _context.PlakaGorsel.Where(x => x.PlakaId == kayitliAracId).ToList();
            foreach (var item in plakaGorsel)
            {
                _context.PlakaGorsel.Remove(item);
            }
            

            _context.KayitliAraclar.Remove(car);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetKayitliAracListViewAsync(car.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public async Task<PlakaGorselViewModel> GetKayitliAracPlaka(int carId)
        {

            var car = await (from p in _context.PlakaGorsel
                                 where p.PlakaId == carId
                                 select new PlakaGorselViewModel()
                                 {
                                      PathName = p.PathName,
                                      PlakaId = carId,
                                      Id = p.Id


                                 }).FirstOrDefaultAsync();
            return car;

        }
    }
}
