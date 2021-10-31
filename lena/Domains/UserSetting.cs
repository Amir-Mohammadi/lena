using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class UserSetting : IEntity
  {
    protected internal UserSetting()
    {
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    public UserSettingValueType ValueType { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
  }
}