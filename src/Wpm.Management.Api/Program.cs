using Microsoft.EntityFrameworkCore;
using Wpm.Management.Api.DataAccess;

namespace Wpm.Management.Api
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
                builder.Services.AddOpenApi();
                builder.Services.AddSwaggerGen();
                builder.Services.AddDbContext<ManagementDbContext>(options =>
                {
                    options.UseInMemoryDatabase("WpmManagement");
                });

                var app = builder.Build();

                app.EnsureSeedData();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                }

                // Configure the HTTP request pipeline.
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHttpsRedirection();

                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
        }
    }
}
