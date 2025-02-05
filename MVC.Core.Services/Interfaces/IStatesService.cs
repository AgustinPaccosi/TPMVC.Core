using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace MVC.Core.Services.Interfaces
{
    public interface IStatesService
    {
        IEnumerable<State> GetAll(Expression<Func<State, bool>>? filter = null,
    Func<IQueryable<State>, IOrderedQueryable<State>>? orderBy = null,
    string? propertiesNames = null);
        void Guardar(State state);
        void Eliminar(State state);
        State? Get(Expression<Func<State, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);
        bool Existe(State state);
        bool EstaRelacionado(int id);
    }
}
