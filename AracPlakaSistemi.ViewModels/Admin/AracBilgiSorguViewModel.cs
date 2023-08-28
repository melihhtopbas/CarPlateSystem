using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AracPlakaSistemi.ViewModels.Admin
{
    public class AracBilgiSorguViewModel
    {
        [Display(Name = "Araç Plakası")]
        [Required(ErrorMessage = "Lütfen giriniz")]
      
        public string Plaka { get; set; }
        public string Ad_Soyad { get; set; }
        public string Arac_Marka { get; set; }
        public string Arac_Model { get; set; }
        public string TC_No { get; set; }
        public bool KaraListeMi { get; set; }
        public bool MisafirAracMi { get; set; }

        public string AracDurum { get; set; }
    }
    public class AracBilgiListViewModel
    {
        public int Id { get; set; }
      
    }
}
