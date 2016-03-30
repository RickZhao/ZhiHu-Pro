using System;

using Windows.UI.Xaml.Controls;


namespace Zhihu.View.Main
{
    internal sealed class MainStatus
    {
        public Boolean IsWide { get; private set; }
        public Frame NavFrame { get; private set; }

        private MainStatus()
        {

        }

        public MainStatus(Boolean isWide, Frame navFrame)
        {
            IsWide = isWide;
            NavFrame = navFrame;
        }
    }
}
