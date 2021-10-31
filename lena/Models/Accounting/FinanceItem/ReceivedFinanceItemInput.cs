using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinanceItem
{
  public class ReceivedFinanceItemInput
  {
    public int FinanceItemId { get; set; }
    public DateTime ReceivedDateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
