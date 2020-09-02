using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace Imagekit.UnitTests
{
    [Collection("Uses Utils.HttpClient")]
    public class ServerImagekitTests
    {
        [Fact]
        public void Constructor()
        {
            var imagekit = new ServerImagekit("test publicKey", "test privateKey", "test urlEndpoint", "test path");
            Assert.NotNull(imagekit);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_Required_PublicKey(string publicKey)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ServerImagekit(publicKey, "test privateKey", "test urlEndpoint", "test path"));
            Assert.Equal("publicKey", ex.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_Required_PrivateKey(string privateKey)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ServerImagekit("test publicKey", privateKey, "test urlEndpoint", "test path"));
            Assert.Equal("privateKey", ex.ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Constructor_Required_UrlEndpoint(string urlEndpoint)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ServerImagekit("test publicKey", "test privateKey", urlEndpoint, "test path"));
            Assert.Equal("urlEndpoint", ex.ParamName);
        }

        [Fact]
        public void Upload()
        {
            var fileName = Guid.NewGuid().ToString();
            var fileUrl = "https://test.com/test.png";
            var responseObj = TestHelpers.ImagekitResponseFaker.Generate();
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(responseObj))
            };
            var httpClient = TestHelpers.GetTestHttpClient(httpResponse,
                TestHelpers.GetUploadRequestMessageValidator(fileUrl, fileName));
            Util.Utils.SetHttpClient(httpClient);

            var imagekit = new ServerImagekit("test publicKey", "test privateKey", "test urlEndpoint", "test path")
                .FileName(fileName);
            var response = imagekit.Upload(fileUrl);
            Assert.Equal(JsonConvert.SerializeObject(responseObj), JsonConvert.SerializeObject(response));
        }

        [Fact]
        public async Task UploadAsync()
        {
            var fileName = Guid.NewGuid().ToString();
            var fileUrl = "https://test.com/test.png";
            var responseObj = TestHelpers.ImagekitResponseFaker.Generate();
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(responseObj))
            };
            var httpClient = TestHelpers.GetTestHttpClient(httpResponse,
                TestHelpers.GetUploadRequestMessageValidator(fileUrl, fileName));
            Util.Utils.SetHttpClient(httpClient);

            var imagekit = new ServerImagekit("test publicKey", "test privateKey", "test urlEndpoint", "test path")
                .FileName(fileName);
            var response = await imagekit.UploadAsync(fileUrl);
            Assert.Equal(JsonConvert.SerializeObject(responseObj), JsonConvert.SerializeObject(response));
        }
    }
}