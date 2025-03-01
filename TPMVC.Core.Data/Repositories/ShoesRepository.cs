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
    public class ShoesRepository : GenericRepository<Shoe>, IShoesRepository
    {
        private readonly Context _context;

        public ShoesRepository(Context context) : base(context)
        {
            _context = context;
        }

        public bool Exist(Shoe shoe)
        {
            if (shoe.ShoeId == 0)
            {
                return _context.Shoes.Any(s => s.BrandId == shoe.BrandId
                                            && s.SportId == shoe.SportId
                                            && s.GenreId == shoe.GenreId
                                            && s.ColourId == shoe.ColourId
                                            && s.Model == shoe.Model
                                            && s.Description == shoe.Description
                                            && s.Price == shoe.Price
                                            && s.Active == shoe.Active);
            }
            return _context.Shoes.Any(s => s.BrandId == shoe.BrandId
                                        && s.SportId == shoe.SportId
                                        && s.GenreId == shoe.GenreId
                                        && s.ColourId == shoe.ColourId
                                        && s.Model == shoe.Model
                                        && s.Description == shoe.Description
                                        && s.Price == shoe.Price
                                        && s.Active == shoe.Active
                                        && s.ShoeId == shoe.ShoeId);
        }

        public bool ItsRelated(int id)
        {
            return _context.ShoesSizes.Any(ss => ss.ShoeId == id);
        }

        public void Update(Shoe shoe)
        {
            var brandExist = _context.Brands.FirstOrDefault(b => b.BrandId == shoe.BrandId);

            if (brandExist != null)
            {
                _context.Attach(brandExist);
                shoe.Brand = brandExist;
            }

            var sportExist = _context.Sports.FirstOrDefault(s => s.SportId == shoe.SportId);

            if (sportExist != null)
            {
                _context.Attach(sportExist);
                shoe.Sport = sportExist;
            }

            var genreExist = _context.Genres.FirstOrDefault(g => g.GenreId == shoe.GenreId);

            if (genreExist != null)
            {
                _context.Attach(genreExist);
                shoe.Genre = genreExist;
            }

            var colourExist = _context.Colours.FirstOrDefault(c => c.ColourId == shoe.ColourId);

            if (colourExist != null)
            {
                _context.Attach(colourExist);
                shoe.Colour = colourExist;
            }

            var shoeExist = _context.Shoes.Local.FirstOrDefault(s => s.ShoeId == shoe.ShoeId);

            if (shoeExist != null)
            {
                _context.Entry(shoeExist).State = EntityState.Detached;
            }

            _context.Shoes.Update(shoe);

        }

        public ShoeSize? GetShoeSizeByIds(int sizeNumber, int shoeId)
        {
            var shoeSize=_context.ShoesSizes.Where(b => b.ShoeId == shoeId && b.SizeId == sizeNumber).FirstOrDefault();
            return shoeSize;
            //return _context.ShoesSizes
            //.FromSqlRaw("EXEC GetShoeSizeBySizeNumber @SizeNumber = {0}, @ShoeId = {1}", sizeNumber, shoeId)
            //.AsEnumerable()
            //.FirstOrDefault();
        }

        public List<ShoeSize> GetAllShoesSizes(int shoeId)
        {
            //List<ShoeSize> shoesSizes = _context.ShoesSizes.Any(b=>b.ShoeId == shoeId);
            ////.FromSqlRaw("EXEC GetAllSizesWithStock @ShoeId = {0}", shoeId)
            ////.ToList();


            ////foreach (var shoeSize in shoesSizes)
            ////{
            ////    shoeSize.Size = _context.Sizes.FirstOrDefault(s => s.SizeId == shoeSize.SizeId);
            ////}

            //return shoesSizes;
            List<ShoeSize> shoesSizes = _context.ShoesSizes
        .Where(b => b.ShoeId == shoeId)
        .Include(s => s.Size) // Asegúrate de que la relación está bien configurada en el modelo
        .ToList();

            return shoesSizes;
        }

        public void UpdateShoeSize(ShoeSize ss)
        {
            _context.ShoesSizes.Update(ss);
        }

        public void InsertShoeSize(ShoeSize shoeSize)
        {
            _context.ShoesSizes.Add(shoeSize);
        }
    }
}
