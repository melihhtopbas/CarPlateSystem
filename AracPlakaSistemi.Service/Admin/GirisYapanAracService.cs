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
    public class GirisYapanAracService
    {
        private readonly AracPlakaSistemiEntities _context;

        public GirisYapanAracService(AracPlakaSistemiEntities context)
        {
            _context = context;

        }
        private IQueryable<GirisYapanAracListViewModel> _getGirisYapanAracListIQueryable(Expression<Func<Data.GirisYapanAraclar, bool>> expr)
        {
           
            return (from b in _context.GirisYapanAraclar.AsExpandable().Where(expr)

                    select new GirisYapanAracListViewModel()
                    {
                        
                        Id = b.id,
                        Plaka = b.plaka,
                        GirisKapiAdresi = b.GirisKapilari.camera_adres,
                        KaraListeAracMi = b.karaListeArac,
                        KayitliAracMi = b.kayitliArac,
                        MisafirAracMi = b.misafirArac,
                        YeniAracMi = b.yeniArac,
                        Tarih = b.tarih
                        




                    });
        }
        public IQueryable<GirisYapanAracListViewModel> GetGirisYapanAracListIQueryable(GirisYapanAracSearchViewModel girisAracSearchViewModel)
        {
            var predicate = PredicateBuilder.New<Data.GirisYapanAraclar>(true);/*AND*/

            if (!string.IsNullOrWhiteSpace(girisAracSearchViewModel.Plaka))
            {
                predicate.And(a => a.plaka.Contains(girisAracSearchViewModel.Plaka));
            }


            return _getGirisYapanAracListIQueryable(predicate);
        }
        public async Task<GirisYapanAracListViewModel> GetGirisYapanAracListViewAsync(long carId)
        {

            var predicate = PredicateBuilder.New<Data.GirisYapanAraclar>(true);/*AND*/
            predicate.And(a => a.id == carId);
            var arac = await _getGirisYapanAracListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return arac;
        }
        public async Task<ServiceCallResult> AddCarAsync(GirisYapanAracAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            var kayitliArac = _context.KayitliAraclar.Any(x => x.plaka == model.Plaka && x.blacklist.Equals(false));
            var karaListeArac = _context.KayitliAraclar.Any(x => x.plaka == model.Plaka && x.blacklist.Equals(true));
            var misafirArac = _context.MisafirAraclar.Any(x=>x.plaka == model.Plaka);

            bool yeniArac = false;
            if (karaListeArac.Equals(false) && kayitliArac.Equals(false) && misafirArac.Equals(false))
            {
                yeniArac = true;
            }

            var arac = new GirisYapanAraclar()
            {
                 plaka = model.Plaka,
                 tarih = DateTime.Now,
                 kapi_id = model.GirisKapisi.KapiId,
                 yeniArac = yeniArac,
                 kayitliArac = kayitliArac,
                 misafirArac = misafirArac,
                 karaListeArac = karaListeArac,
                 
                 






            };






            _context.GirisYapanAraclar.Add(arac);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetGirisYapanAracListViewAsync(arac.id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<ServiceCallResult> DeleteGirisYapanAracAsync(int aracId)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var car = await _context.GirisYapanAraclar.FirstOrDefaultAsync(a => a.id == aracId).ConfigureAwait(false);
            if (car == null)
            {
                callResult.ErrorMessages.Add("Böyle bir araba kaydı bulunamadı.");
                return callResult;
            }

          

            _context.GirisYapanAraclar.Remove(car);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetGirisYapanAracListViewAsync(car.id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }
        }
        public List<GirisKapilariListViewModel> GetKapiList()
        {

            var result = _context.GirisKapilari.Where(x=>x.active.Equals(true)).Select(b => new GirisKapilariListViewModel
            {

                Id = b.Id,
                KapiAdres = b.camera_adres,


            }).ToList();
            return result;
        }
    }
}
