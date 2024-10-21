using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DramaDay.Extract.Models
{
    public class RangedEps
    {
        public int FirstEp { get; set; }
        public int LastEp { get; set; }
        public List<Host> Hosts { get; set; }
    }
}
