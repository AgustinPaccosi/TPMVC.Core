using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface ISizesService
    {
        IEnumerable<Size>? GetAll(Expression<Func<Size,
           bool>>? filter = null,
           Func<IQueryable<Size>,
           IOrderedQueryable<Size>>? orderBy = null,
           string? propertiesNames = null);

        Size? Get(Expression<Func<Size,
            bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);

        void Guardar(Size size);
        void Eliminar(Size size);
        bool EstaRelacionado(Size size);
        bool Existe(Size size);
        int GetCantidad();
        List<Shoe>? GetShoesForSize(int sizeId);
        int ContarZapatillasPorTalle(int sizeId);
    }
}
