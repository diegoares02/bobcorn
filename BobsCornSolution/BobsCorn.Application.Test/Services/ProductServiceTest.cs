using BobsCorn.Application.DTOs;
using BobsCorn.Application.Interfaces;
using BobsCorn.Application.Services;
using Moq;

namespace BobsCorn.Application.Test.Services
{
    public class ProductServiceTest
    {
        [Fact]
        public async Task BuyCornAsyncTooManyRequests()
        {
            var _userProductLogRepositoryMock = new Mock<IUserProductLogRepository>();
            var _productRepositoryMock = new Mock<IProductRepository>();
            var _userRepositoryMock = new Mock<IUserRepository>();
            var dto = new ProductBuyDto()
            {
                Email = It.IsAny<string>(),
                ProductId = It.IsAny<int>()
            };

            _userProductLogRepositoryMock.Setup(x => x.GetLastPurchaseTimeAsync(It.IsAny<int>())).ReturnsAsync(DateTime.UtcNow);
            _userRepositoryMock.Setup(x => x.GetUser(It.IsAny<string>())).Returns(It.IsAny<int>());

            var productService = new ProductService(_userProductLogRepositoryMock.Object, _productRepositoryMock.Object, _userRepositoryMock.Object);

            var response = await productService.BuyCornAsync(dto);

            Assert.Equal(System.Net.HttpStatusCode.TooManyRequests, response.Item1);
        }

        [Fact]
        public async Task BuyCornAsyncCornOutOfStock()
        {
            var _userProductLogRepositoryMock = new Mock<IUserProductLogRepository>();
            var _productRepositoryMock = new Mock<IProductRepository>();
            var _userRepositoryMock = new Mock<IUserRepository>();
            var dto = new ProductBuyDto()
            {
                Email = It.IsAny<string>(),
                ProductId = It.IsAny<int>()
            };

            _userProductLogRepositoryMock.Setup(x => x.GetLastPurchaseTimeAsync(It.IsAny<int>())).ReturnsAsync(DateTime.UtcNow.AddHours(-2));
            _userRepositoryMock.Setup(x => x.GetUser(It.IsAny<string>())).Returns(It.IsAny<int>());
            _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(new Domain.Entities.Product() { Quantity = 0 });

            var productService = new ProductService(_userProductLogRepositoryMock.Object, _productRepositoryMock.Object, _userRepositoryMock.Object);

            var response = await productService.BuyCornAsync(dto);

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.Item1);
        }

        [Fact]
        public async Task BuyCornAsyncSuccess()
        {
            var _userProductLogRepositoryMock = new Mock<IUserProductLogRepository>();
            var _productRepositoryMock = new Mock<IProductRepository>();
            var _userRepositoryMock = new Mock<IUserRepository>();
            var dto = new ProductBuyDto()
            {
                Email = It.IsAny<string>(),
                ProductId = It.IsAny<int>()
            };

            _userProductLogRepositoryMock.Setup(x => x.GetLastPurchaseTimeAsync(It.IsAny<int>())).ReturnsAsync(DateTime.UtcNow.AddHours(-2));
            _userRepositoryMock.Setup(x => x.GetUser(It.IsAny<string>())).Returns(It.IsAny<int>());
            _productRepositoryMock.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync(new Domain.Entities.Product() { Quantity = 10 });

            var productService = new ProductService(_userProductLogRepositoryMock.Object, _productRepositoryMock.Object, _userRepositoryMock.Object);

            var response = await productService.BuyCornAsync(dto);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.Item1);
        }
    }
}
