using ParcelCommon;
using ParcelCommon.Interfaces;

namespace ParcelCreate.Utilities
{
    public enum EventType { BOOK =1, SRC_SORT =2, DEST_SORT=3, DEL_LOC=4, READY=5, DELIVERED=6, RETURNED=-1, FAILED=-2 }
    public static class MasterData
    {
        public static float BaseCharge = 50;
        public static float ExtraCharge = 0.20f;

        public static IServiceCollection RegisterDataProvider(this IServiceCollection services)
        {
            services.AddSingleton<ICDBManager, CDBManager>();
            return services;
        }

        public static IApplicationBuilder LoadMasterData(this IApplicationBuilder builder)
        {
            using (var app = builder.ApplicationServices.CreateScope()) 
            {
                var cdbManager = app.ServiceProvider.GetService<ICDBManager>();
                
            }
            return builder;
        }
    }
}
