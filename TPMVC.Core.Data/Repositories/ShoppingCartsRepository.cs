using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Data.Repositories;
using TPMVC.Core.Entities;

namespace MVC.Core.Data.Services
{
    public class ShoppingCartsRepository : GenericRepository<ShoppingCart>, IShoppingCartsRepository
    {
        private readonly Context context;
        public ShoppingCartsRepository(Context _db):base (_db)
        {
            context = _db ?? throw new ArgumentNullException(nameof(_db));
        }
        public void Update(ShoppingCart shoppingCart)
        {
            context.ShoppingCarts.Update(shoppingCart);
        }
    }
}
