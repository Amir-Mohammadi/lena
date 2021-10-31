////using lena.Services.Core.Foundation.Action;
//using Parlar.DAL;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using System.Linq;
using lena.Services.Internals.Exceptions;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals
{
  public partial class Automation
  {
    public IQueryable<Message> GetMessages(TValue<int> id = null, TValue<string> number = null, TValue<string> sendDate = null, TValue<int> senderUserId = null, TValue<string> title = null, TValue<string> content = null, TValue<bool> isSent = null, TValue<MessageAccessType> messageAccessType = null, TValue<bool> isArchive = null, TValue<bool> isDelete = null)
    {
      var isIdNull = (id == null);
      var isNumberNull = (number == null);
      var isSendDateNull = (sendDate == null);
      var isSenderUserIdNull = (senderUserId == null);
      var isTitleNull = (title == null);
      var isContentNull = (content == null);
      var isIsSentNull = (isSent == null);
      var isMessageAccessTypeNull = (messageAccessType == null);
      var isIsArchiveNull = (isArchive == null);
      var isIsDeleteNull = (isDelete == null);
      var messages = from item in repository.GetQuery<Message>()
                     where
                           (
                           (isIdNull || item.Id == id) &&
                           (isNumberNull || item.Number == number) &&
                           (isSendDateNull || item.SendDate == sendDate) &&
                           (isSenderUserIdNull || item.SenderUserId == senderUserId) &&
                           (isTitleNull || item.Title == title) &&
                           (isContentNull || item.Content == content) &&
                           (isIsSentNull || item.IsSent == isSent) &&
                           (isMessageAccessTypeNull || item.MessageAccessType == messageAccessType) &&
                           (isIsArchiveNull || item.IsArchive == isArchive) &&
                           (isIsDeleteNull || item.IsDelete == isDelete)
                           )
                     select item;
      return messages;
    }
    public Message AddMessage(string number, string sendDate, int senderUserId, string title, string content, bool isSent, MessageAccessType messageAccessType, bool isArchive, bool isDelete)
    {
      var message = new Message
      {
        Number = number,
        SendDate = sendDate,
        SenderUserId = senderUserId,
        IsSent = isSent,
        Title = title,
        IsArchive = isArchive,
        IsDelete = isDelete,
        MessageAccessType = messageAccessType,
        Content = content
      };
      if (GetMessages(number: message.Number).Any())
      {
        throw new MessageExistsException(number);
      }
      repository.Add(message);
      return message;
    }
    public Message GetMessage(int id)
    {
      var message = GetMessages(id: id).SingleOrDefault();
      if (message == null)
        throw new MesssageNotFoundException(id);
      return message;
    }
    Message EditMessage(byte[] rowVersion, int id, TValue<string> number = null, TValue<string> sendDate = null, TValue<int> senderUserId = null, TValue<string> title = null, TValue<string> content = null, TValue<bool> isSent = null, TValue<MessageAccessType> messageAccessType = null, TValue<bool> isArchive = null, TValue<bool> isDelete = null)
    {
      var message = GetMessage(id: id);
      if (number != null)
        message.Number = number;
      if (sendDate != null)
        message.SendDate = sendDate;
      if (senderUserId != null)
        message.SenderUserId = senderUserId;
      if (title != null)
        message.Title = title;
      if (content != null)
        message.Content = content;
      if (isSent != null)
        message.IsSent = isSent;
      if (messageAccessType != null)
        message.MessageAccessType = messageAccessType;
      if (isArchive != null)
        message.IsArchive = isArchive;
      if (isDelete != null)
        message.IsDelete = isDelete;
      repository.Update(message, rowVersion: rowVersion);
      return message;
    }
    public void DeleteMessage(int id)
    {
      var message = GetMessage(id: id);
      repository.Delete(message);
    }
  }
}