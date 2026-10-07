using Microsoft.Extensions.Logging;

namespace BlazorSkeleton.Data.Utilities
{
    /// <summary>
    /// Generic parsing, logging and copy helpers. Put feature-specific validation (for example,
    /// bulk-upload row checks) in a region of its own here, or in a feature-specific helper.
    /// </summary>
    public static class ValidationHelper
    {
        public static void LogUiError(Exception ex, string customErrorMsg, ILogger logger, string methodName, string pageName)
        {
            logger.LogError(ex, "From Method {Method} while on page {Page} -> UI Error occurred: {CustomMessage}",
                methodName, pageName, customErrorMsg);
        }

        #region Parsing (null when the value can't be parsed)

        public static int? ParseToInt32(string? value) => int.TryParse(value, out var v) ? v : null;

        public static decimal? ParseToDecimal(string? value) => decimal.TryParse(value, out var v) ? v : null;

        public static float? ParseToFloat(string? value) => float.TryParse(value, out var v) ? v : null;

        public static bool? ParseToBoolean(string? value) => bool.TryParse(value, out var v) ? v : null;

        #endregion

        #region Table Edit Validation

        public static bool TryParseBoolean(object? value, out bool result) => bool.TryParse(value?.ToString(), out result);

        public static bool TryParseInt(object? value, out int result) => int.TryParse(value?.ToString(), out result);

        public static bool TryParseDecimal(object? value, out decimal result) => decimal.TryParse(value?.ToString(), out result);

        public static bool TryParseDateTime(object? value, out DateTime result) => DateTime.TryParse(value?.ToString(), out result);

        #endregion

        #region Copy Object Value

        /// <summary>
        /// Copies property values with matching name and type from one object to another.
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="parent">Source Object</param>
        /// <param name="child">Destination Object</param>
        public static void Copy<T>(T parent, T child)
        {
            var parentProperties = parent?.GetType().GetProperties();
            var childProperties = child?.GetType().GetProperties();
            if (parentProperties == null || childProperties == null) return;

            foreach (var parentProperty in parentProperties)
            {
                var childProperty = childProperties.FirstOrDefault(c =>
                    c.Name == parentProperty.Name && c.PropertyType == parentProperty.PropertyType && c.CanWrite);

                childProperty?.SetValue(child, parentProperty.GetValue(parent));
            }
        }

        #endregion
    }
}
