using MVC.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Data.Repositories;
using TPMVC.Core.Data;
using TPMVC.Core.Entities;
using MVC.Core.Data.Services;

namespace MVC.Core.Services.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartsRepository? repository;
        private readonly IUnitOfWork? _unitOfWork;

        public ShoppingCartService(IShoppingCartsRepository shoppingCart, IUnitOfWork unitOfWork)
        {
            repository = shoppingCart ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }

        public void Delete(ShoppingCart shoppingCart)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                repository!.Eliminar(shoppingCart);
                _unitOfWork!.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public ShoppingCart? Get(Expression<Func<ShoppingCart, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return repository!.Get(filter, propertiesNames, tracked);
        }

        public IEnumerable<ShoppingCart>? GetAll(Expression<Func<ShoppingCart, bool>>? filter = null, Func<IQueryable<ShoppingCart>, IOrderedQueryable<ShoppingCart>>? orderBy = null, string? propertiesNames = null)
        {
            return repository!.GetAll(filter, orderBy, propertiesNames)!;
        }

        public void Save(ShoppingCart shoppingCart)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (shoppingCart.ShoppingCartId == 0)
                {
                    repository?.Agregar(shoppingCart);
                }
                else
                {
                    repository?.Update(shoppingCart);
                }
                _unitOfWork?.Commit();

            }
            catch (Exception)
            {
                _unitOfWork?.RollBack();
                throw;
            }
        }
    }
}
