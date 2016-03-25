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


namespace Zhihu.View
{
    public sealed partial class SearchPage : Page
    {
        private RelayCommand<Author> _authorTapped;
        private RelayCommand<SearchItem> _contentTapped;

        public SearchPage()
        {
            this.InitializeComponent();

            _authorTapped = new RelayCommand<Author>(AuthorTappedMethod);
            _contentTapped = new RelayCommand<SearchItem>(ContentTappedMethod);
        }

        private void SearchPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.SearchBox.Text))
            {
                this.SearchBox.Focus(FocusState.Programmatic);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            UpdateBackButton();
            this.Loaded -= SearchPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested += QuestionPage_BackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.Loaded -= SearchPage_Loaded;
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
        }


        private void QuestionPage_BackRequested(object sender, BackRequestedEventArgs e)
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


        #region Author Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> AuthorChanging
        {
            get
            {
                return _authorChanging ??
                       (_authorChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Author_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _authorChanging;

        private void Author_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as AuthorView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as Author);

                args.RegisterUpdateCallback(AuthorChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowAuthor();

                args.RegisterUpdateCallback(AuthorChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowHeadline();

                args.RegisterUpdateCallback(AuthorChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowAvatar();

                args.RegisterUpdateCallback(AuthorChanging);
            }
            else if (args.Phase == 4)
            {
                view.RegistEventHandler(this._authorTapped);
            }
            args.Handled = true;
        }

        #endregion

        #region Content Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ContentChanging
        {
            get
            {
                return _contentChanging ??
                       (_contentChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               Search_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _contentChanging;


        private void Search_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as SearchItemView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceHolder(args.Item as SearchItem);

                args.RegisterUpdateCallback(ContentChanging);
            }
            else if (args.Phase == 1)
            {
                view.ShowTitle();

                args.RegisterUpdateCallback(ContentChanging);
            }
            else if (args.Phase == 2)
            {
                view.ShowHeadline();

                args.RegisterUpdateCallback(ContentChanging);
            }
            else if (args.Phase == 3)
            {
                view.ShowAvatar();

                args.RegisterUpdateCallback(ContentChanging);
            }
            else if (args.Phase == 4)
            {
                view.ShowFollowers();

                args.RegisterUpdateCallback(ContentChanging);
            }
            else if (args.Phase == 5)
            {
                view.ShowAnswers();

                args.RegisterUpdateCallback(ContentChanging);
            }
            else if (args.Phase == 6)
            {
                view.RegistEventHandler(this._contentTapped);
            }
            args.Handled = true;
        }

        #endregion

        private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var binding = this.SearchBox.GetBindingExpression(TextBox.TextProperty);

            if (binding != null) binding.UpdateSource();
        }


        private void AuthorTappedMethod(Author author)
        {
            NavHelper.NavToProfilePage(author.Id, this.Frame);
            
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
        }

        private void ContentTappedMethod(SearchItem item)
        {
            if (item.Type == "question")
            {
                NavHelper.NavToQuestionPage(Int32.Parse(item.Id), AppShellPage.AppFrame);
            }
            if (item.Type == "topic")
            {
                NavHelper.NavToTopicPage(Int32.Parse(item.Id), this.Frame);
            }
            
            SystemNavigationManager.GetForCurrentView().BackRequested -= QuestionPage_BackRequested;
        }

    }
}
