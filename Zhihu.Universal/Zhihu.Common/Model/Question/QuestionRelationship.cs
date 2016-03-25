using System;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;


// ReSharper disable once CheckNamespace

namespace Zhihu.Common.Model
{
    public sealed class MyAnswer
    {
        [JsonProperty("answer_id")]
        public Int32 AnswerId { get; set; }

        [JsonProperty("is_deleted")]
        public Boolean IsDeleted { get; set; }
    }

    public sealed class QuesRelationShip : ObservableObject
    {
        [JsonProperty("answer_id")]
        public Int32 AnswerId { get; set; }

        private Boolean _isAuthor;

        [JsonProperty("is_author")]
        public Boolean IsAuthor
        {
            get { return _isAuthor; }
            set
            {
                _isAuthor = value;
                RaisePropertyChanged(() => IsAuthor);
            }
        }

        private Boolean _isFollowing = false;

        [JsonProperty("is_following")]
        public Boolean IsFollowing
        {
            get { return _isFollowing; }
            set
            {
                _isFollowing = value;
                RaisePropertyChanged(() => IsFollowing);
            }
        }

        private Boolean _isAnonymous;

        [JsonProperty("is_anonymous")]
        public Boolean IsAnonymous
        {
            get { return _isAnonymous; }
            set
            {
                _isAnonymous = value;
                RaisePropertyChanged(() => IsAnonymous);
            }
        }

        private Boolean _isDeleted;

        [JsonProperty("is_deleted")]
        public Boolean IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                _isDeleted = value;
                RaisePropertyChanged(() => IsDeleted);
            }
        }

        [JsonProperty("my_answer")]
        public MyAnswer MyAnswer { get; set; }
    }
}