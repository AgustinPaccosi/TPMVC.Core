using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface IGenresService
    {
        IEnumerable<Genre>? GetAll(Expression<Func<Genre,
            bool>>? filter = null,
            Func<IQueryable<Genre>,
            IOrderedQueryable<Genre>>? orderBy = null,
            string? propertiesNames = null);

        void Guardar(Genre genre);

        void Eliminar(Genre genre);

        Genre? Get(Expression<Func<Genre,
            bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);

        bool Existe(Genre genre);

        bool EstaRelacionado(int id);

    }
}
