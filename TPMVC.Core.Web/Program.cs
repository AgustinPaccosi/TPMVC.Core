using Microsoft.AspNetCore.Identity;
using Microsoft.DiaSymReader;
using TPMCV.Core.IOC;
using TPMVC.Core.Data;
using TPMVC.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace TPN1EnWeb.Web
{
    public class Program
    {
        public static async Task Main(string[] args) //PREGUNTAR QU ES ASYNC? POR QU? PARA QU SIRVE? LO MISMO TASK
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("ContextConnection") ?? throw new InvalidOperationException("Connection string 'ContextConnection' not found.");

            builder.Services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Context>();
            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<Context>().AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
            });
            builder.Services.AddRazorPages();//Con esto puedo trabajar con las p�ginas Razor las cuales son las de ingreso y registro a la p�gina
                                             // Add services to the container.

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            DI.ConfigurarServicios(builder.Services, builder.Configuration);

            builder.Services.AddAutoMapper(typeof(Program).Assembly);


            var app = builder.Build();
            using (var scope = app.Services.CreateScope()) //Para utilizar el mtodo de abajo utilizo este Scope
            {
                var services = scope.ServiceProvider; //Ya que solicita los servicios y se los injecta al mtodo de abajo
                await SeedRolesAndAdminUser(services);
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            
            //a
            app.UseHttpsRedirection();
            app.UseStaticFiles();//Indico que utilizo archivos estaticos (estos estan en WWWROOT)

            app.UseRouting();

            app.UseAuthentication();//Se fijo que usuario es y el tipo

            app.UseAuthorization();
            app.MapRazorPages();//Para el mapeo de las Pginas Razor
            app.MapControllerRoute(//Indico como voy a mapear las rutas que le paso en la barra de navegacin
                name: "default",
                //Puede tener recibir un controlador, sino por defecto es Home, puede recibir una accin, sino por defecto es Index
                //y podra llegar a recibir un parametro (este es opcional)
                pattern: "{area=Customer}/{controller=Home}/{action=Hero}/{id?}"); //Ahora como tenemos un area se la debemos agregar al patrn, adems debo pasarle
                                                                                   //el _viewimport y el _viewstart tanto a las vistas del area admin como a customer

            app.Run();
        }

        private static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>(); //Traigo el manejador de roles
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>(); //Traigo el manejador de usuarios

            if (!await roleManager.RoleExistsAsync(WC.Role_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(WC.Role_Admin)); //Si no existe el rol Admin me lo crea
            }
            if (!await roleManager.RoleExistsAsync(WC.Role_Customer))
            {
                await roleManager.CreateAsync(new IdentityRole(WC.Role_Customer));//Si no existe el rol Customer me lo crea
            }

            var adminUser = await userManager.FindByEmailAsync("admin@gmail.com"); //Se fija si existe un usuario de tipo administrador el cual va a trabajar con este correo electronico
            if (adminUser == null) //Si no lo encuentra lo crea 
            {
                adminUser = new IdentityUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin123."); //Me lo agrega con esta contrasea

                // Asignar el rol de Admin al usuario
                await userManager.AddToRoleAsync(adminUser, WC.Role_Admin); //Y por ltimo lo agrega al rol de Admin
            }

        } 
    }
}