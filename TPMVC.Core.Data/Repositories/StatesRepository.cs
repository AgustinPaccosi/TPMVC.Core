using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Repositories
{
    public class StatesRepository: GenericRepository<State>, IStatesRepository
    {
        private readonly Context _context;

        public StatesRepository(Context context) : base(context)
        {
            _context = context;
        }
        public void Editar(State state)
        {
            _context.States.Update(state);
        }

        public bool EstaRelacionado(int id)
        {
            return _context.Cities.Any(s => s.StateId == id);
        }

        public bool Existe(State state)
        {
            if (state.StateId == 0)
            {
                return _context.States
                    .Any(c => c.StateName == state.StateName);
            }
            return _context.States
                .Any(c => c.StateName == state.StateName
                && c.StateId != state.StateId);
        }
    }
}
