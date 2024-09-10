using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Repositories
{
    public class ColoursRepository : GenericRepository<Colour>, IColoursRepository
    {
        private readonly Context _context;

        public ColoursRepository(Context context) : base(context)
        {
            _context = context;
        }

        public void Editar(Colour colour)
        {
            _context.Colours.Update(colour);
        }

        public bool EstaRelacionado(int id)
        {
            return _context.Shoes.Any(s => s.ColourId == id);
        }

        public bool Existe(Colour colour)
        {
            if (colour.ColourId == 0)
            {
                return _context.Colours
                    .Any(c => c.ColourName == colour.ColourName);
            }
            return _context.Colours
                .Any(c => c.ColourName == colour.ColourName
                && c.ColourId != colour.ColourId);
        }
    }
}
