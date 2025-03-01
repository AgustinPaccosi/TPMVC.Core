using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface IShoesService
    {
        IEnumerable<Shoe>? GetAll(Expression<Func<Shoe,
            bool>>? filter = null,
            Func<IQueryable<Shoe>,
            IOrderedQueryable<Shoe>>? orderBy = null,
            string? propertiesNames = null);

        void Guardar(Shoe shoe);

        void Eliminar(Shoe shoe);

        Shoe? Get(Expression<Func<Shoe,
            bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);

        bool Existe(Shoe shoe);

        bool EstaRelacionado(int id);

        public List<ShoeSize> GetAllShoesSizes(int shoeId);
        public ShoeSize? GetShoeSizeByIds(int sizeNumber, int shoeId);

        public void UpdateShoeSize(ShoeSize ss);
        public void InsertShoeSize(ShoeSize shoeSize);
    }
}
