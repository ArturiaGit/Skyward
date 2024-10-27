using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Extensions.Logging;
using Skyward.Session1.WebAPI.Context;
using Skyward.Session1.WebAPI.Services.StaffServices;

namespace Skyward.Session1.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var logger = NLog.LogManager.Setup().LoadConfigurationFromFile("nlog.config", optional: false).GetCurrentClassLogger();

        try
        {
            logger.Info("启动 web API");
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddNLog();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<IStaffService, StaffService>();

            builder.Services.AddDbContext<MyDbContext>(setUp =>
            {
                setUp.UseSqlServer(builder.Configuration.GetConnectionString("ConnString"));
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "应用程序因异常而停止运行.");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }
}