using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class TValue<T>
  {
    public TValue() : this(default(T))
    {
    }
    public TValue(T value)
    {
      this.Value = value;
    }
    public T Value { get; set; }
    public static implicit operator T(TValue<T> value)
    {
      return value != null ? value.Value : default(T);
    }
    public static implicit operator TValue<T>(T value)
    {
      return value == null ? null : new TValue<T>(value);
    }
  }
}
