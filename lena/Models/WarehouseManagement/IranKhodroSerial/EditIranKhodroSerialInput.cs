﻿using lena.Domains.Enums;
namespace lena.Models
{
  public class EditIranKhodroSerialInput
  {
    public string[] AddLinkedSerials { get; set; }
    public string[] DeleteLinkedSerials { get; set; }
    public int CustomerId { get; set; }
    public byte[] RowVersion { get; set; }

  }
}