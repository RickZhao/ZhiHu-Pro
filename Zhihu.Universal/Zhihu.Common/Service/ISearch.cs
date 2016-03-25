using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;

namespace Zhihu.Common.Service
{
    public interface ISearch
    {
        Task<AuthorsResult> FindPeople(String access, String request);
        Task<SearchResult> FindContents(String access, String request);
        Task<QuestionsResult> FindQuestionsAsync(String access, String request);
        Task<TopicsResult> FindTopicsAsync(String access, String request);
    }
}
