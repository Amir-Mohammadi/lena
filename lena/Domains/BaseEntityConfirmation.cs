using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BaseEntityConfirmation : BaseEntity, IEntity
  {
    protected internal BaseEntityConfirmation()
    {
    }
    public ConfirmationStatus Status { get; set; }
    public string ConfirmDescription { get; set; }
    public int BaseEntityConfirmTypeId { get; set; }
    public Nullable<int> ConfirmerId { get; set; }
    public int ConfirmingEntityId { get; set; }
    public DateTime ConfirmDateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual BaseEntityConfirmType BaseEntityConfirmType { get; set; }
    public virtual User Confirmer { get; set; }
    public virtual BaseEntity ConfirmingEntity { get; set; }
  }
}