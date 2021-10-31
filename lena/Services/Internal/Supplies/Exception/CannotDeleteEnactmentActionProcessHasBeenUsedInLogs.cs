using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class CannotDeleteEnactmentActionProcessHasBeenUsedInLogs : InternalServiceException
  {
    public int Id { get; set; }
    public string Name { get; set; }

    public CannotDeleteEnactmentActionProcessHasBeenUsedInLogs(int id, string name)
    {
      this.Id = id;
      this.Name = name;
    }
  }
}
