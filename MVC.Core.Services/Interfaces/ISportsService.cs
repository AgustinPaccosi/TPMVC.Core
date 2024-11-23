using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface ISportsService
    {
        IEnumerable<Sport>? GetAll(Expression<Func<Sport,
            bool>>? filter = null,
            Func<IQueryable<Sport>,
            IOrderedQueryable<Sport>>? orderBy = null,
            string? propertiesNames = null);

        void Guardar(Sport sport);

        void Eliminar(Sport sport);

        Sport? Get(Expression<Func<Sport,
            bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);

        bool Existe(Sport sport);

        bool EstaRelacionado(int id);

        public List<Shoe> GetShoesForSport(int sportId);
    }
}
