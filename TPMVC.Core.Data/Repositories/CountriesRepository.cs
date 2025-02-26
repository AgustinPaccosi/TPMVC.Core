using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Repositories
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        
        private readonly Context _context;

        public CountriesRepository(Context context) : base(context)
        {
            _context = context;
        }
        public void Editar(Country country)
        {
            _context.Countries.Update(country);
        }

        public bool EstaRelacionado(int id)
        {
            return _context.States.Any(s => s.CountryId == id);
        }

        public bool Existe(Country country)
        {
            if (country.CountryId == 0)
            {
                return _context.Countries
                    .Any(c => c.CountryName == country.CountryName);
            }
            return _context.Countries
                .Any(c => c.CountryName == country.CountryName
                && c.CountryId != country.CountryId);
        }

    }
}
