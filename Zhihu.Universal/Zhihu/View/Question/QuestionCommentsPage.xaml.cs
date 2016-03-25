using System;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;
using Zhihu.ViewModel;
using Windows.UI.Xaml.Media.Animation;

namespace Zhihu.View.Question
{
    public sealed partial class QuestionCommentsPage : Page
    {
        public RelayCommand<Comment> CommentAuthorTapped { get; private set; }

        public QuestionCommentsPage()
        {
            this.InitializeComponent();

            CommentAuthorTapped = new RelayCommand<Comment>(CommentAuthorTappedMethod);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                var vm = e.Parameter as QuestionViewModel;

                this.DataContext = vm;
            }

            this.Loaded += Page_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested += Page_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= Page_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested -= Page_BackRequested;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
        }

        private void Page_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
        
        private void OnBackRequested()
        {
            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack(new DrillInNavigationTransitionInfo());

            UpdateBackButton();
        }

        private void CommentAuthorTappedMethod(Comment comment)
        {
            NavHelper.NavToProfilePage(comment.Author.Id, this.Frame);
        }

        #region Comment Content Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> CommentChanging
        {
            get
            {
                return _commentChanging ??
                       (_commentChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Comment_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _commentChanging;

        private void Comment_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as CommentView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as Comment);

                args.RegisterUpdateCallback(CommentChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowAuthor();

                args.RegisterUpdateCallback(CommentChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowSummary();

                args.RegisterUpdateCallback(CommentChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowUpdatedTime();
                view.ShowVoteup();

                args.RegisterUpdateCallback(CommentChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowAvatar();

                view.RegistEventHandler(this.CommentAuthorTapped);
            }
            args.Handled = true;
        }

        #endregion

        private void CommentView_OnTapped(object sender, RoutedEventArgs e)
        {
            var flyout = this.Resources["CommentFlyout"] as MenuFlyout;

            if (flyout == null || flyout.Items == null) return;

            var commentView = sender as CommentView;

            if (commentView == null) return;

            foreach (var menuFlyoutItemBase in flyout.Items)
            {
                menuFlyoutItemBase.DataContext = commentView.Item;
            }

            flyout.ShowAt((FrameworkElement)sender);
        }

        private async void CommentButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var dialog = new UgcDialog("添加评论", "发布", "取消");
            await dialog.ShowAsync();

            var rslt = dialog.Result;

            if (rslt.Canceled == true) return;

            var vm = this.DataContext as QuestionViewModel;

            vm?.CommentQuestion.Execute(rslt.Content);
        }

        private async void ReplyComment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            if (menuItem == null) return;

            var comment = menuItem.DataContext as Comment;
            if (comment == null) return;

            var dialog = new UgcDialog(String.Format("回复 {0} 评论", comment.Author.Name), "发布", "取消");
            await dialog.ShowAsync();

            var rslt = dialog.Result;

            if (rslt.Canceled == true) return;

            var vm = this.DataContext as QuestionViewModel;

            vm.CurrentComment = comment;

            vm?.ReplyComment.Execute(rslt.Content);
        }

        private void VoteupComment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            if (menuItem == null) return;

            var comment = menuItem.DataContext as Comment;
            if (comment == null) return;

            var vm = this.DataContext as QuestionViewModel;

            vm.CurrentComment = comment;

            vm?.VoteUpComment.Execute(comment);
        }

        private void ReportJunk_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            if (menuItem == null) return;

            var comment = menuItem.DataContext as Comment;
            if (comment == null) return;

            var vm = this.DataContext as QuestionViewModel;

            vm.CurrentComment = comment;

            vm?.ReportJunkComment.Execute(comment);
        }

        private void ReportUnFridenly_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            if (menuItem == null) return;

            var comment = menuItem.DataContext as Comment;
            if (comment == null) return;

            var vm = this.DataContext as QuestionViewModel;

            vm.CurrentComment = comment;

            vm?.ReportUnFridenlyComment.Execute(comment);
        }

        private void ReportIllegal_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            if (menuItem == null) return;

            var comment = menuItem.DataContext as Comment;
            if (comment == null) return;

            var vm = this.DataContext as QuestionViewModel;

            vm.CurrentComment = comment;

            vm?.ReportIllegalComment.Execute(comment);
        }

        private void ReportPolitical_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var menuItem = sender as MenuFlyoutItem;
            if (menuItem == null) return;

            var comment = menuItem.DataContext as Comment;
            if (comment == null) return;

            var vm = this.DataContext as QuestionViewModel;

            vm.CurrentComment = comment;

            vm?.ReportPoliticalComment.Execute(comment);
        }
    }
}
