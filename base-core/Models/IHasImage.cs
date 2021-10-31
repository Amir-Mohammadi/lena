using System;
namespace core.Models
{
  public interface IHasImage : IHasRowVersion
  {
    Guid ImageId { get; set; }
    string ImageTitle { get; set; }
    string ImageAlt { get; set; }
    IFile Image { get; set; }
  }
}