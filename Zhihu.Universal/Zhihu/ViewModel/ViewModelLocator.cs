using Microsoft.Practices.ServiceLocation;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using Zhihu.Common.Cache;
using Zhihu.Common.Service;
using Zhihu.Common.Service.Design;
using Zhihu.Common.Service.Runtime;


namespace Zhihu.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        //public ProfileViewModel Profile
        //{
        //    get { return ServiceLocator.Current.GetInstance<ProfileViewModel>(); }
        //}

        public LoginViewModel Login
        {
            get { return ServiceLocator.Current.GetInstance<LoginViewModel>(); }
        }

        //public QuestionViewModel Question
        //{
        //    get { return ServiceLocator.Current.GetInstance<QuestionViewModel>(); }
        //}

        //public AnswerViewModel Answer
        //{
        //    get { return ServiceLocator.Current.GetInstance<AnswerViewModel>(); }
        //}

        //public ArticleViewModel Article
        //{
        //    get { return ServiceLocator.Current.GetInstance<ArticleViewModel>(); }
        //}

        public AmazingGuyViewModel Explore
        {
            get { return ServiceLocator.Current.GetInstance<AmazingGuyViewModel>(); }
        }

        public FindViewModel Hot
        {
            get { return ServiceLocator.Current.GetInstance<FindViewModel>(); }
        }

        public CollectionViewModel Collection
        {
            get { return ServiceLocator.Current.GetInstance<CollectionViewModel>(); }
        }

        public TopicViewModel Topic
        {
            get { return ServiceLocator.Current.GetInstance<TopicViewModel>(); }
        }

        public ColumnViewModel Column
        {
            get { return ServiceLocator.Current.GetInstance<ColumnViewModel>(); }
        }

        public SearchViewModel Search
        {
            get { return ServiceLocator.Current.GetInstance<SearchViewModel>(); }
        }

        public AskQuestionViewModel Ask
        {
            get { return ServiceLocator.Current.GetInstance<AskQuestionViewModel>(); }
        }

        public AboutViewModel About
        {
            get { return ServiceLocator.Current.GetInstance<AboutViewModel>(); }
        }

        public SettingViewModel Setting
        {
            get { return ServiceLocator.Current.GetInstance<SettingViewModel>(); }
        }

        public MessageViewModel Message
        {
            get { return ServiceLocator.Current.GetInstance<MessageViewModel>(); }
        }

        public OfflineViewModel Offline
        {
            get { return ServiceLocator.Current.GetInstance<OfflineViewModel>(); }
        }

        public NotifyViewModel Notify
        {
            get { return ServiceLocator.Current.GetInstance<NotifyViewModel>(); }
        }

        public TableViewModel Table
        {
            get { return ServiceLocator.Current.GetInstance<TableViewModel>(); }
        }

        public WebViewModel WebView
        {
            get { return ServiceLocator.Current.GetInstance<WebViewModel>(); }
        }


        public ViewModelLocator()
        {
            var cache = DbContext.Instance;

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<INavigate, NavigateService>();
          
            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IAnswer, DesignAnswerService>();
                SimpleIoc.Default.Register<IArticle, DesignArticleService>();

                SimpleIoc.Default.Register<IColumn, DesignColumnSerivce>();
                SimpleIoc.Default.Register<ICollection, DesignCollectionService>();
                SimpleIoc.Default.Register<IComment, DesignCommentService>();

                SimpleIoc.Default.Register<IFind, DesignFindService>();
                SimpleIoc.Default.Register<ILogin, DesignLoginService>();
                SimpleIoc.Default.Register<IFeed, DesignFeedService>();

                SimpleIoc.Default.Register<IPerson, DesignPersonService>();
                SimpleIoc.Default.Register<IQuestion, DesignQuestionService>();
                SimpleIoc.Default.Register<ISearch, DesignSearchService>();
                SimpleIoc.Default.Register<ITopic, DesignTopicService>();

                SimpleIoc.Default.Register<ISocial, DesignSocialService>();
                SimpleIoc.Default.Register<ISetting, DesignSettingService>();

                SimpleIoc.Default.Register<INotify, DesignNotifyService>();
                SimpleIoc.Default.Register<IMessage, DesignMessageService>();

                SimpleIoc.Default.Register<ITable, DesignTableService>();
            }
            else
            {
                SimpleIoc.Default.Register<IAnswer, AnswerService>();
                SimpleIoc.Default.Register<IArticle, ArticleService>();

                SimpleIoc.Default.Register<IColumn, ColumnService>();
                SimpleIoc.Default.Register<ICollection, CollectionService>();
                SimpleIoc.Default.Register<IComment, CommentService>();

                SimpleIoc.Default.Register<IFind, FindService>();
                SimpleIoc.Default.Register<ILogin, LoginService>();
                SimpleIoc.Default.Register<IFeed, FeedService>();

                SimpleIoc.Default.Register<IQuestion, QuestionService>();
                SimpleIoc.Default.Register<IPerson, PersonService>();
                SimpleIoc.Default.Register<ISearch, SearchService>();
                SimpleIoc.Default.Register<ITopic, TopicService>();

                SimpleIoc.Default.Register<ISocial, SocialService>();
                SimpleIoc.Default.Register<ISetting, SettingService>();

                SimpleIoc.Default.Register<INotify, NotifyService>();
                SimpleIoc.Default.Register<IMessage, MessageService>();

                SimpleIoc.Default.Register<ITable, TableService>();
            }

            if (false == SimpleIoc.Default.IsRegistered<MainViewModel>())
            {
                SimpleIoc.Default.Register<MainViewModel>(() => new MainViewModel(SimpleIoc.Default.GetInstance<IPerson>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<FeedsViewModel>())
            {
                SimpleIoc.Default.Register<FeedsViewModel>(
                    () =>
                        new FeedsViewModel(SimpleIoc.Default.GetInstance<IFeed>(),
                            SimpleIoc.Default.GetInstance<IPerson>(),
                            SimpleIoc.Default.GetInstance<INavigate>()));
            }

            //if (false == SimpleIoc.Default.IsRegistered<ProfileViewModel>())
            //{
            //    SimpleIoc.Default.Register<ProfileViewModel>(
            //        () => new ProfileViewModel(SimpleIoc.Default.GetInstance<IPerson>()));
            //}

            if (false == SimpleIoc.Default.IsRegistered<LoginViewModel>())
            {
                SimpleIoc.Default.Register<LoginViewModel>(
                    () => new LoginViewModel(SimpleIoc.Default.GetInstance<ILogin>()));
            }

            //if (false == SimpleIoc.Default.IsRegistered<AnswerViewModel>())
            //{
            //    SimpleIoc.Default.Register<AnswerViewModel>(
            //        () =>
            //            new AnswerViewModel(SimpleIoc.Default.GetInstance<IAnswer>(),
            //                SimpleIoc.Default.GetInstance<IComment>(),
            //                SimpleIoc.Default.GetInstance<ICollection>(),
            //                SimpleIoc.Default.GetInstance<ISocial>()));
            //}

            //if (false == SimpleIoc.Default.IsRegistered<ArticleViewModel>())
            //{
            //    SimpleIoc.Default.Register<ArticleViewModel>(
            //        () =>
            //            new ArticleViewModel(SimpleIoc.Default.GetInstance<IArticle>(),
            //                SimpleIoc.Default.GetInstance<IComment>()));
            //}

            //if (false == SimpleIoc.Default.IsRegistered<QuestionViewModel>())
            //{
            //    SimpleIoc.Default.Register<QuestionViewModel>(
            //        () =>
            //            new QuestionViewModel(SimpleIoc.Default.GetInstance<IQuestion>(),
            //                SimpleIoc.Default.GetInstance<IComment>(),
            //                SimpleIoc.Default.GetInstance<ISocial>()));
            //}

            if (false == SimpleIoc.Default.IsRegistered<AmazingGuyViewModel>())
            {
                SimpleIoc.Default.Register<AmazingGuyViewModel>(
                    () =>
                        new AmazingGuyViewModel(SimpleIoc.Default.GetInstance<IPerson>(),
                            SimpleIoc.Default.GetInstance<INavigate>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<FindViewModel>())
            {
                SimpleIoc.Default.Register<FindViewModel>(
                    () =>
                        new FindViewModel(SimpleIoc.Default.GetInstance<IFind>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<CollectionViewModel>())
            {
                SimpleIoc.Default.Register<CollectionViewModel>(
                    () =>
                        new CollectionViewModel(SimpleIoc.Default.GetInstance<ICollection>(),
                            SimpleIoc.Default.GetInstance<INavigate>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<TopicViewModel>())
            {
                SimpleIoc.Default.Register<TopicViewModel>(
                    () =>
                        new TopicViewModel(SimpleIoc.Default.GetInstance<ITopic>(),
                            SimpleIoc.Default.GetInstance<INavigate>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<ColumnViewModel>())
            {
                SimpleIoc.Default.Register<ColumnViewModel>(
                    () =>
                        new ColumnViewModel(SimpleIoc.Default.GetInstance<IColumn>(),
                            SimpleIoc.Default.GetInstance<INavigate>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<SearchViewModel>())
            {
                SimpleIoc.Default.Register<SearchViewModel>(
                    () =>
                        new SearchViewModel(SimpleIoc.Default.GetInstance<ISearch>(),
                            SimpleIoc.Default.GetInstance<IQuestion>(),
                            SimpleIoc.Default.GetInstance<INavigate>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<AskQuestionViewModel>())
            {
                SimpleIoc.Default.Register<AskQuestionViewModel>(
                    () =>
                        new AskQuestionViewModel(SimpleIoc.Default.GetInstance<ISearch>(),
                            SimpleIoc.Default.GetInstance<IQuestion>(),
                            SimpleIoc.Default.GetInstance<INavigate>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<AboutViewModel>())
            {
                SimpleIoc.Default.Register<AboutViewModel>(() => 
                new AboutViewModel(SimpleIoc.Default.GetInstance<ISetting>()));
            }

            if(false == SimpleIoc.Default.IsRegistered<WebViewModel>())
            {
                SimpleIoc.Default.Register<WebViewModel>(() => new WebViewModel());
            }

            if (false == SimpleIoc.Default.IsRegistered<MessageViewModel>())
            {
                SimpleIoc.Default.Register<MessageViewModel>(
                    () =>
                        new MessageViewModel(SimpleIoc.Default.GetInstance<IPerson>(),
                            SimpleIoc.Default.GetInstance<IMessage>(),
                            SimpleIoc.Default.GetInstance<INavigate>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<OfflineViewModel>())
            {
                SimpleIoc.Default.Register<OfflineViewModel>(
                    () =>
                        new OfflineViewModel(SimpleIoc.Default.GetInstance<IAnswer>(),
                            SimpleIoc.Default.GetInstance<IArticle>(),
                            SimpleIoc.Default.GetInstance<IColumn>(),
                            SimpleIoc.Default.GetInstance<IFeed>(),
                            SimpleIoc.Default.GetInstance<IQuestion>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<NotifyViewModel>())
            {
                SimpleIoc.Default.Register<NotifyViewModel>(
                    () =>
                        new NotifyViewModel(SimpleIoc.Default.GetInstance<INotify>(),
                            SimpleIoc.Default.GetInstance<IMessage>(),
                            SimpleIoc.Default.GetInstance<INavigate>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<SettingViewModel>())
            {
                SimpleIoc.Default.Register<SettingViewModel>(
                    () => new SettingViewModel(SimpleIoc.Default.GetInstance<ISetting>()));
            }

            if (false == SimpleIoc.Default.IsRegistered<TableViewModel>())
            {
                SimpleIoc.Default.Register<TableViewModel>(
                    () => new TableViewModel(SimpleIoc.Default.GetInstance<ITable>()));
            }

            //client_id=8d5227e0aaaa4797a763ac64e0c3b8&grant_type=password&password=wGDzw8gq&scope&signature=743e009e6824b366ddd49cca3bd82d2a4a6b1007&source=com.zhihu.android&timestamp=1464006542&username=%2B8615210046829&uuid
            //client_id=8d5227e0aaaa4797a763ac64e0c3b8&grant_type=password&password=wGDzw8gq&scope&signature=0f126af361e29adee751e547f37650ea9c773099&source=com.zhihu.android&timestamp=1464006697&username=%2B8615210046829&uuid
            //var hmasha = new HMACSHA1(Encoding.UTF8.GetBytes("xiaoq931018"));

            //var hasValue = hmasha.ComputeHash(Encoding.UTF8.GetBytes("xiaoq416%40163.com5774b305d2ae4469a2c9258956ea49com.zhihu.ios1423568498"));

            //var builder = new StringBuilder();
            //foreach (var b in hasValue)
            //{
            //    builder.Append(String.Format("{0:X2}", b));
            //}

            //var hashText = builder.ToString();
        }
    }
}
