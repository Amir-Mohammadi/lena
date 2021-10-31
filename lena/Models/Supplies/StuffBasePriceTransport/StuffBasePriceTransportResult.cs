using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Supplies.StuffBasePriceTransport
{
  public class StuffBasePriceTransportResult
  {
    public int Id;
    public StuffBasePriceTransportType? Type = StuffBasePriceTransportType.Percentage;
    public StuffBasePriceTransportComputeType? ComputeType = StuffBasePriceTransportComputeType.Weighing;
    public double? Percent;
    public Byte[] RowVersion;
  }
}