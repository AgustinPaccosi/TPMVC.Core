using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Repositories
{
    public class CitiesRepository:GenericRepository<City>, ICitiesRepository
    {
        private readonly Context _context;

        public CitiesRepository(Context context) : base(context)
        {
            _context = context;
        }
        public void Editar(City city)
        {
            _context.Cities.Update(city);
        }

        public bool EstaRelacionado(int id)
        {
            return _context.ApplicationUsers.Any(s => s.CityId == id);
        }

        public bool Existe(City city)
        {
            if (city.CityId == 0)
            {
                return _context.Cities
                    .Any(c => c.CityName == city.CityName);
            }
            return _context.Cities
                .Any(c => c.CityName == city.CityName
                && c.CityId != city.CityId);
        }
    }
}
