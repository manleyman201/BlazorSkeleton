using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using BlazorSkeleton.Data.Extensions;
using BlazorSkeleton.Data.Providers.Sql;

namespace BlazorSkeleton.Data.Utilities
{
    /// <summary>
    /// Shared stored-procedure execution used by every SQL provider. Gives all data access the same
    /// logging, time limit, and exception translation (SQL errors surface to the UI as DataException).
    /// </summary>
    public static class SqlHelper
    {
        /// <summary>Executes a procedure and maps the first result set to a list of <typeparamref name="T"/>.</summary>
        public static Task<List<T>> ExecuteStoredProcedureAsync<T>(SQLQuery sqlQuery, string connectionName, string methodName,
            ILogger logger, ISqlDataProvider dataProvider, bool isExport = false)
            => Execute(sqlQuery, methodName, logger, isExport,
                () => EntityFactory.GetEntities<T>(dataProvider.GetDataTableFromProcedure(sqlQuery, connectionName)));

        /// <summary>Executes a procedure and returns the first result set as a raw DataTable.</summary>
        public static Task<DataTable> ExecuteStoredProcedureAsync(SQLQuery sqlQuery, string connectionName, string methodName,
            ILogger logger, ISqlDataProvider dataProvider)
            => Execute(sqlQuery, methodName, logger, false,
                () => dataProvider.GetDataTableFromProcedure(sqlQuery, connectionName));

        /// <summary>Executes a procedure and returns one column from the first row (default when there are no rows or the value is NULL).</summary>
        public static Task<T?> ExecuteStoredProcedureReturnOneValueAsync<T>(SQLQuery sqlQuery, string connectionName, string methodName,
            string columnName, ILogger logger, ISqlDataProvider dataProvider)
            => Execute<T?>(sqlQuery, methodName, logger, false, () =>
            {
                var ds = dataProvider.GetDataSetFromProcedure(sqlQuery, connectionName);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return default;

                var value = ds.Tables[0].Rows[0][columnName];
                return value is DBNull ? default : (T?)value;
            });

        /// <summary>Executes an INSERT/UPDATE/DELETE procedure and returns the affected row count.</summary>
        public static Task<int> ExecuteStoredProcedureForDMLAsync(SQLQuery sqlQuery, string connectionName, string methodName,
            ILogger logger, ISqlDataProvider dataProvider)
            => Execute(sqlQuery, methodName, logger, false,
                () => dataProvider.ExecuteStoredProcedure(sqlQuery, connectionName));

        /// <summary>Converts a list to a DataTable (column per public property), e.g. for table-valued parameters.</summary>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (var item in items)
            {
                var values = new object?[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private static Task<TResult> Execute<TResult>(SQLQuery sqlQuery, string methodName, ILogger logger, bool isExport, Func<TResult> action)
        {
            logger.LogEntry();
            try
            {
                var minutes = isExport ? Constants.ExportCancellationTokenTimeDuration : Constants.CancellationTokenTimeDuration;

                // Note: the underlying provider call is synchronous, so this flags an overrun after the
                // call returns rather than aborting it. Set the command timeout on the provider for a hard stop.
                using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(minutes));
                var result = action();
                if (cts.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                return Task.FromResult(result);
            }
            catch (SqlException sqlEx)
            {
                logger.LogError(sqlEx, "From Method {Method} while calling {Procedure} -> SQL Error occurred: {Message}",
                    methodName, sqlQuery.StoredProcedureName, sqlEx.Message);
                throw new DataException($"Database Exception occurred from Method {methodName} while executing {sqlQuery.StoredProcedureName}", sqlEx);
            }
            catch (OperationCanceledException canceledEx)
            {
                logger.LogError(canceledEx, "From Method {Method} while calling {Procedure} -> Time limit exceeded: {Message}",
                    methodName, sqlQuery.StoredProcedureName, canceledEx.Message);
                throw new DataException($"Time limit exceeded in Method {methodName} while executing {sqlQuery.StoredProcedureName}", canceledEx);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "From Method {Method} while calling {Procedure} -> Unable to get data: {Message}",
                    methodName, sqlQuery.StoredProcedureName, ex.Message);
                throw new DataException($"Unexpected Error occurred from Method {methodName} while executing {sqlQuery.StoredProcedureName}", ex);
            }
            finally
            {
                logger.LogExit();
            }
        }
    }
}
