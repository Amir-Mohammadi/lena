using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class StuffQualityControlTestImportanceDegreeResult
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public int QualityControlImportanceDegreeTestImportanceDegreeId { get; set; }
    public long QualityControlTestImportanceDegreeQualityControlTestId { get; set; }


    public byte[] RowVersion { get; set; }
  }
}
