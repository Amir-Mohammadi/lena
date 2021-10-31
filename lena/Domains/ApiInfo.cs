using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ApiInfo : IEntity
  {
    protected internal ApiInfo()
    {
      this.Indicators = new HashSet<Indicator>();
    }
    public int Id { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public string Param { get; set; }
    public string Description { get; set; }
    public string SortTypeName { get; set; }
    public string SortTypeFieldName { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Indicator> Indicators { get; set; }
  }
}