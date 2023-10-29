namespace Game.Shared.Scenes
{
    //public class EntityManager
    //{
    //    private Dictionary<Guid, Entity> _entities = new();
    //    private Dictionary<Guid, ComponentList> _components = new();

    //    private Dictionary<Guid, Entity> _entitiesToAdd = new();
    //    private HashSet<Guid> _entitiesToRemove = new();


    //    public Entity CreateEntity()
    //    {
    //        var guid = Guid.NewGuid();
    //        var entity = new Entity(this, guid);
    //        _entitiesToAdd.Add(guid, entity);
    //        return entity;
    //    }

    //    public void DestroyEntity(Guid guid)
    //    {
    //        _entitiesToRemove.Add(guid);
    //    }

    //    public Entity GetEntity(Guid guid)
    //    {
    //        if (_entities.TryGetValue(guid, out var entity))
    //        {
    //            return entity;
    //        }
    //        throw new InvalidOperationException("This entity does not exist.");
    //    }

    //    public T AddComponent<T>(Guid guid, T component) where T : Component
    //    {
    //        if (_components.TryGetValue(guid, out var componentList))
    //        {
    //            return componentList.AddComponent(component);
    //        }
    //        throw new InvalidOperationException("This entity does not exist.");
    //    }   

    //    public T? GetComponent<T>(Guid guid) where T : Component
    //    {
    //        if (_components.TryGetValue(guid, out var componentList))
    //        {
    //            return componentList.GetComponent<T>();
    //        }
    //        throw new InvalidOperationException("This entity does not exist.");
    //    }

    //    public void RemoveComponent<T>(Guid guid) where T : Component
    //    {
    //        if (_components.TryGetValue(guid, out var componentList))
    //        {
    //            componentList.RemoveComponent<T>();
    //        }
    //        throw new InvalidOperationException("This entity does not exist.");
    //    }

    //    public void Update()
    //    {
    //        foreach (Guid id in _entitiesToRemove)
    //        {
    //            _entities.Remove(id);
    //            _components.Remove(id);
    //        }
    //        _entitiesToRemove.Clear();

    //        foreach (var entity in _entitiesToAdd)
    //        {
    //            _entities.Add(entity.Key, entity.Value);
    //            _components.Add(entity.Key, new ComponentList());
    //        }
    //        _entitiesToAdd.Clear();

    //        foreach (var componentList in _components)
    //        {
    //            componentList.Value.Update();
    //        }
    //    }
    //}

    //public class Scene
    //{
    //    EntityManager _entityManager = new();

    //    public Scene()
    //    {

    //    }

    //    public void Update(float deltaTime)
    //    {
    //        _entityManager.Update();
    //    }

    //    public void Draw()
    //    {
            
    //    }
    //}



    //public class ComponentList
    //{
    //    private Dictionary<Type, Component> _components = new();
    //    private HashSet<Type> _componentsToRemove = new();
    //    private Dictionary<Type, Component> _componentsToAdd = new();

    //    public ComponentList(IEnumerable<Component>? components = null)
    //    {
    //        if (components is not null)
    //        {
    //            _componentsToAdd = new Dictionary<Type, Component>(components.Select(x => new KeyValuePair<Type, Component>(x.GetType(), x)));
    //        }
    //    }

    //    public T AddComponent<T>(T component) where T : Component
    //    {
    //        if (!_components.ContainsKey(typeof(T)) && !_componentsToAdd.ContainsKey(typeof(T)))
    //        {
    //            _componentsToAdd.Add(typeof(T), component);
    //            return component;
    //        }

    //        throw new InvalidOperationException("This component already exists.");
    //    }

    //    public void RemoveComponent<T>() where T : Component
    //    {
    //        if (_components.ContainsKey(typeof(T)) && !_componentsToRemove.Add(typeof(T)))
    //        {
    //            _componentsToRemove.Add(typeof(T));
    //        }
    //        else
    //        {
    //            throw new InvalidOperationException("This component does not exist.");
    //        }
    //    }

    //    public void RemoveComponent<T>(T component) where T : Component
    //    {
    //        RemoveComponent<T>();
    //    }

    //    public T? GetComponent<T>() where T : Component
    //    {
    //        if (_components.TryGetValue(typeof(T), out var component))
    //        {
    //            return (T)component;
    //        }
    //        return null;
    //    }
    //    public void Update()
    //    {
    //        foreach (var component in _componentsToAdd)
    //        {
    //            _components.Add(component.Key, component.Value);
    //        }
    //        _componentsToAdd.Clear();

    //        foreach (var component in _componentsToRemove)
    //        {
    //            _components.Remove(component);
    //        }
    //        _componentsToRemove.Clear();
    //    }
    //}

    //public class Component
    //{
    //    public Component()
    //    {

    //    }
    //}

    //public class Entity
    //{
    //    private EntityManager _entityManager;

    //    public Guid Id { get; }

    //    public Entity(EntityManager entityManager, Guid id)
    //    {
    //        Id = id;
    //        _entityManager = entityManager;
    //    }

    //    public T AddComponent<T>(T component) where T : Component
    //    {
    //        return _entityManager.AddComponent(Id, component);
    //    }

    //    public void RemoveComponent<T>() where T : Component
    //    {
    //        _entityManager.RemoveComponent<T>(Id);
    //    }

    //    public void RemoveComponent<T>(T component) where T : Component
    //    {
    //        _entityManager.RemoveComponent<T>(Id);
    //    }

    //    public T? GetComponent<T>() where T : Component
    //    {
    //        return _entityManager.GetComponent<T>(Id);
    //    }

    //    public void Destroy()
    //    {
    //        _entityManager.DestroyEntity(Id);
    //    }

    //}

    //public class System
    //{
    //    public System()
    //    {

    //    }
    //}

    //public class EntityFactory
    //{
    //    public EntityFactory()
    //    {

    //    }
    //}

    //public class SceneFactory
    //{
    //    public SceneFactory()
    //    {

    //    }
    //}
}