using BasketApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BasketApi.Tests.Controllers
{
    public class StatusControllerTests
    {
        [Fact]
        public void Get_ReturnsOk()
        {
            var sut = new StatusController();

            var actual = sut.Get();

            Assert.IsType<OkResult>(actual);
        }
    }
}
