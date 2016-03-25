
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
    public sealed partial class CollectionPage : Page
    {
        public RelayCommand<Common.Model.Question> AnswerTitleTapped { get; private set; }
        public RelayCommand<Author> AnswerAuthorTapped { get; private set; }
        public RelayCommand<Common.Model.Answer> AnswerTapped { get; private set; }

        private Common.Model.Answer _current;

        public CollectionPage()
        {
            this.InitializeComponent();

            AnswerTitleTapped = new RelayCommand<Common.Model.Question>(AnswerTitleTappedMethod);
            AnswerAuthorTapped = new RelayCommand<Author>(AnswerAuthorTappedMethod);
            AnswerTapped = new RelayCommand<Common.Model.Answer>(AnswerTappedMethod);

            this.PreviewFrame.Navigate(typeof(WaterMarkPage));
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += CollectionPage_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested += CollectionPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= CollectionPage_Loaded;

            SystemNavigationManager.GetForCurrentView().BackRequested -= CollectionPage_BackRequested;
        }

        private void CollectionPage_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
        }

        private void CollectionPage_BackRequested(object sender, BackRequestedEventArgs e)
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

        private void VisualStateGroup_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateForVisualState(e.NewState, e.OldState);
        }

        private void UpdateForVisualState(VisualState newState, VisualState oldState)
        {
            if (oldState == DefaultState && newState == NarrowState)
            {
                AnswerTappedMethod(this._current);
            }
            if (oldState == NarrowState && newState == DefaultState)
            {
                AnswerTappedMethod(this._current);
            }
        }
        private void AnswerTitleTappedMethod(Common.Model.Question question)
        {
            this._current = null;
            NavHelper.NavToQuestionPage(question.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= CollectionPage_BackRequested;
        }

        private void AnswerAuthorTappedMethod(Author author)
        {
            this._current = null;
            NavHelper.NavToProfilePage(author.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= CollectionPage_BackRequested;
        }

        private void AnswerTappedMethod(Common.Model.Answer answer)
        {
            if (answer == null) return;
            _current = answer;

            var navFrame = AdaptiveStates.CurrentState == DefaultState ? this.PreviewFrame : this.Frame;

            NavHelper.NavToAnswerPage(answer.Id, navFrame);

            if (AdaptiveStates.CurrentState != DefaultState)
                SystemNavigationManager.GetForCurrentView().BackRequested -= CollectionPage_BackRequested;
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
            var view = args.ItemContainer.ContentTemplateRoot as CollectedAnswerView;

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
                view.ShowTitle();

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowSummary();

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowVoteup();

                args.RegisterUpdateCallback(AnswerChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowAvatar();

                view.RegistEventHandler(this.AnswerTitleTapped, this.AnswerAuthorTapped, this.AnswerTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
