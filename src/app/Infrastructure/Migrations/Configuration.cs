using System.Data.Entity.Migrations;
using System.Linq;
using Clinic.Core.Domain.Models;
using Clinic.Infrastructure.Data;

namespace Clinic.Infrastructure.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<ClinicContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Clinic.Infrastructure.Data.ClinicContext";

        }

        protected override void Seed(ClinicContext context)
        {

        }
    }
}
