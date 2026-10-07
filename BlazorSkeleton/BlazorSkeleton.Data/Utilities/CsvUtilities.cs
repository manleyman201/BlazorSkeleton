using System.Reflection;
using System.Text;

namespace BlazorSkeleton.Data.Utilities
{
    /// <summary>
    /// Minimal CSV read/write. The first row is a header; columns map to public properties by name (case-insensitive).
    /// Handles quoted fields, embedded commas, doubled quotes and line breaks inside quotes.
    /// </summary>
    public static class CsvUtilities
    {
        /// <summary>Parses CSV text into a list of <typeparamref name="T"/>. Columns with no matching property are ignored.</summary>
        public static List<T> GetCsvRecords<T>(string content)
        {
            var rows = ParseRows(content);
            if (rows.Count == 0) return [];

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
            var columns = rows[0].Select(h => props.GetValueOrDefault(h.Trim())).ToArray();

            var records = new List<T>(rows.Count - 1);
            foreach (var row in rows.Skip(1))
            {
                var record = Activator.CreateInstance<T>();
                for (int i = 0; i < columns.Length && i < row.Count; i++)
                {
                    var prop = columns[i];
                    if (prop == null || string.IsNullOrEmpty(row[i])) continue;

                    prop.SetValue(record, EntityFactory.ConvertValue(row[i], prop.PropertyType));
                }
                records.Add(record);
            }

            return records;
        }

        /// <summary>Writes the items as UTF-8 CSV bytes, one column per public property.</summary>
        public static byte[] CreateCsvFile<T>(IEnumerable<T> items)
        {
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead).ToArray();
            var sb = new StringBuilder();

            sb.AppendLine(string.Join(",", props.Select(p => Escape(p.Name))));
            foreach (var item in items)
            {
                sb.AppendLine(string.Join(",", props.Select(p => Escape(Convert.ToString(p.GetValue(item), System.Globalization.CultureInfo.InvariantCulture)))));
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private static string Escape(string? value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.IndexOfAny([',', '"', '\r', '\n']) >= 0
                ? $"\"{value.Replace("\"", "\"\"")}\""
                : value;
        }

        private static List<List<string>> ParseRows(string content)
        {
            var rows = new List<List<string>>();
            var row = new List<string>();
            var field = new StringBuilder();
            var inQuotes = false;

            for (int i = 0; i < content.Length; i++)
            {
                var c = content[i];
                if (inQuotes)
                {
                    if (c == '"' && i + 1 < content.Length && content[i + 1] == '"') { field.Append('"'); i++; }
                    else if (c == '"') inQuotes = false;
                    else field.Append(c);
                }
                else if (c == '"') inQuotes = true;
                else if (c == ',') { row.Add(field.ToString()); field.Clear(); }
                else if (c == '\r' || c == '\n')
                {
                    if (c == '\r' && i + 1 < content.Length && content[i + 1] == '\n') i++;
                    row.Add(field.ToString()); field.Clear();
                    if (row.Any(f => f.Length > 0)) rows.Add(row);
                    row = [];
                }
                else field.Append(c);
            }

            row.Add(field.ToString());
            if (row.Any(f => f.Length > 0)) rows.Add(row);

            return rows;
        }
    }
}
