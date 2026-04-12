using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tcp_Server
{
   public class ServiceProvider
    {
        // List of all registered services
        private readonly List<ServiceDescriptor> _services;
  
        

        private static Dictionary<Type, object> _scoped = new();

   
        private Dictionary<Type, object> _singleton = new();




        /// <summary>
        /// Constructor initializes the ServiceProvider with registered services.
        /// </summary>
        /// <param name="services">List of service descriptors</param>
        /// 


       

        public ServiceProvider(List<ServiceDescriptor> services)
        {
            _services = services;
        }


       

        public ServiceScope CreateScope()
        {
            return new ServiceScope(this); 
        }

        /// <summary>
        /// Generic method to resolve a service by type.
        /// </summary>
        /// <typeparam name="T">Service type</typeparam>
        /// <returns>Resolved service instance</returns>
        public T GetService<T>() => (T)GetService(typeof(T));

        /// <summary>
        /// Resolves a service given its type.
        /// </summary>
        /// <param name="serviceType">Type of the service to resolve</param>
        /// <returns>Resolved service instance</returns>
        public object GetService(Type serviceType,ServiceScope scope=null)
        {
            // Find the registered service descriptor
            var descriptor = _services.FirstOrDefault(x => x.ServiceType == serviceType)
                ?? throw new Exception($"Service {serviceType.Name} isn't registered");
   
            // Resolve service based on its lifetime
            return descriptor.LifeTime switch
            {
                ServiceLifetime.Transient => CreateInstance(descriptor.ImplementationType),
                ServiceLifetime.Singleton => CreateInstanceSingleton(descriptor.ImplementationType),
                ServiceLifetime.Scoped=> CreateInstanceScoped(descriptor.ImplementationType,scope),
                _ => throw new Exception("Unknown lifetime")
            };
        }

        /// <summary>
        /// Creates an instance of a type using constructor injection.
        /// </summary>
        /// <param name="implType">Implementation type to instantiate</param>
        /// <returns>New instance with dependencies injected</returns>



        private object CreateInstanceSingleton(Type implType)
        {
            //check if already created
            if (_singleton.ContainsKey(implType)) return _singleton[implType];
            // Get the first constructor of the type
            var firstConstructor = implType.GetConstructors().First();


            // resolve all constructors parameters (dependencies)
            var deps = firstConstructor.GetParameters()
               .Select(p => GetService(p.ParameterType))
               .ToArray();

            // Create instance
            var instance = Activator.CreateInstance(implType, deps)!;
            //store innstance
            _singleton[implType] = instance;



            // Create instance using resolved dependencies
            return instance;

        }
        private object CreateInstanceScoped(Type implType,ServiceScope scope)
        {

            if (scope == null) throw new Exception("No scope provided for scoped service");

            if (scope.ScopedInstances.ContainsKey(implType))
            {
                 return scope.ScopedInstances[implType];
            }

            var ctor = implType.GetConstructors();
            var firstConstructor = ctor.First();

            // Resolve all constructor parameters (dependencies)
            var deps = firstConstructor.GetParameters()
                .Select(p => GetService(p.ParameterType,scope))
                .ToArray();


            // Create instance using resolved dependencies
            var instance= Activator.CreateInstance(implType, deps)!;

            scope.ScopedInstances[implType] = instance;

            return instance;
        }

        private object CreateInstance(Type implType)
        {

        // Get the first constructor of the type

        var ctor = implType.GetConstructors();
            var firstConstructor = ctor.First();

            // Resolve all constructor parameters (dependencies)
            var deps = firstConstructor.GetParameters()
                .Select(p => GetService(p.ParameterType))
                .ToArray();


            // Create instance using resolved dependencies
            return Activator.CreateInstance(implType, deps)!;




        }
        /// <summary>
        /// Returns a list of registered controller types.
        /// Controllers are identified by class names ending with 'Controller'.
        /// </summary>
        /// <returns>List of controller types</returns>
        public List<Type> GetControllerTypes()
        {
            return _services
                .Where(d => d.ServiceType.Name.EndsWith("Controller"))
                .Select(d => d.ServiceType)
                .ToList();
        }
    }
}
