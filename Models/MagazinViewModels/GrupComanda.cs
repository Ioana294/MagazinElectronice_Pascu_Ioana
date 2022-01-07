using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MagazinElectronice_Pascu_Ioana.Models.MagazinViewModels
{
    public class GrupComanda
    {
        [DataType(DataType.Date)]
        public DateTime? DataComanda { get; set; }
        public int DeviceCount { get; set; }
    }
}
