using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class AnswerService : IAnswer
    {
        public async Task<AnswerDetailResult> GetAnswerDetailAsync(String accessToken, Int32 answerId, Boolean autoCache = false)
        {
            var api = new AnswerApi();
            var result = await api.GetDetailAsync(accessToken, answerId, autoCache);
            
            return result;
        }

        public async Task<AnswerRelationshipResult> GetAnswerRelationshipAsync(String accessToken, Int32 answerId, Boolean autoCache = false)
        {
            var api = new AnswerApi();
            var result = await api.GetRelationshipAsync(accessToken, answerId, autoCache);

            return result;
        }

        public async Task<AnswerCommentsResult> GetCommentsAsync(String accessToken, String requestUri, Boolean autoCache = false)
        {
            var api = new AnswerApi();
            var result = await api.GetCommentsAsync(accessToken, requestUri, autoCache);

            return result;
        }

        public async Task<AnswerNoHelpeResult> NoHelpAsync(String accessToken, Int32 answerId)
        {
            var api = new AnswerApi();
            var result = await api.BeNoHelpAsync(accessToken, answerId);

            return result;
        }

        public async Task<AnswerNoHelpeResult> UndoNoHelpAsync(String accessToken, Int32 answerId, String userId)
        {
            var api = new AnswerApi();
            var result = await api.UndoNoHelpAsync(accessToken, answerId, userId);

            return result;
        }

        public async Task<AnswerHelpedResult> ThankAsync(String accessToken, Int32 answerId)
        {
            var api = new AnswerApi();
            var result = await api.ExpressThanksAsync(accessToken, answerId);

            return result;
        }

        public async Task<AnswerHelpedResult> CancelThankAsync(String accessToken, Int32 answerId, String loginId)
        {
            var api = new AnswerApi();
            var result = await api.CancelThanksAsync(accessToken, answerId, loginId);

            return result;
        }

        public async Task<OperationResult> CollectAsync(String accessToken, Int32 answerId, IList<Int32> collectionIds)
        {
            var api = new AnswerApi();
            var result = await api.CollectAsync(accessToken, answerId, collectionIds);

            return result;
        }


        public async Task<VoteResult> VoteUpAsync(String access, Int32 answerId)
        {
            var api = new AnswerApi();
            var result = await api.VoteUpAsync(access, answerId);

            return result;
        }

        public async Task<VoteResult> VoteDownAsync(String access, Int32 answerId)
        {
            var api = new AnswerApi();
            var result = await api.VoteDownAsync(access, answerId);

            return result;
        }

        public async Task<VoteResult> CancelVoteAsync(String access, Int32 answerId, String loginId)
        {
            var api = new AnswerApi();
            var result = await api.CancelVoteAsync(access, answerId, loginId);

            return result;
        }


        public async Task<FavoriteResult> CheckFavoriteAsync(String access, Int32 answerId, Boolean autoCache = false)
        {
            var api = new AnswerApi();
            var result = await api.CheckFavoriteAsync(access, answerId, autoCache);

            return result;
        }
    }
}
