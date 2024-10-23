using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.VideoMetadataExtractionService
{
    public class DataNodesVideoMetadataExtractor
    {
        private readonly string _url;

        public DataNodesVideoMetadataExtractor(string url)
        {
            _url = url;
        }

        public async Task<VideoMetadata> ExtractVideoMetadataAsync()
        {
            string scriptPath = @"C:\Users\Lenovo\get_video_info.ps1";

            string arguments = @$"-ExecutionPolicy Bypass -File ""{scriptPath}"" -url ""{_url}""";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (Process process = Process.Start(psi))
                {
                    string output = await process.StandardOutput.ReadToEndAsync();
                    string error = await process.StandardError.ReadToEndAsync();

                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception($"Error executing PowerShell script: {error}");
                    }

                    var videoMetadata = JsonConvert.DeserializeObject<VideoMetadata>(output);
                    return videoMetadata;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }
    }
}
