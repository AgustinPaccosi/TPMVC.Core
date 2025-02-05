using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Data;
using TPMVC.Core.Entities;
using MVC.Core.Services.Interfaces;

namespace MVC.Core.Services.Services
{
    public class CitiesService : ICitiesService
    {
        private readonly ICitiesRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public CitiesService(ICitiesRepository? repository, IUnitOfWork? unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public bool Existe(City city)
        {
            if (_repository is null)
            {
                throw new ApplicationException("Dependency not set");
            }
            return _repository.Existe(city);
        }

        public IEnumerable<City> GetAll(Expression<Func<City, bool>>? filter = null,
            Func<IQueryable<City>, IOrderedQueryable<City>>? orderBy = null, string? propertiesNames = null)
        {
            if (_repository == null)
            {
                throw new ApplicationException("Dependency not set");
            }
            return _repository.GetAll(filter, orderBy, propertiesNames)!;
        }

        public City? Get(Expression<Func<City, bool>> filter, string? propertiesNames = null, bool tracked = true)
        {
            if (_repository == null)
            {
                throw new ApplicationException("Dependency not set");
            }

            return _repository.Get(filter, propertiesNames, tracked);
        }

        public bool EstaRelacionado(int cityId)
        {
            return _repository!.EstaRelacionado(cityId);
        }

        public void Eliminar(City city)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                _repository?.Eliminar(city);
                _unitOfWork?.Commit();

            }
            catch (Exception)
            {
                _unitOfWork?.RollBack();
                throw;
            }

        }

        public void Guardar(City city)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (city.CityId == 0)
                {
                    _repository?.Agregar(city);
                }
                else
                {
                    _repository?.Editar(city);
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
