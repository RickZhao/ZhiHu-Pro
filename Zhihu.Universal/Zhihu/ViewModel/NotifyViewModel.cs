using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Windows.UI.Xaml;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Zhihu.Common.DataSource;
using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;
using Zhihu.Controls;


namespace Zhihu.ViewModel
{
    public sealed class NotifyViewModel : ViewModelBase
    {
        private INotify _notify;
        private IMessage _message;
        private INavigate _navigate;

        private Visibility _followsVisible = Visibility.Collapsed;

        public Visibility FollowsVisible
        {
            get { return _followsVisible; }
            private set
            {
                _followsVisible = value;
                RaisePropertyChanged(() => FollowsVisible);
            }
        }

        private ObservableCollection<NotifyItem> _follows = new ObservableCollection<NotifyItem>();

        [Data]
        public ObservableCollection<NotifyItem> Follows
        {
            get { return _follows; }
            private set
            {
                _follows = value;
                RaisePropertyChanged(() => Follows);
            }
        }

        private IncrementalLoading<NotifyItem> _notifies;

        [Data]
        public IncrementalLoading<NotifyItem> Notifies
        {
            get { return _notifies; }
            private set
            {
                _notifies = value;
                RaisePropertyChanged(() => Notifies);
            }
        }

        private IncrementalLoading<NotifyItem> _likes;

        [Data]
        public IncrementalLoading<NotifyItem> Likes
        {
            get { return _likes; }
            private set
            {
                _likes = value;
                RaisePropertyChanged(() => Likes);
            }
        }

        private IncrementalLoading<Chat> _chats;

        [Data]
        public IncrementalLoading<Chat> Chats
        {
            get { return _chats; }
            private set
            {
                _chats = value;
                RaisePropertyChanged(() => Chats);
            }
        }

        private Boolean _contentsLoading = false;

        public Boolean ContentsLoading
        {
            get { return _contentsLoading; }
            private set
            {
                _contentsLoading = value;
                RaisePropertyChanged(() => ContentsLoading);
            }
        }

        private Boolean _likesLoading = false;

        public Boolean LikesLoading
        {
            get { return _likesLoading; }
            private set
            {
                _likesLoading = value;
                RaisePropertyChanged(() => LikesLoading);
            }
        }

        private Boolean _chatsLoading = false;

        public Boolean ChatsLoading
        {
            get { return _chatsLoading; }
            private set
            {
                _chatsLoading = value;
                RaisePropertyChanged(() => ChatsLoading);
            }
        }

        public RelayCommand CheckUnreadFollows { get; private set; }

        public RelayCommand HasReadFollows { get; private set;}
        public RelayCommand HasReadContents { get; private set; }
        public RelayCommand<NotifyItem> HasReadContent { get; private set; }

        public RelayCommand RefreshNotifies { get; private set; }
        public RelayCommand HasReadLikes { get; private set; }
        public RelayCommand RefreshLikes { get; private set; }
        public RelayCommand RefreshChats { get; private set; }


        private NotifyViewModel()
        {
            Notifies = new IncrementalLoading<NotifyItem>(GetMoreNotifies, "notifications/contents", "limit=20&offset=", false);

            Likes = new IncrementalLoading<NotifyItem>(GetMoreLikes, "notifications/likes", "limit=20&offset=", false);

            Chats = new IncrementalLoading<Chat>(GetMoreChats, "inbox", "limit=20&after_id=", false);

            CheckUnreadFollows = new RelayCommand(CheckUnreadFollowsMethod);
            HasReadContents = new RelayCommand(HasReadContentsMethod);
            HasReadFollows = new RelayCommand(HasReadFollowsMethod);
            HasReadLikes = new RelayCommand(HasReadLikesMethod);
            HasReadContent = new RelayCommand<NotifyItem>(HasReadContentMethod);

            RefreshNotifies = new RelayCommand(RefreshNotifiesMethod);
            RefreshLikes = new RelayCommand(RefreshLikesMethod);
            RefreshChats = new RelayCommand(RefreshChatsMethod);
        }

        public NotifyViewModel(INotify notify, IMessage message, INavigate navigate) : this()
        {
            this._notify = notify;
            this._message = message;
            this._navigate = navigate;
        }

        private async void CheckUnreadFollowsMethod()
        {
            if (null == _notify) return;

            FollowsVisible = Visibility.Collapsed;

            var result = await _notify.CheckFollowsAsync(LoginUser.Current.Token, "notifications/unread_follows", true);

            ContentsLoading = false;

            if (result == null) return;

            if (result.Error != null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                Debug.WriteLine(Regex.Unescape(result.Error.Message));

                return;
            }

            Follows.Clear();
            foreach (var item in result.Result.GetItems().Select(item => item as NotifyItem))
            {
                Follows.Add(item);
            }

            FollowsVisible = Follows.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            
            HasReadFollowsMethod();
        }

