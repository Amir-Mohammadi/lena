using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class ApplicationLog : IEntity
  {
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User User { get; set; }
    public string ClientIP { get; set; }
    public string UserAgent { get; set; }
    public string Action { get; set; }
    public DateTime LogTime { get; set; }
    public DateTime? RequestEndTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}