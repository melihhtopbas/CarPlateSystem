using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
