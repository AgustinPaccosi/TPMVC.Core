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
        public Brand Brand { get; set; } = null!;
        public Sport Sport { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
        public Colour Colour { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; } 
        public int Stock {  get; set; }

    }
}
