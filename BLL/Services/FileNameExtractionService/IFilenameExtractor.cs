using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.FileNameExtractionService
{
    public interface IFilenameExtractor
    {
        public Task<string?> ExtractFilenameAsync(string downloadUrl);
    }
}
