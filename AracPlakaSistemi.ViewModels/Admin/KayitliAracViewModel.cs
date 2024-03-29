﻿using System;
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
            this.PlakaGorselViewModels = new HashSet<PlakaGorselViewModel>();
        }

        public string FileName { get; set; }
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
        
        [Display(Name = "Araç Sahibi TC No")]
        [Required(ErrorMessage = "Lütfen giriniz")]

        public string Tc_No { get; set; }
        public DateTime Date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public  IEnumerable<PlakaGorselViewModel> PlakaGorselViewModels { get; set; }
    }
    public class KayitliAracAddViewModel : KayitliAracCrudBaseViewModel
    {

    }
    public class KayitliAracAEditViewModel : KayitliAracCrudBaseViewModel
    {
        public int Id { get; set; }
        public bool BlackList { get; set; }
    }
    public class KayitliAracListViewModel
    {
        public int Id { get; set; }
        public string Plaka { get; set; }
        
        public string Arac_Marka { get; set; }
      
        public string Arac_Model { get; set; }
       
        public string Ad { get; set; }
        public bool blacklist { get; set; }



    }
    public class KayitliAracSearchViewModel
    {
        public string Plaka { get; set; }
    }

}
