using System;
using System.Collections.Generic;
using System.Text;

namespace Kōkako
{
    class Choice
    {
        private string _text;
        private int _nextBranch;

        public string Text
        {
            get { return _text; }
            private set { _text = value; }
        }

        public int NextBranch
        {
            get { return _nextBranch; }
            private set { _nextBranch = value; }
        }

        public Choice(string text, int nextBranch)
        {
            _text = text;
            _nextBranch = nextBranch;
        }
    }
}
