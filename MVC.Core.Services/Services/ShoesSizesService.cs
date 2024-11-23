using MVC.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Services
{
    public class ShoesSizesService : IShoesSizesService
    {
        private readonly IShoesSizesRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public ShoesSizesService(IShoesSizesRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }


        public void Eliminar(ShoeSize ShoeSize)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Eliminar(ShoeSize);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.RollBack();
                throw;
            }
        }

        public ShoeSize? Get(Expression<Func<ShoeSize, bool>>? filter = null,
            string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<ShoeSize> GetAll(Expression<Func<ShoeSize, bool>>? filter = null,
            Func<IQueryable<ShoeSize>, IOrderedQueryable<ShoeSize>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }

        public void Guardar(ShoeSize ShoeSize)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (ShoeSize.ShoeSizeId == 0)
                {
                    _repository?.Agregar(ShoeSize);
                }
                else
                {
                    _repository?.Update(ShoeSize);
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
