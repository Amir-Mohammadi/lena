using System;
using System.IO;
using System.Text;
using System.Web;
namespace lena.Services.Core.Foundation.Document
{
  //TODO fix ssss 
  // public class DocumentHandler : IHttpHandler
  // {
  //   Guid id;
  //   string rowVersion;
  //   public void ProcessRequest(HttpContext context)
  //   {
  //     if (context.Request.QueryString["id"] != null)
  //       id = Guid.Parse(context.Request.QueryString["id"]);
  //     else
  //       throw new ArgumentException("No parameter specified");
  //     if (context.Request.QueryString["rowVersion"] != null)
  //       rowVersion = context.Request.QueryString["rowVersion"];
  //     else
  //       throw new ArgumentException("No parameter specified");
  //     var documentData = App.Api.ApplicationBase.GetDocument.Run(id);
  //     var document = documentData.Data;
  //     if (documentData.Success)
  //     {
  //       context.Response.BinaryWrite(document.FileStream);
  //       context.Response.Headers.Set("Access-Control-Allow-Origin", "*");
  //       if (document.FileType == "jpg" || document.FileType == "png" || document.FileType == "jpeg")
  //       {
  //         context.Response.ContentType = "image/jpeg";
  //       }
  //       else
  //           if (document.FileType == "pdf")
  //       {
  //         context.Response.ContentType = "application/pdf";
  //       }
  //       else
  //           if (document.FileType == "txt")
  //       {
  //         context.Response.ContentType = "text/plain";
  //       }
  //       context.Response.End();
  //     }
  //   }
  //   public bool IsReusable
  //   {
  //     get
  //     {
  //       return false;
  //     }
  //   }
  // }
}