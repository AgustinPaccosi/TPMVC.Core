using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Repositories
{
    public class ShoesSizesRepository : GenericRepository<ShoeSize>, IShoesSizesRepository
    {
        private readonly Context _context;
        public ShoesSizesRepository(Context? db) : base(db)
        {
            _context=db?? throw new ArgumentNullException(nameof(db));
        }

        public void Update(ShoeSize ShoeSize)
        {
            _context!.ShoesSizes.Update(ShoeSize);
        }

        //public void UpdateShoeSize(ShoeSize shoeSize)
        //{
        //    _context.ShoesSizes.Update(shoeSize);
        //}
    }
}
