using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core.ExceptionHandler;
using core.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
namespace core.FileHandler
{
  public class FileCacheService : IFileCacheService
  {
    private readonly IDistributedCache memoryCache;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly IErrorFactory errorFactory;
    public FileCacheService(IServiceScopeFactory serviceScopeFactory,
                            IErrorFactory errorFactory,
                            IDistributedCache memoryCache)
    {
      this.serviceScopeFactory = serviceScopeFactory;
      this.errorFactory = errorFactory;
      this.memoryCache = memoryCache;
    }
    public async Task<IFile> Get(Guid id, string rowVersion)
    {
      var _rowVersion = Convert.FromBase64String(rowVersion);
      var key = id.ToString();
      IFile file;
      string data = memoryCache.GetString(key);
      if (data != null)
      {
        file = Deserialize<BaseFile>(data);
        if (file.RowVersion != null && file.RowVersion.SequenceEqual(_rowVersion))
        {
          return file;
        }
      }
      var token = new CancellationToken(false);
      using (var scope = this.serviceScopeFactory.CreateScope())
      {
        var documentService = scope.ServiceProvider.GetRequiredService<IFileService>();
        file = await documentService.GetFileResultWithStreamById(id: id, rowVersion: _rowVersion, cancellationToken: token);
        if (file != null)
        {
          data = Serialize(file);
          memoryCache.SetString(key, data);
          return file;
        }
      }
      throw errorFactory.FileNotFound();
    }
    private static string Serialize<T>(T obj)
    {
      if (obj == null)
        return null;
      return JsonConvert.SerializeObject(obj);
    }
    private static T Deserialize<T>(string jsonString)
    {
      if (jsonString == null)
        return default;
      return JsonConvert.DeserializeObject<T>(jsonString);
    }
  }
}