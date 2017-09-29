using Jinyinmao.Application.Constants;
using System.Collections.Generic;

namespace Jinyinmao.Application.ViewModel.MessageManager
{
    public class MessageModel
    {
        public string Args { get; set; }
        public string Cellphone { get; set; }

        public SmsChannel Channel { get; set; }
        public List<int> Gateway { get; set; }

        public string Message { get; set; }

        public string Signature { get; set; }
    }
}