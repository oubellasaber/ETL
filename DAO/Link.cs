namespace DTO
{
    public class Link
    {
        public int EpLinkId { get; set; }
        public EpVersion EpVersion { get; set; }
        public int HostId { get; set; }
        public int QualityId { get; set; }
        public float BitRate { get; set; }
        public string Encoding { get; set; }
        public int ColorDepth { get; set; }
        public float FrameRate { get; set; }
        public string AspectRatio { get; set; }
        public string AudioCodec { get; set; }
        public int AudioChannels { get; set; }
        public float AudioBitrate { get; set; }
        public string ShortURL { get; set; }
        public string? URL { get; set; }
    }
}
