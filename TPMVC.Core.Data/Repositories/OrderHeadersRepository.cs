using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Repositories
{
    public class OrderHeadersRepository : GenericRepository<OrderHeader>, IOrderHeadersRepository
    {
        private readonly Context _db;

        public OrderHeadersRepository(Context db) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public void Update(OrderHeader orderHeader)
        {

            _db.OrderHeaders.Update(orderHeader);

        }
    }
}
