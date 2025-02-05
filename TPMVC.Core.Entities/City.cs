using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPMVC.Core.Entities
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public State States { get; set; }
        public Country Country { get; set; }
    }
}
