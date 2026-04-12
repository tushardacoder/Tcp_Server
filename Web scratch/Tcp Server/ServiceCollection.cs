using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tcp_Server
{

    // This class helps our mini web framework "find" all controllers automatically.
    //
    // In ASP.NET Core, this happens behind the scenes. Here, we do it manually
    // so we can understand how controllers are discovered.
    public class ServiceCollection
    {
        private readonly List<ServiceDescriptor> _services = [];

        public void AddTransient<TService>()
        {
            var typeOfService = typeof(TService);
            AddTransient(typeOfService, typeOfService);
        }

        public void AddSingleton<TService>()
        {
            var typeOfService = typeof(TService);
            AddSingleton(typeOfService, typeOfService);
        }

        public void AddScoped<TService>()
        {
            var typeOfService = typeof(TService);
            AddScoped(typeOfService, typeOfService);
        }


        public void AddTransient<TService, TImplementation>()
        {
            _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
        }

        public void AddTransient(Type serviceType, Type implementationType)
        {
            _services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient));
        }

        public void AddSingleton<TService, TImplementation>()
        {
            _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
        }

        public void AddSingleton(Type serviceType, Type implementationType)
        {
            _services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
        }

        public void AddScoped<TService, TImplementation>()
        {
            _services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped));
        }

        public void AddScoped(Type serviceType, Type implementationType)
        {
            _services.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Scoped));
        }

       
        // Call this method to discover all controller classes in our project.
        // A "controller class" is any class whose name ends with "Controller"
        // (like UserController, ProductController, etc.)
        public void AddControllers()
        {
            var controllers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && t.Name.EndsWith("Controller"));

            foreach (var ctrl in controllers)
            {
                AddTransient(ctrl, ctrl);
            }
        }

        public ServiceProvider BuildServiceProvider() => new(_services);
    }
}
