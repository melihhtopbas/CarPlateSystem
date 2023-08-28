using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AracPlakaSistemi.ViewModels.Admin
{
    public class GirisKapiViewModel
    {
        public GirisKapiViewModel()
        {
            this.GirisYapanAraclar = new HashSet<GirisYapanAracViewModel>();
        }

        public int Id { get; set; }
        public string camera_adres { get; set; }
        public string camera_ip { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GirisYapanAracViewModel> GirisYapanAraclar { get; set; }
    }
    public class GirisKapiIdSelectViewModel
    {
        [Display(Name = "Giriş Kapısı")]
        [Required(ErrorMessage = "Lütfen seçiniz")]
        public int KapiId { get; set; }
    }
    public class GirisKapilariListViewModel
    {
        public int Id { get; set; }
        public string KapiAdres { get; set; }
        public bool Active { get; set; }

    }
    public class GirisKapilariAddViewModel
    {
        public string camera_adres { get; set; }
        public string camera_ip { get; set; }
        public bool Active { get; set; }
    }
    public class GirisKapilariEditViewModel
    {
        public int Id { get; set; }
        public string camera_adres { get; set; }
        public bool Active { get; set; }

    }
    public class GirisKapilariSearchViewModel
    {
        public string KapiAdi { get; set; }
    }
}
