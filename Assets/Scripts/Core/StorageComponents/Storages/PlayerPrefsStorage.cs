using Newtonsoft.Json;
using UnityEngine;
namespace Core.StorageComponents.Storages
{
  public class PlayerPrefsStorage<T> : Storage<T>
    where T : new()
  {

    protected override void LoadData()
    {
      string stringData = PlayerPrefs.GetString(GetKey());
      _data = JsonConvert.DeserializeObject<T>(stringData);
    }

    protected override void SaveData()
    {
      string dataToSave = JsonConvert.SerializeObject(_data, GetSettings());
      PlayerPrefs.SetString(GetKey(), dataToSave);
    }
  }
}