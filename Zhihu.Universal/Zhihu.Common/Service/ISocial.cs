using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface ISocial
    {
        Task<ShareResult> GetQuestionShareTemplate(String access, Int32 questionId, Boolean autoCache = false);
        Task<ShareResult> GetAnswerShareTemplate(String access, Int32 answerId, Boolean autoCache = false);
        Task<OperationResult> ShareQuestionViaSina(String access, Int32 questionId, String content);
        Task<OperationResult> ShareAnswerViaSina(String access, Int32 answerId, String content);
        
        Task<Boolean> ShareViaWeChat(String wechatAppId, String title, String description, String pageUrl);
    }
}
