using System.Collections.Generic;
using UnityEngine;
using Zenject;
namespace Core.MonoPool
{
  public class SimpleMonoObjectPool<T>
    where T : MonoBehaviour
  {
    private readonly Queue<T> _pool;
    private readonly Transform _parent;

    private readonly T _prefab;
    private readonly DiContainer _container;

    public SimpleMonoObjectPool (DiContainer container, T prefab, Transform parent, int initialSize = 10)
    {
      _container = container;
      _prefab = prefab;
      _parent = parent;
      _pool = new Queue<T>(initialSize);

      for (int i = 0; i < initialSize; i++) {
        _pool.Enqueue(CreateNewObject());
      }
    }

    public T Get()
    {
      return _pool.Count > 0 ? _pool.Dequeue() : CreateNewObject();
    }

    public void Return (T obj)
    {
      if (obj == null) {
        return;
      }

      obj.gameObject.SetActive(false);
      _pool.Enqueue(obj);
    }

    private T CreateNewObject()
    {
      T newObject = Object.Instantiate(_prefab, _parent);
      _container.Inject(newObject);
      newObject.gameObject.SetActive(false);

      return newObject;
    }
  }
}