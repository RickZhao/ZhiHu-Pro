
using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class SearchService : ISearch
    {
        public async Task<AuthorsResult> FindPeople(string access, string keyword)
        {
            var api = new SearchApi();
            var result = await api.FindPeopleAsync(access, keyword);

            return result;
        }

        public async Task<SearchResult> FindContents(string access, string request)
        {
            var api = new SearchApi();
            var result = await api.FindContentsAsync(access, request);

            return result;
        }

        public async Task<QuestionsResult> FindQuestionsAsync(String access, String request)
        {
            var api = new SearchApi();
            var result = await api.FindQuestionsAsync(access, request);

            return result;
        }

        public async Task<TopicsResult> FindTopicsAsync(string access, string request)
        {
            var api = new SearchApi();
            var result = await api.FindTopicsAsync(access, request);

            return result;
        }
    }
}