        private async Task<ListResultBase> GetMoreNotifies(String request)
        {
            if (ContentsLoading|| null == _notify) return null;
            
            ContentsLoading = true;

            var result = await _notify.CheckNotifiesAsync(LoginUser.Current.Token, request, true);

            ContentsLoading = false;

            if (result == null) return null;

            if (result.Error == null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            Debug.WriteLine(Regex.Unescape(result.Error.Message));

            HasReadContentsMethod();

            return null;
        }

        private async void RefreshNotifiesMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            if (ContentsLoading || null == _notify) return;

            ContentsLoading = true;

            var result = await _notify.CheckNotifiesAsync(LoginUser.Current.Token, "notifications/contents", true);

            ContentsLoading = false;

            if (result == null) return;

            if (result.Error != null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
                return;
            }

            var notifiesAll = (from object like in Notifies select like as NotifyItem).ToList();
            var newNotifies = new List<NotifyItem>();

            var query = from i in result.Result.GetItems()
                        let asNotify = i as NotifyItem
                        where notifiesAll.All(o => asNotify != null && o.ThreadId != asNotify.ThreadId)
                        select asNotify;

            newNotifies.AddRange(query);

            if (newNotifies.Count == 20)
            {
                Notifies = new IncrementalLoading<NotifyItem>(GetMoreNotifies, "notifications/contents", "limit=20&offset=", false);

                return;
            }

            for (var i = 0; i < newNotifies.Count; i++)
            {
                Notifies.Insert(i, newNotifies[i]);
            }
        }

        private async Task<ListResultBase> GetMoreLikes(String request)
        {
            if (null == _notify || LikesLoading==true) return null;

            LikesLoading = true;

            var result = await _notify.CheckLikesAync(LoginUser.Current.Token, request, true);

            LikesLoading = false;

            if (result == null) return null;

            if (result.Error == null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            Debug.WriteLine(Regex.Unescape(result.Error.Message));

            HasReadLikesMethod();

            return null;
        }

        private async void RefreshLikesMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            if (null == _notify || LikesLoading == true) return;

            LikesLoading = true;

            var result = await _notify.CheckLikesAync(LoginUser.Current.Token, "notifications/likes", true);

            LikesLoading = false;

            if (result == null) return;

            if (result.Error != null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
                return;
            }

            var likesAll = (from object like in Likes select like as NotifyItem).ToList();
            var newLikes = new List<NotifyItem>();

            var query = from i in result.Result.GetItems()
                        let asLike = i as NotifyItem
                        where likesAll.All(o => asLike != null && o.ThreadId != asLike.ThreadId)
                        select asLike;

            newLikes.AddRange(query);

            if (newLikes.Count == 20)
            {
                Likes = new IncrementalLoading<NotifyItem>(GetMoreLikes, "notifications/likes", "limit=20&offset=", false);

                return;
            }

            for (var i = 0; i < newLikes.Count; i++)
            {
                Likes.Insert(i, newLikes[i]);
            }

            HasReadLikesMethod();
        }

        private async Task<ListResultBase> GetMoreChats(String request)
        {
            if (null == _message|| ChatsLoading==true) return null;

            ChatsLoading = true;

            var result = await _message.GetChats(LoginUser.Current.Token, request, true);

            ChatsLoading = false;

            if (result == null) return null;

            if (result.Error == null) return result;

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            Debug.WriteLine(Regex.Unescape(result.Error.Message));

            return null;
        }

        private async void RefreshChatsMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion
            if (null == _message || ChatsLoading == true) return;

            ChatsLoading = true;

            var result = await _message.GetChats(LoginUser.Current.Token, "inbox", true);

            ChatsLoading = false;

            if (result == null) return;

            if (result.Error != null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
                return;
            }

            var chatsAll = (from object chat in Chats select chat as Chat).ToList();
            var newChats = new List<Chat>();

            var query = from i in result.Result.GetItems()
                        let asChat = i as Chat
                        where chatsAll.All(o => asChat != null && o.Url != asChat.Url)
                        select asChat;

            newChats.AddRange(query);

            if (newChats.Count == 20)
            {
                Chats = new IncrementalLoading<Chat>(GetMoreChats, "inbox", "limit=20&after_id=", false);
                return;
            }

            for (var i = 0; i < newChats.Count; i++)
            {
                Chats.Insert(i, newChats[i]);
            }
        }

        private async void HasReadFollowsMethod()
        {
            if (null == _notify) return;

            var result = await _notify.HasReadFollowsAsync(LoginUser.Current.Token);
        }

        private async void HasReadContentsMethod()
        {
            if (null == _notify) return;

            var result = await _notify.HasReadContentsAsync(LoginUser.Current.Token);
        }

        private async void HasReadLikesMethod()
        {
            if (null == _notify) return;

            var result = await _notify.HasReadLikeAsync(LoginUser.Current.Token);
        }

        private async void HasReadContentMethod(NotifyItem notifyItem)
        {
            if (null == _notify) return;

            var result = await _notify.HasReadContentAsync(LoginUser.Current.Token, notifyItem.Id);

            if (result.Error != null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
                return;
            }

            notifyItem.IsRead = result.Result.IsRead;
        }
    }
}
