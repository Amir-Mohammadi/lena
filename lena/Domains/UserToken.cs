using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class UserToken : IEntity
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresIn { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
  }
}