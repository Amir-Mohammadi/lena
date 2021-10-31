using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class DeleteStuffQualityControlTestEquipmentInput
  {
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public long QualityControlTestEquipmentQualityControlTestId { get; set; }
    public int QualityControlEquipmentTestEquipmentId { get; set; }
  }
}
