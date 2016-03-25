using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.UI.Xaml;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Zhihu.Common.DataSource;
using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;
using Zhihu.Helper;
using Zhihu.Controls;
using System.Collections.Generic;

namespace Zhihu.ViewModel
{
    public sealed class FindViewModel : ViewModelBase
    {
        private readonly IFind _hot;

        private const String FirstOffset = "limit=20&offset=0";

        #region Recommends

        private Boolean _recommendsLoading = false;

        public Boolean RecommendsLoading
        {
            get { return _recommendsLoading; }
            private set
            {
                _recommendsLoading = value;
                RaisePropertyChanged(() => RecommendsLoading);
            }
        }

        private IncrementalLoading<EditorRecommend> _recommends;

        [Data]
        public IncrementalLoading<EditorRecommend> Recommends
        {
            get { return _recommends; }
            private set
            {
                if (_recommends == value) return;
                _recommends = value;

                RaisePropertyChanged(() => Recommends);
            }
        }

        #endregion

        #region Collections

        private Boolean _collectionsLoading = false;

        public Boolean CollectionLoading
        {
            get { return _collectionsLoading; }
            private set
            {
                _collectionsLoading = value;
                RaisePropertyChanged(() => CollectionLoading);
            }
        }

        private IncrementalLoading<HotCollection> _collections;

        [Data]
        public IncrementalLoading<HotCollection> Collections
        {
            get { return _collections; }
            private set
            {
                _collections = value;
                RaisePropertyChanged(() => Collections);
            }
        }

        #endregion

        #region Hot answers

        private Boolean _answersLoading = false;

        public Boolean AnswersLoading
        {
            get { return _answersLoading; }
            private set
            {
                _answersLoading = value;
                RaisePropertyChanged(() => AnswersLoading);
            }
        }

        private IncrementalLoading<HotAnswer> _hotAnswers;

        [Data]
        public IncrementalLoading<HotAnswer> HotAnswers
        {
            get { return _hotAnswers; }
            set
            {
                _hotAnswers = value;
                RaisePropertyChanged(() => HotAnswers);
            }
        }

        #endregion

        private Boolean _bannerLoading = false;

        public Boolean BannerLoading
        {
            get { return _bannerLoading; }
            private set
            {
                _bannerLoading = value;
                RaisePropertyChanged(() => BannerLoading);
            }
        }

        private Visibility _bannerVisibility = Visibility.Collapsed;

        [Data]
        public Visibility BannerVisibility
        {
            get { return _bannerVisibility; }
            private set
            {
                _bannerVisibility = value;
                RaisePropertyChanged(() => BannerVisibility);
            }
        }

        private Banner _banner;

        [Data]
        public Banner Banner
        {
            get { return _banner; }
            set
            {
                _banner = value;
                RaisePropertyChanged(() => Banner);
            }
        }

        public RelayCommand GetBanner { get; private set; }

        public RelayCommand RefreshRecommends { get; private set; }
        public RelayCommand RefreshHotAnswers { get; private set; }
        public RelayCommand RefreshCollections { get; private set; }

        private FindViewModel()
        {
        }

        public FindViewModel(IFind hot)
            : this()
        {
            _hot = hot;

            GetBanner = new RelayCommand(GetBannerMethod);
            RefreshRecommends = new RelayCommand(RefreshRecommendsMethod);
            RefreshHotAnswers = new RelayCommand(RefreshHotAnswersMethod);
            RefreshCollections = new RelayCommand(RefreshHotCollectionsMethod);

            Recommends = new IncrementalLoading<EditorRecommend>(GetMoreRecommends, "/hot/editor/recommend", FirstOffset, false);

            HotAnswers = new IncrementalLoading<HotAnswer>(GetMoreHotAnswers, "/hot/top/answers/day", FirstOffset, false);

            Collections = new IncrementalLoading<HotCollection>(GetMoreHotCollections, "/explore/collections", FirstOffset, false);
        }

        private async Task<ListResultBase> GetMoreRecommends(String request)
        {
            if (RecommendsLoading) return null;

            RecommendsLoading = true;

            var result = await _hot.GetRecommends(LoginUser.Current.Token, request, true);

            RecommendsLoading = false;

            if (result == null) return null;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                return null;
            }

            return result;
        }

        private async void RefreshRecommendsMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            if (RecommendsLoading) return;

            RecommendsLoading = true;

