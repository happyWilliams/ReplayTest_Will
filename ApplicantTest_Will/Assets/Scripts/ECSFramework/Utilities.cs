using System;
using System.Reflection;

namespace ECSFramework
{
    public enum ETickStatus
    {
        Auto,
        Pause,
        Stop
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
    }
}