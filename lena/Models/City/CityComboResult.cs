using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.City
{
  public class CityComboResult
  {
    public short Id { get; set; }
    public byte CountryId { get; set; }
    public string CityTitle { get; set; }
  }
}
