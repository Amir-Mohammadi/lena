using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Internals.Terminal.Exception;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Terminal
{
  public class Command
  {
    #region init

    private readonly Dictionary<string, string> _commandDict;

    private Command(Dictionary<string, string> dict)
    {
      _commandDict = dict;
    }

    #endregion



    public static Command From(Dictionary<string, string> commandDict)
    {
      var valid = false;
      foreach (var method in Scheme.DefinedMethods)
      {
        var q = from value in commandDict.Keys
                join known in method
                    on value equals known
                select value;
        if (q.Count() != method.Length) continue;
        valid = true;
        break;
      }

      if (!valid)
      {
        throw new InvalidTerminalCommand();
      }

      return new Command(commandDict);
    }

    public string Name
    {
      get
      {
        var method = _commandDict.Keys.FirstOrDefault(key => key.Contains("@"));
        return method ?? "";
      }
    }

    public bool IsA(Scheme.Definition<string> method)
    {
      return _commandDict.Keys.FirstOrDefault(a => a == method.Key) != null;
    }

    public T Arg<T>(Scheme.Definition<T> def)
    {
      var value = _commandDict[def.Key];

      object returnValue = null;

      if (def.Type == typeof(int))
      {
        returnValue = int.Parse(value);
      }
      else if (def.Type == typeof(long))
      {
        returnValue = long.Parse(value);
      }
      else if (def.Type == typeof(string))
      {
        returnValue = value;
      }
      else if (def.Type == typeof(bool))
      {
        returnValue = bool.Parse(value);
      }
      else if (def.Type == typeof(int[]))
      {
        var explode = value.Split(',');
        var items = new int[explode.Length];
        for (var i = 0; i < explode.Length; i++)
        {
          items[i] = int.Parse(explode[i]);
        }
      }
      else
      {
        throw new TerminalArgumentNotfoundException();
      }


      return (T)Convert.ChangeType(returnValue, typeof(T));
    }


  }

}