using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PriceInquiry
{
  public class PriceInquiryResult
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public byte CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public string Description { get; set; }
    public int? Number { get; set; }
    public double? Price { get; set; }
    public DateTime? PriceAnnunciationDateTime { get; set; }
    public DateTime CreateDateTime { get; set; }
  }
}
