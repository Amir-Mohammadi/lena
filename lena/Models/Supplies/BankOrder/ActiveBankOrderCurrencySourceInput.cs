using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrder
{
  public class ActiveBankOrderCurrencySourceInput
  {
    public int BankOrderId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
