using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignAnswerService : IAnswer
    {
        public Task<AnswerDetailResult> GetAnswerDetailAsync(String accessToken,
            Int32 answerId, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<AnswerRelationshipResult> GetAnswerRelationshipAsync(
            String accessToken, Int32 answerId, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<CommentResult> CommentAsync(String access, Int32 answerId, String content)
        {
            throw new NotImplementedException();
        }

        public Task<AnswerCommentsResult> GetCommentsAsync(String accessToken,
            String requestUri, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<AnswerNoHelpeResult> NoHelpAsync(String accessToken,
            Int32 answerId)
        {
            throw new NotImplementedException();
        }

        public Task<AnswerNoHelpeResult> UndoNoHelpAsync(String accessToken,
            Int32 answerId, String userId)
        {
            throw new NotImplementedException();
        }

        public Task<AnswerHelpedResult> ThankAsync(String accessToken, Int32 answerId)
        {
            throw new NotImplementedException();
        }

        public Task<AnswerHelpedResult> CancelThankAsync(String accessToken, Int32 answerId, String loginId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> CollectAsync(String accessToken, Int32 answerId,
            IList<Int32> collectionIds)
        {
            throw new NotImplementedException();
        }


        public Task<VoteResult> VoteUpAsync(string access, int answerId)
        {
            throw new NotImplementedException();
        }

        public Task<VoteResult> VoteDownAsync(string access, int answerId)
        {
            throw new NotImplementedException();
        }

        public Task<VoteResult> CancelVoteAsync(string access, int answerId, string loginId)
        {
            throw new NotImplementedException();
        }


        public Task<FavoriteResult> CheckFavoriteAsync(String access, Int32 answerId, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }
    }
}
