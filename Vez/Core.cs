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
    internal sealed class Core
    {
        private readonly List<IUpdateService> _updateServices;
        private readonly List<IDrawService> _drawServices;
        private readonly List<IInitializeService> _initializeServices;
        private readonly List<ILoadContentService> _loadContentServices;
        
        public Core(IServiceProvider provider)
        {
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

    public class CoreConfiguration
    {
        private IServiceCollection Services { get; }

        public CoreConfiguration(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Add the default Time class.
        /// </summary>
        /// <returns></returns>
        public CoreConfiguration AddTime()
        {
            Add<Time>();
            return this;
        }

        /// <summary>
        /// Add default input state provider.
        /// </summary>
        /// <returns></returns>
        public CoreConfiguration AddInput()
        {
            AddInput<InputStateProvider>();
            return this;
        }

        public CoreConfiguration AddInput<T>() where T : class, IInputStateProvider
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
        public CoreConfiguration Add<TService>() where TService : class
        {
            Services.AddSingleton<TService>();

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
                    Services.AddSingleton<TInterface>(x => (TInterface)x.GetRequiredService(type));
                }
            }
        }

        public CoreConfiguration Add<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            Services.AddSingleton<TService, TImplementation>();
            Add<TImplementation>();
            return this;
        }

    }

}