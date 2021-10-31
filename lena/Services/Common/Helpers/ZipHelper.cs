using lena.Domains;
using lena.Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Common.Helpers
{
  public static class ZipHelper
  {
    public static FileResult GetDocumentsZipFile(Document[] docs, string fileName = null)
    {
      var memoryStream = new MemoryStream();
      var zipFile = new ZipArchive(memoryStream, ZipArchiveMode.Create);
      foreach (var item in docs)
      {
        var entryName = item.Name.ToLower().Replace(item.Id + "_", "").Replace(item.Id.ToString(), "").Replace(item.FileType, "") + item.FileType;
        var entry = zipFile.CreateEntry(entryName, CompressionLevel.Optimal);
        using (var entryStream = entry.Open())
        {
          entryStream.Write(item.FileStream, 0, item.FileStream.Length);
          entryStream.Close();
        }
      }

      zipFile.Dispose();
      var result = new FileResult();
      result.FileContent = memoryStream.ToArray();
      result.FileName = string.IsNullOrEmpty(fileName) ? "Documents.zip" : $"{fileName}.zip";
      return result;
    }
  }
}
