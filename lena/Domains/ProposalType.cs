using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class ProposalType : IEntity
  {
    public ProposalType()
    {
      this.Proposals = new HashSet<Proposal>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Proposal> Proposals { get; set; }
  }
}
