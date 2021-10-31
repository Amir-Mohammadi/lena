using lena.Models.SaleManagement.PriceAnnunciationItem;
using System.Collections.Generic;
using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PriceAnnunciation
{
  public class PriceAnnunciationResult
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public System.DateTime FromDate { get; set; }
    public System.DateTime? ToDate { get; set; }
    public string RegisterarUserName { get; set; }
    public System.DateTime RegisterDateTime { get; set; }
    public IEnumerable<PriceAnnunciationItemResult> PriceAnnunciationItems { get; set; }
    public string CooperatorName { get; set; }
    public string Description { get; set; }
    public int CooperatorId { get; set; }
    public Nullable<System.Guid> DocumentId { get; set; }
    public PriceAnnunciationStatus Status { get; set; }
  }
}
