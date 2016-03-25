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


namespace Zhihu.View.Topic
{
    public sealed partial class QuestionsPage : Page
    {
        private RelayCommand<Common.Model.Question> QuestionTapped;

        public QuestionsPage()
        {
            this.InitializeComponent();
            this.QuestionTapped = new RelayCommand<Common.Model.Question>(QuestionTappedMethod);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded += QuestionsPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += QuestionsPage_BackRequested;
        }
        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.Loaded -= QuestionsPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionsPage_BackRequested;

            base.OnNavigatedFrom(e);
        }

        private void QuestionsPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void QuestionsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Theme.Instance.UpdateRequestedTheme(this);
            UpdateBackButton();
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
        
        #region Questioin

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ContentChanging
        {
            get
            {
                return _contentChanging ??
                       (_contentChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Question_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _contentChanging;


        private void Question_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as QuestionView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as Common.Model.Question);

                args.RegisterUpdateCallback(ContentChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(ContentChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowFollowers();

                args.RegisterUpdateCallback(ContentChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowAnswers();

                args.RegisterUpdateCallback(ContentChanging);
            }
            else if (args.Phase == 4)
            {
                view.RegistEventHandler(QuestionTapped);
            }
            args.Handled = true;
        }

        #endregion

        private void QuestionTappedMethod(Common.Model.Question ques)
        {
            NavHelper.NavToQuestionPage(ques.Id, this.Frame);
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionsPage_BackRequested;
        }
    }
}
