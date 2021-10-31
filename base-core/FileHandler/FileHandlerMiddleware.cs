using System;
using System.IO;
using System.Threading.Tasks;
using core.Models;
using core.Setting;
using core.StateManager;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
namespace core.FileHandler
{
  public class FileHandlerMiddleware
  {
    private readonly IFileCacheService fileCacheService;
    private readonly ISiteSettingProvider siteSettingProvider;
    private readonly IStateManagerService stateManagerService;
    public FileHandlerMiddleware(
      RequestDelegate next,
      IFileCacheService fileCacheService,
      ISiteSettingProvider siteSettingProvider,
      IStateManagerService stateManagerService)
    {
      if (next is null)
        throw new ArgumentNullException(nameof(next));
      this.fileCacheService = fileCacheService;
      this.siteSettingProvider = siteSettingProvider;
      this.stateManagerService = stateManagerService;
    }
    public async Task Invoke(HttpContext context)
    {
      try
      {
        if (context.Request.Query.Keys.Contains("id"))
        {
          Guid id = Guid.Parse(context.Request.Query["id"]);
          string rowVersion = context.Request.Query["rv"];
          var file = await this.fileCacheService.Get(id, rowVersion);
          context.Response.ContentType = "image/" + file.FileType.Replace(".", "");
          context.Response.Headers[HeaderNames.ETag] = Convert.ToBase64String(file.RowVersion);
          context.Response.Headers[HeaderNames.CacheControl] = siteSettingProvider.SiteSetting.CacheControl;
          await context.Response.Body.WriteAsync(file.FileStream);
        }
        if (context.Request.Query.Keys.Contains("fK"))
        {
          string fileKey = context.Request.Query["fk"];
          var uploadedFile = await this.stateManagerService.GetState<UploadedFile>("fk" + fileKey);
          context.Response.ContentType = "image/" + Path.GetExtension(uploadedFile.FileName).Replace(".", "");
          context.Response.Headers[HeaderNames.ETag] = fileKey;
          context.Response.Headers[HeaderNames.CacheControl] = siteSettingProvider.SiteSetting.CacheControl;
          await context.Response.Body.WriteAsync(uploadedFile.Stream);
        }
      }
      catch (Exception ex)
      {
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync(ex.Message);
      }
    }
  }
}