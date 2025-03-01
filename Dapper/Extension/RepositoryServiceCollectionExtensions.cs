using System.Reflection;

namespace Dapper.Extension
{
    public static class RepositoryServiceCollectionExtensions
    {
            #region Public Routines
            public static void AddRepositories(this IServiceCollection services, Assembly assembly, string connectionString)
            {
                // Get all types that are classes and end with "Repository" (this is an example pattern)
                var repositoryAssembly = Assembly.Load("DALL");
                var repositoryTypes = repositoryAssembly.GetTypes()
                .Where(t => t.IsClass && t.Name.EndsWith("Repository"));

                foreach (var repositoryType in repositoryTypes)
                {
                    // Assuming the repository has a constructor that takes a connection string
                    var interfaces = repositoryType.GetInterfaces();
                    if (interfaces.Any())
                    {
                        // Register each repository as Scoped with its interface
                        foreach (var interfaceType in interfaces)
                        {
                            services.AddScoped(interfaceType, provider =>
                                ActivatorUtilities.CreateInstance(provider, repositoryType, connectionString));
                        }
                    }
                    else
                    {
                        // If no interfaces, register the repository directly as Scoped
                        services.AddScoped(repositoryType, provider =>
                            ActivatorUtilities.CreateInstance(provider, repositoryType, connectionString));
                    }
                }
            }
            #endregion
        
    }
}
