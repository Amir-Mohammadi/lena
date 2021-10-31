using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.Supplies.PriceInquiry
{
  public class AddPriceInquiryInput
  {
    public int StuffId { get; set; }
    public int CooperatorId { get; set; }
    public byte CurrencyId { get; set; }
    public int? Number { get; set; }
    public double? Price { get; set; }
    public DateTime? PriceAnnunciationDateTime { get; set; }
    public string Description { get; set; }
  }
}
