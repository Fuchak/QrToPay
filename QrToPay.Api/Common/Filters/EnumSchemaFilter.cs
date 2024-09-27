using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// (Swagger filter) Adding (key = value) documentation to enums
/// </summary>
public sealed class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Type = "string";
            schema.Format = null;

            schema.Enum.Clear();

            List<string> enumStrings = Enum.GetNames(context.Type).ToList();
            enumStrings.ForEach(name => schema.Enum.Add(new OpenApiString(name)));
        }
    }
}