using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class EpVersion
    {
        public int EpVersionId { get; set; }
        public int SerieId { get; set; }
        public int EpNumber { get; set; }
        public int? NetworkId { get; set; }
        public int? HostedFileReleaseGroup { get; set; }
        public int VideoSourceId { get; set; }
        public TimeSpan RunTime { get; set; }
        public int FileSize { get; set; }
        public string FileName { get; set; }
    }
}