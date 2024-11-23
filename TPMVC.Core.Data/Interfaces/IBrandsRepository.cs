using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Interfaces
{
    public interface IBrandsRepository : IGenericRepository<Brand>
    {
        void Editar(Brand brand);
        bool EstaRelacionado(int id);
        bool Existe(Brand brand);
        List<Shoe> GetShoesForBrand(int brandId);

    }
}
