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
    
    }
    public class MisafirAracCrudBaseViewModel
    {


       
        [Display(Name = "TC Kimlik No")]
        [Required(ErrorMessage = "Lütfen giriniz")]
        public string TC_No { get; set; }
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
    }
    public class MisafirAracAddViewModel : MisafirAracCrudBaseViewModel
    {

    }
    public class MisafirAracEditViewModel : MisafirAracCrudBaseViewModel
    {
        public int Id { get; set; }
        
    }
    public class MisafirAracListViewModel
    {
        public int Id { get; set; }
        public string Plaka { get; set; }

        public string Arac_Marka { get; set; }

        public string Arac_Model { get; set; }

        public string Ad { get; set; }
        



    }
    public class MisafirAracSearchViewModel
    {
        public string Plaka { get; set; }
    }
}
