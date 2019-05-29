using IRunes.Data;
using SIS.MvcFramework;
using SIS.MvcFramework.Routing;

namespace IRunes.App
{
    public class StartUp : IMvcApplication
    {
        public void Configure(ServerRoutingTable serverRoutingTable)
        {
            using (var context = new RunesDbContext())
            {
                context.Database.EnsureCreated();
            }
        }

        public void ConfigureServices()
        {
           
        }

    
    }
}