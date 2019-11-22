using System;
using System.Collections.Generic;
using System.Text;

namespace Kōkako
{
    class Branch
    {
        private int _currentMessage;
        private Message[] _messages;
        private Choice[] _choices;

        public Message CurrentMessage
        {
            get { return _messages[_currentMessage]; }
        }

        public Message[] Messages
        {
            get { return _messages; }
        }

        public Branch(Message[] messages, Choice[] choices)
        {
            _currentMessage = 0;
            _messages = messages;
            _choices = choices;
        }

        public void NextMessage(out bool lastEntry)
        {
            if (_currentMessage < _messages.Length) { _currentMessage++; }

            if (_currentMessage >= _messages.Length) { lastEntry = true; }
            else { lastEntry = false; }
        }

        public Message.Sender NextSender()
        {
            return _messages[_currentMessage]._Sender;
        }

        public int NextMessageLength()
        {
            if (_currentMessage > 0 && _currentMessage < _messages.Length)
            { return _messages[_currentMessage].Text.Length; }
            else { return 0; }
        }

        public int PreviousMessageLength()
        {
            if (_currentMessage - 1 >= 0 && _currentMessage - 1 < _messages.Length)
            { return _messages[_currentMessage - 1].Text.Length; }
            else { return 0; }
        }

        public Choice[] GetChoices()
        {
            return _choices;
        }
    }
}
