using MVC.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Data;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Services
{
    public class OrderHeadersService : IOrderHeadersService
    {
        private readonly IOrderHeadersRepository? _repository;
        private readonly IShoesSizesRepository? _shoeSizeRepository;
        private readonly IShoppingCartsRepository? _shoppingCartsRepository;
        private readonly IUnitOfWork? _unitOfWork;

        public OrderHeadersService(IOrderHeadersRepository? repository,
            IShoesSizesRepository shoeSizesRepository,
            IShoppingCartsRepository shoppingCartsRepository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Dependencies not set");
            _shoeSizeRepository = shoeSizesRepository ?? throw new ArgumentException("Dependencies not set");
            _shoppingCartsRepository = shoppingCartsRepository ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }

        public void Eliminar(OrderHeader orderHeader)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Eliminar(orderHeader);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.RollBack();
                throw;
            }
        }
        public OrderHeader? Get(Expression<Func<OrderHeader, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<OrderHeader> GetAll(Expression<Func<OrderHeader, bool>>? filter = null,
            Func<IQueryable<OrderHeader>, IOrderedQueryable<OrderHeader>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames)!;
        }
        public void Guardar(OrderHeader orderHeader)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (orderHeader.OrderHeaderId == 0)
                {
                    _repository?.Agregar(orderHeader);
                    //_unitOfWork.SaveChanges();
                    foreach (var item in orderHeader.OrderDetail)
                    {
                        item.ShoeSizes = _shoeSizeRepository!.Get(
                            filter: p => p.ShoeSizeId == item.ShoeSizeId);
                        item.ShoeSizes!.QuantityInStock -= item.Quantity;
                        item.ShoeSizes.StockInCarts -= item.Quantity;
                        _shoeSizeRepository.Update(item.ShoeSizes);

                        var shoppingCart = _shoppingCartsRepository!.Get(
                                filter: sc => sc.ShoeSizeId == item.ShoeSizeId
                                && sc.ApplicationUserId == orderHeader.ApplicationUserId);
                        _shoppingCartsRepository.Eliminar(shoppingCart);
                    }
                }
                else
                {
                    _repository?.Update(orderHeader);
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
