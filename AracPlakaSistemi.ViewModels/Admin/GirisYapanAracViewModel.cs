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

        [Display(Name = "Araç Sahibi Adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Ad { get; set; }
        [Display(Name = "Araç Sahibi TC No")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string TC_NO { get; set; }
        public Nullable<int> Kapi_Id { get; set; }
        public string Plaka { get; set; }
       
        public int id { get; set; }

        public virtual GirisKapiViewModel GirisKapilari { get; set; }
    }
}
