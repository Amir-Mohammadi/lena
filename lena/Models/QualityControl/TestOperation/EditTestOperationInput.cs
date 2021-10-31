using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class EditTestOperationInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Code { get; set; }
    public int QualityControlTestOperationId { get; set; }
    public byte[] QualityControlTestOperationRowVersion { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
