using System;
using System.Collections.Generic;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using Zhihu.Common.Service;
using Zhihu.ViewModel;


namespace Zhihu.Helper
{
    public sealed class ViewModelHelper
    {
        public Dictionary<String, ViewModelBase> _viewModels = new Dictionary<string, ViewModelBase>();

        #region Singleton

        private static ViewModelHelper _instance;
        private ViewModelHelper() { }

        public static ViewModelHelper Instance
        {
            get { return _instance ?? (_instance = new ViewModelHelper()); }
        }

        #endregion
    
        #region Private Methods

        private Boolean Contains(String key)
        {
            return _viewModels.ContainsKey(key);
        }

        private ViewModelBase GetViewModel(String key)
        {
            if (Contains(key)) return _viewModels[key];

            else return null;
        }

        private void SaveViewModel(String vmKey, ViewModelBase viewModel)
        {
            if (Contains(vmKey)) _viewModels[vmKey] = viewModel;
            else _viewModels.Add(vmKey, viewModel);
        }

        private void RemoveViewModel(String vmKey)
        {
            if (Contains(vmKey)) _viewModels.Remove(vmKey);
        }

        private String GetKey(Type viewModel, Int32 id)
        {
            return GetKey(viewModel, id.ToString());
        }

        private String GetKey(Type viewModel, String id)
        {
            return String.Format("{0}_{1}", viewModel.Name, id);
        }

        #endregion

        public ProfileViewModel GetProfile(String userId)
        {
            var vmKey = GetKey(typeof(ProfileViewModel), userId);

            if (Contains(vmKey))
            {
                return GetViewModel(vmKey) as ProfileViewModel;
            }

            var person = SimpleIoc.Default.GetInstance<IPerson>();

            var vm = new ProfileViewModel(person);
            vm.Setup(userId);

            SaveViewModel(vmKey, vm);

            return vm;
        }

        public void RemoveProfile(String userId)
        {
            var vmKey = GetKey(typeof(ProfileViewModel), userId);
            RemoveViewModel(vmKey);
        }

        public QuestionViewModel GetQuestion(Int32 questionId)
        {
            var vmKey = GetKey(typeof(QuestionViewModel), questionId);

            if (Contains(vmKey))
            {
                return GetViewModel(vmKey) as QuestionViewModel;
            }

            var question = SimpleIoc.Default.GetInstance<IQuestion>();
            var comment = SimpleIoc.Default.GetInstance<IComment>();
            var social = SimpleIoc.Default.GetInstance<ISocial>();

            var vm = new QuestionViewModel(question, comment, social);
            vm.Setup(questionId);

            SaveViewModel(vmKey, vm);

            return vm;
        }

        public void RemoveQuestion(Int32 questionId)
        {
            var vmKey = GetKey(typeof(QuestionViewModel), questionId);
            RemoveViewModel(vmKey);
        }

        public AnswerViewModel GetAnswer(Int32 answerId)
        {
            var vmKey = GetKey(typeof(AnswerViewModel), answerId);

            if (Contains(vmKey))
            {
                return GetViewModel(vmKey) as AnswerViewModel;
            }

            var answer = SimpleIoc.Default.GetInstance<IAnswer>();
            var comment = SimpleIoc.Default.GetInstance<IComment>();
            var collection = SimpleIoc.Default.GetInstance<ICollection>();
            var social = SimpleIoc.Default.GetInstance<ISocial>();

            var vm = new AnswerViewModel(answer, comment, collection, social);
            vm.Setup(answerId);

            SaveViewModel(vmKey, vm);

            return vm;
        }

        public void RemoveAnswer(Int32 answerId)
        {
            var vmKey = GetKey(typeof(AnswerViewModel), answerId);
            RemoveViewModel(vmKey);
        }

        public ArticleViewModel GetArticle(Int32 articleId)
        {
            var vmKey = GetKey(typeof(ArticleViewModel), articleId);

            if (Contains(vmKey))
            {
                return GetViewModel(vmKey) as ArticleViewModel;
            }

            var article = SimpleIoc.Default.GetInstance<IArticle>();
            var comment = SimpleIoc.Default.GetInstance<IComment>();
            var social = SimpleIoc.Default.GetInstance<ISocial>();

            var vm = new ArticleViewModel(article, comment, social);
            vm.Setup(articleId);

            SaveViewModel(vmKey, vm);

            return vm;
        }

        public void RemoveArticle(Int32 articleId)
        {
            var vmKey = GetKey(typeof(QuestionViewModel), articleId);
            RemoveViewModel(vmKey);
        }
    }
}
