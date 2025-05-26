using System;
using Common.SO;
using Newtonsoft.Json;
using UnityEngine;
namespace Core.StorageComponents.Converters
{
  public class SpriteConverter : JsonConverter
  {
    private readonly ImageDatabase _imageDatabase;

    public SpriteConverter (ImageDatabase imageDatabase)
    {
      _imageDatabase = imageDatabase;
    }

    public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
    {
      Sprite sprite = value as Sprite;

      if (sprite == null) {
        writer.WriteNull();

        return;
      }

      writer.WriteValue(sprite.name);
    }

    public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null) {
        return null;
      }


      string spriteName = reader.Value.ToString();
      Sprite sprite = null;

      if (!string.IsNullOrEmpty(spriteName)) {
        sprite = _imageDatabase.GetSpriteByName(spriteName);
      }

      return sprite;
    }

    public override bool CanConvert (Type objectType)
    {
      return objectType == typeof(Sprite);
    }
  }
}