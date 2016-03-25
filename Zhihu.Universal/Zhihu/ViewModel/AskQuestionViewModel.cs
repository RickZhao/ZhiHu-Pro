using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Windows.UI.Xaml;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Zhihu.Common.Model;
using Zhihu.Common.DataSource;
using Zhihu.Common.Helper;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;

using Zhihu.Helper;
using Zhihu.Controls;

namespace Zhihu.ViewModel
{
    public sealed class AskQuestionViewModel : ViewModelBase
    {
        private readonly ISearch _search;
        private readonly IQuestion _question;
        private readonly INavigate _navigate;

        private Boolean _searching = false;

        public Boolean Searching
        {
            get { return _searching; }
            private set
            {
                _searching = value;
                RaisePropertyChanged(() => Searching);
            }
        }

        private const String FirstOffset = "limit=20&offset=0";

        private String _keyword = String.Empty;

        [Data]
        public String Keyword
        {
            get { return _keyword; }
            set
            {
                if (_keyword == value) return;

                _keyword = value;

                RaisePropertyChanged(() => Keyword);

                SearchQuestionsMethod();
            }
        }

        private IncrementalLoading<Question> _questions;

        [Data]
        public IncrementalLoading<Question> Questions
        {
            get { return _questions; }
            private set
            {
                _questions = value;
                RaisePropertyChanged(() => Questions);
            }
        }

        private String _title = String.Empty;

        [Data]
        public String Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private String _description = String.Empty;

        [Data]
        public String Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        private Boolean _isAnonymous = false;

        [Data]
        public Boolean IsAnonymous
        {
            get { return _isAnonymous; }
            set
            {
                _isAnonymous = value;
                RaisePropertyChanged(() => IsAnonymous);
            }
        }

        private Visibility _searchVisibility = Visibility.Visible;

        [Data]
        public Visibility SearchVisibility
        {
            get { return _searchVisibility; }
            private set
            {
                _searchVisibility = value;
                RaisePropertyChanged(() => SearchVisibility);
            }
        }

        private Visibility _makeQuesVisibility = Visibility.Collapsed;

        [Data]
        public Visibility MakeQuesVisibility
        {
            get { return _makeQuesVisibility; }
            private set
            {
                _makeQuesVisibility = value;
                RaisePropertyChanged(() => MakeQuesVisibility);
            }
        }

        #region Topics

        private String _topicsKeyword = String.Empty;

        [Data]
        public String TopicsKeyword
        {
            get { return _topicsKeyword; }
            set
            {
                if (_topicsKeyword == value) return;
                _topicsKeyword = value;

                RaisePropertyChanged(() => TopicsKeyword);

                SearchTopicsMethod();
            }
        }

        [Data]
        public ObservableCollection<Topic> Topics { get; private set; }

        private Boolean _topicsSearching = false;

        public Boolean TopicsSearching
        {
            get { return _topicsSearching; }
            private set
            {
                _topicsSearching = value;
                RaisePropertyChanged(() => TopicsSearching);
            }
        }

        private IncrementalLoading<Topic> _searchTopics;

        [Data]
        public IncrementalLoading<Topic> SearchTopics
        {
            get { return _searchTopics; }
            private set
            {
                _searchTopics = value;
                RaisePropertyChanged(() => SearchTopics);
            }
        }

        #endregion

        public RelayCommand<Topic> TopicTapToAdd { get; private set; }

        public RelayCommand PostQuestion { get; private set; }

        public RelayCommand NavToMakeQues { get; private set; }
        public RelayCommand NavToAddTopics { get; private set; }

        private AskQuestionViewModel()
        {
            NavToMakeQues = new RelayCommand(NavToMakeQuesMethod);
            NavToAddTopics = new RelayCommand(NavToAddTopicsMethod);

            TopicTapToAdd = new RelayCommand<Topic>(TopicTapToAddMethod);

            PostQuestion = new RelayCommand(PostQuestionMethod);

            Setup(String.Empty);
        }

        public AskQuestionViewModel(ISearch search, IQuestion question, INavigate navigate)
            : this()
        {
            _search = search;
            _question = question;
            _navigate = navigate;
        }

        public void Setup(String noneSense)
        {
            //VmHelper.Save(this);

            Topics = new ObservableCollection<Topic>();

            SearchVisibility = Visibility.Visible;
            MakeQuesVisibility = Visibility.Collapsed;
        }
        

        private async void PostQuestionMethod()
        {
            if (_question == null) return;

            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            var topicIds = Topics.Aggregate(String.Empty,
                (current, topic) => current + String.Format("{0},", topic.Id));

            if (topicIds.Length >= 1)
            {
                topicIds = topicIds.Substring(0, topicIds.Length - 1);
            }

            var result = await _question.CreateAsync(LoginUser.Current.Token, IsAnonymous, topicIds, Title, Description);

            if (result.Error != null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
                return;
            }

            if (_navigate == null) return;

            _navigate.GoBack();

            //VmNavHelper.NavToProfileQuestionsPage(LoginUser.Current.Profile.Id, _navigate);
        }

        private void SearchQuestionsMethod()
        {
            if (String.IsNullOrEmpty(Keyword) && Questions != null)
            {
                Questions.Clear();
            }

            Questions = new IncrementalLoading<Question>(GetMoreQuestions, "search?t=question&q={0}", FirstOffset, false);
        }

        private void SearchTopicsMethod()
        {
            if (String.IsNullOrEmpty(TopicsKeyword) && SearchTopics != null)
            {
                SearchTopics.Clear();
            }

            SearchTopics = new IncrementalLoading<Topic>(GetMoreTopics, "search?t=topic&q={0}", FirstOffset,
                false);
        }

        private async Task<ListResultBase> GetMoreQuestions(String request)
        {
            if (Searching) return null;

            Searching = true;

            var result = await _search.FindQuestionsAsync(LoginUser.Current.Token, String.Format(request, Keyword));

            Searching = false;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            return result;
        }

        private async Task<ListResultBase> GetMoreTopics(String request)
        {
            if (TopicsSearching) return null;

            TopicsSearching = true;

            var result = await _search.FindTopicsAsync(LoginUser.Current.Token, String.Format(request, TopicsKeyword));

            TopicsSearching = false;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            return result;
        }


        private void TopicTapToAddMethod(Topic topic)
        {
            if (topic == null) return;
            if (Topics == null) Topics = new ObservableCollection<Topic>();

            if (Topics.Contains(topic) == false)
            {
                Topics.Add(topic);
            }

            RaisePropertyChanged(() => Topics);

            if (_navigate == null) return;

            _navigate.GoBack();
        }

        private void NavToMakeQuesMethod()
        {
            SearchVisibility = Visibility.Collapsed;
            MakeQuesVisibility = Visibility.Visible;
        }

        private void NavToAddTopicsMethod()
        {
            //if (_navigate != null) _navigate.NavigateTo(typeof(AddTopicsPage));
        }
    }
}