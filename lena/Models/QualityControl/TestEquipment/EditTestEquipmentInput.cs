using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class EditTestEquipmentInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int QualityControlTestEquipmentId { get; set; }
    public byte[] QualityControlTestEquipmentRowVersion { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
