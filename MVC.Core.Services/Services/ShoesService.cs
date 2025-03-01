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
    public class ShoesService : IShoesService
    {
        private readonly IShoesRepository _repository;

        private readonly IUnitOfWork _unitOfWork;

        public ShoesService(IShoesRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Eliminar(Shoe shoe)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _repository.Eliminar(shoe);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public bool EstaRelacionado(int id)
        {
            try
            {
                return _repository.ItsRelated(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Existe(Shoe shoe)
        {
            try
            {
                return _repository.Exist(shoe);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Shoe? Get(Expression<Func<Shoe, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }

        public IEnumerable<Shoe>? GetAll(Expression<Func<Shoe, bool>>? filter = null, Func<IQueryable<Shoe>, IOrderedQueryable<Shoe>>? orderBy = null, string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }

        public void Guardar(Shoe shoe)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (shoe.ShoeId == 0)
                {
                    _repository.Agregar(shoe);
                }
                else
                {
                    _repository.Update(shoe);
                }

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public List<ShoeSize> GetAllShoesSizes(int shoeId)
        {
            return _repository.GetAllShoesSizes(shoeId);
        }

        public ShoeSize? GetShoeSizeByIds(int sizeNumber, int shoeId)
        {
            return _repository.GetShoeSizeByIds( sizeNumber,  shoeId);
        }

        public void UpdateShoeSize(ShoeSize ss)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                _repository.UpdateShoeSize(ss);

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public void InsertShoeSize(ShoeSize shoeSize)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                _repository.InsertShoeSize(shoeSize);

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}
