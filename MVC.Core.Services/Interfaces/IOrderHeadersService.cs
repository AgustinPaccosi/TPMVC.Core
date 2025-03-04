using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface IOrderHeadersService
    {
        IEnumerable<OrderHeader>? GetAll(Expression<Func<OrderHeader, bool>>? filter = null,
            Func<IQueryable<OrderHeader>, IOrderedQueryable<OrderHeader>>? orderBy = null,
            string? propertiesNames = null);
        void Guardar(OrderHeader OrderHeader);
        void Eliminar(OrderHeader OrderHeader);
        OrderHeader? Get(Expression<Func<OrderHeader, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);
    }
}
