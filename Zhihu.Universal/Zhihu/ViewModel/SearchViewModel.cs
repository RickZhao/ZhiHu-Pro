using System;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Zhihu.Common.DataSource;
using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;
using Zhihu.Controls;


namespace Zhihu.ViewModel
{
    public sealed class SearchViewModel : ViewModelBase
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

                SearchMethod();
            }
        }


        private IncrementalLoading<Author> _people;

        [Data]
        public IncrementalLoading<Author> People
        {
            get { return _people; }
            private set
            {
                _people = value;
                RaisePropertyChanged(() => People);
            }
        }

        private IncrementalLoading<SearchItem> _contents;

        [Data]
        public IncrementalLoading<SearchItem> Contents
        {
            get { return _contents; }
            private set
            {
                _contents = value;
                RaisePropertyChanged(() => Contents);
            }
        }

        [Data]
        private Int32 _questionId;

        public RelayCommand<Author> InviteAnswerTapped { get; private set; }
        

        private SearchViewModel()
        {

        }

        public SearchViewModel(ISearch search, IQuestion question, INavigate navigate) : this()
        {
            _search = search;
            _question = question;
            _navigate = navigate;
            
            InviteAnswerTapped = new RelayCommand<Author>(InviteAnswerTappedMethod);
        }
        
        public void Setup(Int32 questionId)
        {
            //if (_questionId>0)
            //{
            //    VmHelper.SaveStates(this, _questionId.ToString());
            //}
            
            _questionId = questionId;
            //VmHelper.ResumeStates(this, _questionId.ToString());
        }

        private void SearchMethod()
        {
            if (String.IsNullOrEmpty(Keyword) && Contents != null)
            {
                Contents.Clear();
            }
            if (String.IsNullOrEmpty(Keyword) && People != null)
            {
                People.Clear();
            }

            Contents = new IncrementalLoading<SearchItem>(GetMoreContents,
                "search?t=union&q={0}", FirstOffset, false);

            People = new IncrementalLoading<Author>(GetMorePeople,
                "search?t=people&q={0}", FirstOffset, false);
        }

        private async Task<ListResultBase> GetMoreContents(String request)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                await Task.Delay(3000);

                return null;
            }

            #endregion

            if (Searching) return null;

            Searching = true;

            var result = await _search.FindContents(LoginUser.Current.Token, String.Format(request, Keyword));

            Searching = false;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            return result;
        }

        private async Task<ListResultBase> GetMorePeople(String request)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
                await Task.Delay(3000);

                return null;
            }

            #endregion

            if (Searching) return null;

            Searching = true;

            var result = await _search.FindPeople(LoginUser.Current.Token, String.Format(request, Keyword));

            Searching = false;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            return result;
        }

        private async void InviteAnswerTappedMethod(Author author)
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);
                
                return;
            }

            #endregion

            if (author == null || _question == null || _questionId <= 0) return;

            var result = await _question.Invite(LoginUser.Current.Token, _questionId, author.Id);

            Searching = false;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            ToasteIndicator.Instance.Show(String.Empty, "邀请成功", null, 3);

#if WINDOWS_PHONE_APP

            _questionId = -1;
            Keyword = String.Empty;

            if (_navigate != null) _navigate.GoBack();

#endif

        }
    }
}
