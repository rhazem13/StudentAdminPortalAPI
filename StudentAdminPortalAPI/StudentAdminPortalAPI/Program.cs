using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using StudentAdminPortalAPI.DataModels;
using StudentAdminPortalAPI.Repositories;
using StudentAdminPortalAPI.Validators;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors((options) =>
{
    options.AddPolicy("angularApplication", (builder) =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .WithMethods("GET", "POST", "PUT", "DELETE")
        .WithExposedHeaders("*");
    });
});
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(AddStudentRequestValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(UpdateStudentRequestValidator).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StudentAdminContext>(options=>
                options.UseSqlServer(builder.Configuration.GetConnectionString("StudentAdminPortalDbSQLServer")));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
//custom services
builder.Services.AddScoped<IStudentRepository,SqlStudentRepository>();
builder.Services.AddScoped<IImageRepository,LocalStorageImageRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath,"Resources")),
    RequestPath = "/Resources"
});

app.UseCors("angularApplication");

app.UseAuthorization();

app.MapControllers();
app.Run();
