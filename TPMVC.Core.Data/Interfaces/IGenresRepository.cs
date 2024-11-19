﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Interfaces
{
    public interface IGenresRepository : IGenericRepository<Genre>
    {
        void Editar(Genre genre);
        bool EstaRelacionado(int id);
        bool Existe(Genre genre);
    }
}