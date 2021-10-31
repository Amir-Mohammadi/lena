using lena.Models.Supplies.CargoItem;
using System;
using lena.Domains.Enums;


using lena.Domains.Enums;
namespace lena.Models.Supplies.Cargo
{
  public class AddCargoInput
  {
    public AddCargoItemDetailInput[] AddCargoItemDetails { get; set; }
    public string Description { get; set; }
    public DateTime EstimateDateTime { get; set; }
    public DateTime CargoItemDateTime { get; set; }
    public short HowToBuyId { get; set; }
    public BuyingProcess BuyingProcess { get; set; }
    public int? ForwarderId { get; set; }
    public string ForwarderFileKey { get; set; }
    public bool IsSaveAutoLading { get; set; }
  }
}
