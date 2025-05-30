using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
namespace Core.MonoPool
{
  public class MultiMonoPool<TBase>
    where TBase : MonoBehaviour
  {
    private readonly DiContainer _container;
    private readonly Transform _parent;
    private readonly Dictionary<Type, Queue<TBase>> _pools;
    private readonly Dictionary<Type, TBase> _prefabs;

    public MultiMonoPool (DiContainer container, IEnumerable<TBase> prefabs, Transform parent = null, int initialPerPrefab = 0)
    {
      _container = container;
      _parent = parent;
      _pools = new Dictionary<Type, Queue<TBase>>();
      _prefabs = new Dictionary<Type, TBase>();

      foreach (TBase prefab in prefabs) {
        Type type = prefab.GetType();
        _prefabs[type] = prefab;
        _pools[type] = new Queue<TBase>();

        for (int i = 0; i < initialPerPrefab; i++) {
          TBase instance = CreateInactive(prefab);
          _pools[type].Enqueue(instance);
        }
      }
    }

    public TBase GetRandom()
    {
      List<Type> types = new List<Type>(_prefabs.Keys);
      Type randomType = types[Random.Range(0, types.Count)];

      return GetByType(randomType);
    }

    public T Get<T>()
      where T : TBase
    {
      return (T)GetByType(typeof(T));
    }

    public void Return (TBase tank)
    {
      tank.gameObject.SetActive(false);
      Type type = tank.GetType();
      _pools[type].Enqueue(tank);
    }

    private TBase GetByType (Type type)
    {
      Queue<TBase> queue = _pools[type];

      if (queue.Count > 0) {
        TBase existing = queue.Dequeue();

        return existing;
      }

      TBase prefab = _prefabs[type];
      TBase newObject = _container.InstantiatePrefabForComponent<TBase>(prefab, _parent);

      return newObject;
    }

    private TBase CreateInactive (TBase prefab)
    {
      TBase newObject = _container.InstantiatePrefabForComponent<TBase>(prefab, _parent);
      newObject.gameObject.SetActive(false);

      return newObject;
    }
  }
}