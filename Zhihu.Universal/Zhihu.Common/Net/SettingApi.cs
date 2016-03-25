
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;


namespace Zhihu.Common.Net
{
    public sealed class SettingApi
    {
        public async Task<UseTopStoryResult> CheckUseStory(String access, Boolean autoCache = false)
        {
            var http = new HttpUtility();

            var response = await http.GetAsync(Utility.Instance.BaseUri, "settings/lab", access, autoCache);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                var jsonModel = JsonConvert.DeserializeObject<UseTopStory>(json);

                return new UseTopStoryResult(jsonModel);
            }
            else
            {
                var json = response.Error;
                //json = Regex.Unescape(json);

                return new UseTopStoryResult(new Exception(json));
            }
        }

        public async Task<UseTopStoryResult> SwitchTopStory(String access, Boolean isOn)
        {
            var http = new HttpUtility();

            var body = new StringContent(String.Format("use_topstory={0}", isOn.ToString().ToLower()));
            body.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response =
                await
                    http.PutAsync(Utility.Instance.BaseUri, "settings/lab", access, body);

            if (false == String.IsNullOrEmpty(response.Json))
            {
                var json = response.Json;
                var jsonModel = JsonConvert.DeserializeObject<UseTopStory>(json);

                return new UseTopStoryResult(jsonModel);
            }
            else
            {
                var json = response.Error;

                return new UseTopStoryResult(new Exception(json));
            }
        }
    }
}
