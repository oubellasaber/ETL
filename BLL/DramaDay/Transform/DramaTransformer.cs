using BLL.DramaDay.Extract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DramaDay.Transform
{
    public class DramaTransformer
    {
        public Drama Drama { get; set; }

        public DramaTransformer(Drama drama)
        {
            Drama = drama;
        }

        public void TransformDrama()
        {

        }

    }
}
