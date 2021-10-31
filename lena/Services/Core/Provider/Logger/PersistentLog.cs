using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Provider.Logger
{
  public class PersistentLog
  {
    [JsonProperty("debug")]
    public bool Debug { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("value")]
    public string Value { get; set; }

    [JsonIgnore]
    private object _rawVal;

    [JsonIgnore]
    public object RawValue
    {
      get { return _rawVal; }
      set
      {
        _rawVal = value;
        this.Value = JsonConvert.SerializeObject(value);
      }
    }

    [JsonProperty("steps")]
    public List<PersistentLogStep> Steps { get; set; }



    public PersistentLog(string name)
    {
      Steps = new List<PersistentLogStep>();
      this.Debug = false;

#if DEBUG
      this.Debug = true;
#endif
      this.Name = name;
    }

    public string ToJson()
    {
      return JsonConvert.SerializeObject(this);
    }
  }
}
