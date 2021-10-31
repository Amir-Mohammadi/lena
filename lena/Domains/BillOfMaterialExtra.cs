using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BillOfMaterial
  {
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public bool NotHasAnyActiveAndPublishedVersion { get; set; }
  }
}