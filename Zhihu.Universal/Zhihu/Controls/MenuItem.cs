using System;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;


namespace Zhihu.Controls
{
    public sealed class MenuItem : ObservableObject
    {
        private Symbol icon;
        private string title;
        private Type pageType;


        public Symbol Icon
        {
            get { return this.icon; }
            set { Set(ref this.icon, value); }
        }

        public string Title
        {
            get { return this.title; }
            set { Set(ref this.title, value); }
        }

        public Type PageType
        {
            get { return this.pageType; }
            set { Set(ref this.pageType, value); }
        }
    }
}
