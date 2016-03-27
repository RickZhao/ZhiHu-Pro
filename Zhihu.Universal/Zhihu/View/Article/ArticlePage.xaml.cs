using System;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Ioc;

using Zhihu.Helper;
using Zhihu.ViewModel;
using Zhihu.Common.Helper;
using Zhihu.Controls;


namespace Zhihu.View.Article
{
    public sealed partial class ArticlePage : Page
    {
        private Int32 _articleId;

        public ArticlePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                _articleId = Int32.Parse(e.Parameter.ToString());
                var vm = ViewModelHelper.Instance.GetArticle(_articleId);

                this.DataContext = vm;
            }

            this.Loaded -= ArticlePage_Loaded;
            this.Loaded += ArticlePage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= AnswerPage_BackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += AnswerPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModelHelper.Instance.RemoveArticle(_articleId);
            }

            this.Loaded -= ArticlePage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= AnswerPage_BackRequested;
        }
        
        private void ArticlePage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBackButton();
            Theme.Instance.UpdateRequestedTheme(this);
        }
        
        private void AnswerPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void OnBackRequested()
        {
            if (ImageViewerPopup.Instance.IsOpen == true)
            {
                ImageViewerPopup.Instance.Close();
                return;
            }

            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack(new DrillInNavigationTransitionInfo());

            UpdateBackButton();
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }


        private void ArticleAuthor_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var stack = sender as Grid;

            var articleVm = ViewModelHelper.Instance.GetArticle(_articleId);

            if (articleVm == null) return;

            NavHelper.NavToProfilePage(articleVm.Detail.Author.Id, this.Frame);
        }

        private void ShowComments_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ArticleCommentsPage), this.DataContext);
        }
    }
}