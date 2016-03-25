using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Zhihu.Common.Model;


namespace Zhihu.Common.Helper
{
    public sealed class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Sender { get; set; }
        public DataTemplate Receiver { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var msg = item as Message;

            if (msg == null) throw new NullReferenceException();

            return msg.Sender.Id == LoginUser.Current.Profile.Id ? Sender : Receiver;
        }
    }
}
