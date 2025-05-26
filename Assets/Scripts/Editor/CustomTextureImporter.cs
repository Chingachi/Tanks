using System;
using UnityEditor;
using UnityEngine;
namespace Editor
{
  public class CustomTextureImporter : AssetPostprocessor
  {
    private void OnPreprocessTexture()
    {
      string assetPath = assetPath = assetImporter.assetPath;

      if (assetPath.StartsWith("Assets/Images/", StringComparison.OrdinalIgnoreCase)) {
        TextureImporter importer = (TextureImporter)assetImporter;

        importer.textureType = TextureImporterType.Sprite;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.filterMode = FilterMode.Point;
      }
    }
  }
}