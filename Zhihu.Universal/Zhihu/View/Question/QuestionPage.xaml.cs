using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;

using Zhihu.Controls.ItemView;
using Zhihu.Helper;


namespace Zhihu.View.Question
{
    public sealed partial class QuestionPage : Page
    {
        private Int32 _questionId;
        private Common.Model.Answer _current;
        
        public RelayCommand<Common.Model.Answer> AnswerTapped { get; private set; }
        public RelayCommand<Common.Model.Answer> AnswerAuthorTapped { get; private set; }

        private Double _itemContainerHeight = -1;
        private Int32 _itemKey = -1;
        private String _itemPos = String.Empty;

        public QuestionPage()
        {
            this.InitializeComponent();

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                this.NavigationCacheMode = NavigationCacheMode.Enabled;
            }

            AnswerTapped = new RelayCommand<Common.Model.Answer>(AnswerTappedMethod);
            AnswerAuthorTapped = new RelayCommand<Common.Model.Answer>(AnswerAuthorTappedMethod);

            this.PreviewFrame.Navigate(typeof(WaterMarkPage));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                _questionId = Int32.Parse(e.Parameter.ToString());
                var vm = ViewModelHelper.Instance.GetQuestion(_questionId);

                this.DataContext = vm;
            }

            this.Loaded -= QuestionPage_Loaded;
            this.Loaded += QuestionPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += QuestionPage_BackRequested;
        }

        private void QuestionPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBackButton();
            Theme.Instance.UpdateRequestedTheme(this);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if(e.NavigationMode == NavigationMode.Back)
            {
                ViewModelHelper.Instance.RemoveQuestion(_questionId);
            }

            this.Loaded -= QuestionPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
        }
        
        private void QuestionPage_BackRequested (object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;

            OnBackRequested();

            UpdateBackButton();
        }
        
        private void OnBackRequested()
        {
            if (this.PreviewFrame.BackStack.Count > 0) return;

            this.Frame.GoBack(new DrillInNavigationTransitionInfo());
        }

        private async void UpdateBackButton()
        {
            await Task.Delay(50);

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState)
        {
            AnswerTappedMethod(this._current);
        }

        #region Answer Content Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> AnswerChanging
        {
            get
            {
                return _answerChanging ??
                       (_answerChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Answer_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _answerChanging;

        private void Answer_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as AnswerView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as Common.Model.Answer);

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowAuthor();

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowSummary();

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowUpdatedTime();
                view.ShowVoteup();

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowAvatar();

                view.RegistEventHandler(this.AnswerAuthorTapped, this.AnswerTapped);
            }
            args.Handled = true;
        }

        #endregion
        
        private Frame GetNavFrame()
        {
            Frame navFrame = null;

            if (AdaptiveStates.CurrentState == DefaultState)
            {
                navFrame = this.PreviewFrame;
            }
            else
            {
                navFrame = this.Frame;
            }
            return navFrame;
        }

        private void AnswerTappedMethod(Common.Model.Answer answer)
        {
            if (answer == null) return;

            this._current = answer;

            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
            }
            else
            {
                if (this.PreviewFrame.CanGoBack) this.PreviewFrame.GoBack();
            }

            NavHelper.NavToAnswerPage(answer.Id, GetNavFrame());
        }

        private void AnswerAuthorTappedMethod(Common.Model.Answer answer)
        {
            if (answer == null) return;
            
            NavHelper.NavToProfilePage(answer.Author.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
        }
        
        private void Topic_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var border = sender as Border;
            var topic = border?.DataContext as Common.Model.Topic;
            
            if (topic != null && topic.Id > 0)
            {
                NavHelper.NavToTopicPage(topic.Id, this.Frame);
                SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
            }
        }

        private void QuestionComments_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var navFrame = GetNavFrame();

            if (AdaptiveStates.CurrentState != DefaultState)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
            }

            navFrame.Navigate(typeof(QuestionCommentsPage), this.DataContext);
        }

        private async void Answer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var dialog = new UgcDialog("添加回答", "发布", "取消", true, true, true);
            await dialog.ShowAsync();

            var rslt = dialog.Result;

            if (rslt.Canceled == true) return;

            var vm = ViewModelHelper.Instance.GetQuestion(_questionId);
            if (rslt.Anonymited) vm.AnonymousCommand.Execute(null);
            vm.CreateAnswer.Execute(rslt.Content);
        }

        private void Invite_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void AnswerListGoToTop_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var firstAnswer = AnswerList.Items.FirstOrDefault();

            if (firstAnswer == null) return;

            AnswerList.ScrollIntoView(firstAnswer);
        }
    }
}
