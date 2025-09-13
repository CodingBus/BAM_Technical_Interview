using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StargateAPI.Business.Commands;
using StargateAPI.Controllers;
using Xunit;

namespace StargateAPI.Tests
{
    public class PersonControllerTests
    {
        [Fact]
        public async Task CreatePerson_ShouldReturnSuccess_WhenMediatorReturnsResult()
        {
            var mockMediator = new Mock<IMediator>();
            var controller = new PersonController(mockMediator.Object);

            var requestName = "John Smith";
            var mediatorResponse = new CreatePersonResult { Id = 123, Success = true };

            mockMediator
                .Setup(m => m.Send(It.Is<CreatePerson>(r => r.Name == requestName), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            var result = await controller.CreatePerson(requestName);

            var okResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsType<CreatePersonResult>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal(mediatorResponse.Id, ((CreatePersonResult)response).Id);

            // Verify that mediator.Send ran once
            mockMediator.Verify(m => m.Send(It.IsAny<CreatePerson>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreatePerson_ShouldReturnError_WhenMediatorThrowsException()
        {
            var mockMediator = new Mock<IMediator>();
            var controller = new PersonController(mockMediator.Object);

            var requestName = "John Smith";

            mockMediator
                .Setup(m => m.Send(It.IsAny<CreatePerson>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Bad Request"));

            var result = await controller.CreatePerson(requestName);

            var objectResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsType<BaseResponse>(objectResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Bad Request", response.Message);

            mockMediator.Verify(m => m.Send(It.IsAny<CreatePerson>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
