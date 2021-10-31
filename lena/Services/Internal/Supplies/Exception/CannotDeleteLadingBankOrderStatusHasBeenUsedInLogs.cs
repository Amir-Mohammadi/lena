using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public class CannotDeleteLadingBankOrderStatusHasBeenUsedInLogs : InternalServiceException
  {

    public int Id { get; set; }

    public string Name { get; set; }
    public CannotDeleteLadingBankOrderStatusHasBeenUsedInLogs(int id, string name)
    {
      Id = id;
      Name = name;
    }
  }
}
