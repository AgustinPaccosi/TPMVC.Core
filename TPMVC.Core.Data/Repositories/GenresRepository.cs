using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Repositories
{
    public class GenresRepository : GenericRepository<Genre>, IGenresRepository
    {
        private readonly Context _context;
        public GenresRepository(Context context) : base(context)
        {
            _context = context;
        }

        public void Editar(Genre genre)
        {
            _context.Genres.Update(genre);
        }

        public bool EstaRelacionado(int id)
        {
            return _context.Shoes.Any(s => s.GenreId == id);
        }

        public bool Existe(Genre genre)
        {
            if (genre.GenreId == 0)
            {
                return _context.Genres
                    .Any(c => c.GenreName == genre.GenreName);
            }
            return _context.Genres
                .Any(c => c.GenreName == genre.GenreName
                && c.GenreId != genre.GenreId);
        }
    }
}
