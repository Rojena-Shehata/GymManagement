
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace GymManagementPL.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly JsonSerializer _jsonSerializer=new();
        private readonly IDistributedCache _cache;

        public JsonStringLocalizer(IDistributedCache cache)
        {
            _cache = cache;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value=GetString(name);
                return new LocalizedString(name,value);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var actualValue = this[name];
                return actualValue.ResourceNotFound
                                                    ? actualValue
                                                    : new LocalizedString(name, string.Format(actualValue, arguments));
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var filePath = $"Localization/Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
            //if (!File.Exists(filePath))
            //   yield return [];

            var fileStream =new  FileStream(filePath,FileMode.Open,FileAccess.Read,FileShare.Read);
            var streamReader=new   StreamReader(fileStream);
            var  jsonReader=new  JsonTextReader(streamReader);
            while(jsonReader.Read())
            {
                if (JsonToken.PropertyName != jsonReader.TokenType)
                    continue;

                var key = jsonReader.Value as string;
                jsonReader.Read();
                var value = _jsonSerializer.Deserialize<string>(jsonReader);
                yield return new LocalizedString(key, value);
            }

        }
        private string GetString(string key)
        {
            //Resources/ar-EG.json
            //Resources/en-US.json

            var filePath = $"Localization/Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
            var fileFullPath = Path.GetFullPath(filePath);
            if(File.Exists(fileFullPath))
            {
                var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
                //locale_en-US_welcome     //locale_ar-EG_welcome
                var cacheValue=_cache.GetString(cacheKey);

                if(!string.IsNullOrEmpty(cacheValue))
                    return cacheValue;

                var result = GetValueFromJson(key,fileFullPath);

                if(!string.IsNullOrEmpty(result))
                    _cache.SetString(cacheKey, result);

                return result;
            }
            return string.Empty;
        }

        private string  GetValueFromJson(string propertyName/*key*/,  string  filePathe)
        {
            if(string.IsNullOrEmpty(filePathe)&&string.IsNullOrEmpty(propertyName)) 
                return string.Empty;
            if(!File.Exists(filePathe))
                return  string.Empty ;
           using var fileStream=new FileStream(filePathe,FileMode.Open,FileAccess.Read,FileShare.Read);
            using var streamReader = new StreamReader(fileStream);
            using var jsonReader=new JsonTextReader(streamReader);

            while (jsonReader.Read())
            {
                if (JsonToken.PropertyName == jsonReader.TokenType && jsonReader.Value as string == propertyName)
                {
                    jsonReader.Read();  
                    return _jsonSerializer.Deserialize<string>(jsonReader);
                }
            }
                
            

            return string.Empty;
        }
    }
}
