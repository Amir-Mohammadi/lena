using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.EmployeeCartable.Proposals
{
  public class ProposalResult
  {
    public int Id { get; set; }
    public string CurrentSituationDescription { get; set; }
    public string ProposalDescription { get; set; }
    public string ProposalEffect { get; set; }
    public int ProposalTypeId { get; set; }
    public string ProposalTypeName { get; set; }
    public bool IsOpen { get; set; }
    public bool? IsEffective { get; set; }
    public ProposalStatus Status { get; set; }
    public bool IsIncognitoUser { get; set; }
    public int? UserId { get; set; }
    public string UserFullName { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
