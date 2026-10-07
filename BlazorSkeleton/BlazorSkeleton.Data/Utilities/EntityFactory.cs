using System.Data;
using System.Reflection;

namespace BlazorSkeleton.Data.Utilities
{
    /// <summary>Maps DataTable rows onto objects by matching column names to public property names (case-insensitive).</summary>
    public static class EntityFactory
    {
        public static List<T> GetEntities<T>(DataTable table)
        {
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && table.Columns.Contains(p.Name))
                .ToList();

            var entities = new List<T>(table.Rows.Count);
            foreach (DataRow row in table.Rows)
            {
                var entity = Activator.CreateInstance<T>();
                foreach (var prop in props)
                {
                    var value = row[prop.Name];
                    if (value is DBNull) continue;

                    prop.SetValue(entity, ConvertValue(value, prop.PropertyType));
                }
                entities.Add(entity);
            }

            return entities;
        }

        internal static object? ConvertValue(object? value, Type targetType)
        {
            if (value == null) return null;

            var type = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (type.IsInstanceOfType(value)) return value;
            if (type.IsEnum) return Enum.Parse(type, value.ToString()!, ignoreCase: true);

            return Convert.ChangeType(value, type, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
