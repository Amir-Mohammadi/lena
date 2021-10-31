using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class BaseEntity : IEntity
  {
    protected internal BaseEntity()
    {
      this.ScrumEntities = new HashSet<ScrumEntity>();
      this.BaseEntityConfirmations = new HashSet<BaseEntityConfirmation>();
      this.BaseEntityDocuments = new HashSet<BaseEntityDocument>();
      this.BaseEntityLogs = new HashSet<BaseEntityLog>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string Description { get; set; }
    public string EntityDescription { get; set; }
    public int UserId { get; set; }
    public int? TransactionBatchId { get; set; }
    public int? FinancialTransactionBatchId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ScrumEntity> ScrumEntities { get; set; }
    public virtual TransactionBatch TransactionBatch { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<BaseEntityConfirmation> BaseEntityConfirmations { get; set; }
    public virtual ICollection<BaseEntityDocument> BaseEntityDocuments { get; set; }
    public virtual ICollection<BaseEntityLog> BaseEntityLogs { get; set; }
    public virtual FinancialTransactionBatch FinancialTransactionBatch { get; set; }
  }
}