namespace Api.IntegrationTests
{
    public class UnitTest1 : IClassFixture<ApiWebApplicationFactory>
    {

        private readonly HttpClient _httpClient;


        public UnitTest1(ApiWebApplicationFactory apiWebApplicationFactory)
        {
            _httpClient = apiWebApplicationFactory.CreateClient();
        }


        [Fact]
        public void Test1()
        {
            Console.WriteLine("Hello");
        }
    }
}