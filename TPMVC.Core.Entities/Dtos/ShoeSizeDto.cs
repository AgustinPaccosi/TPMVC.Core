using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMVC.Core.Entities.Dtos
{
    public class ShoeSizeDto
    {
        public int ShoeId { get; set; }
        public int SizeId { get; set; }
        public decimal SizeNumber { get; set; }
        public int QuantityInStock { get; set; }
    }
}
