////using lena.Services.Core.Foundation.Action;
using lena.Services.Internals.Exceptions;
//using Parlar.DAL;
using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals
{
  public partial class Automation
  {
    public IQueryable<Attachment> GetAttachments(TValue<int> id = null, TValue<int> messageId = null, TValue<string> fileName = null, TValue<double> size = null, TValue<string> format = null, TValue<Byte[]> fileContent = null, TValue<string> description = null)
    {
      var isIdNull = (id == null);
      var isMessageIdNull = (messageId == null);
      var isFileNameNull = (fileName == null);
      var isSizeNull = (size == null);
      var isFormatNull = (format == null);
      var isFileContentNull = (fileContent == null);
      var isDescriptionNull = (description == null);
      var attachment = from item in repository.GetQuery<Attachment>()
                       where
                             (
                             (isIdNull || item.Id == id) &&
                             (isMessageIdNull || item.MessageId == messageId) &&
                             (isFileNameNull || item.FileName == fileName) &&
                             (isSizeNull || item.Size == size) &&
                             (isFormatNull || item.Format == format) &&
                             (isFileContentNull || item.FileContent == fileContent.Value) &&
                             (isDescriptionNull || item.Description == description)
                             )
                       select item;
      return attachment;
    }
    public Attachment GetAttachment(int id)
    {
      var attachment = GetAttachments(id: id).SingleOrDefault();
      if (attachment == null)
        throw new AttachmentNotFoundException(id);
      return attachment;
    }
    public Attachment EditAttachment(byte[] rowVersion, int id, TValue<int> messageId = null, TValue<string> fileName = null, TValue<double> size = null, TValue<string> format = null, TValue<Byte[]> fileContent = null, TValue<string> description = null)
    {
      var attachment = GetAttachment(id: id);
      if (messageId != null)
        attachment.MessageId = messageId;
      if (fileName != null)
        attachment.FileName = fileName;
      if (size != null)
        attachment.Size = size;
      if (format != null)
        attachment.Format = format;
      if (fileContent != null)
        attachment.FileContent = fileContent;
      if (description != null)
        attachment.Description = description;
      repository.Update(attachment, rowVersion: rowVersion);
      return attachment;
    }
    public Attachment AddAttachment(int messageId, string fileName, double size, string format, Byte[] fileContent, string description)
    {
      var attachment = new Attachment
      {
        MessageId = messageId,
        FileName = fileName,
        Size = size,
        Format = format,
        Description = description,
        FileContent = fileContent,
      };
      repository.Add(attachment);
      return attachment;
    }
    public void DeleteAttachment(int id)
    {
      var attachment = GetAttachment(id: id);
      repository.Delete(attachment);
    }
  }
}