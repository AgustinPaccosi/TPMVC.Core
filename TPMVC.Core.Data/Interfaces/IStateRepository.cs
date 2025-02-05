using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Interfaces
{
    public interface IStatesRepository: IGenericRepository<State>
    {
        void Editar(State state);
        bool EstaRelacionado(int id);
        bool Existe(State state);
    }
}
