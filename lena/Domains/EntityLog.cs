using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class EntityLog : IEntity
  {
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User User { get; set; }
    public DateTime DateTime { get; set; }
    public string Ip { get; set; }
    public string Api { get; set; }
    public string ApiParams { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
