using System;
namespace core.Models
{
  public interface IRemovable
  {
    DateTime? DeletedAt { get; set; }
    bool IsDelete { get; set; }
  }
}