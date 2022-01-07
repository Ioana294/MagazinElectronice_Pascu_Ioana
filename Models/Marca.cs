using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MagazinElectronice_Pascu_Ioana.Models
{
    public class Marca
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Denumire marca")]
        [StringLength(50)]
        public string DenumireMarca { get; set; }

        [StringLength(70)]
        public string Fondator { get; set; }

        public ICollection<MarcaDevice> MarcaDevices { get; set; }
    }
}
