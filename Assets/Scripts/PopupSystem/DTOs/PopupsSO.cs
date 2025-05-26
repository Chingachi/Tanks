using System;
using System.Collections.Generic;
using Core.PopupSystem.Components;
using UnityEngine;
namespace Core.PopupSystem.Dtos
{
  [CreateAssetMenu(fileName = "Popups", menuName = "ScriptableObjects/Popups", order = 1)]
  public class PopupsSO : ScriptableObject
  {
    [SerializeField]
    private List<PopupDto> _popups = new List<PopupDto>();
    [SerializeField]
    private GameObject PopupCanvas;

    public GameObject GetCanvasInstance()
    {
      return PopupCanvas;
    }

    public BasePopup GetPopupInstance (Type dataPopupType)
    {

      foreach (PopupDto popupDto in _popups) {
        BasePopup popupResult = popupDto.GetPopupInstance(dataPopupType);

        if (popupResult != null) {
          return popupResult;
        }
      }

      return null;
    }
  }
}