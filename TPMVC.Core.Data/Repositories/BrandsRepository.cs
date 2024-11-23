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
    public class BrandsRepository : GenericRepository<Brand>, IBrandsRepository
    {
        private readonly Context _context;
        public BrandsRepository(Context context) : base(context)
        {
            _context = context;
        }

        public void Editar(Brand brand)
        {
            _context.Brands.Update(brand);
        }

        public bool EstaRelacionado(int id)
        {
            return _context.Shoes.Any(s => s.BrandId == id);
        }

        public bool Existe(Brand brand)
        {
            if (brand.BrandId == 0)
            {
                return _context.Brands
                    .Any(c => c.BrandName == brand.BrandName);
            }
            return _context.Brands
                .Any(c => c.BrandName == brand.BrandName
                && c.BrandId != brand.BrandId);
        }

        public List<Shoe> GetShoesForBrand(int brandId)
        {
            return _context.Shoes
                      .Include(s => s.Brand)
                      .Include(s => s.Sport)
                      .Include(s => s.Genre)
                      .Include(s => s.Colour)
                      .Where(s => s.BrandId == brandId)
                      .ToList();
        }
    }
}
