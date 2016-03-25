using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;

namespace Zhihu.Common.Service
{
    public interface IAnswer
    {
        Task<AnswerDetailResult> GetAnswerDetailAsync(String accessToken, Int32 answerId, Boolean autoCache = false);
        Task<AnswerRelationshipResult> GetAnswerRelationshipAsync(String accessToken, Int32 answerId, Boolean autoCache = false);
        Task<FavoriteResult> CheckFavoriteAsync(String access, Int32 answerId, Boolean autoCache = false);

        Task<AnswerCommentsResult> GetCommentsAsync(String accessToken, String requestUri, Boolean autoCache = false);
       
        Task<AnswerNoHelpeResult> NoHelpAsync(String accessToken, Int32 answerId);
        Task<AnswerNoHelpeResult> UndoNoHelpAsync(String accessToken, Int32 answerId, String userId);
        Task<AnswerHelpedResult> ThankAsync(String accessToken, Int32 answerId);
        Task<AnswerHelpedResult> CancelThankAsync(String accessToken, Int32 answerId, String loginId);
        Task<OperationResult> CollectAsync(String accessToken, Int32 answerId, IList<Int32> collectionIds);
     
        Task<VoteResult> VoteUpAsync(String access, Int32 answerId);
        Task<VoteResult> VoteDownAsync(String access, Int32 answerId);
        Task<VoteResult> CancelVoteAsync(String access, Int32 answerId, String loginId);
    }
}
