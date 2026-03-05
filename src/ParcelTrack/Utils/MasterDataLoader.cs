using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParcelCommon;
using ParcelCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ParcelTrack.Utils
{
    internal static class MasterDataLoader
    {
     
        public static IHostBuilder RegisterDataProvider(this  IHostBuilder builder)
        {
            builder.ConfigureServices((hostContext, services) => {
                services.AddScoped<ICDBManager, CDBManager>();
            });
            return builder;
        }

        public static IHost LoadMasterData(this IHost host)
        {
            using(var hostSvcProvider = host.Services.CreateScope())
            {
                var svcCDBManager = hostSvcProvider.ServiceProvider.GetService<ICDBManager>();
                svcCDBManager.LoadEventTypesMaster();
                svcCDBManager.LoadShippingCostMaster();
            }
            return host;
        }
    }
}
