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
    public class KaraListeAracService
    {
        private readonly AracPlakaSistemiEntities _context;
        public KaraListeAracService(AracPlakaSistemiEntities context)
        {
            _context = context;
        }
        private IQueryable<KaraListeAracListViewModel> _getKaraListeAracListIQueryable(Expression<Func<Data.KaraListeAraclar, bool>> expr)
        {
            return (from b in _context.KaraListeAraclar.AsExpandable().Where(expr)
                    select new KaraListeAracListViewModel()
                    {
                        Id = b.Id,
                         
                        Plaka = b.plaka,
                        AdSoyad = b.Ad + " " + b.Soyad,
                        Arac_Marka = b.arac_marka,
                        Tarih = b.date,
                        tc_no = b.tc_no,




                    });
        }
        public IQueryable<KaraListeAracListViewModel> GetKaraListeAracListIQueryable(KaraListeAracSearchViewModel karaListeAracSearchViewModel)
        {
            var predicate = PredicateBuilder.New<Data.KaraListeAraclar>(true);/*AND*/

            if (!string.IsNullOrWhiteSpace(karaListeAracSearchViewModel.Plaka))
            {
                predicate.And(a => a.plaka.Contains(karaListeAracSearchViewModel.Plaka));
            }


            return _getKaraListeAracListIQueryable(predicate);
        }
        public async Task<KaraListeAracListViewModel> GetKaraListeAracListViewAsync(long carId)
        {

            var predicate = PredicateBuilder.New<Data.KaraListeAraclar>(true);/*AND*/
            predicate.And(a => a.Id == carId);
            var arac = await _getKaraListeAracListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return arac;
        }
        public async Task<ServiceCallResult> AddCarAsync(KaraListeAracAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.KaraListeAraclar.AnyAsync(a => a.plaka == model.Plaka).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu plaka zaten sistemde bulunmaktadır.");
                return callResult;
            }

            var arac = new KaraListeAraclar()
            {

                Ad = model.Ad,
                Soyad = model.Soyad,
                arac_marka = model.Arac_Marka,
                tc_no = model.tc_no,
                plaka = model.Plaka,
                date = DateTime.Now




            };
          




            _context.KaraListeAraclar.Add(arac);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetKaraListeAracListViewAsync(arac.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<KaraListeAracEditViewModel> GetAracEditViewModelAsync(int aracId)
        {
            var car = await (from p in _context.KaraListeAraclar
                             where p.Id == aracId
                             select new KaraListeAracEditViewModel()
                             {
                                 Arac_Marka = p.arac_marka,
                                 Ad = p.Ad,
                                 Soyad = p.Soyad,
                                 Plaka = p.plaka,
                                 Id = p.Id,
                                 tc_no = p.tc_no,



                             }).FirstOrDefaultAsync();
            return car;
        }
        public async Task<ServiceCallResult> EditAracAsync(KaraListeAracEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.KaraListeAraclar.AnyAsync(a => a.Id != model.Id && a.plaka == model.Plaka).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu plaka bulunmaktadır.");
                return callResult;
            }

            var arac = await _context.KaraListeAraclar.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (arac == null)
            {
                callResult.ErrorMessages.Add("Böyle bir araç bulunamadı.");
                return callResult;
            }


            arac.tc_no = model.tc_no;
            arac.plaka = model.Plaka;
            arac.Ad= model.Ad;
            arac.arac_marka = model.Arac_Marka;
            arac.Soyad = model.Soyad;
           
                







            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetKaraListeAracListViewAsync(arac.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeleteKaraListeAracAsync(int klAracId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var car = await _context.KaraListeAraclar.FirstOrDefaultAsync(a => a.Id == klAracId).ConfigureAwait(false);
            if (car == null)
            {
                callResult.ErrorMessages.Add("Böyle bir araba kaydı bulunamadı.");
                return callResult;
            }

           
            _context.KaraListeAraclar.Remove(car);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetKaraListeAracListViewAsync(car.Id).ConfigureAwait(false);
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
