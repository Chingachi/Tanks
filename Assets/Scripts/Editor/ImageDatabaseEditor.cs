using System.Collections.Generic;
using System.Linq;
using Common.SO;
using UnityEditor;
using UnityEngine;
namespace Editor
{
  #if UNITY_EDITOR
  [InitializeOnLoad]
  public static class ImageDatabaseEditor
  {
    private static readonly string imagesPath = "Assets/Images";
    private static readonly string databasePath = "Assets/Resources/ImageDatabase.asset";
    private static readonly ImageDatabase imageDatabase;

    private static void OnAssetsChanged (string packagename)
    {
      UpdateImageDatabase();
    }

    private static void OnAssetsChanged()
    {
      UpdateImageDatabase();
    }

    private static void UpdateImageDatabase()
    {
      string [] textureGuids = AssetDatabase.FindAssets("t:Texture2D", new []
      {
        imagesPath
      });

      List<Sprite> sprites = new List<Sprite>();

      foreach (string guid in textureGuids) {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        Object [] subAssets = AssetDatabase.LoadAllAssetsAtPath(path);

        foreach (Object obj in subAssets) {
          if (obj is Sprite sprite) {
            sprites.Add(sprite);
          }
        }
      }

      bool isChanged = false;

      for (int i = imageDatabase.sprites.Count - 1; i >= 0; i--) {
        SpriteEntry entry = imageDatabase.sprites[i];

        if (string.IsNullOrEmpty(entry.SpriteName) || !sprites.Any(x => x.name == entry.SpriteName) || !sprites.Contains(entry.Sprite) || entry.Sprite == null) {
          imageDatabase.sprites.RemoveAt(i);
          isChanged = true;
        }
      }

      foreach (Sprite sprite in sprites) {
        if (!imageDatabase.ContainsValue(sprite.name)) {
          imageDatabase.Add(sprite.name, sprite);
          isChanged = true;
        }
      }

      if (isChanged) {
        EditorUtility.SetDirty(imageDatabase);
        AssetDatabase.SaveAssets();
        Debug.Log("Sprite database updated");
      }
    }

    static ImageDatabaseEditor()
    {
      AssetDatabase.importPackageCompleted += OnAssetsChanged;
      EditorApplication.projectChanged += OnAssetsChanged;

      imageDatabase = AssetDatabase.LoadAssetAtPath<ImageDatabase>(databasePath);

      if (imageDatabase == null) {
        imageDatabase = ScriptableObject.CreateInstance<ImageDatabase>();
        AssetDatabase.CreateAsset(imageDatabase, databasePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
      }

      UpdateImageDatabase();
    }
  }
  #endif
}