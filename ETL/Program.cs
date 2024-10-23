using BLL.DramaDay.Extract.Models;
using BLL.Services.DownloadLinkExtractionService.Impl;
using BLL.Services.VideoMetadataExtractionService;

var ext = new DataNodesDownloadLinkExtractor();
var dl = await ext.ExtractDownloadFileAsync("https://datanodes.to/uqf3ltltt257");
Console.WriteLine(dl);


VideoMetadata videoMetadata = await (new DataNodesVideoMetadataExtractor(dl).ExtractVideoMetadataAsync());

Console.WriteLine(videoMetadata.ToString());

//using BLL.DramaDay.Extract;
//using BLL.DramaDay.Extract.Models;
//using BLL.Services.DownloadLinkExtractionService.Impl;
//using BLL.Services.VideoMetadataExtractionService;
//using FileNameExtractors.FilenameExtrctors.Impl;
//using Newtonsoft.Json;

//var ext = await DramaExtractor.BuildDramaDataExtractorAsync("https://dramaday.me/dear-hyeri/");

//Drama drama = ext.drama;
//int i = 1;
//foreach (var stdEp in drama.StandardEpsDictionary["Ohhhh"])
//{

//    foreach (var version in stdEp.EpVersions)
//    {
//        foreach (var host in version.Hosts)
//        {
//            if (host.UnresolvedUrl.Contains("datanodes"))
//            {
//                version.filename = await (new DatenodesFilenameExtractor()).ExtractFilenameAsync(host.UnresolvedUrl);
//                version.videoMetadata = await (new DataNodesVideoMetadataExtractor(await (new DataNodesDownloadLinkExtractor()).ExtractDownloadFileAsync(host.UnresolvedUrl))).ExtractVideoMetadataAsync();
//                break;
//            }
//        }
//        Console.WriteLine(i++);
//    }
//}

//string jsonDrama = JsonConvert.SerializeObject(drama, Formatting.Indented);

//// Define the file path where you want to save the JSON
//string filePath = @"C:\Users\Lenovo\Desktop\subs\dramaData.json";

//Console.WriteLine(jsonDrama);

//// Save the JSON to a file
//await File.WriteAllTextAsync(filePath, jsonDrama);

//Console.WriteLine($"JSON data saved to: {filePath}");