using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignQuestionService : IQuestion
    {
        public Task<InviteResult> Invite(String access, Int32 questionId, String personId)
        {
            throw new NotImplementedException();
        }

        public async Task<QuestionResult> GetDetailAsync(String accessToken, Int32 questionId,
            Boolean autoCache = false)
        {
            var json =
                @"{\'status\':{},\'title\':\'\u5728EVA\u4e2d \uff0c\u4e3a\u4ec0\u4e48\u521d\u53f7\u673a\u80fd\u5728\u6700\u540e\u78be\u8f67\u529b\u5929\u4f7f\uff1f\',\'url\':\'https:\/\/api.zhihu.com\/questions\/24605722\',\'topics\':[{\'name\':\'\u65b0\u4e16\u7eaa\u798f\u97f3\u6218\u58eb\uff08EVA\uff09\',\'url\':\'https:\/\/api.zhihu.com\/topics\/19593131\',\'excerpt\':\'\u300a\u65b0\u4e16\u7eaa\u798f\u97f3\u6218\u58eb\u300b\uff08\u65e5\u8bed\uff1a\u65b0\u4e16\u7d00\u30a8\u30f4\u30a1\u30f3\u30b2\u30ea\u30aa\u30f3\uff1b\u82f1\u8bed\uff1aNeon Genesis Evangelion\uff09\u662f\u65e5\u672c\u52a8\u753b\u516c\u53f8 GAINAX \u7684\u673a\u5668\u4eba\u52a8\u753b\u4f5c\u54c1\uff0c\u7531\u5eb5\u91ce\u79c0\u660e\u5bfc\u6f14\uff0c\u4e8e 1995 \u5e74\u2026\',\'introduction\':\'\u300a\u65b0\u4e16\u7eaa\u798f\u97f3\u6218\u58eb\u300b\uff08\u65e5\u8bed\uff1a\u65b0\u4e16\u7d00\u30a8\u30f4\u30a1\u30f3\u30b2\u30ea\u30aa\u30f3\uff1b\u82f1\u8bed\uff1aNeon Genesis Evangelion\uff09\u662f\u65e5\u672c\u52a8\u753b\u516c\u53f8 GAINAX \u7684\u673a\u5668\u4eba\u52a8\u753b\u4f5c\u54c1\uff0c\u7531\u5eb5\u91ce\u79c0\u660e\u5bfc\u6f14\uff0c\u4e8e 1995 \u5e74\u5728\u65e5\u672c\u9996\u6b21\u64ad\u653e\u3002\',\'avatar_url\':\'http:\/\/pic2.zhimg.com\/7eeb7bac3_s.jpg\',\'type\':\'topic\',\'id\':\'19593131\'}],\'author\':{\'headline\':\'\u5b8c\u5168\u6ca1\u660e\u767d\',\'avatar_url\':\'http:\/\/pic2.zhimg.com\/9efcc37df_s.jpg\',\'name\':\'\u9646\u9a81\u5251\',\'url\':\'https:\/\/api.zhihu.com\/people\/d605363ff3db0db069d7b9a88614e6f6\',\'gender\':1,\'type\':\'people\',\'id\':\'d605363ff3db0db069d7b9a88614e6f6\'},\'excerpt\':\'\u9664\u4e86\u4e3b\u89d2\u5149\u73af,\u6c42\u5ba2\u89c2\u5206\u6790.\',\'detail\':\'\u9664\u4e86\u4e3b\u89d2\u5149\u73af,\u6c42\u5ba2\u89c2\u5206\u6790.\',\'answer_count\':5,\'updated_time\':1406212692,\'to\':[],\'comment_count\':0,\'draft\':{},\'redirection\':{\'to\':{},\'from\':[]},\'follower_count\':18,\'type\':\'question\',\'id\':24605722}";

            var obj = JsonConvert.DeserializeObject<Question>(json);

            await Task.Delay(100);

            return new QuestionResult(obj);
        }

        public Task<QuesRelaResult> GetRelationshipAsync(String accessToken, Int32 questionId,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<TopicsResult> GetTopicsAsync(String accessToken, Int32 questionId,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<AnswersResult> GetAnswersAsync(String accessToken, String requestUri,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<CommentsResult> GetCommentsAsync(String accessToken, String requestUri,
            Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<FollowingResult> FollowAsync(String accessToken, Int32 questionId)
        {
            throw new NotImplementedException();
        }

        public Task<FollowingResult> UnFollowAsync(String accessToken,
            Int32 questionId, String userId)
        {
            throw new NotImplementedException();
        }

        public Task<CreateQuesResult> CreateAsync(string accessToken, bool isAnonymous, string topicIds, string title,
            string detail)
        {
            throw new NotImplementedException();
        }


        public Task<AnswerResult> AnswerAsync(string accessToken, int questionId, string answer)
        {
            throw new NotImplementedException();
        }


        public Task<AnonymousResult> AnonymousAsync(string access, int questionId)
        {
            throw new NotImplementedException();
        }

        public Task<AnonymousResult> CancelAnonymousAsync(string access, int questionId)
        {
            throw new NotImplementedException();
        }
    }
}