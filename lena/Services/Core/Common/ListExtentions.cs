using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Common
{
  public static class ListExtentions
  {
    public static void AddRange<T>(this IList<T> list, IList<T> items)
    {
      foreach (var item in items)
      {
        list.Add(item);
      }
    }

    public static void AddRange<T>(this ICollection<T> list, IList<T> items)
    {
      foreach (var item in items)
      {
        list.Add(item);
      }
    }
  }
}
