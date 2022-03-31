using Newtonsoft.Json;

namespace Mre.Visas.FacturaElectronica.Application.Helpers
{
    public static class DeserializationHelpers
    {
        public static T DeserializeData<T>(object data)
        {
            var isValidData = data is not null;
            if (!isValidData)
            {
                return default;
            }

            var dataInString = data.ToString();
            var deserializedData = JsonConvert.DeserializeObject<T>(dataInString);

            return deserializedData;
        }
    }
}