using Core.Player.Events;
using Core.Player.Movement;
using Core.Player.Shooting.FrontShooting;
using Core.Player.Shooting.TurretShooting;
using Core.StorageComponents.Storages;
using Core.Tanks.Events;
using EventSystemComponents;
using SaveLoad;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
namespace UI
{
  public class UIController : MonoBehaviour
  {
    [SerializeField]
    private Button _frontShootingButton;
    [SerializeField]
    private Button _turretShootingButton;
    [SerializeField]
    private Button _classicMovementButton;
    [SerializeField]
    private Button _trackMovementButton;

    [SerializeField]
    private Button _resetAllButton;

    [SerializeField]
    private TMP_Text _deathsField;
    [SerializeField]
    private TMP_Text _killsField;

    [Inject]
    private EventManager _eventManager;
    [Inject]
    private Storage<SessionData> _storage;

    private void Awake()
    {
      _frontShootingButton.onClick.AddListener(() => _eventManager.Fire(new ChangeShootingTypeEvent(typeof(FrontShootingSchema))));
      _turretShootingButton.onClick.AddListener(() => _eventManager.Fire(new ChangeShootingTypeEvent(typeof(TurretShootingSchema))));
      _classicMovementButton.onClick.AddListener(() => _eventManager.Fire(new ChangeMovementTypeEvent(typeof(ClassicMovementSchema))));
      _trackMovementButton.onClick.AddListener(() => _eventManager.Fire(new ChangeMovementTypeEvent(typeof(TrackMovementSchema))));
      _resetAllButton.onClick.AddListener(Reset);
    }

    private void Start()
    {
      _eventManager.SubscribeEvent<DestroyAiTankEvent>(HandleKill);
      _eventManager.SubscribeEvent<PlayerDeathEvent>(HandleDeath);
      UpdateFields();
    }

    private void OnDestroy()
    {
      _eventManager.UnsubscribeEvent<DestroyAiTankEvent>(HandleKill);
      _eventManager.UnsubscribeEvent<PlayerDeathEvent>(HandleDeath);
    }


    private void Reset()
    {
      _storage.Data.EnemyKillCount = 0;
      _storage.Data.PlayerDeathCount = 0;
      _storage.UpdateData();
      UpdateFields();
      _eventManager.Fire(new ResetEvent());
    }

    private void HandleDeath (PlayerDeathEvent eventData)
    {
      _storage.Data.PlayerDeathCount++;
      _storage.UpdateData();
      UpdateFields();
    }

    private void HandleKill (DestroyAiTankEvent eventData)
    {
      _storage.Data.EnemyKillCount++;
      _storage.UpdateData();
      UpdateFields();
    }

    private void UpdateFields()
    {
      _deathsField.text = $"Deaths: {_storage.Data.PlayerDeathCount}";
      _killsField.text = $"Kills: {_storage.Data.EnemyKillCount}";
    }
  }
}