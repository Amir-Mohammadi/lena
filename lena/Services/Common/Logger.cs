using lena.Services.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Common
{
  public class Logger
  {

    private string logFileDir = null;
    private object _fileLock = new object();

    private static Logger _logger = null;

    public Logger()
    {
      var entryAssembly = Assembly.GetExecutingAssembly();
      var appDir = Path.GetDirectoryName(entryAssembly.Location);
      logFileDir = Path.Combine(appDir, "log.txt");
    }

    public static Logger Instance
    {
      get
      {
        if (_logger == null)
          _logger = new Logger();
        return _logger;
      }
    }
    public void Log(Exception ex)
    {
      StringBuilder sb = new StringBuilder();
      var loginInfo = App.Providers.Security.CurrentLoginData;
      sb.AppendLine($"UserId: {loginInfo.UserId}\t User Name: {loginInfo.UserFullName}\t Date: {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}");
      sb.AppendLine($"Error Message: {ex.GetInnerMessage()}");
      sb.AppendLine($"Stack Trace: {ex.StackTrace}");
      sb.AppendLine("------------------------------------------------------------------------------");
      Log(sb.ToString());
    }

    public void Log(string logText)
    {
      lock (_fileLock)
      {
        var streamWriter = new StreamWriter(logFileDir, true);
        streamWriter.WriteLine(logText);
        streamWriter.Close();
      }
    }
  }
}
