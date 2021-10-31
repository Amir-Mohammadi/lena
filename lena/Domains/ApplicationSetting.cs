using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ApplicationSetting : IEntity
  {
    protected internal ApplicationSetting()
    {
    }
    public SettingKey SettingKey { get; set; }
    public string SettingKeyName { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}