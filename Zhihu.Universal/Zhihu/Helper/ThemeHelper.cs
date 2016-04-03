using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using GalaSoft.MvvmLight;

using Zhihu.Common.Helper;


namespace Zhihu.Helper
{
    public sealed class ThemeHelper
    {
        public Theme Theme
        {
            get { return Theme.Instance; }
        }
    }

    public sealed class Theme : ObservableObject
    {
        private Boolean _noImage = true;

        /// <summary>
        /// 无图模式默认开启
        /// </summary>
        public Boolean NoImage
        {
            get
            {
                var noImageKey = Utility.Instance.NoImageKey;
                var noImageValue = LocalSettingUtility.Instance.Read<String>(noImageKey);

                if (noImageValue != null)
                {
                    _noImage = Boolean.Parse(noImageValue);
                }

                return _noImage;
            }
            set
            {
                _noImage = value;

                var noImageKey = Utility.Instance.NoImageKey;
                LocalSettingUtility.Instance.Add(noImageKey, _noImage.ToString());

                RaisePropertyChanged();
            }
        }

        private Boolean _textSelectionEnable = true;

        public Boolean TextSelectionEnable
        {
            get
            {
                var noImageKey = Utility.Instance.AllowTextSelectionKey;
                var noImageValue = LocalSettingUtility.Instance.Read<String>(noImageKey);

                if (noImageValue != null)
                {
                    _textSelectionEnable = Boolean.Parse(noImageValue);
                }

                return _textSelectionEnable;
            }
            set
            {
                _textSelectionEnable = value;

                var noImageKey = Utility.Instance.AllowTextSelectionKey;
                LocalSettingUtility.Instance.Add(noImageKey, _textSelectionEnable.ToString());

                RaisePropertyChanged();
            }
        }

        private Boolean _statusBarIsOpen = false;

        public Boolean StatusBarIsOpen
        {
            get
            {
                var noImageKey = Utility.Instance.StatusBarIsOpenKey;
                var noImageValue = LocalSettingUtility.Instance.Read<String>(noImageKey);

                if (noImageValue != null)
                {
                    _statusBarIsOpen = Boolean.Parse(noImageValue);
                }

                return _statusBarIsOpen;
            }
            set
            {
                _statusBarIsOpen = value;

                var noImageKey = Utility.Instance.StatusBarIsOpenKey;
                LocalSettingUtility.Instance.Add(noImageKey, _statusBarIsOpen.ToString());

                RaisePropertyChanged();
            }
        }
        
        public Boolean IsBlackOn
        {
            get
            {
                var themeKey = Utility.Instance.CurrentThemeKey;
                var currentTheme = LocalSettingUtility.Instance.Read<String>(themeKey);

                if (String.IsNullOrEmpty(currentTheme) || currentTheme == "Light") return false;
                else return true;
            }
        }

        #region Colors

        private Color _pageBackColor;

