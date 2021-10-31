using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Common.Helpers
{
  public class AssetsHelper
  {
    public static readonly string[] FontTypes = { ".ttf" };
    public static string GetFontDirectory()
    {
      return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets", "Fonts");
    }

    public static string[] GetAllFonts()
    {
      var dir = GetFontDirectory();
      var dirs = Directory.GetFiles(dir).Select(x => new DirectoryInfo(x));
      var files = dirs.Where(x => FontTypes.Contains(x.Extension));
      return files.Select(x => x.FullName).ToArray();
    }
  }
}
