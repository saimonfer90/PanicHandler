using PanicHandler.Data;
using SimpleInjector;
using System.Threading.Tasks;

namespace PanicHandler
{
    internal class Core
    {
        private readonly Container _serviceContainer;
        private readonly ConfigurationHandler _configurationHandler;

        internal Core()
        {
            /*It create a Container instance, add it to the service collection, and initialize all dependencies*/
            _serviceContainer = InjectionConfigurator.GetContainerService();

            _serviceContainer.InitializeContainer();

            _serviceContainer.Verify();

            _configurationHandler = _serviceContainer.GetInstance<ConfigurationHandler>();
        }

        internal async Task Run()
        {
            while (true)
            {
                _configurationHandler.UpdateBindingsList();

                await Task.Delay(1000);
            }
        }
    }
}