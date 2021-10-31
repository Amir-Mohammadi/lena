using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class AddQualityControlTestImportanceDegreeInput
  {
    public long QualityControlTestId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
  }
}
