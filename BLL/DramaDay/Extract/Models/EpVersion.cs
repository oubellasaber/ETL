using BLL.Services.VideoMetadataExtractionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DramaDay.Extract.Models
{
    public class EpVersion
    {
        public string RawQualityFormat { get; set; }
        public VideoMetadata videoMetadata { get; set; }
        public string filename { get; set; }
        public List<Host> Hosts { get; set; }
    }
}
