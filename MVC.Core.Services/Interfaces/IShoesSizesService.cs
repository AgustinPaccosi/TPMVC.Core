using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface IShoesSizesService
    {
        IEnumerable<ShoeSize>? GetAll(Expression<Func<ShoeSize, bool>>? filter = null,
            Func<IQueryable<ShoeSize>, IOrderedQueryable<ShoeSize>>? orderBy = null,
            string? propertiesNames = null);
        void Guardar(ShoeSize ShoeSize);
        void Eliminar(ShoeSize ShoeSize);
        ShoeSize? Get(Expression<Func<ShoeSize, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);
    }
}
