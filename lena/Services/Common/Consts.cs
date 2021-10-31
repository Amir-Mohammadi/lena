using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Common
{
  public class Consts
  {
    public static string[] SecurityCheckIgnoreList = new[] {
            "CheckPermission",
            "Login",
            "Logout",
            "CheckLogin",
            "GetLoginedUser",
            "GetAssemblyVersion",
            "GetStartTime",
            "GetBuildDate" };
  }
}
