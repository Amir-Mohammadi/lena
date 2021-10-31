using System;
//using System.Data.Entity.Migrations.Model;
using System.Globalization;
using System.IO;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.ApplicationBase.Calendar;
using lena.Models.UserManagement.SecurityAction;
using Calendar = lena.Domains.Calendar;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    public Document GetDocument(Guid id)
    {

      var document = GetDocuments(id: id)


                .FirstOrDefault();
      if (document == null)
        throw new DocumentNotFoundException(id);
      return document;
    }
    public Document GetDownloadableDocument(Guid id)
    {

      #region GetDocument
      var document = GetDocument(id: id);
      #endregion
      #region GetBillOfMaterialDocument
      var bomDocument = App.Internals.Planning.GetBillOfMaterialDocuments(
          documentId: document.Id,
          isDelete: false)


          .FirstOrDefault();

      #endregion
      #region CheckAccess if Document is BillOfMaterialDocument
      //todo koohgard uncomment after fix token
      //if (bomDocument != null)
      //{
      //    var actionParameter = new ActionParameterInput();
      //    actionParameter.Key = "DocumentTypeId";
      //    actionParameter.Value = bomDocument.BillOfMaterialDocumentTypeId.ToString();
      //    var checkPermissionResult = App.Internals.UserManagement.CheckPermission(
      //            actionName: Models.StaticData.StaticActionName.DownloadBillOfMaterialDocumentAction,
      //            actionParameters: new[] { actionParameter })
      //        
      //;
      //    if (checkPermissionResult == AccessType.Denied)
      //        throw new DownloadBillOfMaterialDocumentTypeAccessDeniedExecption(
      //            documentTypeId: bomDocument.BillOfMaterialDocumentTypeId);
      //}
      #endregion
      return document;

    }

    public IQueryable<Document> GetDocumentInfo(TValue<Guid[]> documentIds = null)
    {


      var query = repository.GetQuery<Document>();
      if (documentIds != null)
        query = query.Where(i => documentIds.Value.Contains(i.Id));
      return query;
    }
    public IQueryable<Document> GetDocuments(TValue<Guid> id = null)
    {

      var isIdNull = id == null;
      var query = from document in repository.GetQuery<Document>()
                  where (isIdNull || document.Id == id)
                  select document;
      return query;
    }
    public IQueryable<Document> GetDocumentResults(TValue<Guid> id = null)
    {


      var query = repository.GetQuery<Document>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      return query;
    }
    public Document AddDocument(string name, byte[] fileStream)
    {

      var document = repository.Create<Document>();
      document.Id = Guid.NewGuid();
      document.Name = name;
      document.FileType = Path.GetExtension(name);
      document.FileSize = fileStream.Length; // byte
      document.FileStream = fileStream;
      document.CreationTime = DateTime.UtcNow;
      repository.Add(document);
      return document;
    }
    public void DeleteDocument(Guid id)
    {

      var document = GetDocument(id: id);
      repository.Delete(document);
    }

    public Document CreateDocument()
    {

      var document = repository.Create<Document>();
      return document;
    }


  }
}
