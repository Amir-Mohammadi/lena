using Microsoft.AspNetCore.Http;
using core.Autofac;
namespace core.Security.Steganography
{
  public interface IImageContentCheckerService : IScopedDependency
  {
    bool IsImage(IFormFile postedFiles);
  }
}