            var result = await _hot.GetRecommends(LoginUser.Current.Token, "/hot/editor/recommend", true);

            RecommendsLoading = false;

            if (result == null) return;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                return;
            }

            var recommendsAll = (from object recommend in Recommends select recommend as EditorRecommend).ToList();
            var newRecommends = new List<EditorRecommend>();

            var query = from i in result.Result.GetItems()
                        let asRecommend = i as EditorRecommend
                        where recommendsAll.All(o => asRecommend != null && o.Url != asRecommend.Url)
                        select asRecommend;

            newRecommends.AddRange(query);
            
            if (newRecommends.Count == 20)
            {
                Recommends = new IncrementalLoading<EditorRecommend>(GetMoreRecommends, "/hot/editor/recommend", FirstOffset, false);
                return;
            }

            for (var i = 0; i < newRecommends.Count; i++)
            {
                Recommends.Insert(i, newRecommends[i]);
            }
        }


        private async Task<ListResultBase> GetMoreHotAnswers(String request)
        {
            if (AnswersLoading) return null;

            AnswersLoading = true;

            var result = await _hot.GetTodayHotAnswers(LoginUser.Current.Token, request, true);

            AnswersLoading = false;

            if (result == null) return null;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                return null;
            }

            return result;
        }

        private async void RefreshHotAnswersMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            if (AnswersLoading) return;

            AnswersLoading = true;

            var result = await _hot.GetTodayHotAnswers(LoginUser.Current.Token, "/hot/top/answers/day", true);

            AnswersLoading = false;

            if (result == null) return;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

                return;
            }

            var hotAnswersAll = (from object hotAnswer in HotAnswers select hotAnswer as HotAnswer).ToList();
            var newHotAnswers = new List<HotAnswer>();

            var query = from i in result.Result.GetItems()
                        let asHotAnswer = i as HotAnswer
                        where hotAnswersAll.All(o => asHotAnswer != null && o.Answers[0].Url != asHotAnswer.Answers[0].Url)
                        select asHotAnswer;

            newHotAnswers.AddRange(query);
            
            if (newHotAnswers.Count == 20)
            {
                HotAnswers = new IncrementalLoading<HotAnswer>(GetMoreHotAnswers, "/hot/top/answers/day", FirstOffset, false);
                return;
            }

            for (var i = 0; i < newHotAnswers.Count; i++)
            {
                HotAnswers.Insert(i, newHotAnswers[i]);
            }
        }

        private async Task<ListResultBase> GetMoreHotCollections(String request)
        {
            if (CollectionLoading) return null;

            CollectionLoading = true;

            var result = await _hot.GetCollections(LoginUser.Current.Token, request, true);

            CollectionLoading = false;

            if (result == null) return null;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
            }

            return result;
        }

        private async void RefreshHotCollectionsMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            if (CollectionLoading) return;

            CollectionLoading = true;

            var result = await _hot.GetCollections(LoginUser.Current.Token, "/explore/collections", true);

            CollectionLoading = false;

            if (result == null) return;

            if (result.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
                return;
            }

            var hotCollectionsAll = (from object hotAnswer in Collections select hotAnswer as HotCollection).ToList();
            var newHotCollections = new List<HotCollection>();

            var query = from i in result.Result.GetItems()
                        let asHotCollection = i as HotCollection
                        where hotCollectionsAll.All(o => asHotCollection != null && o.Url != asHotCollection.Url)
                        select asHotCollection;

            newHotCollections.AddRange(query);

            if (newHotCollections.Count == 20)
            {
                Collections = new IncrementalLoading<HotCollection>(GetMoreHotCollections, "/explore/collections", FirstOffset, false);
                return;
            }

            for (var i = 0; i < newHotCollections.Count; i++)
            {
                Collections.Insert(i, newHotCollections[i]);
            }
        }

        private Boolean _bannerLoaded = false;

        private async void GetBannerMethod()
        {
            if (_bannerLoaded) return;

            BannerLoading = true;

            var result = await _hot.GetBanner(LoginUser.Current.Token, "banners", true);

            BannerLoading = false;

            if (result?.Result == null)
            {
                ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);
                return;
            }

            if (result.Result != null && result.Result.Normal != null && result.Result.Normal.Length > 0)
                _bannerLoaded = true;

            Banner = result.Result;

            BannerVisibility = result.Result?.Default?.FirstOrDefault() == null
                ? Visibility.Collapsed
                : Visibility.Visible;
        }
    }
}