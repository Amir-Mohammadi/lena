using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region Message
  public class MesssageNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public MesssageNotFoundException(int messageId)
    {
      Id = messageId;
    }
  }

  public class MessageExistsException : InternalServiceException
  {
    public string Number { get; set; }

    public MessageExistsException(string number)
    {
      Number = number;
    }
  }
  #endregion
}
