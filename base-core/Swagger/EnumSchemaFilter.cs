using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace core.Swagger
{
  public class EnumSchemaFilter : ISchemaFilter
  {
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
      if (context.Type.IsEnum)
      {
        var items = new Dictionary<int, string>();
        foreach (var i in Enum.GetValues(context.Type))
        {
          items.Add(Convert.ToInt32(i), Enum.GetName(context.Type, i));
        };
        schema.Description += string.Join(", ", items.Select(x => x.Key + ": " + x.Value));
      }
    }
  }
}