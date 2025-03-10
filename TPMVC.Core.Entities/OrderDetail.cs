﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMVC.Core.Entities
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ShoeSizeId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public OrderHeader? OrderHeader { get; set; }
        public ShoeSize? ShoeSizes { get; set; }
    }
}
