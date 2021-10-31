using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Common
{
  public static class ImageHelper
  {
    public static Image LoadImage(this byte[] byteArray)
    {
      MemoryStream ms = new MemoryStream(byteArray);
      Image image = Image.FromStream(ms);
      return image;
    }
  }
}
