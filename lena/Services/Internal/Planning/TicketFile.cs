using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Planning.TicketSoftware;
using System.Runtime.Serialization;
using System.Collections.Generic;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region  AddTicketFile
    public TicketFile AddTicketFile(
    int ticketSoftWareId,
    Guid documentId
    )
    {

      var ticketFiles = repository.Create<TicketFile>();
      ticketFiles.TicketSoftWareId = ticketSoftWareId;
      ticketFiles.DocumentId = documentId;
      repository.Add(ticketFiles);
      return ticketFiles;
    }
    #endregion

    #region Get
    public TicketSoftware GetTicketFile(int id) => GetTicketFile(selector: e => e, id: id);
    public TResult GetTicketFile<TResult>(
        Expression<Func<TicketSoftware, TResult>> selector,
        int id)
    {

      var ticketSoftware = GetTicketSoftwares(selector: selector,
                id: id).FirstOrDefault();
      return ticketSoftware;
    }
    #endregion
  }
}
