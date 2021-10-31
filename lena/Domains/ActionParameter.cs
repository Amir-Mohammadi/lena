using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ActionParameter : IEntity
  {
    protected internal ActionParameter()
    {
    }
    public int Id { get; set; }
    public int SecurityActionId { get; set; }
    public string ParameterKey { get; set; }
    public string ParameterValue { get; set; }
    public bool CheckParameterValue { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual SecurityAction SecurityAction { get; set; }
  }
}