using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Common.Helpers;
using lena.Services.PrinterManagment;
//using lena.Services.PrinterManagment;

namespace lena.Services.Common
{
  public class PrinterFactory
  {
    private static Dictionary<string, Assembly> _cachedAssembly = new Dictionary<string, Assembly>();
    private static readonly object LockObject = new object();
    public static IPrinter GetPrinterModule(string moduleName)
    {
      throw new NotImplementedException();
    }


    public static IBarcodePrinter CreateBarcodePrinterModule(string moduleName, string printerName)
    {
      var currentAssLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      if (currentAssLocation == null)
        throw new Exception("Can not find sdk folder");

      var sdkFolder = currentAssLocation;
      var assemblies = Directory.GetFiles(sdkFolder).Where(x => Path.GetExtension(x) == ".dll" && x.ToLower().Contains("sdk")).ToArray();
      foreach (var assemblyPath in assemblies)
      {
        lock (LockObject)
        {
          Assembly assembly = _cachedAssembly.FirstOrDefault(x => x.Key == assemblyPath).Value;
          if (assembly == null)
          {
            var assName = AssemblyHelper.GetAssembleyName(assemblyPath);
            if (assName != null)
            {
              assembly = Assembly.Load(assName);
              if (assembly == null)
                throw new Exception($"Can not load assembly {assemblyPath}");
              _cachedAssembly.Add(assemblyPath, assembly);
            }
            else
              continue;
          }

          Type type = assembly.GetType(moduleName) ?? assembly.DefinedTypes.FirstOrDefault(x => x.Name == moduleName);

          if (type == null)
            throw new Exception($"Can not find '{moduleName}' type in '{assembly.FullName}'!");
          var module = Activator.CreateInstance(type, printerName);
          if (module is IBarcodePrinter)
            return module as IBarcodePrinter;
        }
      }

      throw new Exception($"Can not find sdk assembly that has '{moduleName}' type in app folder!");

    }
  }
}
