using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Vez.CoreServices;
using Vez.CoreServices.Inputs;
using Vez.Graphics;
using Vez.Graphics.Textures;
using Vez.Utils.Extensions;

namespace Vez
{
    public class Core
    {
        private readonly List<IUpdateService> _updateServices;
        private readonly List<IDrawService> _drawServices;
        private readonly List<IInitializeService> _initializeServices;
        private readonly List<ILoadContentService> _loadContentServices;
        
        public Core(IServiceProvider provider, IGame game, GraphicsService graphicsService)
        {
            graphicsService.Initialize(game.GraphicsDeviceService);

            _updateServices = provider.GetServices<IUpdateService>().ToList();
            _drawServices = provider.GetServices<IDrawService>().ToList();
            _initializeServices = provider.GetServices<IInitializeService>().ToList();
            _loadContentServices = provider.GetServices<ILoadContentService>().ToList();
        }

        public void Initialize()
        {
            foreach (IInitializeService initializeService in _initializeServices)
            {
                initializeService.Initialize();
            }
        }

        public void LoadContent()
        {
            foreach (ILoadContentService loadContentService in _loadContentServices)
            {
                loadContentService.LoadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _updateServices.Count; i++)
            {
                _updateServices[i].Update(gameTime.ElapsedGameTime.Milliseconds);
            }
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < _drawServices.Count; i++)
            {
                _drawServices[i].Draw();
            }
        }
    }

    public class GraphicsService
    {
        public IGraphicsDeviceService GraphicsDeviceService { get; private set; }
        public GraphicsDevice GraphicsDevice => GraphicsDeviceService.GraphicsDevice;

        public void Initialize(IGraphicsDeviceService graphicsDeviceService)
        {
            GraphicsDeviceService = graphicsDeviceService;
        }
    }

    public static class CoreRegistrations
    {
        /// <summary>
        /// Adds your IGame implementation to the service collection, and builds the core services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddVez<T>(this IServiceCollection services, Action<CoreBuilder> configuration) where T : class, IGame
        {
            return services
                .AddScoped<T>()
                .AddScoped<IGame, T>(isp => isp.GetRequiredService<T>())
                .AddScoped<GraphicsService>()
                .AddScoped(x => x.GetRequiredService<GraphicsService>().GraphicsDevice)
                .AddCore(configuration);
        }

        private static IServiceCollection AddCore(this IServiceCollection services, Action<CoreBuilder> configuration)
        {
            var builder = new CoreBuilder(services);
            configuration(builder);

            return services.AddScoped<Core>();
        }
    }

    public class CoreBuilder
    {
        private IServiceCollection Services { get; }

        public CoreBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Add the default Time class.
        /// </summary>
        /// <returns></returns>
        public CoreBuilder AddTime()
        {
            Add<Time>();
            return this;
        }

        /// <summary>
        /// Add default input state provider.
        /// </summary>
        /// <returns></returns>
        public CoreBuilder AddInput()
        {
            AddInput<InputStateProvider>();
            return this;
        }

        public CoreBuilder AddInput<T>() where T : class, IInputStateProvider
        {
            Add<IInputStateProvider, T>();
            Add<Input>();
            return this;
        }

        /// <summary>
        /// Adds a scoped service to the service, additionally setting up the service
        /// to be handled by core if it implements a core service interface.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public CoreBuilder Add<TService>() where TService : class
        {
            Services.AddScoped<TService>();

            var serviceType = typeof(TService);

            AddServiceIfMatches<IUpdateService>(serviceType);
            AddServiceIfMatches<IDrawService>(serviceType);
            AddServiceIfMatches<IInitializeService>(serviceType);
            AddServiceIfMatches<ILoadContentService>(serviceType);

            return this;

            void AddServiceIfMatches<TInterface>(Type type) where TInterface : class
            {

                if (typeof(TInterface).IsAssignableFrom(type))
                {
                    Services.AddScoped<TInterface>(x => (TInterface)x.GetRequiredService(type));
                }
            }
        }

        public CoreBuilder Add<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            Services.AddScoped<TService, TImplementation>();

            var serviceType = typeof(TService);

            AddServiceIfMatches<IUpdateService>(serviceType);
            AddServiceIfMatches<IDrawService>(serviceType);
            AddServiceIfMatches<IInitializeService>(serviceType);
            AddServiceIfMatches<ILoadContentService>(serviceType);

            return this;

            void AddServiceIfMatches<TInterface>(Type type) where TInterface : class
            {

                if (typeof(TInterface).IsAssignableFrom(type))
                {
                    Services.AddScoped<TInterface>(x => (TInterface)x.GetRequiredService(type));
                }
            }
        }

    }

}