﻿using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class UnPublishBillOfMaterialInput
  {
    public int StuffId { get; set; }
    public short Version { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
