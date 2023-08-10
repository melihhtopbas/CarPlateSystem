using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AracPlakaSistemi.ViewModels.Admin
{
    public class PlakaGorselViewModel
    {
        public int Id { get; set; }
        public Nullable<int> PlakaId { get; set; }
        public string PathName { get; set; }

        public virtual KayitliAracViewModel KayitliAraclar { get; set; }
    }
}
