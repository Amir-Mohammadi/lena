using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class AdvanceSearchItem
  {
    public string FieldName { get; set; }
    public ComparisonOperators ComparisonOperator { get; set; }
    public object Value { get; set; }
    public ColumnType ColumnType { get; set; }

    public override string ToString()
    {

      switch (ComparisonOperator)
      {

        case ComparisonOperators.IsEqualTo:
          if (ColumnType == ColumnType.Enum)
            return $"{FieldName} ==" + Value;
          if (ColumnType == ColumnType.Moment)
          {
            return $"{FieldName} >= @0 && {FieldName} < @1";
          }
          else
            return $"{FieldName} == @0";

        case ComparisonOperators.Contains:
          return $"{FieldName}.Contains(@0)";
        case ComparisonOperators.StartsWith:
          return $"{FieldName}.StartsWith(@0)";
        case ComparisonOperators.EndsWith:
          return $"{FieldName}.EndsWith(@0)";
        case ComparisonOperators.IsGreaterThan:
          return $"{FieldName} > @0";
        case ComparisonOperators.IsGreaterThanOrEqualTo:
          return $"{FieldName} >= @0";
        case ComparisonOperators.IsLessThan:
          return $"{FieldName} < @0";
        case ComparisonOperators.IsLessThanOrEqualTo:
          return $"{FieldName} <= @0";
        case ComparisonOperators.IsNotEqualTo:
          if (ColumnType == ColumnType.Enum)
            return $"{FieldName} !=" + Value;
          else
            return $"{FieldName} != @0";
        case ComparisonOperators.NotContains:
          return $"!{FieldName}.Contains(@0)" + $" || {FieldName} == null";
        case ComparisonOperators.IsNull:
          return $"{FieldName} == null";
        case ComparisonOperators.IsNotNull:
          return $"({FieldName} != null)";
        case ComparisonOperators.IsNotContainedIn:
          return $"!@0.Contains({FieldName})";
        case ComparisonOperators.IsContainedIn:
          return $"@0.Contains({FieldName})";
        case ComparisonOperators.IsEmpty:
          return $"{FieldName}.Trim() == \"\"";
        case ComparisonOperators.IsNotEmpty:
          return $"({FieldName}.Trim() != \"\")";
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
