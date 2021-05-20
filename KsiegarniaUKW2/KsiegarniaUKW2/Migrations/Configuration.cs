namespace KsiegarniaUKW2.Migrations
{
    using KsiegarniaUKW2.DAL;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<KsiegarniaUKW2.DAL.Ksiazkicontext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "KsiegarniaUKW2.DAL.Ksiazkicontext";
        }

        protected override void Seed(KsiegarniaUKW2.DAL.Ksiazkicontext context)
        {
            ksiazkiInitiaizer.SeedksiazkiData(context);
        }
    }
}
