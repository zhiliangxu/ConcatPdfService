using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Configuration;

namespace ConcatPdf.Core.Implementations
{
    public class ContainerProvider
    {
        private static Lazy<IUnityContainer> current = new Lazy<IUnityContainer>(CreateNew);

        public static IUnityContainer Current
        {
            get { return current.Value; }
        }

        public static IUnityContainer CreateNew()
        {
            var container = new UnityContainer();
            var config = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            if (config != null)
            {
                config.Configure(container);
            }
            return container;
        }
    }
}
