using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface IShoppingCartService
    {
        IEnumerable<ShoppingCart>? GetAll(Expression<Func<ShoppingCart, bool>>? filter = null,
            Func<IQueryable<ShoppingCart>, IOrderedQueryable<ShoppingCart>>? orderBy = null,
            string? propertiesNames = null);
        void Save(ShoppingCart shoppingCart);
        void Delete(ShoppingCart shoppingCart);
        ShoppingCart? Get(Expression<Func<ShoppingCart, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);
    }
}
