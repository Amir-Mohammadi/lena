using lena.Domains.Enums;
namespace lena.Domains.Enums
{

  public enum ProjectERPTaskDependencyType : byte
  {
    FS, //Finish-to-Start (FS): The finish date of one task drives the start date of another.
    SS, //Start-to-Start (SS): The start date of one task drives the start date of another.
    FF, //Finish-to-Finish (FF): The finish date of one task drives the finish date of another.
    SF, //Start-to-Finish (SF): The start date of one task drives the finish date of another.
  }
}
