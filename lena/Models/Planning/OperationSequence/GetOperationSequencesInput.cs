using lena.Domains.Enums;
namespace lena.Models.Planning.OperationSequence
{
  public class GetOperationSequencesInput
  {
    public int? WorkplanStepId { get; set; }
    public string Serial { get; set; }
  }
}
