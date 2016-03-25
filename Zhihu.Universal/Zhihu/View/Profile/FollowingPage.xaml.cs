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
    public sealed partial class FollowingPage : Page
    {
        private String _profileId = String.Empty;
        private RelayCommand<Collection> CollectionTapped;
        private RelayCommand<Common.Model.Topic> TopicTapped;
        private RelayCommand<Common.Model.Question> QuestionActivityTapped;

        public FollowingPage()
        {
            this.InitializeComponent();

            this.CollectionTapped = new RelayCommand<Collection>(CollectionTappedMethod);

            this.TopicTapped = new RelayCommand<Common.Model.Topic>(TopicTappedMethod);

            this.QuestionActivityTapped = new RelayCommand<Common.Model.Question>(QuestionActivityTappedMethod);
        }

        private void CollectionTappedMethod(Collection collection)
        {
            NavHelper.NavToCollectionPage(collection.Id, this.Frame);

            SystemNavigationManager.GetForCurrentView().BackRequested -= FollowingPage_BackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.Loaded -= FollowingPage_Loaded;
            this.Loaded += FollowingPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= FollowingPage_BackRequested;
            SystemNavigationManager.GetForCurrentView().BackRequested += FollowingPage_BackRequested;

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
            
            this.Loaded -= FollowingPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= FollowingPage_BackRequested;
        }

        private void FollowingPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void FollowingPage_Loaded(object sender, RoutedEventArgs e)
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

        private void TopicTappedMethod(Common.Model.Topic topic)
        {
            NavHelper.NavToTopicPage(topic.Id, this.Frame);

            SystemNavigationManager.GetForCurrentView().BackRequested -= FollowingPage_BackRequested;
        }

        private void QuestionActivityTappedMethod(Common.Model.Question question)
        {
            NavHelper.NavToQuestionPage(question.Id, this.Frame);

            SystemNavigationManager.GetForCurrentView().BackRequested -= FollowingPage_BackRequested;
        }

        #region Question Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> QuestionChanging
        {
            get
            {
                return _questionChanging ??
                       (_questionChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Question_OnContainerContentChanging));
            }
        }
        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _questionChanging;

        private void Question_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
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

                view.RegistEventHandler(this.QuestionActivityTapped);
            }
            args.Handled = true;
        }

        #endregion

        #region Collection Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> CollectionChanging
        {
            get
            {
                return _collectionChanging ??
                       (_collectionChanging = new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                           Collection_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _collectionChanging;

        private void Collection_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as CollectionView;
            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else
                switch (args.Phase)
                {
                    case 0:
                        view.ShowPlaceHolder(args.Item as Common.Model.Collection);

                        args.RegisterUpdateCallback(CollectionChanging);
                        break;
                    case 1:
                        view.ShowTitle();

                        args.RegisterUpdateCallback(CollectionChanging);
                        break;
                    case 2:
                        view.ShowDescription();

                        args.RegisterUpdateCallback(CollectionChanging);
                        break;
                    case 3:
                        view.ShowAvator();
                        view.ShowAuthor();

                        args.RegisterUpdateCallback(CollectionChanging);
                        break;
                    case 4:
                        view.ShowCount();

                        view.RegistEventHandler(CollectionTapped);
                        break;
                }
            args.Handled = true;
        }

        #endregion

        #region Topic Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> TopicChanging
        {
            get
            {
                return _topicChanging ??
                       (_topicChanging = new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                           Topic_OnContainerContentChanging));
            }
        }

        public object SysteNavigationManager { get; private set; }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _topicChanging;

        private void Topic_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as TopicView;
            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as Common.Model.Topic);

                args.RegisterUpdateCallback(TopicChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(TopicChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowDescription();

                args.RegisterUpdateCallback(TopicChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowAvator();

                args.RegisterUpdateCallback(TopicChanging);
            }
            else if (args.Phase == 4)
            {
                view.RegistEventHandler(this.TopicTapped);
            }
            args.Handled = true;
        }

        #endregion
    }
}
