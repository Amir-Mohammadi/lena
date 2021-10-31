using lena.Domains.Enums;
using lena.Models.Supplies.CargoItem;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Cargo
{
  public class EditCargoItemInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
    public DateTime EstimateDateTime { get; set; }
    public DateTime CargoItemDateTime { get; set; }
    public int HowToBuyId { get; set; }
    public AddCargoItemDetailInput[] AddCargoItemDetails { get; set; }
    public EditCargoItemDetailInput[] EditCargoItemDetails { get; set; }
    public DeleteCargoItemDetailInput[] DeleteCargoItemDetails { get; set; }
    public BuyingProcess BuyingProcess { get; set; }
    public string ForwarderFileKey { get; set; }
    public int? ProviderId { get; set; }
  }
}
