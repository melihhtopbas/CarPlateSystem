//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AracPlakaSistemi.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class KayitliAraclar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KayitliAraclar()
        {
            this.PlakaGorsel = new HashSet<PlakaGorsel>();
        }
    
        public int Id { get; set; }
        public string plaka { get; set; }
        public string marka { get; set; }
        public string model { get; set; }
        public string ad_soyad { get; set; }
        public string tc_no { get; set; }
        public System.DateTime datetime { get; set; }
        public bool blacklist { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlakaGorsel> PlakaGorsel { get; set; }
    }
}
