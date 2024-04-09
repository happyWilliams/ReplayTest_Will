using System;
using System.Reflection;
using UnityEditor;

namespace ECSFramework
{
    public enum ETickStatus
    {
        Auto,
        Pause,
    }

    public static class Utilities
    {
        public static object CreateStructFromEnumValue(EComponentType enumValue)
        {
            FieldInfo fieldInfo = typeof(EComponentType).GetField(enumValue.ToString());
            AssociatedStructAttribute attribute = (AssociatedStructAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(AssociatedStructAttribute));
            if (attribute != null)
            {
                Type structType = attribute.StructType;
                return Activator.CreateInstance(structType);
            }
            else
            {
                throw new ArgumentException("No associated struct found for the enum value.");
            }
        }

        public static object CreateStructFromEnumValue(EComponentType enumValue, params object[] args)
        {
            FieldInfo fieldInfo = typeof(EComponentType).GetField(enumValue.ToString());
            AssociatedStructAttribute attribute = (AssociatedStructAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(AssociatedStructAttribute));
            if (attribute != null)
            {
                Type structType = attribute.StructType;
                return Activator.CreateInstance(structType, args);
            }
            else
            {
                throw new ArgumentException("No associated struct found for the enum value.");
            }
        }

        public static int GetUniqueId()
        {
            var guid = Guid.NewGuid();
            var bytes = guid.ToByteArray();
            var uniqueId = BitConverter.ToInt32(bytes, 0);
            return uniqueId;
        }
    }
}