using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class DetailedCodeConfirmationRequest : IEntity
  {
    protected internal DetailedCodeConfirmationRequest()
    {
    }
    public int Id { get; set; }
    public int? CooperatorId { get; set; }
    public int? ProductionLineId { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public int? ConfirmationUserId { get; set; }
    public DateTime? ConfirmationDateTime { get; set; }
    public DetailedCodeEntityType DetailedCodeEntityType { get; set; }
    public DetailedCodeRequestType DetailedCodeRequestType { get; set; }
    public DetailCodeConfirmationStatus Status { get; set; }
    public string Description { get; set; }
    public string DetailedCode { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual User DetailedCodeConfirmerUser { get; set; }
    public virtual Cooperator Cooperator { get; set; }
    public virtual ProductionLine ProductionLine { get; set; }
  }
}
