using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMVC.Core.Entities
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public int ShoeSizeId { get; set; }
        public int Quantity { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public DateTime LastUpdate { get; set; }= DateTime.Now;
        public ShoeSize ShoeSize { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; }

    }
}
