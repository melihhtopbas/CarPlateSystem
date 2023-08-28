using AracPlakaSistemi.Data;
using AracPlakaSistemi.ViewModels.Admin;
using AracPlakaSistemi.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AracPlakaSistemi.Service.Admin
{
    public class AracBilgiService
    {
        private readonly AracPlakaSistemiEntities _context;
        public AracBilgiService(AracPlakaSistemiEntities context)
        {
            _context = context;
        }
        public async Task<ServiceCallResult> AracBilgileriSorgula(AracBilgiSorguViewModel model)
        {
            
            var callResult = new ServiceCallResult() { Success = false };

            var misafirArac = await _context.MisafirAraclar.FirstOrDefaultAsync(x => x.plaka.Equals(model.Plaka));
            var kayitliArac = await _context.KayitliAraclar.FirstOrDefaultAsync(x => x.plaka.Equals(model.Plaka));

            if (misafirArac == null && kayitliArac == null)
            {
                callResult.ErrorMessages.Add("Bu plaka sistemde bulunamadı!");
                return callResult;
            }

            else if (misafirArac == null && kayitliArac != null)
            {
                model.Ad_Soyad = kayitliArac.ad_soyad;
                model.Plaka = model.Plaka;
                model.Arac_Marka = kayitliArac.marka;
                model.Arac_Model = kayitliArac.model;
                model.TC_No = kayitliArac.tc_no;
                if (kayitliArac.blacklist.Equals(true))
                {
                    model.AracDurum = "Bu araç sistemimizde kayıtlıdır ancak araç kara listededir.";
                    model.KaraListeMi = true;
                }
                else
                {
                    model.AracDurum = "Bu araç sistemimizde kayıtlıdır";
                    model.KaraListeMi = false;
                }
            }
            else if (misafirArac != null && kayitliArac == null)
            {
                model.Ad_Soyad = misafirArac.Ad + " " + misafirArac.Soyad;
                model.Plaka = model.Plaka;
                model.Arac_Marka = misafirArac.arac_marka;
                model.Arac_Model = misafirArac.arac_model;
                model.TC_No = misafirArac.tc_no;
                model.AracDurum = "Bu araç misafir araçtır";
                model.MisafirAracMi = true;

            }


            









            using (var dbtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    dbtransaction.Commit();


                    callResult.Success = true;
                    callResult.Item = model;


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
