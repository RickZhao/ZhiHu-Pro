using System;
using System.ComponentModel;

using Newtonsoft.Json;


namespace Zhihu.Common.Model
{
    public sealed class Following : INotifyPropertyChanged
    {
        private Boolean _isFollowing;

        [JsonProperty("is_following")]
        public Boolean IsFollowing
        {
            get { return _isFollowing; }
            set
            {
                _isFollowing = value;

                if (null != PropertyChanged)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsFollowing"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
