using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tcp_Server
{
    public class ServiceScope
    {
        public Dictionary<Type,Object> ScopedInstances = new Dictionary<Type,Object>();

        public ServiceProvider Provider { get; set; }

        public ServiceScope(ServiceProvider provider) { 
        
                         Provider = provider;
        
        }





    }
}
