﻿using lena.Domains.Enums;
namespace lena.Models
{
  public class EditExchangeInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
  }
}