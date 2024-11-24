﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Interfaces
{

    public interface IShoesSizesRepository : IGenericRepository<ShoeSize>
    {
        void Update(ShoeSize ShoeSize);
        //void UpdateShoeSize(ShoeSize shoeSize);
    }

}
