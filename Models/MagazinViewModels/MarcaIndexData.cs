using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazinElectronice_Pascu_Ioana.Models.MagazinViewModels
{
    public class MarcaIndexData
    {
        public IEnumerable<Marca> Marci { get; set; }
        public IEnumerable<Device> Devices { get; set; }
        public IEnumerable<Comanda> Comenzi { get; set; }
    }
}
