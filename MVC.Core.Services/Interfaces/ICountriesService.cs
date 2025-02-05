using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface ICountriesService
    {
        //IEnumerable<Country>? GetPaged(int pageSize, int? page, Expression<Func<Country, bool>>? filter = null,
        // Func<IQueryable<Country>, IOrderedQueryable<Country>>? orderBy = null,
        // string? propertiesNames = null);
        IEnumerable<Country> GetAll(Expression<Func<Country, bool>>? filter = null,
            Func<IQueryable<Country>, IOrderedQueryable<Country>>? orderBy = null,
            string? propertiesNames = null);
        void Guardar(Country country);
        void Eliminar(Country country);
        Country? Get(Expression<Func<Country, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);
        bool Existe(Country country);
        bool EstaRelacionado(int id);
    }
}
