using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignSearchService : ISearch
    {
        public Task<AuthorsResult> FindPeople(string access, string keyword)
        {
            throw new NotImplementedException();
        }

        public Task<SearchResult> FindContents(string access, string keyword)
        {
            throw new NotImplementedException();
        }

        public Task<QuestionsResult> FindQuestionsAsync(string access, string request)
        {
            throw new NotImplementedException();
        }
        public Task<TopicsResult> FindTopicsAsync(string access, string request)
        {
            throw new NotImplementedException();
        }
    }
}
