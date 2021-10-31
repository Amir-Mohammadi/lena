using System;

using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  [Flags]
  public enum ProductionOperationStatus : byte
  {

    Done = 0,
    QualityControlPass = 1,
    QualityControlFaild = 2,
    Faild = 4
  }
}
