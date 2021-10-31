using lena.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class AddCargoItemLogInput
  {
    public int ModifierUserId { get; set; }
    public DateTime ModifyDateTime { get; set; }
    public int CargoItemId { get; set; }
    public string CargoItemCode { get; set; }
    public bool IsDelete { get; set; }
    public double NewCargoItemQty { get; set; }
    public double OldCargoItemQty { get; set; }
    public CargoItemLogStatus cargoItemLogStatus { get; set; }
  }
}