        public Color PageBackColor
        {
            get { return _pageBackColor; }
            private set
            {
                _pageBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _appBarForeColor;

        public Color AppBarForeColor
        {
            get { return _appBarForeColor; }
            private set
            {
                _appBarForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _appBarHighlightForeColor;

        public Color AppBarHighlightForeColor
        {
            get { return _appBarHighlightForeColor; }
            private set
            {
                _appBarHighlightForeColor = value;
                RaisePropertyChanged();
            }
        }


        private Color _appBarBackColor;

        public Color AppBarBackColor
        {
            get { return _appBarBackColor; }
            private set
            {
                _appBarBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _feedTitleColor;

        public Color FeedTitleColor
        {
            get { return _feedTitleColor; }
            private set
            {
                _feedTitleColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _feedSummaryColor;

        public Color FeedSummaryColor
        {
            get { return _feedSummaryColor; }
            private set
            {
                _feedSummaryColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _feedVerbColor;

        public Color FeedVerbColor
        {
            get { return _feedVerbColor; }
            private set
            {
                _feedVerbColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _authorColor;

        public Color AuthorColor
        {
            get { return _authorColor; }
            private set
            {
                _authorColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _titleBorderColor;

        public Color TitleBorderColor
        {
            get { return _titleBorderColor; }
            private set
            {
                _titleBorderColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _titleForeColor;

        public Color TitleForeColor
        {
            get { return _titleForeColor; }
            private set
            {
                _titleForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _titleBackColor;

        public Color TitleBackColor
        {
            get { return _titleBackColor; }
            private set
            {
                _titleBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _voteForeColor;

        public Color VoteForeColor
        {
            get { return _voteForeColor; }
            private set
            {
                _voteForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _voteSecondForeColor;

        public Color VoteSecondForeColor
        {
            get { return _voteSecondForeColor; }
            private set
            {
                _voteSecondForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _voteBackColor;

        public Color VoteBackColor
        {
            get { return _voteBackColor; }
            private set
            {
                _voteBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _topicBackColor;

        public Color TopicBackColor
        {
            get { return _topicBackColor; }
            private set
            {
                _topicBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _topicForeColor;

        public Color TopicForeColor
        {
            get { return _topicForeColor; }
            private set
            {
                _topicForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _quesTitleForeColor;

        public Color QueTitleForeColor
        {
            get { return _quesTitleForeColor; }
            private set
            {
                _quesTitleForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _quesContentForeColor;

        public Color QueContentForeColor
        {
            get { return _quesContentForeColor; }
            private set
            {
                _quesContentForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _noteForeColor;

        public Color NoteForeColor
        {
            get { return _noteForeColor; }
            private set
            {
                _noteForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _followForeColor;

        public Color FollowForeColor
        {
            get { return _followForeColor; }
            private set
            {
                _followForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _followBackColor;

        public Color FollowBackColor
        {
            get { return _followBackColor; }
            private set
            {
                _followBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _followerForeColor;

        public Color FollowerForeColor
        {
            get { return _followerForeColor; }
            private set
            {
                _followerForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _answerTitleForeColor;

        public Color AnswerTitleForeColor
        {
            get { return _answerTitleForeColor; }
            private set
            {
                _answerTitleForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _answerTitleBackColor;

        public Color AnswerTitleBackColor
        {
            get { return _answerTitleBackColor; }
            private set
            {
                _answerTitleBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _answerContentForeColor;

        public Color AnswerContentForeColor
        {
            get { return _answerContentForeColor; }
            private set
            {
                _answerContentForeColor = value;
                RaisePropertyChanged();
            }
        }


        private Color _lightBackColor;

        public Color LightBackColor
        {
            get { return _lightBackColor; }
            private set
            {
                _lightBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _flyoutBackColor;

        public Color FlyoutBackColor
        {
            get { return _flyoutBackColor; }
            private set
            {
                _flyoutBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _borderForeColor;

        public Color BorderForeColor
        {
            get { return _borderForeColor; }
            private set
            {
                _borderForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _borderBackColor;

        public Color BorderBackColor
        {
            get { return _borderBackColor; }
            private set
            {
                _borderBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _progressBackColor;

        public Color ProgressBackColor
        {
            get { return _progressBackColor; }
            private set
            {
                _progressBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _progressForeColor;

        public Color ProgressForeColor
        {
            get { return _progressForeColor; }
            private set
            {
                _progressForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _toastForeColor;

        public Color ToastForeColor
        {
            get { return _toastForeColor; }
            private set
            {
                _toastForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _toastBackColor;

        public Color ToastBackColor
        {
            get { return _toastBackColor; }
            private set
            {
                _toastBackColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _linkForeColor;

        public Color LinkForeColor
        {
            get { return _linkForeColor; }
            private set
            {
                _linkForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _firstForeColor;

        public Color FirstForeColor
        {
            get { return _firstForeColor; }
            private set
            {
                _firstForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _secondForeColor;

        public Color SecondForeColor
        {
            get { return _secondForeColor; }
            private set
            {
                _secondForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _thirdForeColor;

        public Color ThirdForeColor
        {
            get { return _thirdForeColor; }
            private set
            {
                _thirdForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _authorForeColor;

        public Color AuthorForeColor
        {
            get { return _authorForeColor; }
            private set
            {
                _authorForeColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _lineColor;

        public Color LineColor
        {
            get { return _lineColor; }
            private set
            {
                _lineColor = value;
                RaisePropertyChanged();
            }
        }

        private Color _boldForeColor;

        public Color BoldForeColor
        {
            get { return _boldForeColor; }
            private set
            {
                _boldForeColor = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region FontSizes

        private Double _pageTitle;

        public Double PageTitle
        {
            get
            {
                if (_pageTitle < 1.0)
                {
                    Double.TryParse(LocalSettingUtility.Instance.Read<String>("PageTitle"), out _pageTitle);
                }

                if (_pageTitle < 10)
                    _pageTitle = Constant.DefaultFontSize.PageTitle;

                return _pageTitle;
            }
            set
            {
                _pageTitle = value;

                RaisePropertyChanged();

                LocalSettingUtility.Instance.Add("PageTitle", value.ToString());
            }
        }


        private Double _feedTitle;

        public Double FeedTitle
        {
            get
            {
                if (_feedTitle < 1.0)
                {
                    Double.TryParse(LocalSettingUtility.Instance.Read<String>("FeedTitle"), out _feedTitle);
                }

                if (_feedTitle < 6)
                    _feedTitle = Constant.DefaultFontSize.FeedTitle;

                return _feedTitle;
            }
            set
            {
                _feedTitle = value;

                RaisePropertyChanged();

                LocalSettingUtility.Instance.Add("FeedTitle", value.ToString());
            }
        }

        private Double _feedSummary;

        public Double FeedSummary
        {
            get
            {
                if (_feedSummary < 1.0)
                {
                    Double.TryParse(LocalSettingUtility.Instance.Read<String>("FeedSummary"), out _feedSummary);
                }

                if (_feedSummary < 6)
                    _feedSummary = Constant.DefaultFontSize.FeedSummary;

                return _feedSummary;
            }
            set
            {
                _feedSummary = value;

                RaisePropertyChanged();

                LocalSettingUtility.Instance.Add("FeedSummary", value.ToString());
            }
        }

        private Double _feedVerb;

        public Double FeedVerb
        {
            get
            {
                if (_feedVerb < 1.0)
                {
                    Double.TryParse(LocalSettingUtility.Instance.Read<String>("FeedVerb"), out _feedVerb);
                }

                if (_feedVerb < 6)
                    _feedVerb = Constant.DefaultFontSize.FeedVerb;

                return _feedVerb;
            }
            set
            {
                _feedVerb = value;

                RaisePropertyChanged();

                LocalSettingUtility.Instance.Add("FeedVerb", value.ToString());
            }
        }

        private Double _voteCount;

        public Double VoteCount
        {
            get
            {
                if (_voteCount < 1.0)
                {
                    Double.TryParse(LocalSettingUtility.Instance.Read<String>("VoteCount"), out _voteCount);
                }

                if (_voteCount < 6)
                    _voteCount = Constant.DefaultFontSize.VoteCount;

                return _voteCount;
            }
            set
            {
                _voteCount = value;

                RaisePropertyChanged();

                LocalSettingUtility.Instance.Add("VoteCount", value.ToString());
            }
        }

        #endregion
        
        #region Singleton

        private static Theme _instance;

        private Theme()
        {
            LoadTheme();
        }

        public static Theme Instance => _instance ?? (_instance = new Theme());

        #endregion

        private void LoadTheme()
        {
            if (IsBlackOn==false)
            {
                LoadLight();
            }
            else
            {
                LoadDark();
            }
        }

        public void UpdateRequestedTheme(Page page)
        {
            if (page == null) return;

            page.RequestedTheme = IsBlackOn == false ? ElementTheme.Light : ElementTheme.Dark;
        }

        public void UpdateIsBlack()
        {
            RaisePropertyChanged(() => IsBlackOn);
        }

        public void TurnLight()
        {
            LocalSettingUtility.Instance.Add(Utility.Instance.CurrentThemeKey, "Light");
            LoadLight();
            RaisePropertyChanged(() => IsBlackOn);
        }

        public void TurnDark()
        {
            LocalSettingUtility.Instance.Add(Utility.Instance.CurrentThemeKey, "Dark");
            LoadDark();
            RaisePropertyChanged(() => IsBlackOn);
        }

        private void LoadLight()
        {
            PageBackColor = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);

            AppBarForeColor = Color.FromArgb(0xFF, 0x22, 0x22, 0x22);
            AppBarHighlightForeColor = Color.FromArgb(0xFF, 0x07, 0x67, 0xC8);
            AppBarBackColor = Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD);

            FeedTitleColor = Color.FromArgb(0xFF, 0x4C, 0x56, 0x6C);
            FeedSummaryColor = Color.FromArgb(0xFF, 0x64, 0x64, 0x64);
            FeedVerbColor = Color.FromArgb(0xFF, 0x9A, 0x9A, 0x9A);

            AuthorColor = Color.FromArgb(0xFF, 0x95, 0x96, 0xAB);

            TitleBorderColor = Color.FromArgb(0xFF, 0x04, 0x4E, 0x97);
            TitleForeColor = Color.FromArgb(0xFF, 0x07, 0x67, 0xC8);
            TitleBackColor = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);

            VoteForeColor = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            VoteSecondForeColor = Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD);
            VoteBackColor = Color.FromArgb(0xFF, 0x64, 0xB3, 0xF1);

            TopicBackColor = Color.FromArgb(0xFF, 0xE8, 0xF0, 0xFE);
            TopicForeColor = Color.FromArgb(0xFF, 0x8D, 0xAA, 0xB8);

            QueTitleForeColor = Color.FromArgb(0xFF, 0x4C, 0x56, 0x6C);
            QueContentForeColor = Color.FromArgb(0xFF, 0x6E, 0x6E, 0x6E);

            FollowBackColor = Color.FromArgb(0xFF, 0x45, 0xCB, 0x7F);
            FollowForeColor = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);

            NoteForeColor = Color.FromArgb(0xFF, 0x6A, 0x74, 0x80);

            FollowerForeColor = Color.FromArgb(0xFF, 0xCC, 0xCC, 0xCC);

            AnswerTitleForeColor = Color.FromArgb(0xFF, 0x4C, 0x56, 0x6C);
            AnswerTitleBackColor = Color.FromArgb(0xFF, 0xEF, 0xEF, 0xF4);

            AnswerContentForeColor = Color.FromArgb(0xFF, 0x44, 0x44, 0x44);

            LightBackColor = Color.FromArgb(0xff, 0xEE, 0xEE, 0xEE);

            FlyoutBackColor = Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE);

            BorderForeColor = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            BorderBackColor = Color.FromArgb(0xFF, 0x07, 0x67, 0xC8);

            ProgressBackColor = Colors.Transparent;
            ProgressForeColor = Color.FromArgb(0xFF, 0x07, 0x67, 0xC8);

            ToastForeColor = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
            ToastBackColor = Color.FromArgb(0xFD, 0x3D, 0x70, 0x96);

            LinkForeColor = Color.FromArgb(0xFF, 0x07, 0x67, 0xC8);

            FirstForeColor = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            SecondForeColor = Color.FromArgb(0xFF, 0x66, 0x66, 0x66);
            ThirdForeColor = Color.FromArgb(0xFF, 0xAA, 0xAA, 0xAA);

            AuthorForeColor = Color.FromArgb(0xFF, 0x07, 0x67, 0xC8);

            LineColor = Color.FromArgb(0xFF, 0xD8, 0xD8, 0xD9);

            BoldForeColor = Color.FromArgb(0xFF, 0x55, 0x55, 0x55);
        }

        private void LoadDark()
        {
            PageBackColor = Color.FromArgb(0xFD, 0x11, 0x11, 0x11);

            AppBarForeColor = Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE);
            AppBarHighlightForeColor = Color.FromArgb(0xFF, 0x07, 0x67, 0xC8);
            AppBarBackColor = Color.FromArgb(0xFF, 0x22, 0x22, 0x22);

            FeedTitleColor = Color.FromArgb(0xFF, 0x85, 0x8D, 0x9E);
            FeedSummaryColor = Color.FromArgb(0xFF, 0x77, 0x77, 0x7F);
            FeedVerbColor = Color.FromArgb(0xFF, 0x60, 0x64, 0x76);

            AuthorColor = Color.FromArgb(0xFF, 0x70, 0x76, 0x84);

            TitleBorderColor = Color.FromArgb(0xFF, 0x04, 0x4E, 0x97);
            TitleForeColor = Color.FromArgb(0xDD, 0x07, 0x67, 0xC8);
            TitleBackColor = Color.FromArgb(0xFF, 0x22, 0x22, 0x22);

            VoteForeColor = Color.FromArgb(0xFF, 0xC8, 0xC8, 0xC8);
            VoteSecondForeColor = Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE);
            VoteBackColor = Color.FromArgb(0xFF, 0x46, 0x8B, 0xBB);

            TopicBackColor = Color.FromArgb(0xFF, 0x24, 0x24, 0x30);
            TopicForeColor = Color.FromArgb(0xFF, 0x5D, 0x69, 0x82);

            QueTitleForeColor = Color.FromArgb(0xFF, 0x85, 0x8D, 0x9E);
            QueContentForeColor = Color.FromArgb(0xFF, 0x97, 0x9D, 0x9F);

            FollowBackColor = Color.FromArgb(0xFF, 0x1D, 0xB5, 0x76);
            FollowForeColor = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);

            NoteForeColor = Color.FromArgb(0xFF, 0x6A, 0x74, 0x80);

            FollowerForeColor = Color.FromArgb(0xFF, 0xCC, 0xCC, 0xCC);

            AnswerTitleForeColor = Color.FromArgb(0xFF, 0x83, 0x8B, 0x9C);
            AnswerTitleBackColor = Color.FromArgb(0xFF, 0x25, 0x26, 0x34);

            AnswerContentForeColor = Color.FromArgb(0xFF, 0x7B, 0x80, 0x94);

            LightBackColor = Color.FromArgb(0x40, 0x55, 0x55, 0x55);

            FlyoutBackColor = Color.FromArgb(0xFF, 0x44, 0x44, 0x44);

            BorderForeColor = Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE);
            BorderBackColor = Color.FromArgb(0xDD, 0x07, 0x67, 0xC8);

            ProgressBackColor = Colors.Transparent;
            ProgressForeColor = Color.FromArgb(0xDD, 0x07, 0x67, 0xC8);

            ToastForeColor = Color.FromArgb(0xFF, 0x00, 0x00, 0x00);
            ToastBackColor = Color.FromArgb(0xFD, 0x3D, 0x70, 0x96);

            LinkForeColor = Color.FromArgb(0xDD, 0x07, 0x67, 0xC8);

            FirstForeColor = Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE);
            SecondForeColor = Color.FromArgb(0xFF, 0xA0, 0xA0, 0xA0);
            ThirdForeColor = Color.FromArgb(0xFF, 0x66, 0x66, 0x66);

            AuthorForeColor = Color.FromArgb(0xDD, 0x07, 0x67, 0xC8);

            LineColor = Color.FromArgb(0xFF, 0x28, 0x2C, 0x3A);

            BoldForeColor = Color.FromArgb(0xFF, 0xEE, 0xEE, 0xEE);
        }

        internal void ResetFontSize()
        {
            PageTitle = Constant.DefaultFontSize.PageTitle;
            FeedTitle = Constant.DefaultFontSize.FeedTitle;
            FeedSummary = Constant.DefaultFontSize.FeedSummary;
            FeedVerb = Constant.DefaultFontSize.FeedVerb;
            VoteCount = Constant.DefaultFontSize.VoteCount;
        }

        #region layout

        // upper voting button
        private Boolean _upperVotingButtonVisiable;

        public Boolean UpperVotingButtonVisiable
        {
            get
            {
                var success = Boolean.TryParse(LocalSettingUtility.Instance.Read<String>("upperVotingButtonVisiable"),
                    out _lowerVotingButtonVisiable);

                if (!success)
                    _upperVotingButtonVisiable = Constant.DefaultLayout.UpperVotingButtonVisible;

                return _upperVotingButtonVisiable;
            }
            set
            {
                _upperVotingButtonVisiable = value;
                LocalSettingUtility.Instance.Add("upperVotingButtonVisiable", value.ToString());
            }
        }

        public String UpperVotingButtonVisiblity => UpperVotingButtonVisiable ? "Visible" : "Collapsed";

        // lower voting button
        private Boolean _lowerVotingButtonVisiable;

        public Boolean LowerVotingButtonVisiable
        {
            get
            {
                var success = Boolean.TryParse(LocalSettingUtility.Instance.Read<String>("lowerVotingButtonVisiable"),
                    out _lowerVotingButtonVisiable);

                if (!success)
                    _lowerVotingButtonVisiable = Constant.DefaultLayout.LowerVotingButtonVisible;

                return _lowerVotingButtonVisiable;
            }
            set
            {
                _lowerVotingButtonVisiable = value;
                LocalSettingUtility.Instance.Add("lowerVotingButtonVisiable", value.ToString());
            }
        }

        public String LowerVotingButtonVisiblity => LowerVotingButtonVisiable ? "Visible" : "Collapsed";

        // upvote and down button (lowerVoteButtonPair)
        private Boolean _lowerVotingButtonPairVisiable;

        public Boolean LowerVotingButtonPairVisiable
        {
            get
            {
                var success = Boolean.TryParse(LocalSettingUtility.Instance.Read<String>("lowerVotingButtonPairVisiable"),
                    out _lowerVotingButtonPairVisiable);

                if (!success)
                    _lowerVotingButtonPairVisiable = Constant.DefaultLayout.LowerVotingButtonPairVisible;

                return _lowerVotingButtonPairVisiable;
            }
            set
            {
                _lowerVotingButtonPairVisiable = value;
                LocalSettingUtility.Instance.Add("lowerVotingButtonVisiable", value.ToString());
            }
        }

        public String LowerVotingButtonPairVisiblity => LowerVotingButtonPairVisiable ? "Visible" : "Collapsed";

        // Thanks button
        private Boolean _thanksButtonVisiable;

        public Boolean ThanksButtonVisiable
        {
            get
            {
                var success = Boolean.TryParse(LocalSettingUtility.Instance.Read<String>("ThanksButtonVisiable"),
                    out _thanksButtonVisiable);

                if (!success)
                    _thanksButtonVisiable = Constant.DefaultLayout.ThanksButtonVisible;

                return _thanksButtonVisiable;
            }
            set
            {
                _thanksButtonVisiable = value;
                LocalSettingUtility.Instance.Add("ThanksButtonVisiable", value.ToString());
            }
        }
        public String ThanksButtonVisiblity => ThanksButtonVisiable ? "Visible" : "Collapsed";
        public String SecondaryThanksButtonVisiblity => ThanksButtonVisiable ? "Visible" : "Collapsed";

        // not-helpful button
        private Boolean _notHelpfulButtonVisiable;

        public Boolean NotHelpfullButtonVisiable
        {
            get
            {
                var success = Boolean.TryParse(LocalSettingUtility.Instance.Read<String>("NotHelpfulButtonVisiable"),
                    out _notHelpfulButtonVisiable);

                if (!success)
                    _notHelpfulButtonVisiable = Constant.DefaultLayout.NotHelpfulButtonVisible;

                return _notHelpfulButtonVisiable;
            }
            set
            {
                _notHelpfulButtonVisiable = value;
                LocalSettingUtility.Instance.Add("NotHelpfulButtonVisiable", value.ToString());
            }
        }
        public String NotHelpfulButtonVisiblity => NotHelpfullButtonVisiable ? "Visible" : "Collapsed";
        public String SecondaryNotHelpfulButtonVisiblity => NotHelpfullButtonVisiable ? "Visible" : "Collapsed";

        // add to collection button
        private Boolean _addToCollectionButtonVisiable;

        public Boolean AddToCollectionButtonVisiable
        {
            get
            {
                var success = Boolean.TryParse(LocalSettingUtility.Instance.Read<String>("AddToCollectionButtonVisiable"),
                    out _addToCollectionButtonVisiable);

                if (!success)
                    _addToCollectionButtonVisiable = Constant.DefaultLayout.AddToCollectionButtonVisible;

                return _addToCollectionButtonVisiable;
            }
            set
            {
                _addToCollectionButtonVisiable = value;
                LocalSettingUtility.Instance.Add("AddToCollectionButtonVisiable", value.ToString());
            }
        }
        public String AddToCollectionButtonVisiblity => AddToCollectionButtonVisiable ? "Visible" : "Collapsed";
        public String SecondaryAddToCollectionButtonVisiblity => AddToCollectionButtonVisiable ? "Visible" : "Collapsed";

        // comment button
        private Boolean _commentButtonVisiable;

        public Boolean CommentButtonVisiable
        {
            get
            {
                var success = Boolean.TryParse(LocalSettingUtility.Instance.Read<String>("CommentButtonVisiable"),
                    out _commentButtonVisiable);

                if (!success)
                    _commentButtonVisiable = Constant.DefaultLayout.CommentButtonVisible;

                return _commentButtonVisiable;
            }
            set
            {
                _commentButtonVisiable = value;
                LocalSettingUtility.Instance.Add("CommentButtonVisiable", value.ToString());
            }
        }
        public String CommentButtonVisiblity => CommentButtonVisiable? "Visible" : "Collapsed";
        public String SecondaryCommentButtonVisiblity => CommentButtonVisiable ? "Visible" : "Collapsed";

        // share to wechat button
        private Boolean _shareToWechatButtonVisiable;

        public Boolean ShareToWechatButtonVisiable
        {
            get
            {
                var success = Boolean.TryParse(LocalSettingUtility.Instance.Read<String>("ShareToWechatButtonVisiable"),
                    out _shareToWechatButtonVisiable);

                if (!success)
                    _shareToWechatButtonVisiable = Constant.DefaultLayout.ShareToWechatButtonVisible;

                return _shareToWechatButtonVisiable;
            }
            set
            {
                _shareToWechatButtonVisiable = value;
                LocalSettingUtility.Instance.Add("ShareToWechatButtonVisiable", value.ToString());
            }
        }
        public String ShareToWechatButtonVisiblity => ShareToWechatButtonVisiable ? "Visible" : "Collapsed";
        public String SecondaryShareToWechatButtonVisiblity => ShareToWechatButtonVisiable ? "Visible" : "Collapsed";

        // open with edge button
        private Boolean _openWithEdgeButtonVisiable;

        public Boolean OpenWithEdgeButtonVisiable
        {
            get
            {
                var success = Boolean.TryParse(LocalSettingUtility.Instance.Read<String>("OpenWithEdgeButtonVisiable"),
                    out _openWithEdgeButtonVisiable);

                if (!success)
                    _openWithEdgeButtonVisiable = Constant.DefaultLayout.OpenWithEdgeButtonVisible;

                return _openWithEdgeButtonVisiable;
            }
            set
            {
                _openWithEdgeButtonVisiable = value;
                LocalSettingUtility.Instance.Add("OpenWithEdgeButtonVisiable", value.ToString());
            }
        }
        public String OpenWithEdgeButtonVisiblity => OpenWithEdgeButtonVisiable ? "Visible" : "Collapsed";
        public String SecondaryOpenWithEdgeButtonVisiblity => OpenWithEdgeButtonVisiable ? "Visible" : "Collapsed";
        #endregion

    }
}