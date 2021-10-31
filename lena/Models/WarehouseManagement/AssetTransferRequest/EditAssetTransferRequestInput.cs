using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement
{
  public class EditAssetTransferRequestInput
  {
    public int Id { get; set; }
    public AssetTransferRequestStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
