using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParcelCommon;
using ParcelCommon.Interfaces;
using ParcelCommon.Utilities;
using ParcelTrack.BLogic;
using ParcelTrack.Interfaces;
using ParcelTrack.Services;
using ParcelTrack.Utils;
using System.Configuration;
using System.Xml.Linq;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IEventConsumerSvc, EventConsumerSvc>();
        services.AddScoped<IValidateScan, ValidateScan>();
        services.AddScoped<IAlertSender, MobileUpdateSender>();
    })
    .ConfigureServices((hostcontext,services) => 
    {
        IConfiguration configuration = hostcontext.Configuration;
        //services.Configure<CDBSettings>(hostcontext.Configuration.GetSection("CDBSettings"));
        services.AddDbContext<CDBContext>((serviceProvider,options) => 
        {

            //var optionsSettings = serviceProvider.GetRequiredService<CDBSettings>();
            options.UseCosmos(configuration["CDBSettings_EPUri"], configuration["CDBSettings_PKey"], configuration["CDBSettings_DBName"]);
        });
    })
    .RegisterDataProvider()
    .Build();

host.LoadMasterData();
host.Run();