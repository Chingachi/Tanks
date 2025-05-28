using System.Collections.Generic;
using Core.MonoPool;
using Core.Tanks.AI;
using Core.Tanks.Events;
using Cysharp.Threading.Tasks;
using EventSystemComponents;
using UnityEngine;
using Zenject;
namespace Core.Tanks.Spawn
{
  public class SpawnController : MonoBehaviour
  {
    private readonly Dictionary<string, BaseAi> _spawnedTanks = new Dictionary<string, BaseAi>();
    [SerializeField]
    private SpawnPoint [] _spawnPoints;
    [SerializeField]
    private BaseAi _baseTankAiPrefab; //TODO: Change when it comes to polymorph mechanics

    [SerializeField]
    private Transform _tanksContainer;
    [SerializeField]
    private float _spawnInterval = 2f; //TODO: Move to settings
    [SerializeField]
    private int _tanksLimit = 20;

    private SimpleMonoObjectPool<BaseAi> _tanksPool;

    private DiContainer _container;
    private EventManager _eventManager;

    private void Awake()
    {
      _container.Inject(this);
      _eventManager.SubscribeEvent<DestroyAiTankEvent>(HandleTankDestroy);
    }

    private void Start()
    {
      CreatePool();
      StartSpawning();
    }

    [Inject]
    public void Construct (DiContainer container, EventManager eventManager)
    {
      _container = container;
      _eventManager = eventManager;
    }

    private void HandleTankDestroy (DestroyAiTankEvent eventData)
    {
      if (!_spawnedTanks.ContainsKey(eventData.Id)) {
        return;
      }

      BaseAi tank = _spawnedTanks[eventData.Id];
      _spawnedTanks.Remove(tank.Id);
      tank.gameObject.SetActive(false);
      _tanksPool.Return(tank);
    }

    private void CreatePool()
    {
      _tanksPool = new SimpleMonoObjectPool<BaseAi>(_container, _baseTankAiPrefab, _tanksContainer, _tanksLimit);
    }

    private async void StartSpawning()
    {
      do {
        SpawnTank();
        await UniTask.WaitForSeconds(_spawnInterval);
      } while (true);
    }

    private void SpawnTank()
    {
      if (_spawnedTanks.Count >= _tanksLimit) {
        return;
      }

      BaseAi tank = _tanksPool.Get();
      Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform;

      tank.transform.position = spawnPoint.position;
      tank.transform.rotation = spawnPoint.rotation;

      _spawnedTanks.Add(tank.Id, tank);

      tank.gameObject.SetActive(true);
    }
  }
}