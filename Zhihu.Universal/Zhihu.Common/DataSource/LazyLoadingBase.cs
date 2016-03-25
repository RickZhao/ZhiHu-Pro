using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.UI.Xaml.Data;


namespace Zhihu.Common.DataSource
{
    public abstract class LazyLoadingBase : IList, ISupportIncrementalLoading, INotifyCollectionChanged
    {

        private readonly List<Object> _storage = new List<object>();

        private readonly AutoResetEvent _autoReset = new AutoResetEvent(true);

        #region IList

        public Int32 Add(object value)
        {
            _autoReset.WaitOne();

            _storage.Add(value);

            _autoReset.Set();

            return _storage.Count;
        }

        public void Clear()
        {
            _storage.Clear();
        }

        public bool Contains(object value)
        {
            return _storage.Contains(value);
        }

        public Int32 IndexOf(object value)
        {
            try
            {
                return _storage.IndexOf(value);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void Insert(Int32 index, object value)
        {
            _autoReset.WaitOne();

            _storage.Insert(index, value);

            NotifyOfInsertedItems(index, 1);

            _autoReset.Set();
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(Int32 index)
        {
            throw new NotImplementedException();
        }

        public object this[Int32 index]
        {
            get
            {
                try
                {
                    return _storage[index];
                }
                catch (Exception)
                {
                    throw;
                }
            }
            set { throw new NotImplementedException(); }
        }

        public void CopyTo(Array array, Int32 index)
        {
            ((IList)_storage).CopyTo(array, index);
        }

        public Int32 Count
        {
            get { return _storage.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        #endregion

        #region ISupportIncrementalLoading

        public bool HasMoreItems
        {
            get { return HasMoreItemsOverride(); }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run(c => LoadMoreItemsAsync(c, count));
        }

        #endregion

        #region Overridable methods

        protected abstract Task<IList<object>> LoadMoreItemsOverrideAsync(CancellationToken c, uint count);

        protected abstract bool HasMoreItemsOverride();

        #endregion

        #region Private methods

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken cancellation, uint count)
        {
            try
            {
                _autoReset.WaitOne();

                var items = await LoadMoreItemsOverrideAsync(cancellation, count);
                var baseIndex = _storage.Count;

                if (items == null)
                {
                    _autoReset.Reset();
                    return new LoadMoreItemsResult() { Count = 0 };
                }

                _storage.AddRange(items);

                // Now notify of the new items
                NotifyOfInsertedItems(baseIndex, items.Count);

                _autoReset.Reset();

                return new LoadMoreItemsResult { Count = (UInt32)items.Count };

            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                _autoReset.Set();
            }
        }

        private void NotifyOfInsertedItems(Int32 baseIndex, Int32 count)
        {
            if (CollectionChanged == null)
            {
                return;
            }

            for (var i = 0; i < count; i++)
            {
                var newAdded = _storage[i + baseIndex];
                if (_storage.Contains(newAdded))
                {
                    var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                        newAdded, i + baseIndex);

                    CollectionChanged(this, args);
                }
            }
        }

        #endregion

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
