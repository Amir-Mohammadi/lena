using System;

namespace core.Models
{
  public interface ITimestamp
  {
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }

  }
}