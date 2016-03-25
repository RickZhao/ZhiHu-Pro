using System;
using System.IO;
using System.Threading.Tasks;

using Windows.ApplicationModel;

using UmengSDK;
using MicroMsg.sdk;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class SocialService : ISocial
    {
        public async Task<ShareResult> GetQuestionShareTemplate(String access, Int32 questionId,
            Boolean autoCache = false)
        {
            var api = new ShareApi();
            var result = await api.GetQuestionShareTemplate(access, questionId, autoCache);

            return result;
        }

        public async Task<ShareResult> GetAnswerShareTemplate(String access, Int32 answerId,
            Boolean autoCache = false)
        {
            var api = new ShareApi();
            var result = await api.GetAnswerShareTemplate(access, answerId, autoCache);

            return result;
        }

        public async Task<OperationResult> ShareQuestionViaSina(String access, Int32 questionId, String content)
        {
            var api = new ShareApi();
            var result = await api.ShareQuestionViaSina(access, questionId, content);

            return result;
        }

        public async Task<OperationResult> ShareAnswerViaSina(String access, Int32 answerId, String content)
        {
            var api = new ShareApi();
            var result = await api.ShareAnswerViaSina(access, answerId, content);

            return result;
        }

        public async Task<Boolean> ShareQuestionViaWeChatTimeline(String wechatAppId, String title, String description, String pageUrl)
        {
            try
            {
                var scene = SendMessageToWX.Req.WXSceneTimeline;

                var folder = await Package.Current.InstalledLocation.GetFolderAsync("Resource");
                var file = await folder.GetFileAsync("zhihu_m.jpg");

                using (var stream = await file.OpenReadAsync())
                {
                    var pic = new byte[stream.Size];
                    await stream.AsStream().ReadAsync(pic, 0, pic.Length);
                    var message = new WXWebpageMessage
                    {
                        WebpageUrl = pageUrl,
                        Title = title,
                        Description = description,
                        ThumbData = pic
                    };

                    var req = new SendMessageToWX.Req(message, scene);
                    IWXAPI api = WXAPIFactory.CreateWXAPI(wechatAppId);
                    var isValid = await api.SendReq(req);

                    return !String.IsNullOrEmpty(isValid);
                }
            }
            catch (WXException ex)
            {
                await UmengAnalytics.TrackException(ex, "ShareQuestionViaWeChatTimeline: " + ex.Message);
                return false;
            }
        }

        public async Task<Boolean> ShareAnswerToWeChatTimeline(String wechatAppId, String title, String description, String pageUrl)
        {
            try
            {
                var scene = SendMessageToWX.Req.WXSceneTimeline;

                var folder = await Package.Current.InstalledLocation.GetFolderAsync("Resource");
                var file = await folder.GetFileAsync("zhihu_m.jpg");

                using (var stream = await file.OpenReadAsync())
                {
                    var pic = new byte[stream.Size];
                    await stream.AsStream().ReadAsync(pic, 0, pic.Length);

                    var message = new WXWebpageMessage
                    {
                        WebpageUrl = pageUrl,
                        Title = title,
                        Description = description,
                        ThumbData = pic,
                    };

                    var req = new SendMessageToWX.Req(message, scene);
                    IWXAPI api = WXAPIFactory.CreateWXAPI(wechatAppId);
                    var isValid = await api.SendReq(req);

                    return !String.IsNullOrEmpty(isValid);
                }
            }
            catch (WXException ex)
            {
                await UmengAnalytics.TrackException(ex, "ShareAnswerToWeChatTimeline: " + ex.Message);
                return false;
            }
        }

        public async Task<Boolean> ShareViaWeChat(String wechatAppId, String title, String description, String pageUrl)
        {
            try
            {
                var scene = SendMessageToWX.Req.WXSceneChooseByUser;

                var folder = await Package.Current.InstalledLocation.GetFolderAsync("Resource");
                var file = await folder.GetFileAsync("zhihu_m.jpg");

                using (var stream = await file.OpenReadAsync())
                {
                    var pic = new byte[stream.Size];
                    await stream.AsStream().ReadAsync(pic, 0, pic.Length);
                    var message = new WXWebpageMessage
                    {
                        WebpageUrl = pageUrl,
                        Title = title,
                        Description = description,
                        ThumbData = pic
                    };

                    var req = new SendMessageToWX.Req(message, scene);
                    IWXAPI api = WXAPIFactory.CreateWXAPI(wechatAppId);
                    var isValid = await api.SendReq(req);

                    return !String.IsNullOrEmpty(isValid);
                }
            }
            catch (WXException ex)
            {
                await UmengAnalytics.TrackException(ex, "ShareArticleToWeChatTimeline: " + ex.Message);
                return false;
            }
        }
    }
}
