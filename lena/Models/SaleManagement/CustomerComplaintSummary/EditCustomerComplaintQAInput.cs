using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaintSummary
{
  public class EditCustomerComplaintQAInput
  {
    public int Id { get; set; }
    public ComplaintStatus Status { get; set; }
    public string QAOpinion { get; set; }
    public string CorrectiveAction { get; set; }
    public string FileKey { get; set; }
  }
}