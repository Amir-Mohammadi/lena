using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.ExitReceiptDeleteRequest
{
  public class AcceptExitReceiptDeleteRequestInput
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
