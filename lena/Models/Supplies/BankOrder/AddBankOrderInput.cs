using lena.Domains.Enums;
using lena.Models.Supplies.BankOrderDetail;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrder
{
  public class AddBankOrderInput
  {
    public string OrderNumber { get; set; }
    public string FolderCode { get; set; }
    public int StuffPriority { get; set; }
    public DateTime RegisterDate { get; set; }
    public DateTime ExpireDate { get; set; }
    public int ProviderId { get; set; }
    public byte CurrencyId { get; set; }
    public double TransferCost { get; set; }
    public double FOB { get; set; }
    public double TotalAmount { get; set; }
    public BankOrderStatus Status { get; set; }
    public BankOrderType BankOrderType { get; set; }
    public byte BankId { get; set; }
    public short CustomhouseId { get; set; }
    public byte CountryId { get; set; }
    public AddBankOrderDetailInput[] AddBankOrderDetails { get; set; }
    public short BankOrderContractTypeId { get; set; }
    public string Description { get; set; }
  }
}
