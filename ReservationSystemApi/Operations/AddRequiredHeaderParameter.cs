using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using ReservationSystemApi.Services;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = Constants.UserHeaderKey,
            In = ParameterLocation.Header,
            Schema = new OpenApiSchema { Type = "String" },
            Required = false
        });

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = Constants.TokenHeaderKey,
            In = ParameterLocation.Header,
            Schema = new OpenApiSchema { Type = "String" },
            Required = false
        });
    }
}