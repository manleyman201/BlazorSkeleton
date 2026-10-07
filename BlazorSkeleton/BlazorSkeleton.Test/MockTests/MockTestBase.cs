namespace BlazorSkeleton.Test.MockTests
{
    /// <summary>Base for tests that run providers against a mocked ISqlDataProvider (no database).</summary>
    public class MockTestBase
    {
        public IConfiguration Configuration { get; set; }
        public IOptions<DBConnections> Connections { get; set; }

        protected void PopulateDataTables(List<DataTable> dataTableList)
        {
            foreach (var dt in dataTableList)
            {
                PopulateDataTable(dt);
            }
        }

        protected void PopulateDataTable(DataTable dt)
        {
            for (int j = 0; j < 10; j++)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var colName = dt.Columns[i].ColumnName;
                    var typeName = dt.Columns[i].DataType.Name;
                    switch (typeName)
                    {
                        case "String":
                            dr[colName] = It.IsAny<string>();
                            break;
                        case "Boolean":
                            dr[colName] = It.IsAny<bool>();
                            break;
                        case "DateTime":
                            dr[colName] = It.IsAny<DateTime>();
                            break;
                        case "TimeSpan":
                            dr[colName] = It.IsAny<TimeSpan>();
                            break;
                        case "Int16":
                            dr[colName] = It.IsAny<short>();
                            break;
                        case "Int32":
                            dr[colName] = It.IsAny<int>();
                            break;
                        case "Decimal":
                            dr[colName] = It.IsAny<decimal>();
                            break;
                        case "Int64":
                            dr[colName] = It.IsAny<long>();
                            break;
                        default:
                            // if the switch statement falls here
                            // the new type needs to be added
                            break;
                    }
                }
                dt.Rows.Add(dr);
            }
        }

        protected DataTable GetDataTable<T>()
        {
            DataTable dt = new DataTable();
            PropertyInfo[] props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.PropertyType.Name.ToLower().Contains("nullable"))
                    dt.Columns.Add(prop.Name, prop.PropertyType.GenericTypeArguments[0]);
                else
                    dt.Columns.Add(prop.Name, prop.PropertyType);
            }
            return dt;
        }

        protected DataSet GetDataSet(List<DataTable> dataTableList)
        {
            DataSet dataSet = new DataSet("MockDataSet");
            foreach (var dt in dataTableList)
            {
                dataSet.Tables.Add(dt);
            }
            return dataSet;
        }

        protected ISqlDataProvider GetMockSqlDataProvider(DataSet ds)
        {
            var mock = new Mock<ISqlDataProvider>();

            // Mock GetDataTableFromProcedure Method
            mock
                .Setup(o => o.GetDataSetFromProcedure(It.IsAny<SQLQuery>(), It.IsAny<string>()))
                .Returns(ds);

            return mock.Object;
        }

        protected ISqlDataProvider GetMockSqlDataProvider(DataTable dt)
        {
            var mock = new Mock<ISqlDataProvider>();

            // Mock GetDataTableFromProcedure Method
            mock
                .Setup(o => o.GetDataTableFromProcedure(It.IsAny<SQLQuery>(), It.IsAny<string>()))
                .Returns(dt);

            // Mock ExecuteStoredProcedure Method
            mock
                .Setup(o => o.ExecuteStoredProcedure(It.IsAny<SQLQuery>(), It.IsAny<string>()))
                .Returns(It.IsAny<int>());

            return mock.Object;
        }

        protected ISqlDataProvider GetMockSqlDataProvider()
        {
            return GetMockSqlDataProvider(new DataTable());
        }

        public MockTestBase()
        {
            Configuration = Mock.Of<IConfiguration>();
            Connections = Mock.Of<IOptions<DBConnections>>();
        }
    }
}
