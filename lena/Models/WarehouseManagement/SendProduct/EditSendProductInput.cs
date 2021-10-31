using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SendProduct
{
  public class EditSendProductInput
  {
    public int Id { get; set; }
    public int? PriceAnnunciationItemId { get; set; }
  }
}
