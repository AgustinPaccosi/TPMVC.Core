using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPMVC.Core.Data.Interfaces;
using TPMVC.Core.Entities;

namespace TPMVC.Core.Data.Repositories
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly Context _context;

        public ApplicationUserRepository(Context context): base(context)
        {

            _context = context ?? throw new ArgumentNullException(nameof(context));

        }
        public void Update(ApplicationUser applicationUser)
        {
           _context.ApplicationUsers.Update(applicationUser);
        }
    }
}
