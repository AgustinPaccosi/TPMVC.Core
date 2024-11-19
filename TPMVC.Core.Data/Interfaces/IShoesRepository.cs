using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Interfaces
{
    public interface IShoesRepository : IGenericRepository<Shoe>
    {
        void Update(Shoe shoe);
        bool Exist(Shoe shoe);
        bool ItsRelated(int id);
        public List<ShoeSize> GetAllShoesSizes(int shoeId);
        public ShoeSize? GetShoeSizeBySizeNumber(decimal sizeNumber, int shoeId);
        public void UpdateShoeSize(ShoeSize ss);
        public void InsertShoeSize(ShoeSize shoeSize);
    }
}
