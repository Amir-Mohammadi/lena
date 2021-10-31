using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class Proposal : IEntity
  {
    public int Id { get; set; }
    public string CurrentSituationDescription { get; set; }
    public string ProposalDescription { get; set; }
    public string ProposalEffect { get; set; }
    public int ProposalTypeId { get; set; }
    public virtual ProposalType ProposalType { get; set; }
    public bool IsOpen { get; set; }
    public bool? IsEffective { get; set; }
    public ProposalStatus Status { get; set; }
    public bool IsIncognitoUser { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ProposalReviewCommittee> ReviewCommittees { get; set; }
    public virtual ICollection<ProposalQAReview> QAReviews { get; set; }
  }
}
