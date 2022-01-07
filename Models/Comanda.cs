using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazinElectronice_Pascu_Ioana.Models
{
    public class Comanda
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public int DeviceID { get; set; }
        public int OfertaID { get; set; }
        public int MembruID { get; set; }
        public DateTime DataComanda { get; set; }

        public Client Client { get; set; }
        public Device Device { get; set; }
        public Oferta Oferta { get; set; }
        public Membru Membru { get; set; }
    }
}
