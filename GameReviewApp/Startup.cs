using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameReviewApp.Startup))]
namespace GameReviewApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
