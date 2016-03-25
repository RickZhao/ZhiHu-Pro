using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignSocialService : ISocial
    {
        public Task<ShareResult> GetQuestionShareTemplate(String access, Int32 questionId,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<ShareResult> GetAnswerShareTemplate(String access, Int32 answerId,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> ShareQuestionViaSina(String access, Int32 questionId, String content)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> ShareAnswerViaSina(String access, Int32 answerId, String content)
        {
            throw new NotImplementedException();
        }
        
        public Task<Boolean> ShareViaWeChat(String wechatAppId, String title, String description, String pageUrl)
        {
            throw new NotImplementedException();
        }
    }
}
