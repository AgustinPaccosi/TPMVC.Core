using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data
{
    public partial class Context : IdentityDbContext<IdentityUser>
    {
        public Context()
        {

        }

        public Context(DbContextOptions<Context> options) : base(options)
        { 

        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<ShoeSize> ShoesSizes { get; set; }
        public DbSet<Size> Sizes { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts {get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source =.;
                            Initial Catalog = TP01EF2024;
                            Trusted_Connection = true;
                            TrustServerCertificate = true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var brands = new List<Brand>()
            {
                new Brand
                {
                    BrandId = 1,
                    BrandName = "Vans",
                    Active = true
                },
                new Brand
                {
                    BrandId = 2,
                    BrandName = "Adidas",
                    Active = true
                },
                new Brand
                {
                    BrandId = 3,
                    BrandName = "Topper",
                    Active = true
                },
            };

            var genres = new List<Genre>()
            {
                new Genre
                {
                    GenreId = 1,
                    GenreName = "Femenino"
                },
                new Genre
                {
                    GenreId = 2,
                    GenreName = "Masculino"
                },
                new Genre
                {
                    GenreId = 3,
                    GenreName = "Unisex"
                },
            };

            var sports = new List<Sport>()
            {
                new Sport
                {
                    SportId = 1,
                    SportName = "Futbol",
                    Active = true
                },
                new Sport
                {
                    SportId = 2,
                    SportName = "Tenis",
                    Active = true
                },
                new Sport
                {
                    SportId = 3,
                    SportName = "Basquet",
                    Active = true
                },
            };

            var colours = new List<Colour>()
            {
                new Colour
                {
                    ColourId = 1,
                    ColourName = "Rojo",
                    Active = true
                },
                new Colour
                {
                    ColourId = 2,
                    ColourName = "Negro",
                    Active = true
                },
                new Colour
                {
                    ColourId = 3,
                    ColourName = "Blanco",
                    Active = true
                },
            };

            var sizes = new List<Size>();

            int sizeId = 1;

            for (decimal i = 28; i <= 50; i += 0.5m)
            {
                sizes.Add(new Size
                {
                    SizeId = sizeId++,
                    SizeNumber = i
                });
            }

            var shoes = new List<Shoe>()
            {
                new Shoe
                {
                    ShoeId = 1,
                    BrandId = 1,
                    SportId = 3,
                    GenreId = 2,
                    ColourId = 1,
                    Model = "Deportivas",
                    Description = "Vans Deportivas",
                    Price = 15,
                    Active = true
                },
                new Shoe
                {
                    ShoeId = 2,
                    BrandId = 2,
                    SportId = 1,
                    GenreId = 1,
                    ColourId = 2,
                    Model = "Botines",
                    Description = "Botines Femeninos",
                    Price = 20,
                    Active = true
                },
                new Shoe
                {
                    ShoeId = 3,
                    BrandId = 3,
                    SportId = 2,
                    GenreId = 3,
                    ColourId = 1,
                    Model = "1982",
                    Description = "Importados",
                    Price = 35,
                    Active = true
                },
            };
            modelBuilder.Entity<IdentityUserLogin<string>>()
                   .HasKey(login => new { login.LoginProvider, login.ProviderKey });
            //LoginProvider: Es un identificador del proveedor externo de inicio de sesión (por ejemplo, "Google", "Facebook", o "Microsoft")
            //ProviderKey: Es el identificador único que el proveedor externo utiliza para identificar al usuario. Por ejemplo:
            //En el caso de Google, esto sería el ID único del usuario en Google.
            //En el caso de Facebook, sería el ID único del usuario en Facebook.

            //La combinación de estas dos propiedades es única por cada inicio de sesión externo. Al usarlas como clave primaria compuesta:
            //Garantizamos unicidad: Evitamos duplicados en la tabla AspNetUserLogins. Por ejemplo, el mismo usuario no debería tener dos entradas con el mismo LoginProvider y ProviderKey.
            //Facilitamos consultas rápidas: ASP.NET Identity necesita buscar rápidamente si un usuario ya tiene un inicio de sesión externo registrado.
            //Es una convención estándar: Esta estructura ya está definida por diseño en ASP.NET Identity y es consistente con las mejores prácticas.




            // Configurar la clave primaria compuesta de IdentityUserRole
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId }); // Clave primaria compuesta
            });
            //Usando las propiedades UserId y RoleId. Esto asegura que:
            //Un mismo usuario no pueda estar en el mismo rol más de una vez.
            //El sistema pueda buscar y eliminar relaciones de usuario - rol de manera eficiente


            //IdentityUserToken<string> Sirve para crear tokens personalizados
            //Un token es un valor que puede representar información sobre un usuario o proporcionar acceso temporal a ciertos recursos.
            //Ejemplos comunes:
            //Tokens para autenticación persistente(como cookies o JWT).
            //Tokens para reiniciar contraseñas o realizar verificaciones de correo electrónico.
            //Tokens de integración con otros servicios(como OAuth o API externas).
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name }); // Clave primaria compuesta
            });
            //UserId: Un usuario puede tener varios tokens, pero cada token pertenece a un usuario específico.
            //LoginProvider: Permite distinguir tokens emitidos por diferentes proveedores(como Google, Facebook, etc.).
            //Name: Permite que un mismo proveedor emita diferentes tipos de tokens para el mismo usuario(como AccessToken y RefreshToken).
            //Con esta combinación, el sistema asegura que:
            //No existan tokens duplicados para un mismo usuario, proveedor y nombre.
            //Se pueda identificar y eliminar un token específico de forma eficiente

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.BrandId);
                entity.HasIndex(t => t.BrandName).IsUnique();
                entity.Property(e => e.BrandName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Active).IsRequired().HasDefaultValue(true);
                entity.HasData(brands);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.GenreId);
                entity.HasIndex(t => t.GenreName).IsUnique();
                entity.Property(e => e.GenreName).IsRequired().HasMaxLength(10);
                entity.HasData(genres);
            });

            modelBuilder.Entity<Sport>(entity =>
            {
                entity.HasKey(e => e.SportId);
                entity.HasIndex(t => t.SportName).IsUnique();
                entity.Property(e => e.SportName).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Active).IsRequired().HasDefaultValue(true);
                entity.HasData(sports);
            });

            modelBuilder.Entity<Colour>(entity =>
            {
                entity.HasKey(e => e.ColourId);
                entity.HasIndex(t => t.ColourName).IsUnique();
                entity.Property(e => e.ColourName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Active).IsRequired().HasDefaultValue(true);
                entity.HasData(colours);
            });

            modelBuilder.Entity<Shoe>(entity =>
            {
                entity.HasKey(e => e.ShoeId);
                entity.HasOne(s => s.Brand).WithMany(b => b.Shoes).HasForeignKey(s => s.BrandId);
                entity.HasOne(s => s.Genre).WithMany(g => g.Shoes).HasForeignKey(s => s.GenreId);
                entity.HasOne(s => s.Sport).WithMany(s => s.Shoes).HasForeignKey(s => s.SportId);
                entity.HasOne(s => s.Colour).WithMany(s => s.Shoes).HasForeignKey(s => s.ColourId);
                entity.Property(e => e.Model).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Price).HasPrecision(10, 2);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Active).IsRequired().HasDefaultValue(true);
                entity.HasData(shoes);
            });

            modelBuilder.Entity<Size>(entity =>
            {
                entity.HasKey(s => s.SizeId);
                entity.HasIndex(s => s.SizeNumber).IsUnique();
                entity.Property(s => s.SizeNumber).HasColumnType("decimal (3, 1)").HasPrecision(3, 1).IsRequired();
                entity.HasData(sizes);
                entity.ToTable("Sizes");
            });

            modelBuilder.Entity<ShoeSize>(entity =>
            {
                entity.HasKey(ss => ss.ShoeSizeId);
                entity.HasIndex(ss => new { ss.ShoeId, ss.SizeId }).IsUnique();
                entity.HasOne(ss => ss.Shoe).WithMany(s => s.ShoesSizes).HasForeignKey(sc => sc.ShoeId);
                entity.HasOne(ss => ss.Size).WithMany(s => s.ShoesSizes).HasForeignKey(sc => sc.SizeId);
                entity.Property(ss => ss.QuantityInStock).IsRequired();
                entity.ToTable("ShoesSizes");

            });
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId);
                entity.HasIndex(t => t.CountryName).IsUnique();
                entity.Property(e => e.CountryName).IsRequired().HasMaxLength(50);
            });
            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => e.StateId);
                entity.HasIndex(t => t.StateName).IsUnique();
                entity.Property(e => e.StateName).IsRequired().HasMaxLength(50);
                //entity.HasOne(ss => ss.Country).WithMany(s => s.States).HasForeignKey(sc => sc.StateId);
                entity.HasOne(ss => ss.Country)
                    .WithMany(s => s.States)
                     .HasForeignKey(sc => sc.CountryId); // 🔴 Debe ser CountryId

            });
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityId);
                entity.HasIndex(t => t.CityName).IsUnique();
                entity.Property(e => e.CityName).IsRequired().HasMaxLength(50);
                entity.HasOne(ss => ss.Country).WithMany(s => s.Cities).HasForeignKey(sc => sc.CountryId);
                entity.HasOne(ss => ss.States).WithMany(s => s.Cities).HasForeignKey(sc => sc.StateId);
            });
            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => e.ShoppingCartId);
                entity.HasIndex(t => t.ShoppingCartId).IsUnique();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.LastUpdate).IsRequired();
                entity.HasOne(sc => sc.ShoeSize)
                    .WithMany() // Sin relación inversa, ya que ShoeSize no necesita conocer ShoppingCart
                    .HasForeignKey(sc => sc.ShoeSizeId); // O el comportamiento de tu preferencia

                // Configura la relación con ApplicationUser
                entity.HasOne(sc => sc.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(sc => sc.ApplicationUserId);
                //entity.HasOne(ss => ss.Country).WithMany(s => s.Cities).HasForeignKey(sc => sc.CountryId);
                //entity.HasOne(ss => ss.States).WithMany(s => s.Cities).HasForeignKey(sc => sc.StateId);
            });
            modelBuilder.Entity<ApplicationUser>()
            .HasMany(au => au.OrderHeaders)
            .WithOne(oh => oh.ApplicationUser)
            .HasForeignKey(oh => oh.ApplicationUserId);

            modelBuilder.Entity<OrderDetail>()
                         .HasOne(od => od.ShoeSizes)
                         .WithMany()
                         .HasForeignKey(od => od.ShoeSizeId);


            

        }
    }
}
