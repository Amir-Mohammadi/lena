using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class EditTestImportanceDegreeInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int QualityControlTestImportanceDegreeId { get; set; }
    public byte[] QualityControlTestImportanceDegreeRowVersion { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
