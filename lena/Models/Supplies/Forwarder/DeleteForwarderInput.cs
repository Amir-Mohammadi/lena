using lena.Domains.Enums;
namespace lena.Models.Supplies.Forwarder
{
  public class DeleteForwarderInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
