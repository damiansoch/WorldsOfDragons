using Dragons.Services.Dragons;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // ensures 406 when ask for something that is not supported
    options.ReturnHttpNotAcceptable = true;
})
    .AddXmlDataContractSerializerFormatters()
    .ConfigureApiBehaviorOptions(setupAction =>
    {
        setupAction.InvalidModelStateResponseFactory = (context) =>
        {
            var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var problemDetails =
                problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);

            problemDetails.Detail = "See the errors field for details.";
            problemDetails.Instance = context.HttpContext.Request.Path;

            // find out which status code to use
            var actionExecutingContext = context as ActionExecutingContext;

            // if there were modelstate errors and all arguments were found
            // then it is a validation error
            if (context.ModelState.ErrorCount > 0
                && actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count)
            {
                problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                problemDetails.Title = "One or more validation errors occurred";
                return new UnprocessableEntityObjectResult(problemDetails)
                {
                    ContentTypes =
                    {
                        "application/problem+json"
                    }
                };
            }
            else
            {
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "One or more input errors occurred";
                return new UnprocessableEntityObjectResult(problemDetails)
                {
                    ContentTypes =
                    {
                        "application/problem+json"
                    }
                };
            }
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(name: "v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Dragons API V1",
        Description = "An ASP.NET Core Web Api for info about Dragons",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example license",
            Url = new Uri("https://example.com/licence")
        }
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,xmlFilename));
});

builder.Services.AddScoped<IDragonService, DragonService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMvc().AddFluentValidation(fv =>
{
    fv.DisableDataAnnotationsValidation = true;
    fv.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler(appBuilder =>
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(handler: (async ContextBoundObject =>
            {
                ContextBoundObject.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await ContextBoundObject.Response.WriteAsync(text: "An unexpected error occurred. Please try again later.");


            }));
        });
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
