﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class DeleteTestOperationInput
  {
    public int TestOperationId { get; set; }
    public int QualityControlTestId { get; set; }
  }
}
