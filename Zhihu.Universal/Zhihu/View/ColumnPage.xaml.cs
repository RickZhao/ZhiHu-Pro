
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;
using Zhihu.ViewModel;


namespace Zhihu.View
{
    public sealed partial class ColumnPage : Page
    {
        private Common.Model.Article _current;
        public RelayCommand<Common.Model.Article> NavToArticle { get; private set; }

        public ColumnPage()
        {
            this.InitializeComponent();

            NavToArticle = new RelayCommand<Common.Model.Article>(NavToArticleMethod);

            this.PreviewFrame.Navigate(typeof(WaterMarkPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += ColumnPage_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested += QuestionPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= ColumnPage_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
        }

        private void ColumnPage_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
        }

        private void QuestionPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }
        
        private void OnBackRequested()
        {
            if (this.PreviewFrame.BackStack.Count > 0) return;

            this.Frame.GoBack(new DrillInNavigationTransitionInfo());

            UpdateBackButton();
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        #region Article Content Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ArticleChanging
        {
            get
            {
                return _articleChanging ??
                       (_articleChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Article_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _articleChanging;

        private void Article_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as ArticleView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as Common.Model.Article);

                args.RegisterUpdateCallback(ArticleChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(ArticleChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowAvatar();

                args.RegisterUpdateCallback(ArticleChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowAuthor();

                args.RegisterUpdateCallback(ArticleChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowCreatedTime();

                args.RegisterUpdateCallback(ArticleChanging);
            }
            else if (args.Phase == 5)
            {
                view.ShowComments();

                args.RegisterUpdateCallback(ArticleChanging);
            }
            else if (args.Phase == 6)
            {
                view.ShowExcerpt();

                args.RegisterUpdateCallback(ArticleChanging);
            }
            else if (args.Phase == 7)
            {
                view.ShowVoteUp();

                view.RegistEventHandler(this.NavToArticle);
            }
            args.Handled = true;
        }

        #endregion

        private void NavToArticleMethod(Common.Model.Article article)
        {
            _current = article;

            if (article == null) return;

            Frame navFrame = null;

            if (this.AdaptiveStates.CurrentState == DefaultState)
            {
                navFrame = this.PreviewFrame;
            }
            else
            {
                navFrame = this.Frame;
            }

            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToArticlePage(article.Id, navFrame);
        }

        private void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState)
        {
            NavToArticleMethod(_current);
        }
    }
}
