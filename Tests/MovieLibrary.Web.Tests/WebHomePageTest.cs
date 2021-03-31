namespace MovieLibrary.Web.Tests
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    using Xunit;

    public class WebHomePageTest
    {
        [Fact]
        public async Task HomePageShouldReturnSuccessStatusCode()
        {
            var webApplicationFactory = new WebApplicationFactory<Startup>();
            HttpClient client = webApplicationFactory.CreateClient();

            var response = await client.GetAsync("/");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task HomePageShouldContainCookiesHeader()
        {
            var webApplicationFactory = new WebApplicationFactory<Startup>();
            HttpClient client = webApplicationFactory.CreateClient();

            var response = await client.GetAsync("/Movies/All");
            Assert.True(response.Headers.Contains("set-cookie"));
        }

        [Fact]
        public async Task HomePageShouldContainInformationFromHomePage()
        {
            var webApplicationFactory = new WebApplicationFactory<Startup>();
            HttpClient client = webApplicationFactory.CreateClient();

            var response = await client.GetAsync("/");

            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("<i>Register and view a lot of movies with comments and ratings.</i>", html);

        }

    }
}
