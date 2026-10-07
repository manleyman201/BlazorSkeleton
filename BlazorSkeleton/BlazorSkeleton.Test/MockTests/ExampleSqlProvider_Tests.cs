using BlazorSkeleton.Data.Models;
using BlazorSkeleton.Data.Providers;

namespace BlazorSkeleton.Test.MockTests
{
    public class ExampleSqlProvider_Tests : MockTestBase
    {
        [Fact]
        public async Task FetchExampleItems_MapsRowsFromDataProvider()
        {
            var table = new DataTable();
            table.Columns.Add(nameof(ExampleItem.ID), typeof(int));
            table.Columns.Add(nameof(ExampleItem.NAME), typeof(string));
            table.Rows.Add(1, "One");
            table.Rows.Add(2, "Two");

            var provider = new ExampleSqlProvider(Mock.Of<ILogger<ExampleSqlProvider>>(), GetMockSqlDataProvider(table));

            var result = await provider.FetchExampleItemsAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal("Two", result[1].NAME);
        }

        [Fact]
        public async Task FetchExampleItems_WrapsFailuresInDataException()
        {
            var dataProvider = new Mock<ISqlDataProvider>();
            dataProvider
                .Setup(o => o.GetDataTableFromProcedure(It.IsAny<SQLQuery>(), It.IsAny<string>()))
                .Throws(new InvalidOperationException("connection failed"));

            var provider = new ExampleSqlProvider(Mock.Of<ILogger<ExampleSqlProvider>>(), dataProvider.Object);

            await Assert.ThrowsAsync<DataException>(() => provider.FetchExampleItemsAsync());
        }
    }
}
