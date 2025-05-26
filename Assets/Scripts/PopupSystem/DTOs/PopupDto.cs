using System;
using System.Collections.Generic;
using System.Linq;
using Core.PopupSystem.Components;
using UnityEngine;
namespace Core.PopupSystem.Dtos
{
  [Serializable]
  public class PopupDto
  {
    [SerializeField]
    private string _groupName;
    [SerializeField]
    private List<BasePopup> _popups = new List<BasePopup>();



    public BasePopup GetPopupInstance (Type dataPopupType)
    {
      return _popups.FirstOrDefault(x => x.GetType() == dataPopupType);
    }
  }
}