using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KsiegarniaUKW2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // GlobalConfiguration.Configuration.UseSqlServerStorage("KsiazkiContext");
            //app.UseHangfireDashboard();
            // app.UseHangfireServer();
        }
    }
}