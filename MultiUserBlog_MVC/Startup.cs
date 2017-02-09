using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MultiUserBlog_MVC.Startup))]
namespace MultiUserBlog_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
