using BLL.Services.VideoMetadataExtractionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DramaDay.Extract.Models
{
    public class StandardEp
    {
        public int Number { get; set; }
        public List<EpVersion> EpVersions { get; set; }
    }
}
