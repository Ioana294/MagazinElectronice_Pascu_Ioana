using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazinElectronice_Pascu_Ioana.Models
{
    public class Client
    {
        public int ID { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Adresa { get; set; }
        public decimal Total { get; set; }

        public ICollection<Comanda> Comenzi { get; set; }
    }
}
