using FileNameExtractors.FilenameExtrctors.Impl;

namespace BLL.Services.FileNameExtractionService
{
    public class FilenameExtractorFactory
    {
        public IFilenameExtractor GetFilenameExtractor(string domain, string downloadUrl)
        {
            switch (domain)
            {
                case "datanodes.to":
                    return new DatenodesFilenameExtractor();

                case "gdbot.site":
                    return new GdbotFilenameExtractor();

                case "pixeldrain.com":
                    return new PixeldrainFilenameExtractor();

                case "gofile.io":
                    return new GofileFilenameExtractor();

                case "buzzheavier.com":
                    return new BuzzheavierFilenameExtractor();

                case "mega.nz":
                    return new MegaFilenameExtractor();

                case "send.cm":
                    return new SendFilenameExtractor();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
