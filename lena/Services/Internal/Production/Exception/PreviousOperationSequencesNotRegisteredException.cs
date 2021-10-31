using System.Linq;
using lena.Services.Core.Foundation;
using lena.Models.Planning.OperationSequence;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class PreviousOperationSequencesNotRegisteredException : InternalServiceException
  {
    public OperationSequenceResult[] OperationSequences { get; }

    public PreviousOperationSequencesNotRegisteredException(IQueryable<OperationSequenceResult> operationSequences)
    {
      this.OperationSequences = operationSequences.ToArray();
    }
  }
}
