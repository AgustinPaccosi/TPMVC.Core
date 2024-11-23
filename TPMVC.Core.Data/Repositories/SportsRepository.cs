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
    public class SportsRepository : GenericRepository<Sport>, ISportsRepository
    {
        private readonly Context _context;

        public SportsRepository(Context context) : base(context)
        {
            _context = context;
        }

        public void Editar(Sport sport)
        {
            _context.Sports.Update(sport);
        }

        public bool EstaRelacionado(int id)
        {
            return _context.Shoes.Any(s => s.SportId == id);
        }

        public bool Existe(Sport sport)
        {
            if (sport.SportId == 0)
            {
                return _context.Sports
                    .Any(c => c.SportName == sport.SportName);
            }
            return _context.Sports
                .Any(c => c.SportName == sport.SportName
                && c.SportId != sport.SportId);
        }
        public List<Shoe> GetShoesForSport(int sportId)
        {
            return _context.Shoes
                      .Include(s => s.Brand)
                      .Include(s => s.Sport)
                      .Include(s => s.Genre)
                      .Include(s => s.Colour)
                      .Where(s => s.SportId == sportId)
                      .ToList();
        }
    }
}
