using System;

using Windows.ApplicationModel;
using Windows.UI.Xaml;


namespace Zhihu.Helper
{
    internal sealed class RumtimeResourceDictionary : ResourceDictionary
    {
        private string _runtimeSource;

        public String RuntimeSource
        {
            get { return this._runtimeSource; }
            set
            {
                this._runtimeSource = value;

                if (DesignMode.DesignModeEnabled == false)
                {
                    base.Source = new Uri(_runtimeSource);
                }
            }
        }
    }
}
