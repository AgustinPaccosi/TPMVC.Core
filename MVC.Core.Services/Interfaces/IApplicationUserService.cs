using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface IApplicationUserService
    {
        IEnumerable<ApplicationUser>? GetAll(Expression<Func<ApplicationUser, bool>>? filter = null,
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>>? orderBy = null,
            string? propertiesNames = null);
        void Guardar(ApplicationUser applicationUser);
        void Eliminar(ApplicationUser applicationUser);
        ApplicationUser? Get(Expression<Func<ApplicationUser, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);
    }
}
