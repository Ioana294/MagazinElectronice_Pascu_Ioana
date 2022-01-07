using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazinElectronice_Pascu_Ioana.Models
{
    public class MarcaDevice
    {
        public int MarcaID { get; set; }
        public int DeviceID { get; set; }
        public Marca Marca {get; set;}
        public Device Device {get; set;}

    }
}
