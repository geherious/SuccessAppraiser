
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Entities;
using Microsoft.OpenApi.Models;
using SuccessAppraiser.Services;
using SuccessAppraiser.Data.Seeding;



namespace SuccessAppraiser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddConfiguredDbContext(builder.Configuration);

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            builder.Services.AddAuthorization();
            builder.Services.AddIdentityApiEndpoints<ApplicationUser>(x =>
                {
                    x.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
            });

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddProjectServices();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowReact",
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:7127",
                                                          "http://localhost:3000")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials();
                                  });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapIdentityApi<ApplicationUser>();

            app.UseHttpsRedirection();

            app.UseCors("AllowReact");

            app.UseAuthorization(); 


            app.MapControllers();

            SeedData.PopulateDb(app);

            app.Run();
        }
    }
}

