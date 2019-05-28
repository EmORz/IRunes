using SIS.MvcFramework.Routing;

namespace SIS.MvcFramework
{
    public interface IMvcApplication
    {
        void Configure(ServerRoutingTable serverRoutingTable);

        void ConfigureServices();
    }
}