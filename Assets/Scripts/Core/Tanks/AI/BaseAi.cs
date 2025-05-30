using System;
using System.Collections.Generic;
using Core.Projectiles;
using Core.Tanks.Events;
using Core.Tanks.States.Base;
using EventSystemComponents;
using UnityEngine;
using Utils;
using Zenject;
namespace Core.Tanks.AI
{
  [RequireComponent(typeof(Rigidbody))]
  public abstract class BaseAi : MonoBehaviour
  {
    [Inject]
    protected readonly EventManager _eventManager;
    [Inject]
    protected DiContainer _container;

    protected Dictionary<Type, BaseTankState> _states;

    protected BaseTankState _currentState;

    protected virtual void Awake()
    {
      Id = Guid.NewGuid().ToString();
    }

    protected void Start()
    {
      _states = RegisterStates();
      RunFirstState();
    }

    private void OnTriggerEnter (Collider trigger)
    {
      if (trigger.gameObject.layer != Layers.Projectile) {
        return;
      }

      if (trigger.gameObject.GetComponent<Projectile>().ShooterLayer == Layers.Enemy) {

        return;
      }

      HandleDestruction();
    }

    public void SetId (string id)
    {
      Id = id;
    }

    protected abstract void RunFirstState();

    protected virtual void ChangeState<T>()
      where T : BaseTankState
    {
      Type type = typeof(T);

      if (!_states.ContainsKey(type)) {
        Debug.LogError($"No {type} state found!");

        return;
      }

      _currentState?.ExitState();
      _currentState = _states[type];
      _currentState.EnterState();
    }

    protected abstract Dictionary<Type, BaseTankState> RegisterStates();

    protected virtual void HandleDestruction()
    {
      _eventManager.Fire(new DestroyAiTankEvent(Id));
    }

    public string Id { get; private set; }
  }
}