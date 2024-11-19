using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Repositories
{
    public class SizesRepository : GenericRepository<Size>, ISizesRepository
    {
        private readonly Context _context;

        public SizesRepository(Context context) : base(context)
        {
            _context = context;
        }

        public void Editar(Size size)
        {
            _context.Sizes.Update(size);
        }

        public bool EstaRelacionado(Size size)
        {
            return _context.ShoesSizes.Any(ss => ss.SizeId == size.SizeId);
        }

        public bool Existe(Size size)
        {
            if (size.SizeId == 0)
            {
                return _context.Sizes.Any(s => s.SizeNumber == size.SizeNumber);
            }
            return _context.Sizes.Any(s => s.SizeNumber == size.SizeNumber && s.SizeId != size.SizeId);
        }

        public int GetCantidad()
        {
            return _context.Sizes.Count();
        }

        public List<Shoe> GetShoesForSize(int sizeId)
        {

            return _context.ShoesSizes
                .Include(ss => ss.Shoe).ThenInclude(s => s.Brand)
                .Include(ss => ss.Shoe).ThenInclude(s => s.Genre)
                .Include(ss => ss.Shoe).ThenInclude(s => s.Sport)
                .Include(ss => ss.Shoe).ThenInclude(s => s.Colour)
                .Where(ss => ss.SizeId == sizeId)
                .Select(ss => ss.Shoe)
                .ToList();

        }
    }
}
