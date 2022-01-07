using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagazinElectronice_Pascu_Ioana.Models
{
    public class Device
    {
        public int ID { get; set; }
        public string Denumire { get; set; }
        public string Descriere { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Pret { get; set; }

        public ICollection<Comanda> Comenzi { get; set; }

        public ICollection<MarcaDevice> MarcaDevice { get; set; }
    }
}
