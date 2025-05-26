using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Common.SO
{
  [CreateAssetMenu(fileName = "ImageDatabase", menuName = "ScriptableObjects/ImageDatabase", order = 1), Serializable]
  public class ImageDatabase : ScriptableObject
  {
    public List<SpriteEntry> sprites = new List<SpriteEntry>();

    public Sprite GetSpriteByName (string spriteName)
    {
      return sprites.FirstOrDefault(x => x.SpriteName == spriteName)?.Sprite;
    }

    public bool ContainsValue (string spriteName)
    {
      foreach (SpriteEntry spriteEntry in sprites) {
        if (spriteEntry.SpriteName == spriteName) {
          return true;
        }
      }

      return false;
    }

    public void Add (string spriteName, Sprite sprite)
    {
      sprites.Add(new SpriteEntry(spriteName, sprite));
    }
  }

  [Serializable]
  public class SpriteEntry
  {
    public string SpriteName;
    public Sprite Sprite;

    public SpriteEntry (string spriteName, Sprite sprite)
    {
      SpriteName = spriteName;
      Sprite = sprite;
    }
  }
}