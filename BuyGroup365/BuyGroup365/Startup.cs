using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BuyGroup365.Startup))]

namespace BuyGroup365
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}