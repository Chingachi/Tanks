using System;
using Core.Tanks.Events;
using EventSystemComponents;
using UnityEngine;
using Zenject;
namespace Core.Tanks.AI
{
  [RequireComponent(typeof(Rigidbody))]
  public class BaseTankAi : MonoBehaviour
  {
    [Inject]
    protected readonly EventManager _eventManager;

    protected Rigidbody _rigidbody;

    protected virtual void Awake()
    {
      _rigidbody = GetComponent<Rigidbody>();
      Id = Guid.NewGuid().ToString();
    }

    protected virtual void HandleDestruction()
    {
      _eventManager.Fire(new DestroyAiTankEvent(Id));
    }

    public string Id { get; private set; }
  }
}