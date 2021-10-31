using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Provider.Logger
{
  public enum PersistentLogStepType
  {
    Error,
    Warn,
    Info,
    Log
  }
  public class PersistentLogStep
  {

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }


    public PersistentLogStep(PersistentLogStepType type, object value, int maxDepth = 1)
    {
      var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
      this.Type = type.ToString().ToLower();
      var serialzerSettings = new JsonSerializerSettings();

      serialzerSettings.MaxDepth = maxDepth;
      serialzerSettings.ContractResolver = jsonResolver;
      this.Value = JsonConvert.SerializeObject(value, serialzerSettings);
    }
  }

  public class PropertyRenameAndIgnoreSerializerContractResolver : DefaultContractResolver
  {
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
      var property = base.CreateProperty(member, memberSerialization);
      if (member.ReflectedType.Namespace == "System.Data.Entity.DynamicProxies")
        property.Ignored = true;
      return property;
    }
  }
}
