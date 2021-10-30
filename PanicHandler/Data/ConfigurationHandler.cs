using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using PanicHandler.Models;

namespace PanicHandler.Data
{
    internal class ConfigurationHandler
    {
        private readonly IConfigurationRoot _configuration;
        private readonly KeyBindingsCollection _bindingsCollection;

        public ConfigurationHandler(IConfigurationRoot configuration, KeyBindingsCollection bindingsCollection)
        {
            _configuration = configuration;
            _bindingsCollection = bindingsCollection;
        }

        /// <summary>
        /// Refresh in memory the configuration settings
        /// </summary>
        internal void UpdateBindingsList()
        {
            _bindingsCollection.KeyBindings = _configuration.GetSection("PanicHandler:KeyBindings")
                .Get<IEnumerable<KeyBinding>>()
                .ToList();
        }
    }
}