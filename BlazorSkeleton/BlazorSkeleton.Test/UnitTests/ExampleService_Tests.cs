using BlazorSkeleton.Data.Models;
using BlazorSkeleton.Data.Providers.Interfaces;
using BlazorSkeleton.Data.Services;

namespace BlazorSkeleton.Test.UnitTests
{
    public class ExampleService_Tests : UnitTestBase
    {
        private readonly IExampleSqlProvider _sqlProvider;
        private readonly ExampleService _service;

        public ExampleService_Tests()
        {
            _sqlProvider = Mock.Of<IExampleSqlProvider>();
            _service = new ExampleService(_sqlProvider, Mock.Of<ILogger<ExampleService>>());
        }

        #region Fetch Example Items

        [Fact]
        public async Task FetchExampleItems_ReturnsProviderResults()
        {
            var expected = new List<ExampleItem> { new() { ID = 1, NAME = "One" }, new() { ID = 2, NAME = "Two" } };
            Mock.Get(_sqlProvider).Setup(s => s.FetchExampleItemsAsync()).ReturnsAsync(expected);

            var result = await _service.FetchExampleItemsAsync();

            Assert.Equal(expected, result);
            Mock.Get(_sqlProvider).Verify(s => s.FetchExampleItemsAsync(), Times.Once);
        }

        [Fact]
        public async Task FetchExampleItems_Rethrows_WhenProviderFails()
        {
            Mock.Get(_sqlProvider).Setup(s => s.FetchExampleItemsAsync()).ThrowsAsync(new DataException("boom"));

            await Assert.ThrowsAsync<DataException>(() => _service.FetchExampleItemsAsync());
        }

        #endregion

        #region Update Example Item

        [Fact]
        public async Task UpdateExampleItem_Returns1_WhenSuccessful()
        {
            var item = new ExampleItem { ID = 1, NAME = "One" };
            Mock.Get(_sqlProvider).Setup(s => s.UpdateExampleItemAsync(item)).ReturnsAsync(1);

            var result = await _service.UpdateExampleItemAsync(item);

            Assert.Equal(1, result);
            Mock.Get(_sqlProvider).Verify(s => s.UpdateExampleItemAsync(item), Times.Once);
        }

        [Fact]
        public async Task UpdateExampleItem_Throws_WhenItemIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateExampleItemAsync(null!));
        }

        #endregion
    }
}
