using System;
using System.Collections.Generic;
using System.Text;

namespace Kōkako
{
    class Message
    {
        public enum Sender { User, Thomas, Charlie };

        private string _text;
        private Sender _sender;

        public string Text
        {
            get { return _text; }
            private set { _text = value; }
        }

        public Sender _Sender
        {
            get { return _sender; }
            private set { _sender = value; }
        }

        public Message(string text, Sender sender)
        {
            _text = text;
            _sender = sender;
        }
    }
}
