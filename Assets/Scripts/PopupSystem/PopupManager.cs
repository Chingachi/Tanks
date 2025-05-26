using System;
using System.Collections.Generic;
using System.Linq;
using Core.PopupSystem.Components;
using Core.PopupSystem.Dtos;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
namespace PopupSystem
{
  public class PopupManager
  {
    private readonly PopupsSO _popupList;
    private readonly DiContainer _container;

    private readonly Queue<QueuePopup> _popupInstancesQueue = new Queue<QueuePopup>();
    private readonly List<BasePopup> _visiblePopups = new List<BasePopup>();
    private Transform _canvas;

    public PopupManager (DiContainer container)
    {
      _container = container;
      _popupList = Resources.Load<PopupsSO>("PopupsList");
      InitCanvas();
    }

    public void OpenPopup<T> (T data)
      where T : IPopupData
    {
      BasePopup popupInstance = _popupList.GetPopupInstance(data.GetPopupType());

      if (popupInstance == null) {
        Debug.LogError($"{data.GetPopupType().Name} popup is not in the list");

        return;
      }

      _popupInstancesQueue.Enqueue(new QueuePopup(popupInstance, data));
      CheckQueue();
    }


    public void OpenForcePopup<T> (T data)
      where T : IPopupData
    {
      BasePopup popupInstance = _popupList.GetPopupInstance(data.GetPopupType());

      if (popupInstance == null) {
        Debug.LogError($"{data.GetPopupType().Name} popup is not in the list");

        return;
      }

      ShowPopup(popupInstance, data);
    }

    public void ClosePopup (Type popupType)
    {
      BasePopup popup = _visiblePopups.FirstOrDefault(x => x.GetType() == popupType);

      if (popup == null) {
        return;
      }

      _visiblePopups.Remove(popup);
      Object.Destroy(popup.gameObject);
      CheckQueue();
    }

    public BasePopup GetPopup (Type popupType)
    {
      return _visiblePopups.FirstOrDefault(x => x.GetType() == popupType);
    }

    private void InitCanvas()
    {
      GameObject canvasInstance = _popupList.GetCanvasInstance();
      GameObject canvasGo = Object.Instantiate(canvasInstance);
      canvasGo.name = "PopupCanvas";
      Object.DontDestroyOnLoad(canvasGo);
      _canvas = canvasGo.transform;
    }

    private void CheckQueue()
    {
      if (_popupInstancesQueue.Count == 0 || _visiblePopups.Count > 0) {
        return;
      }

      QueuePopup queueItem = _popupInstancesQueue.Dequeue();
      ShowPopup(queueItem.Instance, queueItem.Data);
    }

    private void ShowPopup (BasePopup instance, IPopupData data)
    {
      GameObject popupGo = _container.InstantiatePrefab(instance, _canvas);
      BasePopup popup = popupGo.GetComponent<BasePopup>();

      popup.SetData(data);

      _visiblePopups.Add(popup);

      popup.OnClose += () =>
      {
        ClosePopup(data.GetPopupType());
      };

      data.Callback?.Invoke(popup);
      popup.Show();
    }

    public bool AnyPopupOpened
    {
      get
      {
        return _popupInstancesQueue.Count > 0 || _visiblePopups.Count > 0;
      }
    }
  }
}