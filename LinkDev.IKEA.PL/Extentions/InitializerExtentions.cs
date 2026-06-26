using LinkDev.IKEA.DAL.Contracts;

namespace LinkDev.IKEA.PL.Extentions
{
    public static class InitializerExtentions
    {
        public static async Task InitializeDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var dbInitializer = services.GetRequiredService<IDbInitializer>();
            dbInitializer.Initialize();
            await dbInitializer.Seed();
            
        }

    }
}
