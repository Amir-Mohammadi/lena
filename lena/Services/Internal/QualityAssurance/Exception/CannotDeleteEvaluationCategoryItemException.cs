using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance.Exception
{
  public class CannotDeleteEvaluationCategoryItemException : InternalServiceException
  {
    public int Id { get; set; }
    public CannotDeleteEvaluationCategoryItemException(int id)
    {
      this.Id = id;
    }
  }
}
