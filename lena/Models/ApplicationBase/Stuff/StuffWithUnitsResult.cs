using System.Collections.Generic;
using lena.Models.Unit;

using lena.Domains.Enums;
namespace lena.Models.Stuff
{
  public class StuffWithUnitsResult : StuffResult
  {
    public List<UnitResult> Units { get; set; }
  }
}