using System;
using System.ComponentModel;

using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Zhihu.Common.Model
{
    public sealed class AnswerRelationship : INotifyPropertyChanged
    {
        private Int32 _voting = 0;

        [JsonProperty("voting")]
        public Int32 Voting
        {
            get { return _voting; }
            set
            {
                _voting = value;
                NotifyPropertyChanged("Voting");
            }
        }

        private Boolean _isThanked = false;

        [JsonProperty("is_thanked")]
        public Boolean IsThanked
        {
            get { return _isThanked; }
            set
            {
                _isThanked = value;
                NotifyPropertyChanged("IsThanked");
            }
        }

        private Boolean _isFavorited = false;

        [JsonProperty("is_favorited")]
        public Boolean IsFavorited
        {
            get { return _isFavorited; }
            set
            {
                _isFavorited = value;
                NotifyPropertyChanged("IsFavorited");
            }
        }

        private Boolean _isNohelp = false;

        [JsonProperty("is_nothelp")]
        public Boolean IsNotHelp
        {
            get { return _isNohelp; }
            set
            {
                _isNohelp = value;
                NotifyPropertyChanged("IsNotHelp");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                var handler = PropertyChanged;
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
