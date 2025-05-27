using System;
using System.Collections.Generic;
using UnityEngine;
namespace EventSystemComponents
{
  public class EventManager
  {
    private readonly Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();

    public void SubscribeEvent<T> (Action<T> action)
      where T : BaseEvent
    {
      Type type = typeof(T);

      if (!_events.ContainsKey(type)) {
        _events.TryAdd(type, null);
      }

      Action<T> actionDelegate = _events[type] as Action<T>;
      actionDelegate += action;
      _events[type] = actionDelegate;
    }

    public void UnsubscribeEvent<T> (Action<T> action)
      where T : BaseEvent
    {
      Type type = typeof(T);

      if (!_events.ContainsKey(type)) {
        return;
      }

      Action<T> actionDelegate = _events[type] as Action<T>;
      actionDelegate -= action;
      _events[type] = actionDelegate;
    }

    public void Fire<T> (T signal)
      where T : BaseEvent
    {
      Type signalType = typeof(T);

      if (!_events.ContainsKey(signalType)) {
        Debug.LogWarning($"Event {signalType} has not been subscribed");

        return;
      }

      Action<T> signalDelegate = _events[signalType] as Action<T>;
      signalDelegate?.Invoke(signal);
    }
  }
}