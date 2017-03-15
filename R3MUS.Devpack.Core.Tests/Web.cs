using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Moq;
using System.Text;
using System.IO;
using System.Net;
using FluentAssertions;
using R3MUS.Devpack.Core.HttpAbstraction;

namespace R3MUS.Devpack.Core.Tests
{
    [TestClass]
    public class Web : TestBase
    {
        [TestMethod]
        public void BaseRequest_ReturnsString()
        {
            //  Arrange
            var expected = "response content";
            var expectedBytes = Encoding.UTF8.GetBytes(expected);
            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);

            var response = new Mock<IHttpWebResponse>();
            response.Setup(c => c.GetResponseStream()).Returns(responseStream);

            var request = new Mock<IHttpWebRequest>();
            request.Setup(c => c.GetResponse()).Returns(response.Object);

            var factory = new Mock<IHttpWebRequestFactory>();
            factory.Setup(c => c.Create(It.IsAny<string>()))
                .Returns(request.Object);

            //  Act
            var req = factory.Object.Create(It.IsAny<string>());
            req.Method = WebRequestMethods.Http.Get;

            string result;

            using (var httpWebResponse = req.GetResponse())
            {
                using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }

            //  Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
