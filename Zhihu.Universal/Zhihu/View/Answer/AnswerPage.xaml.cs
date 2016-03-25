using System;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using Zhihu.Helper;
using Zhihu.ViewModel;


namespace Zhihu.View.Answer
{
    public sealed partial class AnswerPage : Page
    {
        private Int32 _answerId;

        public AnswerPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                _answerId = Int32.Parse(e.Parameter.ToString());
                var vm = ViewModelHelper.Instance.GetAnswer(_answerId);

                this.DataContext = vm;
                
                this.Loaded += AnswerPage_Loaded;

                SystemNavigationManager.GetForCurrentView().BackRequested += AnswerPage_BackRequested;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (e.NavigationMode == NavigationMode.Back)
            {
                ViewModelHelper.Instance.RemoveAnswer(_answerId);
            }

            this.Loaded -= AnswerPage_Loaded;
            
            SystemNavigationManager.GetForCurrentView().BackRequested -= AnswerPage_BackRequested;
        }
        
        private void AnswerPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBackButton();
            Theme.Instance.UpdateRequestedTheme(this);
        }

        private void UpdateBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void AnswerPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void OnBackRequested()
        {
            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack(new DrillInNavigationTransitionInfo());

            UpdateBackButton();
        }

        private void VoteFlyout_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //VoteFlyout.Hide();
        }

        private void CollectCancel_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            CollectionFlyout.Hide();
        }

        private void CollectDo_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var vm = this.DataContext as AnswerViewModel;
            if (vm == null) return;

            vm.AnswerCollectTapped.Execute(null);

            CollectionFlyout.Hide();
        }

        private void AnswerTitle_OnTapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void AnswerAuthor_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var panel = sender as Grid;

            var answerVm = panel?.DataContext as AnswerViewModel;

            if (answerVm == null) return;

            NavHelper.NavToProfilePage(answerVm.Detail.Author.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= AnswerPage_BackRequested;
        }

        private void ShowComments_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AnswerCommentsPage), this.DataContext);
        }
        
    }
}
