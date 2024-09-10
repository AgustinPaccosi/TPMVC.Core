using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Interfaces
{
    public interface IColoursRepository : IGenericRepository<Colour>
    {
        void Editar(Colour colour);
        bool EstaRelacionado(int id);
        bool Existe(Colour colour);


    }
}
