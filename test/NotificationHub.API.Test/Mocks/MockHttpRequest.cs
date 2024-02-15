using Microsoft.AspNetCore.Http;
using Moq;

namespace NotificationHub.API.Test.Mocks;

internal static class MockHttpRequest
{
    internal static Mock<HttpRequest> GetMock(string host, string pathBase)
    {
        var request = new Mock<HttpRequest>();
        request
            .Setup(x => x.Scheme)
            .Returns("http");
        request
            .Setup(x => x.Host)
            .Returns(HostString.FromUriComponent(host));
        request
            .Setup(x => x.PathBase)
            .Returns(PathString.FromUriComponent(pathBase));

        return request;
    }
}
