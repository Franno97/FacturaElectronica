using Mre.Visas.FacturaElectronica.Application.Formats;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Text;

namespace Mre.Visas.FacturaElectronica.Application.Helpers
{
    public static class CommonHelpers
    {
        public static StringContent GenerateHttpContent(object content)
        {
            if (content is null)
            {
                return new StringContent(string.Empty);
            }

            JsonSerializerSettings _jsonSerializerSettings = new()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var serializedContent = JsonConvert.SerializeObject(content, _jsonSerializerSettings);
            var httpContent = new StringContent(serializedContent, Encoding.UTF8, ApiFormats.ApplicationMediaType);

            return httpContent;
        }
    }
}