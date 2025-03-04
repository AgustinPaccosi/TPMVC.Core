using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Repositories
{
    public class OrderDetailsRepository : GenericRepository<OrderDetail>, IOrderDetailsRepository
    {
        private readonly Context _db;

        public OrderDetailsRepository(Context db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public void Update(OrderDetail orderDetail)
        {

            _db.OrderDetails.Update(orderDetail);

        }
    }
}
