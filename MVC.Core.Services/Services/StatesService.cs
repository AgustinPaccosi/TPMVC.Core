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
    public class StatesService: IStatesService
    {
        private readonly IStatesRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public StatesService(IStatesRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }

        public void Eliminar(State state)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Eliminar(state);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.RollBack();
                throw;
            }
        }


        public bool Existe(State state)
        {

            return _repository!.Existe(state);
        }

        public State? Get(Expression<Func<State, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<State> GetAll(Expression<Func<State, bool>>? filter = null,
            Func<IQueryable<State>, IOrderedQueryable<State>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames)!;
        }


        public bool EstaRelacionado(int id)
        {

            return _repository!.EstaRelacionado(id);
        }

        public void Guardar(State state)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (state.StateId == 0)
                {
                    _repository?.Agregar(state);
                }
                else
                {
                    _repository?.Editar(state);
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
