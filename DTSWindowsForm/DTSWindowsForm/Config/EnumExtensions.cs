using System.ComponentModel;
using System.Reflection;

namespace DTSWindowsForm.Config
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            // Caso não haja uma descrição, retorna o nome do enum
            return value.ToString();
        }

        public static T GetEnumByDescription<T>(string description) where T : Enum
        {
            var type = typeof(T);
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            throw new ArgumentException($"Nenhum valor do enum '{typeof(T)}' corresponde à descrição '{description}'.");
        }
    }
}
