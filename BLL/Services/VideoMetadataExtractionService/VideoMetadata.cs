using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.VideoMetadataExtractionService
{
    public class VideoMetadata
    {
        public int Resolution { get; set; }
        public float Bitrate { get; set; }
        public string Encoding { get; set; }
        public int ColorDepth { get; set; }
        public float FrameRate { get; set; }
        public string AspectRatio { get; set; }
        public string AudioCodec { get; set; }
        public int AudioChannels { get; set; }
        public float AudioBitrate { get; set; }

        public override string ToString()
        {
            return $@"
Video Metadata:
---------------
Resolution:        {Resolution}p
Bitrate:           {Bitrate} Mbps
Encoding:          {Encoding}
Color Depth:       {ColorDepth} bits
Frame Rate:        {FrameRate} fps
Aspect Ratio:      {AspectRatio}
Audio Codec:       {AudioCodec}
Audio Channels:    {AudioChannels}
Audio Bitrate:     {AudioBitrate} kbps";
        }
    }
}
