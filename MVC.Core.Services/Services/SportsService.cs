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
    public class SportsService : ISportsService
    {
        private readonly ISportsRepository _repository;

        private readonly IUnitOfWork _unitOfWork;

        public SportsService(ISportsRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Eliminar(Sport sport)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _repository.Eliminar(sport);
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
                return _repository.EstaRelacionado(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Existe(Sport sport)
        {
            try
            {
                return _repository.Existe(sport);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Sport? Get(Expression<Func<Sport, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }

        public IEnumerable<Sport>? GetAll(Expression<Func<Sport, bool>>? filter = null, Func<IQueryable<Sport>, IOrderedQueryable<Sport>>? orderBy = null, string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames);
        }

        public void Guardar(Sport sport)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                if (sport.SportId == 0)
                {
                    _repository.Agregar(sport);
                }
                else
                {
                    _repository.Editar(sport);
                }

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
