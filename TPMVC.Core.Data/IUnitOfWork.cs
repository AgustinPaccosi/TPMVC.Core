﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMVC.Core.Data
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void RollBack();
        int SaveChanges();
    }
}
