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
    public class CountriesService:ICountriesService
    {
        private readonly ICountriesRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public CountriesService(ICountriesRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }

        public void Eliminar(Country category)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Eliminar(category);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.RollBack();
                throw;
            }
        }


        public bool Existe(Country category)
        {

            return _repository!.Existe(category);
        }

        public Country? Get(Expression<Func<Country, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<Country> GetAll(Expression<Func<Country, bool>>? filter = null,
            Func<IQueryable<Country>, IOrderedQueryable<Country>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames)!;
        }

        //public IEnumerable<Country>? GetPaged(int pageSize, int? page, Expression<Func<Country, bool>>? filter = null, Func<IQueryable<Country>, IOrderedQueryable<Country>>? orderBy = null, string? propertiesNames = null)
        //{
        //    return _repository!.GetPaged(pageSize, page, filter, orderBy, propertiesNames);
        //}

        public bool EstaRelacionado(int id)
        {

            return _repository!.EstaRelacionado(id);
        }

        public void Guardar(Country category)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (category.CountryId == 0)
                {
                    _repository?.Agregar(category);
                }
                else
                {
                    _repository?.Editar(category);
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
