using Newtonsoft.Json;

namespace Project_Based_Learning.Helpers
{
    namespace Project_Based_Learning.Helpers
    {
        public static class CookieHelper
        {
            public static void SetObjectAsJson(this HttpResponse response, string key, object value, int days = 7)
            {
                var options = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(days),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };

                var json = JsonConvert.SerializeObject(value);
                response.Cookies.Append(key, json, options);
            }

            public static T? GetObjectFromJson<T>(this HttpRequest request, string key)
            {
                if (request.Cookies.TryGetValue(key, out string value))
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
                return default;
            }

            public static void RemoveCookie(this HttpResponse response, string key)
            {
                response.Cookies.Delete(key);
            }
        }
    }

}
