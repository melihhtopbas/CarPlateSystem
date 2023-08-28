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
        private IQueryable<KaraListeAracListViewModel> _getKaraListeAracListIQueryable(Expression<Func<Data.KayitliAraclar, bool>> expr)
        {
            return (from b in _context.KayitliAraclar.AsExpandable().Where(expr)
                    .Where(x=>x.blacklist.Equals(true))
                    
                  
                    select new KaraListeAracListViewModel()
                    {
                        Id = b.Id,
                        AdSoyad = b.ad_soyad,
                        Arac_Marka = b.marka,
                        Arac_Model = b.model,
                        
                        Plaka = b.plaka,
                        tc_no = b.tc_no




                    });
        }
        public IQueryable<KaraListeAracListViewModel> GetKaraListeAracListIQueryable(KaraListeAracSearchViewModel kayitliAracSearchViewModel)
        {
            var predicate = PredicateBuilder.New<Data.KayitliAraclar>(true);/*AND*/

            if (!string.IsNullOrWhiteSpace(kayitliAracSearchViewModel.Plaka))
            {
                predicate.And(a => a.plaka.Contains(kayitliAracSearchViewModel.Plaka));
            }


            return _getKaraListeAracListIQueryable(predicate);
        }
        public async Task<KaraListeAracListViewModel> GetKaraListeAracListViewAsync(long carId)
        {

            var predicate = PredicateBuilder.New<Data.KayitliAraclar>(true);/*AND*/
            predicate.And(a => a.Id == carId);
            var arac = await _getKaraListeAracListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return arac;
        }
        public async Task<ServiceCallResult> AddCarAsync(KaraListeAracAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var car = await _context.KayitliAraclar.FirstOrDefaultAsync(a => a.Id == model.AracListesi.AracId).ConfigureAwait(false);
            car.blacklist = true;


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

        public async Task<ServiceCallResult> DeleteKaraListeAracAsync(int klAracId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var car = await _context.KayitliAraclar.FirstOrDefaultAsync(a => a.Id == klAracId).ConfigureAwait(false);
            if (car == null)
            {
                callResult.ErrorMessages.Add("Böyle bir araba kaydı bulunamadı.");
                return callResult;
            }

            car.blacklist = false;

          
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
        public List<KaraListeAracListViewModel> GetAracList()
        {

            var result = _context.KayitliAraclar.Where(x => x.blacklist.Equals(false)).Select(b => new KaraListeAracListViewModel
            {

                Id = b.Id,
                Plaka = b.plaka


            }).ToList();
            return result;
        }

    }
}