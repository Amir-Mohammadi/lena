using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class QualityControlTestImportanceDegreeResult
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public int TestImportanceDegreeId { get; set; }
    public long QualityControlTestId { get; set; }
    public string QualityControlTestName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
