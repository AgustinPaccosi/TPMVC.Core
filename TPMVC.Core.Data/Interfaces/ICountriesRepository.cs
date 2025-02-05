using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Interfaces
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        void Editar(Country country);
        bool EstaRelacionado(int id);
        bool Existe(Country country);
        
    }
}
