//using Parlar.DAL;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
// using System.Runtime.Remoting.Messaging;
using System.Text;
////using lena.Services.Core.Foundation.Action;
using lena.Models.Common;
using lena.Models.Common;
using System.Linq.Dynamic;
using lena.Services.Core.Foundation;
// //using System.Data.Entity.Infrastructure;
using System.Globalization;
using Newtonsoft.Json;
using System.Dynamic;
using System.Security.Cryptography;
using lena.Domains.Enums;
namespace lena.Services.Common
{
  public static class Extentions
  {
    public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> query, PagingInput pagingInput)
    {
      if (pagingInput.PageNumber * pagingInput.PageSize > 0)
      {
        if (pagingInput.PageNumber <= 0)
          throw new ArgumentOutOfRangeException(nameof(pagingInput.PageNumber));
        if (pagingInput.PageSize <= 0)
          throw new ArgumentOutOfRangeException(nameof(pagingInput.PageSize));
        query = query.Skip((pagingInput.PageNumber - 1) * pagingInput.PageSize).Take(pagingInput.PageSize);
      }
      return query;
    }
    public static string ToResponse(this Exception ex)
    {
      var type = ex.GetType();
      var rootException = ex;
      var name = type.ToUnderScores();
      var message = ex.Message;
      try
      {
        if (ex != null)
        {
          var innerException = ex.InnerException;
          while (innerException != null)
          {
            message = innerException.Message;
            innerException = innerException.InnerException;
          }
        }
      }
      catch (Exception) { }
      var actionParameters = ex.ToKeyValue(delimeter: "_", containsNullFileds: false, exportArraysAsSerializedJson: false, convertArray: true);
      var container = new ApiErrorContainer(name);
      container.data = actionParameters;
      return JsonConvert.SerializeObject(container);
    }
    public static string ToUnderScores(this Type str)
    {
      return
          System.Text.RegularExpressions.Regex.Replace(str.Name, "([A-Z][a-z]+)", " $1",
                  System.Text.RegularExpressions.RegexOptions.Compiled)
              .Trim()
              .Replace(" Exception", "")
              .Replace(" ", "_").ToUpper();
    }
    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, SortOrder sortOrder)
    {
      if (sortOrder == SortOrder.Ascending || sortOrder == SortOrder.Unspecified)
        return source.OrderBy(keySelector);
      else
        return source.OrderByDescending(keySelector);
    }
    public static string GenrateRefreshToken()
    {
      var randomNumber = new byte[32];
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
      }
    }
    public static string ComputeHashToken(this string str)
    {
      using (var sha256 = SHA256.Create())
      {
        var byteValue = Encoding.UTF8.GetBytes(str);
        var byteHash = sha256.ComputeHash(byteValue);
        return Convert.ToBase64String(byteHash);
      }
    }
    public static TimeSpan ComputeTimeSpan(this DateTime dt)
    {
      var date = DateTime.Now;
      var expirationTimeSpan = dt.Subtract(date);
      return new TimeSpan(expirationTimeSpan.Hours, expirationTimeSpan.Minutes, expirationTimeSpan.Seconds);
    }
    public static string KeyPrefix(this string str, string prefix)
    {
      return prefix + "|" + str;
    }
    public static string CheckSetPrefix(this string str, string prefix)
    {
      if (str.Contains("|"))
        return str;
      else
        return prefix + "|" + str;
    }
  }
  public static class QueryableExtensions
  {
    public static IQueryable<T> Where<T>(this IQueryable<T> source, AdvanceSearchItem[] advanceSearchItems)
    {
      if (advanceSearchItems != null)
      {
        foreach (var item in advanceSearchItems)
        {
          var exp = item.ToString();
          if (exp.Contains("@"))
          {
            if (item.Value != null)
            {
              if (item.ColumnType == ColumnType.Moment)
              {
                var fromDate = (DateTime)item.Value;
                var toDate = fromDate.AddDays(1);
                source = source.Where(exp, fromDate, toDate);
              }
              else
                source = source.Where(exp, item.Value);
            }
          }
          else
            source = source.Where(exp);
        }
      }
      return source;
    }
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    {
      return source.OrderBy(ToLambda<T>(propertyName));
    }
    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    {
      return source.OrderByDescending(ToLambda<T>(propertyName));
    }
    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
      var parameter = Expression.Parameter(typeof(T));
      var property = Expression.Property(parameter, propertyName);
      var propAsObject = Expression.Convert(property, typeof(object));
      return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
    public static string GetInnerMessage(this Exception ex)
    {
      if (ex.InnerException == null)
        return ex.Message;
      return GetInnerMessage(ex.InnerException);
    }
    //========================================Calendar Extentions=====================================
    public static string Format(this DateTime date, CultureInfo culture, string format)
    {
      if (culture.Name.ToLower() == "fa-ir")
      {
        var pc = new PersianCalendar();
        var year = pc.GetYear(date);
        var month = pc.GetMonth(date);
        var day = pc.GetDayOfMonth(date);
        var dayName = GetDayOfWeekName(date);
        var monthName = GetMonthName(date);
        var shortDayName = GetShortDayOfWeekName(date);
        var result = format.Replace("yyyy", year.ToString())
             .Replace("yyy", year.ToString())
             .Replace("yy", year.ToString("0000").Substring(2, 2))
             .Replace("MMMM", monthName)
             .Replace("MMM", monthName)
             .Replace("MM", month.ToString("00"))
             .Replace("dddd", dayName)
             .Replace("ddd", shortDayName)
             .Replace("dd", day.ToString("00"))
             .Replace("HH", date.ToString("HH"))
             .Replace("hh", date.ToString("hh"))
             .Replace("mm", date.ToString("mm"))
             .Replace("ss", date.ToString("ss"));
        return result;
      }
      else// (culture.OptionalCalendars.Any(x => x.ToString() == culture.ToString()))
        return date.ToString(format, culture);
    }
    public static string GetMonthName(this DateTime date)
    {
      PersianCalendar jc = new PersianCalendar();
      string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));
      string[] dates = pdate.Split('/');
      int month = Convert.ToInt32(dates[1]);
      switch (month)
      {
        case 1: return "فررودين";
        case 2: return "ارديبهشت";
        case 3: return "خرداد";
        case 4: return "تير‏";
        case 5: return "مرداد";
        case 6: return "شهريور";
        case 7: return "مهر";
        case 8: return "آبان";
        case 9: return "آذر";
        case 10: return "دي";
        case 11: return "بهمن";
        case 12: return "اسفند";
        default: return "";
      }
    }
    public static string GetDayOfWeekName(this DateTime date)
    {
      switch (date.DayOfWeek)
      {
        case DayOfWeek.Saturday: return "شنبه";
        case DayOfWeek.Sunday: return "يکشنبه";
        case DayOfWeek.Monday: return "دوشنبه";
        case DayOfWeek.Tuesday: return "سه‏ شنبه";
        case DayOfWeek.Wednesday: return "چهارشنبه";
        case DayOfWeek.Thursday: return "پنجشنبه";
        case DayOfWeek.Friday: return "جمعه";
        default: return "";
      }
    }
    public static string GetShortDayOfWeekName(this DateTime date)
    {
      switch (date.DayOfWeek)
      {
        case DayOfWeek.Saturday: return "ش";
        case DayOfWeek.Sunday: return "ي";
        case DayOfWeek.Monday: return "د";
        case DayOfWeek.Tuesday: return "س";
        case DayOfWeek.Wednesday: return "چ";
        case DayOfWeek.Thursday: return "پ";
        case DayOfWeek.Friday: return "ج";
        default: return "";
      }
    }
  }
}