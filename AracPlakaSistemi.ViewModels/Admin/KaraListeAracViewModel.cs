using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AracPlakaSistemi.ViewModels.Admin
{
    public class KaraListeAracListCrudBaseViewModel
    {
        
        [Display(Name = "Araç Plakası")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Plaka { get; set; }
        [Display(Name = "Araç Markası")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Arac_Marka { get; set; }
        
        [Display(Name = "Araç Sahibi Adı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Ad { get; set; }
        [Display(Name = "Araç Sahibi Soyadı")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string Soyad { get; set; }
        [Display(Name = "Araç Sahibi TC No")]
        [Required(ErrorMessage = "Lütfen giriniz")]

        public string tc_no { get; set; }
    }
    public class KaraListeAracAddViewModel : KaraListeAracListCrudBaseViewModel
    {

    }
    public class KaraListeAracEditViewModel : KaraListeAracListCrudBaseViewModel
    {
        public int Id { get; set; }
    }
    public class KaraListeAracListViewModel
    {
        public int Id { get; set; }
       
       
        public string Plaka { get; set; }
    
        public string Arac_Marka { get; set; }


        public DateTime Tarih { get; set; }


        public string AdSoyad { get; set; }
       
      
       
      

        public string tc_no { get; set; }



    }
    public class KaraListeAracSearchViewModel
    {
        public string Plaka { get; set; }
    }
}
