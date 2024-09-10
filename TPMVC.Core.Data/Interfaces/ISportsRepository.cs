using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Interfaces
{
    public interface ISportsRepository : IGenericRepository<Sport>
    {
        void Editar(Sport sport);
        bool EstaRelacionado(int id);
        bool Existe(Sport sport);

    }
}
