using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMVC.Core.Entities.Dtos
{
    public class ShoeDto
    {
        public int ShoeId { get; set; }
        public string Brand { get; set; }
        public string Sport { get; set; }
        public string Genre { get; set; }
        public string Colour { get; set; }
        public string Model { get; set; } = null!;  
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}
