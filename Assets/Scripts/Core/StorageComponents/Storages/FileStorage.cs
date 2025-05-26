using System.IO;
using Newtonsoft.Json;
using UnityEngine;
namespace Core.StorageComponents.Storages
{
  public class FileStorage<T> : Storage<T>
    where T : new()
  {
    protected override void LoadData()
    {
      if (!File.Exists(GetFileName())) {
        _data = new T();

        return;
      }

      StreamReader sr = new StreamReader(GetFileName());

      using (sr) {
        string readData = sr.ReadToEnd();

        _data = JsonConvert.DeserializeObject<T>(readData, GetSettings());
      }
    }

    protected override void SaveData()
    {
      StreamWriter sw = new StreamWriter(GetFileName());

      using (sw) {

        string dataToSave = JsonConvert.SerializeObject(_data, GetSettings());
        sw.Write(dataToSave);
      }
    }

    private string GetFileName()
    {
      return $"{Application.persistentDataPath}/{GetKey()}.dt";
    }
  }
}