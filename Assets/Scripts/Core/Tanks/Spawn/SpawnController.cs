using System.Collections.Generic;
using System.Linq;
using Core.MonoPool;
using Core.Player;
using Core.StorageComponents.Storages;
using Core.Tanks.AI;
using Core.Tanks.Events;
using Cysharp.Threading.Tasks;
using EventSystemComponents;
using SaveLoad;
using UnityEngine;
using Zenject;
namespace Core.Tanks.Spawn
{
  public class SpawnController : MonoBehaviour
  {
    private readonly Dictionary<string, BaseAi> _spawnedTanks = new Dictionary<string, BaseAi>();
    [SerializeField]
    private PlayerTank _playerPrefab;
    [SerializeField]
    private List<Transform> _playerSpawnPoints;
    [SerializeField]
    private float _playerRespawnTime = 1;

    [SerializeField]
    private SpawnPoint [] _spawnPoints;
    [SerializeField]
    private TanksDatabase _database;

    [SerializeField]
    private Transform _tanksContainer;
    [SerializeField]
    private float _spawnInterval = 2f;
    [SerializeField]
    private int _tanksLimit = 20;

    private MultiMonoPool<BaseAi> _tanksPool;

    private DiContainer _container;
    private EventManager _eventManager;
    private Storage<FieldTanksData> _storage;

    private PlayerTank _player;

    private void Awake()
    {
      _container.Inject(this);
      _eventManager.SubscribeEvent<DestroyAiTankEvent>(HandleTankDestroy);
    }

    private void Start()
    {
      CreatePool();
      SpawnPlayer();
      SpawnSavedTanks();
      StartSpawningAi();
    }

    private void OnApplicationQuit()
    {
      List<BaseAi> spawnedTanks = _spawnedTanks.Values.ToList();

      FieldTanksData data = new FieldTanksData
      {
        Tanks = spawnedTanks.Select(t => new TankSaveData
        {
          Id = t.Id,
          TankType = t.GetType(),
          TransformData = new TankPositionSaveData
          {
            Position = new CustomVector3(t.transform.position),
            Rotation = new CustomVector3(t.transform.rotation)
          }
        }).ToList(),
        Player = new TankPositionSaveData
        {
          Position = new CustomVector3(_player.transform.position),
          Rotation = new CustomVector3(_player.transform.rotation)
        }
      };


      _storage.UpdateData(data);
    }

    [Inject]
    public void Construct (DiContainer container, EventManager eventManager, Storage<FieldTanksData> storage)
    {
      _container = container;
      _eventManager = eventManager;
      _storage = storage;
    }

    private async void SpawnSavedTanks()
    {
      if (_storage.Data.Tanks.Count == 0) {
        return;
      }

      foreach (TankSaveData tank in _storage.Data.Tanks) {
        SpawnTank(tank);
      }

      _player.transform.position = _storage.Data.Player.Position.ToVector3();
      _player.transform.rotation = _storage.Data.Player.Rotation.ToQuaternion();
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
      _tanksPool = new MultiMonoPool<BaseAi>(_container, _database.TankList, _tanksContainer);
    }

    private async void StartSpawningAi()
    {
      do {
        await UniTask.WaitForSeconds(_spawnInterval);
        SpawnTank();
      } while (true);
    }

    private void SpawnPlayer()
    {
      if (_player == null) {
        _player = Instantiate(_playerPrefab);
        _container.Inject(_player);
        _container.BindInstance(_player);
        _player.OnDeath += HandlePlayerDeath;
      }

      _player.gameObject.SetActive(true);
      _player.transform.position = _playerSpawnPoints[Random.Range(0, _playerSpawnPoints.Count)].position;
      _player.transform.rotation = Quaternion.identity;
      _player.HandleSpawn();
    }

    private async void HandlePlayerDeath()
    {
      _player.gameObject.SetActive(false);
      await UniTask.WaitForSeconds(_playerRespawnTime);
      SpawnPlayer();
    }

    private void SpawnTank()
    {
      if (_spawnedTanks.Count >= _tanksLimit) {
        return;
      }

      BaseAi tank = _tanksPool.GetRandom();
      Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform;

      tank.transform.position = spawnPoint.position;
      tank.transform.rotation = spawnPoint.rotation;

      _spawnedTanks.Add(tank.Id, tank);

      tank.gameObject.SetActive(true);
    }

    private void SpawnTank (TankSaveData data)
    {
      BaseAi tank = _tanksPool.GetByType(data.TankType);
      tank.SetId(data.Id);
      tank.transform.position = data.TransformData.Position.ToVector3();
      tank.transform.position = new Vector3(tank.transform.position.x, 1, tank.transform.position.z);
      tank.transform.rotation = data.TransformData.Rotation.ToQuaternion();
      _spawnedTanks.Add(tank.Id, tank);
      tank.gameObject.SetActive(true);

    }
  }
}