using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMVC.Core.Entities
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public ICollection<State> States { get; set; } = new List<State>();
        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
