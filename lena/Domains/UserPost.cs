using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class UserPost : IEntity
  {
    protected internal UserPost()
    {
    }
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public Nullable<DateTime> DeleteDate { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Post Post { get; set; }
    public virtual User User { get; set; }
  }
}