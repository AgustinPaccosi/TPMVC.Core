using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface ICitiesService
    {
        bool Existe(City city);
        IEnumerable<City> GetAll(Expression<Func<City, bool>>? filter = null,
            Func<IQueryable<City>, IOrderedQueryable<City>>? orderBy = null,
            string? propertiesNames = null);
        City? Get(Expression<Func<City, bool>> filter,
            string? propertiesNames = null,
            bool tracked = true);
        bool EstaRelacionado(int cityId);
        void Eliminar(City city);
        void Guardar(City city);
    }
}
