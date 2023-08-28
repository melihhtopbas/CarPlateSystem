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
    public class GirisKapisiService
    {
        private readonly AracPlakaSistemiEntities _context;
        public GirisKapisiService(AracPlakaSistemiEntities context)
        {
            _context = context;
        }
        private IQueryable<GirisKapilariListViewModel> _getGirisKapisiListIQueryable(Expression<Func<Data.GirisKapilari, bool>> expr)
        {
            return (from b in _context.GirisKapilari.AsExpandable().Where(expr)

                    select new GirisKapilariListViewModel()
                    {
                        Id = b.Id,
                        Active = b.active,
                        KapiAdres = b.camera_adres,





                    });
        }
        public IQueryable<GirisKapilariListViewModel> GetGirisKapilariListIQueryable(GirisKapilariSearchViewModel misafirAracSearchViewModel)
        {
            var predicate = PredicateBuilder.New<Data.GirisKapilari>(true);/*AND*/

            if (!string.IsNullOrWhiteSpace(misafirAracSearchViewModel.KapiAdi))
            {
                predicate.And(a => a.camera_adres.Contains(misafirAracSearchViewModel.KapiAdi));
            }


            return _getGirisKapisiListIQueryable(predicate);
        }
        public async Task<GirisKapilariListViewModel> GetGirisKapilariListViewAsync(long carId)
        {

            var predicate = PredicateBuilder.New<Data.GirisKapilari>(true);/*AND*/
            predicate.And(a => a.Id == carId);
            var kapi = await _getGirisKapisiListIQueryable(predicate).SingleOrDefaultAsync().ConfigureAwait(false);
            return kapi;
        }
        public async Task<ServiceCallResult> AddKapiAsync(GirisKapilariAddViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };

            bool nameExist = await _context.GirisKapilari.AnyAsync(a => a.camera_adres == model.camera_adres).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu kapı adresi bulunmaktadır.");
                return callResult;
            }


            var kapi = new GirisKapilari()
            {
                camera_adres = model.camera_adres,
                active = model.Active,





            };






            _context.GirisKapilari.Add(kapi);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = await GetGirisKapilariListViewAsync(kapi.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }



        }
        public async Task<GirisKapilariEditViewModel> GetGirisKapilariEditViewModelAsync(int kapiId)
        {
            var kapi = await (from p in _context.GirisKapilari


                              where p.Id == kapiId
                              select new GirisKapilariEditViewModel()
                              {
                                  Id = p.Id,
                                  camera_adres = p.camera_adres,
                                  Active = p.active,



                              }).FirstOrDefaultAsync();
            return kapi;
        }
        public async Task<ServiceCallResult> GirisKapilariEditAracAsync(GirisKapilariEditViewModel model)
        {
            var callResult = new ServiceCallResult() { Success = false };
            bool nameExist = await _context.GirisKapilari.AnyAsync(a => a.camera_adres == model.camera_adres && a.Id != model.Id).ConfigureAwait(false);
            if (nameExist)
            {
                callResult.ErrorMessages.Add("Bu kapı adresi bulunmaktadır.");
                return callResult;
            }


            var kapi = await _context.GirisKapilari.FirstOrDefaultAsync(a => a.Id == model.Id).ConfigureAwait(false);
            if (kapi == null)
            {
                callResult.ErrorMessages.Add("Böyle kapı adresi bulunamadı.");
                return callResult;
            }


            kapi.camera_adres = model.camera_adres;
            kapi.active = model.Active;











            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetGirisKapilariListViewAsync(kapi.Id).ConfigureAwait(false);
                    return callResult;
                }
                catch (Exception exc)
                {
                    callResult.ErrorMessages.Add(exc.GetBaseException().Message);
                    return callResult;
                }
            }

        }
        public async Task<ServiceCallResult> DeleteGirisKapilariAsync(int id)
        {
            var callResult = new ServiceCallResult() { Success = false };


            var kapi = await _context.GirisKapilari.FirstOrDefaultAsync(a => a.Id == id).ConfigureAwait(false);
            if (kapi== null)
            {
                callResult.ErrorMessages.Add("Böyle bir kapı adresi bulunamadı.");
                return callResult;
            }




            _context.GirisKapilari.Remove(kapi);
            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();



                    callResult.Success = true;
                    callResult.Item = await GetGirisKapilariListViewAsync(kapi.Id).ConfigureAwait(false);
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
