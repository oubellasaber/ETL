using BLL.Services.FileNameExtractionService;

namespace FilenameExtractors
{
    public class FilenameExtractor
    {
        private static readonly string[] _supportedDownloadServices =
        {
            "pixeldrain.com",
            "datanodes.to",
            "gdbot.site",
            "gofile.io",
            "buzzheavier.com",
            "mega.nz",
            "send.cm"
        };

        public string DownloadUrl { get; private set; }
        public string? Domain { get; private set; }
        private IFilenameExtractor _extr;

        private FilenameExtractor(string downloadUrl, IFilenameExtractor filenameExtractor)
        {
            DownloadUrl = downloadUrl;
            _extr = filenameExtractor;
        }

        public static FilenameExtractor? Instance(string downloadUrl)
        {
            if (!IsDownloadUrlSupported(ref downloadUrl, out string? domain))
                return null;

            IFilenameExtractor filenameExtractor = (new FilenameExtractorFactory()).GetFilenameExtractor(domain!, downloadUrl);

            return new FilenameExtractor(downloadUrl, filenameExtractor);
        }

        public string? ExtractFilename() => _extr.ExtractFilename(DownloadUrl);

        private static bool IsDownloadUrlSupported(ref string downloadUrl, out string? domain)
        {
            domain = ExtractDomain(downloadUrl);

            if (string.IsNullOrEmpty(domain) || !_supportedDownloadServices.Contains(domain))
                return false;

            return true;
        }

        private static string? ExtractDomain(string downloadUrl)
        {
            // Add "https://" if the scheme is missing
            if (!downloadUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                downloadUrl = "https://" + downloadUrl;
            }

            // Validate and create URI
            if (Uri.TryCreate(downloadUrl, UriKind.Absolute, out Uri? uri))
            {
                string host = uri.Host;

                if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
                {
                    host = host.Substring(4); // Remove the first 4 characters ('www.')
                }

                return host; // Return normalized domain
            }

            // Return null if the URL is invalid
            return null;
        }
    }
}
