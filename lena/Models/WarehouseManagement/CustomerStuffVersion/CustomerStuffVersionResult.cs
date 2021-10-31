using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class CustomerStuffVersionResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public bool IsPublish { get; set; }

    public int CustomerStuffId { get; set; }
    public string CustomerStuffCode { get; set; }
    public string CustomerStuffName { get; set; }
    public CustomerStuffType CustomerStuffType { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }

    public int CustomerId { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public byte[] RowVersion { get; set; }

  }
}