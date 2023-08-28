using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AracPlakaSistemi.ViewModels.Admin
{
    public class GirisYapanAracViewModel
    {

      
    }
    public class GirisYapanAracCrudBaseViewModel
    {
       
        public Nullable<int> Kapi_Id { get; set; }
        [Required(ErrorMessage ="Plakayı giriniz")]
        public string Plaka { get; set; }

        public int Id { get; set; }

        public virtual GirisKapiViewModel GirisKapilari { get; set; }
        public GirisKapiIdSelectViewModel GirisKapisi { get; set; }
    }
    public class GirisYapanAracAddViewModel : GirisYapanAracCrudBaseViewModel
    {


    }
    public class GirisYapanAracListViewModel
    {
        public string Plaka { get; set; }

        public int Id { get; set; }
        public string GirisKapiAdresi{ get; set; }
        public bool MisafirAracMi { get; set; }
        public bool KayitliAracMi { get; set; }
        public bool KaraListeAracMi { get; set; }
        public bool YeniAracMi { get; set; }
        public DateTime Tarih { get; set; }

    }
    public class GirisYapanAracSearchViewModel
    {
        public string Plaka { get; set; }
    }
    public class GirisYapanAracGrafik
    {
        public string car { get; set; }
       

        public int count { get; set; }
    
       
    }

}
