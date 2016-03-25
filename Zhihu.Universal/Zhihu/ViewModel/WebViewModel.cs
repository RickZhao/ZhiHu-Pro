using System;

using GalaSoft.MvvmLight;

using Zhihu.Common.Helper;



namespace Zhihu.ViewModel
{
    public sealed class WebViewModel : ViewModelBase
    {
        private String _webUri = String.Empty;

        [Data]
        public String WebUri
        {
            get { return _webUri; }
            private set
            {
                _webUri = value;
                RaisePropertyChanged(() => WebUri);
            }
        }

        public WebViewModel()
        {

        }

        public void Setup(String webUri)
        {
            this.WebUri = webUri;
        }
    }
}
