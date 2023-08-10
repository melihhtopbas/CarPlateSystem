using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AracPlakaSistemi.ViewModels.Admin
{
    public class KayitliAracViewModel
    {
       
    }
    public class KayitliAracCrudBaseViewModel
    {
        public KayitliAracCrudBaseViewModel()
        {
            this.PlakaGorsel = new HashSet<PlakaGorselViewModel>();
        }

       
        [Display(Name = "Araç Plakası")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Plaka { get; set; }
        [Display(Name = "Araç Markası")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Arac_Marka { get; set; }
        [Display(Name = "Araç Modeli")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Arac_Model { get; set; }
        [Display(Name = "Araç Sahibi Adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Ad { get; set; }
        [Display(Name = "Araç Sahibi Soyadı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Soyad { get; set; }
        [Display(Name = "Araç Sahibi TC No")]
        [Required(ErrorMessage = "Lütfen giriniz")]

        public string Tc_No { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlakaGorselViewModel> PlakaGorsel { get; set; }
    }
    public class KayitliAracAddViewModel : KayitliAracCrudBaseViewModel
    {

    }
    public class KayitliAracAEditViewModel : KayitliAracCrudBaseViewModel
    {
        public int Id { get; set; }
    }
    public class KayitliAracListViewModel
    {
        public int Id { get; set; }
        public string Plaka { get; set; }
        
        public string Arac_Marka { get; set; }
      
        public string Arac_Model { get; set; }
       
        public string Ad { get; set; }
      
        public string Soyad { get; set; }
        
    }
    public class KayitliAracSearchViewModel
    {
        public string Plaka { get; set; }
    }

}
