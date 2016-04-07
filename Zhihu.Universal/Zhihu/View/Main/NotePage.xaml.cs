using System;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;
using Zhihu.ViewModel;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace Zhihu.View.Main
{
    public sealed partial class NotePage : Page
    {
        private MainStatus MainStatus;

        private readonly RelayCommand<NotifyItem> _notifyAuthorTapped;
        private readonly RelayCommand<NotifyItem> _notifyTitleTapped;
        private readonly RelayCommand<NotifyItem> _notifySummaryTapped;

        private readonly RelayCommand<Chat> _chatTapped;

        public NotePage()
        {
            this.InitializeComponent();

            _notifyAuthorTapped = new RelayCommand<NotifyItem>(NotifyAuthorTappedMethod);
            _notifyTitleTapped = new RelayCommand<NotifyItem>(NotifyTitleTappedMethod);
            _notifySummaryTapped = new RelayCommand<NotifyItem>(NotifySummaryTappedMethod);

            _chatTapped = new RelayCommand<Chat>(ChatTappedMethod);

            Messenger.Default.Register<MainStatus>(this, OnMainPageStatusChanged);
        }

        private void OnMainPageStatusChanged(MainStatus mainStatus)
        {
            MainStatus = mainStatus;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            UpdateBackButton();
            SystemNavigationManager.GetForCurrentView().BackRequested += NotePage_BackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemNavigationManager.GetForCurrentView().BackRequested -= NotePage_BackRequested;
        }

        private void NotePage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            UpdateBackButton();
            Theme.Instance.UpdateRequestedTheme(this);
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
        
        private void NotifyFollowerAvatar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var ellipse = sender as Ellipse;
            if (ellipse == null) return;

            var notifyItem = ellipse.DataContext as NotifyItem;

            if (notifyItem == null || MainStatus == null) return;

            if (MainStatus.IsWide)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= NotePage_BackRequested;
            }
            else
            {
                if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
            }

            NavHelper.NavToProfilePage(notifyItem.Operators[0].Id, MainStatus.NavFrame);
        }

        private void NotifyAuthorTappedMethod(NotifyItem item)
        {
            if (item == null || MainStatus == null) return;

            if (MainStatus.IsWide)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= NotePage_BackRequested;
            }
            else
            {
                if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
            }

            NavHelper.NavToProfilePage(item.Operators[0].Id, MainStatus.NavFrame);
        }

        private void NotifyTitleTappedMethod(NotifyItem item)
        {
            if (item == null || MainStatus == null) return;

            HasReadNotifyItem(item);

            if ("question" == item.Target.Type)
            {
                NavHelper.NavToQuestionPage(item.Target.GetId(), AppShellPage.AppFrame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= NotePage_BackRequested;
            }
            else if ("answer" == item.Target.Type)
            {
                if (MainStatus.IsWide)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= NotePage_BackRequested;
                }
                else
                {
                    if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
                }
                NavHelper.NavToQuestionPage(item.Target.Question.Id, MainStatus.NavFrame);
            }
            else if ("article" == item.Target.Type)
            {
                if (MainStatus.IsWide)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= NotePage_BackRequested;
                }
                else
                {
                    if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
                }
                NavHelper.NavToArticlePage(item.Target.GetId(), MainStatus.NavFrame);
            }
            else if ("column" == item.Target.Type)
            {
                NavHelper.NavToColumnPage(item.Target.Id, AppShellPage.AppFrame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= NotePage_BackRequested;
            }
        }

        private void NotifySummaryTappedMethod(NotifyItem item)
        {
            if (item == null || MainStatus == null) return;

            HasReadNotifyItem(item);

            if (MainStatus.IsWide)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= NotePage_BackRequested;
            }
            else
            {
                if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
            }

            if ("question" == item.Target.Type && item.Answer != null)
            {
                NavHelper.NavToAnswerPage(item.Answer.Id, MainStatus.NavFrame);
            }
            if ("answer" == item.Target.Type)
            {
                NavHelper.NavToAnswerPage(item.Target.GetId(), MainStatus.NavFrame);
            }
        }

        private void HasReadNotifyItem(NotifyItem item)
        {
            var mainVm = this.DataContext as MainViewModel;
            if (mainVm == null) return;

            var notifyVm = mainVm.Notify as NotifyViewModel;
            if (notifyVm == null) return;

            if (notifyVm.DismissContentNotify.CanExecute(item))
            {
                notifyVm.DismissContentNotify.Execute(item);
            }
        }

        private void ChatTappedMethod(Chat chat)
        {
            if (chat?.Participant == null || MainStatus == null)

                if (MainStatus.IsWide)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= NotePage_BackRequested;
                }
                else
                {
                    if (this.MainStatus.NavFrame.CanGoBack) this.MainStatus.NavFrame.GoBack();
                }

            NavHelper.NavToMessagePage(chat.Participant.Id, MainStatus.NavFrame);
        }

        #region Notifies

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> NotifyChanging
        {
            get
            {
                return _notifyChanging ??
                       (_notifyChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               NotifyView_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _notifyChanging;


        private void NotifyView_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var feedView = args.ItemContainer.ContentTemplateRoot as NotifyView;

            if (feedView == null) return;

            if (args.InRecycleQueue == true)
            {
                feedView.Clear();
            }
            else if (args.Phase == 0)
            {
                feedView.ShowPlaceholder(args.Item as NotifyItem);

                args.RegisterUpdateCallback(NotifyChanging);
            }
            else if (args.Phase == 1)
            {
                feedView.ShowTitle();

                args.RegisterUpdateCallback(NotifyChanging);
            }
            else if (args.Phase == 2)
            {
                feedView.ShowAuthor();

                args.RegisterUpdateCallback(NotifyChanging);
            }
            else if (args.Phase == 3)
            {
                feedView.ShowVerb();

                args.RegisterUpdateCallback(NotifyChanging);
            }
            else if (args.Phase == 4)
            {
                feedView.ShowSummary();

                feedView.RegistEventHandler(_notifyAuthorTapped, _notifyTitleTapped, _notifySummaryTapped);
            }
            args.Handled = true;
        }

        #endregion

        #region Likes

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> LikeChanging
        {
            get
            {
                return _likeChanging ??
                       (_likeChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Likes_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _likeChanging;

        private void Likes_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var feedView = args.ItemContainer.ContentTemplateRoot as NotifyView;

            if (feedView == null) return;

            if (args.InRecycleQueue == true)
            {
                feedView.Clear();
            }
            else if (args.Phase == 0)
            {
                feedView.ShowPlaceholder(args.Item as NotifyItem);

                args.RegisterUpdateCallback(LikeChanging);
            }
            else if (args.Phase == 1)
            {
                feedView.ShowTitle();

                args.RegisterUpdateCallback(LikeChanging);
            }
            else if (args.Phase == 2)
            {
                feedView.ShowAuthor();

                args.RegisterUpdateCallback(LikeChanging);
            }
            else if (args.Phase == 3)
            {
                feedView.ShowVerb();

                args.RegisterUpdateCallback(LikeChanging);
            }
            else if (args.Phase == 4)
            {
                feedView.ShowSummary();

                feedView.RegistEventHandler(_notifyAuthorTapped, _notifyTitleTapped, _notifySummaryTapped);
            }
            args.Handled = true;
        }

        #endregion

        #region Chat

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ChatChanging
        {
            get
            {
                return _chatChanging ??
                       (_chatChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               ChatView_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _chatChanging;

        private void ChatView_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var feedView = args.ItemContainer.ContentTemplateRoot as ChatView;

            if (feedView == null) return;

            if (args.InRecycleQueue == true)
            {
                feedView.Clear();
            }
            else if (args.Phase == 0)
            {
                feedView.ShowPlaceholder(args.Item as Chat);

                args.RegisterUpdateCallback(ChatChanging);
            }
            else if (args.Phase == 1)
            {
                feedView.ShowTitle();

                args.RegisterUpdateCallback(ChatChanging);
            }
            else if (args.Phase == 2)
            {
                feedView.ShowAvatar();

                args.RegisterUpdateCallback(ChatChanging);
            }
            else if (args.Phase == 3)
            {
                feedView.ShowSummary();

                feedView.RegistEventHandler(_chatTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
