using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class DeleteStuffQualityControlTestOperationInput
  {
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public long QualityControlTestOperationQualityControlTestId { get; set; }
    public int QualityControlOperationTestOperationId { get; set; }
  }
}
