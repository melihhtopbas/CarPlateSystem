using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AracPlakaSistemi.ViewModels.Admin
{
    public class KullaniciViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string KullaniciAd { get; set; }
        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Sifre { get; set; }
     
    }
}
