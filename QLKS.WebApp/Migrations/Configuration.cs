

using QLKS.WebApp.DAL;

namespace QLKS.WebApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QLKS.WebApp.DAL.HotelDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(QLKS.WebApp.DAL.HotelDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            AccountSeeder.Seed(context);
            SupplierSeeder.Seed(context);
            CategorySeeder.Seed(context);

        }
    }
}
