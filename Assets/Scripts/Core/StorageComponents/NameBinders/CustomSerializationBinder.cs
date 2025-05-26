using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
namespace Core.StorageComponents.NameBinders
{
  public class CustomSerializationBinder : SerializationBinder
  {
    private readonly Dictionary<string, Type> _typeMapping = new Dictionary<string, Type>();

    public override Type BindToType (string assemblyName, string typeName)
    {
      string fullTypeName = $"{typeName}, {assemblyName}";

      if (_typeMapping.TryGetValue(fullTypeName, out Type type)) {
        return type;
      }

      Type resolvedType = Type.GetType(fullTypeName);

      if (resolvedType != null) {
        return resolvedType;
      }

      Debug.LogError($"Не удалось найти тип: {fullTypeName}");

      return null;
    }

    public override void BindToName (Type serializedType, out string assemblyName, out string typeName)
    {
      assemblyName = serializedType.Assembly.FullName;
      typeName = serializedType.FullName;
    }
  }
}