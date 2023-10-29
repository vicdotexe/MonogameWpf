namespace Vez.CoreServices
{
    public interface IInitializeService : ICoreService
    {
        void Initialize();
    }

    public interface ILoadContentService : ICoreService
    {
        void LoadContent();
    }

    public interface IUpdateService : ICoreService
    {
        void Update(float deltaTime);
    }

    public interface IDrawService : ICoreService
    {
        void Draw();
    }

    public interface ICoreService { }
}