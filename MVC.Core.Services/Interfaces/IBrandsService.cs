using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface IBrandsService
    {
        IEnumerable<Brand>? GetAll(Expression<Func<Brand,
            bool>>? filter = null,
            Func<IQueryable<Brand>,
            IOrderedQueryable<Brand>>? orderBy = null,
            string? propertiesNames = null);

        void Guardar(Brand brand);

        void Eliminar(Brand brand);

        Brand? Get(Expression<Func<Brand,
            bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);

        bool Existe(Brand brand);

        bool EstaRelacionado(int id);
        List<Shoe> GetShoesForBrand(int brandId);
    }
}
