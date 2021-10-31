using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace lena.Services.Common.Helpers
{
  public class AssemblyHelper
  {
    public static bool CheckFileIsAssembely(string filePath)
    {
      try
      {
        var ass = System.Reflection.AssemblyName.GetAssemblyName(filePath);
        return true;
      }
      catch (System.BadImageFormatException)
      {
        return false;
      }
    }
    public static AssemblyName GetAssembleyName(string filePath)
    {
      try
      {
        return System.Reflection.AssemblyName.GetAssemblyName(filePath);
      }
      catch (System.BadImageFormatException)
      {
        return null;
      }
    }
  }
}