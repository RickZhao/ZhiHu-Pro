using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using Zhihu.Common.Model;
using Zhihu.Common.DataSource;
using Zhihu.Common.Helper;
using Zhihu.Common.Model.Result;
using Zhihu.Common.Service;
using Zhihu.Controls;

namespace Zhihu.ViewModel
{
    public sealed class MessageViewModel : ViewModelBase
    {
        private readonly IPerson _person;
        private readonly IMessage _message;
        private readonly INavigate _navigate;

        [Data] private String _receiverId;

        #region Receiver

        private Profile _receiver;

        [Data]
        public Profile Receiver
        {
            get { return _receiver; }
            private set
            {
                _receiver = value;
                RaisePropertyChanged(() => Receiver);
            }
        }

        #endregion

        private const String Offset = "limit=20&sender_id={0}&after_id=";

        private Boolean _messagesLoading = false;

        public Boolean MessagesLoading
        {
            get { return _messagesLoading; }
            private set
            {
                _messagesLoading = value;
                RaisePropertyChanged(() => MessagesLoading);
            }
        }

        private IncrementalLoading<Message> _messages;

        [Data]
        public IncrementalLoading<Message> Messages
        {
            get { return _messages; }
            private set
            {
                _messages = value;

                RaisePropertyChanged(() => Messages);
            }
        }

        private String _messageToSend = String.Empty;

        [Data]
        public String MessageToSend
        {
            get { return _messageToSend; }
            set
            {
                _messageToSend = value;
                RaisePropertyChanged(() => MessageToSend);
            }
        }
        
        public RelayCommand GetReceiver { get; private set; }
        public RelayCommand SendMessage { get; private set; }

        private MessageViewModel()
        {
        }

        public MessageViewModel(IPerson person, IMessage message, INavigate navigate) : this()
        {
            this._person = person;
            this._message = message;
            this._navigate = navigate;
            
            GetReceiver = new RelayCommand(GetReceiverMethod);
            SendMessage = new RelayCommand(SendMessageMethod);
        }

        public void Setup(String receiverId)
        {
            //if (false == String.IsNullOrEmpty(_receiverId))
            //{
            //    VmHelper.SaveStates(this, this._receiverId);
            //}

            this._receiverId = receiverId;
       
            //VmHelper.ResumeStates(this, this._receiverId);

            this.Messages = new IncrementalLoading<Message>(GetMessages,
                String.Format("messages?sender_id={0}", _receiverId),
                String.Format(Offset, _receiverId),
                false);
        }
        

        private async Task<ListResultBase> GetMessages(String request)
        {
            if (MessagesLoading) return null;

            MessagesLoading = true;

            if (String.IsNullOrEmpty(LoginUser.Current.Token))
            {
                return null;
            }

            var result = await _message.GetMessages(LoginUser.Current.Token, request, true);

            MessagesLoading = false;

            if (result == null) return null;

            if (result.Result != null)
            {
                return result;
            }

            ToasteIndicator.Instance.Show(String.Empty, result.Error.Message, null, 3);

            return null;
        }

        private async void GetReceiverMethod()
        {
            if (null == _person || String.IsNullOrEmpty(LoginUser.Current.Token)) return;

            if (_receiverId == "-10005")
            {
                Receiver = new Profile()
                {
                    Name = "知乎管理员"
                };

                return;
            }

            MessagesLoading = true;

            var result = await _person.GetProfileAsync(LoginUser.Current.Token, _receiverId, true);

            MessagesLoading = false;

            if (result == null) return;

            if (null != result.Error)
            {
                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            if (result.Result == null) return;

            Receiver = result.Result;
        }

        private async void SendMessageMethod()
        {
            #region Checking Network

            if (false == Utility.Instance.IsNetworkAvailable)
            {
                ToasteIndicator.Instance.Show(String.Empty, "网络连接已中断", null, 3);

                return;
            }

            #endregion

            if (null == _message || String.IsNullOrEmpty(LoginUser.Current.Token) || String.IsNullOrEmpty(MessageToSend))
                return;

            MessagesLoading = true;

            var result = await _message.SendMessage(LoginUser.Current.Token, _receiverId, MessageToSend);

            MessagesLoading = false;

            if (result == null) return;

            if (null != result.Error)
            {
                Debug.WriteLine(Regex.Unescape(result.Error.Message));
                return;
            }

            if (result.Result == null) return;

            MessageToSend = String.Empty;

            this.Messages = new IncrementalLoading<Message>(GetMessages,
                String.Format("messages?sender_id={0}", _receiverId),
                String.Format(Offset, _receiverId),
                false);
        }
    }
}