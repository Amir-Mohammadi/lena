using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffBasePriceTransport
{
  public class AddStuffBasePriceTransportInput
  {
    public StuffBasePriceTransportType Type = StuffBasePriceTransportType.Percentage;
    public StuffBasePriceTransportComputeType ComputeType = StuffBasePriceTransportComputeType.Weighing;
    public double Percent;
    public double Price;

  }
}