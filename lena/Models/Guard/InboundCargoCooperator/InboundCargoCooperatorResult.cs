using lena.Domains.Enums;
namespace lena.Models.Guard.InboundCargoCooperator
{
  public class InboundCargoCooperatorResult
  {
    public int CooperatorId { get; set; }
    public int InboundCargoId { get; set; }
    public string CooperatorName { get; set; }
    public string CooperatorCode { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
