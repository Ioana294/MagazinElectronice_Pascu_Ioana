using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazinElectronice_Pascu_Ioana.Models
{
    public class Oferta
    {
        public int ID { get; set; }
        public string Denumire { get; set; }
        public decimal PretVechi { get; set; }
        public decimal PretRedus { get; set; }
        public DateTime Valabilitate { get; set; }

        public ICollection<Comanda> Comenzi { get; set; }
    }
}
