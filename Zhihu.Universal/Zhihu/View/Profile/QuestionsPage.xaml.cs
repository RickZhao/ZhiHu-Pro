using System;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Controls.ItemView;
using Zhihu.Helper;


namespace Zhihu.View.Profile
{
    public sealed partial class QuestionsPage : Page
    {
        private String _profileId = String.Empty;
        private readonly RelayCommand<Common.Model.Question> _questionActivityTapped;

        public QuestionsPage()
        {
            this.InitializeComponent();

            _questionActivityTapped = new RelayCommand<Common.Model.Question>(QuestionActivityTappedMethod);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded -= QuestionsPage_Loaded;
            this.Loaded += QuestionsPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionsPage_BackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += QuestionsPage_BackRequested;

            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Back)
            {
                _profileId = e.Parameter == null ? String.Empty : e.Parameter.ToString();
                var vm = ViewModelHelper.Instance.GetProfile(_profileId);
                this.DataContext = vm;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            
            this.Loaded -= QuestionsPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionsPage_BackRequested;
        }

        private void QuestionsPage_Loaded(object sender, RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
        }

        private void QuestionsPage_BackRequested(object sender, BackRequestedEventArgs e)
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

        private void QuestionActivityTappedMethod(Common.Model.Question question)
        {
            NavHelper.NavToQuestionPage(question.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionsPage_BackRequested;
        }

        #region Question Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> QuestionChanging
        {
            get
            {
                return _questionChanging ??
                       (_questionChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               QuestionAcitivity_OnContainerContentChanging));
            }
        }
        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _questionChanging;

        private void QuestionAcitivity_OnContainerContentChanging(ListViewBase sender,
            ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as QuestionActivityView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as Common.Model.Question);

                args.RegisterUpdateCallback(QuestionChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(QuestionChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowFollowers();

                args.RegisterUpdateCallback(QuestionChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowAnswers();

                args.RegisterUpdateCallback(QuestionChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowOverview();

                view.RegistEventHandler(this._questionActivityTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
