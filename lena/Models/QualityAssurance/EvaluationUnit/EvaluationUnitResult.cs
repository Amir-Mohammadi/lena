using lena.Domains.Enums;
namespace lena.Models
{
  public class EvaluationUnitResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public double Target { get; set; }
    public double Weight { get; set; }

    public int ApiInfoId { get; set; }
    public string ApiInfoUrl { get; set; }
    public string ApiInfoParam { get; set; }

    public string Formula { get; set; }
    public int DepartmentId { get; set; }
    public string Description { get; set; }
    public string DepartmentName { get; set; }
    public double NumberObtained { get; set; } // عدد بدست آمده از از محاسبه شاخص ها
    public double ScoreOfThisMonth { get; set; }  // امتیاز این ماه
    public double TotalScore { get; set; } // امتیاز کل شاخص 
    public double UnitPerformanceScore { get; set; } // امتیاز عملکرد واحد
    public byte[] RowVersion { get; set; }
  }
}
