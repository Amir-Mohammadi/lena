using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class AddTestEquipmentInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

  }
}
