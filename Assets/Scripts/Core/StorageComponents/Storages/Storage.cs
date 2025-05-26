using System.Collections.Generic;
using Common.SO;
using Core.StorageComponents.Converters;
using Core.StorageComponents.NameBinders;
using Newtonsoft.Json;
using Zenject;
namespace Core.StorageComponents.Storages
{
  public abstract class Storage<T>
    where T : new()
  {
    [Inject]
    protected ImageDatabase _imageDatabase;

    protected T _data;

    [Inject]
    public void Construct (ImageDatabase imageDatabase)
    {
      _imageDatabase = imageDatabase;
      LoadData();
    }

    public virtual void UpdateData()
    {
      SaveData();
    }

    public virtual void UpdateData (T newData)
    {
      _data = newData;
      SaveData();
    }

    protected abstract void LoadData();

    protected abstract void SaveData();

    protected virtual JsonSerializerSettings GetSettings()
    {

      JsonSerializerSettings settings = new JsonSerializerSettings
      {
        TypeNameHandling = TypeNameHandling.Auto,
        Converters = new List<JsonConverter>
        {
          new SpriteConverter(_imageDatabase)
        },
        Binder = new CustomSerializationBinder()
      };

      return settings;
    }

    protected virtual string GetKey()
    {
      return typeof(T).Name;
    }

    public T Data
    {
      get
      {
        if (_data == null) {
          _data = new T();
        }

        return _data;
      }
    }
  }
}