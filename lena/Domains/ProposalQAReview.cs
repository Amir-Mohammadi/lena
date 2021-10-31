using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class ProposalQAReview : IEntity
  {
    public int Id { get; set; }
    public string ReviewResult { get; set; }
    public DateTime ReviewDateTime { get; set; }
    public int ResponsibleUserId { get; set; }
    public User ResponsibleUser { get; set; }
    public int ProposalId { get; set; }
    public virtual Proposal Proposal { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
