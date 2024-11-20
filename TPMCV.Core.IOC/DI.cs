using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MVC.Core.Services.Interfaces;
using MVC.Core.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Data.Repositories;

namespace TPMCV.Core.IOC
{
    public static class DI
    {
        public static void ConfigurarServicios(IServiceCollection servicios, IConfiguration configuration)
        {

            servicios.AddScoped<IBrandsRepository, BrandsRepository>();

            servicios.AddScoped<IColoursRepository, ColoursRepository>();

            servicios.AddScoped<ISportsRepository, SportsRepository>();

            servicios.AddScoped<IGenresRepository, GenresRepository>();

            servicios.AddScoped<IShoesRepository, ShoesRepository>();

            servicios.AddScoped<ISizesRepository, SizesRepository>();

            //SERVICIOS

            servicios.AddScoped<IBrandsService, BrandsService>();

            servicios.AddScoped<IColoursService, ColoursService>();

            servicios.AddScoped<ISportsService, SportsService>();

            servicios.AddScoped<IGenresService, GenresService>();

            servicios.AddScoped<IShoesService, ShoesService>();

            servicios.AddScoped<ISizesService, SizesService>();

            servicios.AddScoped<IUnitOfWork, UnitOfWork>();

            servicios.AddDbContext<Context>(optiones =>
            {
                optiones.UseSqlServer(configuration.GetConnectionString("MyConn"));
            });

        }

    }
}
