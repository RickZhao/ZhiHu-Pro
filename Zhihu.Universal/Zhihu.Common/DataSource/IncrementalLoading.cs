using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;

using UmengSDK;


namespace Zhihu.Common.DataSource
{
    public sealed class IncrementalLoading<T> : IncrementalLoadingBase
    {
        private String _requestUri;

        private String _firstOffset;
        private Boolean _firstNeedOffset;

        private Paging _current;

        private bool _hasMore = true;


        private readonly Func<String, Task<ListResultBase>> _generator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="requestUri"></param>
        /// <param name="firstOffset"></param>
        /// <param name="firstNeedOffset">首次请求时，是否使用firstOffset</param>
        public IncrementalLoading(Func<String, Task<ListResultBase>> generator, String requestUri,
            String firstOffset, Boolean firstNeedOffset = true)
        {
            _generator = generator;
            _hasMore = true;
            _current = null;

            _requestUri = requestUri;
            _firstOffset = firstOffset;
            _firstNeedOffset = firstNeedOffset;

            base.Clear();
        }

        protected override async Task<IList<object>> LoadMoreItemsOverrideAsync(CancellationToken cancellationToken,
            uint count)
        {
            try
            {
                var firstRequest = _firstNeedOffset
                    ? String.Format("{0}?{1}", _requestUri, _firstOffset)
                    : _requestUri;

                var requestUri = _current == null
                    ? firstRequest
                    : _current.Next.Replace(Utility.Instance.BaseUri, String.Empty);

                Debug.WriteLine("Request Uri: {0}", requestUri);

                var result = await _generator(requestUri);

                if (result == null || result.Result == null) return null;

                _current = result.Result.Paging;

                if (result.Result.Paging == null) return result.Result.GetItems();

                var nextUri = new Uri(result.Result.Paging.Next);
                //"?limit=20&after_id=16"
                var limitPart = nextUri.Query.Split('&').FirstOrDefault(seg => seg.Contains("limit="));
                var startIndex = limitPart.IndexOf("limit=", StringComparison.Ordinal) + "limit=".Length;

                var pageLimit = 0;
                var itemsCount = result.Result.GetItems() == null ? 0 : result.Result.GetItems().Length;

                Int32.TryParse(limitPart.Substring(startIndex), out pageLimit);

                if (result.Result.Paging.IsEnd || (pageLimit > 0 && itemsCount < pageLimit/2) ||
                    result.Result.GetItems() == null || result.Result.GetItems().Length == 0)
                {
                    _hasMore = false;
                }

                return result.Result.GetItems();
            }
            catch (Exception ex)
            {
                await UmengAnalytics.TrackException(ex, ex.Message);
                return null;
            }
        }

        protected override bool HasMoreItemsOverride()
        {
            if (_current == null)
            {
                return _hasMore;
            }
            else
            {
                return _hasMore;
            }
        }
    }
}