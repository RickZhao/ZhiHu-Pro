using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Zhihu.Common.DataSource
{
    public class LazyLoading<T> : LazyLoadingBase
    {
        public Int32 Index { get; private set; }
        public List<T> All { get; private set; }

        public LazyLoading(IList<T> all)
        {
            Index = 0;
            All = all.ToList();
        }

        protected override Task<IList<object>> LoadMoreItemsOverrideAsync(CancellationToken c, uint count)
        {
            var task = new TaskCompletionSource<IList<object>>();

            var page = All.Skip(Index).Take(2);

            Index += 2;

            var ret = new List<object>();

            foreach (var item in page)
            {
                ret.Add(item);
            }

            task.SetResult(ret);

            return task.Task;
        }

        protected override bool HasMoreItemsOverride()
        {
            if (All == null) return true;

            return Index < All.Count;
        }
    }
}
