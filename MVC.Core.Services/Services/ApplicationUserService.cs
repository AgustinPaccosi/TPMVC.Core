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
    public class ApplicationUserService:IApplicationUserService
    {
        private readonly IApplicationUserRepository? _repository;
        private readonly IUnitOfWork? _unitOfWork;

        public ApplicationUserService(IApplicationUserRepository? repository,
            IUnitOfWork? unitOfWork)
        {
            _repository = repository ?? throw new ArgumentException("Dependencies not set");
            _unitOfWork = unitOfWork ?? throw new ArgumentException("Dependencies not set");
        }

        public void Eliminar(ApplicationUser applicationUser)
        {
            try
            {
                _unitOfWork!.BeginTransaction();
                _repository!.Eliminar(applicationUser);
                _unitOfWork!.Commit();

            }
            catch (Exception)
            {
                _unitOfWork!.RollBack();
                throw;
            }
        }


        public ApplicationUser? Get(Expression<Func<ApplicationUser, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            return _repository!.Get(filter, propertiesNames, tracked);
        }


        public IEnumerable<ApplicationUser> GetAll(Expression<Func<ApplicationUser, bool>>? filter = null,
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>>? orderBy = null,
            string? propertiesNames = null)
        {
            return _repository!.GetAll(filter, orderBy, propertiesNames)!;
        }



        public void Guardar(ApplicationUser applicationUser)
        {
            try
            {
                _unitOfWork?.BeginTransaction();
                if (applicationUser.Id == string.Empty)
                {
                    _repository?.Agregar(applicationUser);
                }
                else
                {
                    _repository?.Update(applicationUser);
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
