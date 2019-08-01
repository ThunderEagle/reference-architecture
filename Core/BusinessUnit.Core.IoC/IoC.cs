
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BusinessUnit.Core.IoC
{
    public class IoC
    {
        private static IoC _instance;
        private static readonly object _lock = new object();

        private IoC()
        {
            Container = CreateContainer();
        }

        public static IoC Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new IoC();
                        }
                    }
                }
                return _instance;
            }
        }


        public CompositionHost Container { get; }

        private CompositionHost CreateContainer()
        {
            //var exeAssembly = Assembly.GetEntryAssembly();

            var currentAssembly = Assembly.GetEntryAssembly();
            var directory = Path.GetDirectoryName(currentAssembly.Location);


            // Find and load all the DLLs in the folder 
            var assemblies = Directory.GetFiles(directory, "*.dll")
                                      .Select(Assembly.LoadFrom)
                                      .Where(x => x != null);


            // Add the loaded assemblies to the container 
            //TODO probably don't need the .WithAssembly(this.... line
            var configuration = new ContainerConfiguration()
                                .WithAssemblies(assemblies)
                                .WithAssembly(this.GetType().GetTypeInfo().Assembly)
                                .WithAssembly(currentAssembly);
            var container = configuration.CreateContainer();
            return container;

        }
    }
}
