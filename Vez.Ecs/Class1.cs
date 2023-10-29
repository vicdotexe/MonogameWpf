using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

using DefaultEcs.System;

namespace Vez.Ecs
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddDefaultEcs(this IServiceCollection services)
        {
            var entry = Assembly.GetEntryAssembly();
            var calling = Assembly.GetCallingAssembly();
            var executing = Assembly.GetExecutingAssembly();
            var referenced = Assembly.GetEntryAssembly()?.GetReferencedAssemblies()
                .SelectMany(x => Assembly.Load(x).GetTypes()) ?? Enumerable.Empty<Type>();


            var allTypes = entry.GetTypes()
                .Concat(calling.GetTypes())
                .Concat(executing.GetTypes())
                .Concat(referenced);
            
            var types = allTypes
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISystem<>)) && !t.IsAbstract);

            foreach (var type in types)
            {
                services.AddScoped(type);
            }

            services.AddScoped<SystemFactory>();

            return services;
        }

    }

    public interface ISystem
    {
        bool IsEnabled { get; set; }
    }

    public abstract class BaseSystem<T> : ISystem<T> , ISystem
    {
        public abstract void Update(T state);
        public virtual void Dispose() { }
        public virtual bool IsEnabled { get; set; }
    }


    public class SystemFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SystemFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T New<T, TU>() where T : ISystem<TU>
        {
            return _serviceProvider.GetRequiredService<T>();
        }
        public T New<T>() where T : ISystem
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }

}
