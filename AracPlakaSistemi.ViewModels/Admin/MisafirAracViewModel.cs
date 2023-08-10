using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AracPlakaSistemi.ViewModels.Admin
{
    public class MisafirAracViewModel
    {
        public int Id { get; set; }
        [Display(Name = "TC Kimlik No")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string TC_No { get; set; }
        [Display(Name = "Araç Plakası")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Plaka { get; set; }
        [Display(Name = "Araç Markası")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Arac_Marka{ get; set; }
        [Display(Name = "Araç Sahibi Adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Ad { get; set; }
        [Display(Name = "Araç Sahibi Soyadı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Soyad { get; set; }
    }
}
