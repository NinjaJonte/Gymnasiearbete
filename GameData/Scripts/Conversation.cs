using System;
using System.Collections.Generic;
using System.Threading;

namespace Kōkako
{
    class Conversation
    {
        private int _currentBranch;
        private List<Branch> _branches;
        private List<KeyboardEvent> _keyEvents;

        public Conversation(List<Branch> branches)
        {
            InputManager.KeyDown += OnKeyDown;

            _keyEvents = new List<KeyboardEvent>();
            _branches = branches;
        }

        public void Update(out bool endOfLine)
        {
            //If the next branch is empty, the conversation is over
            if (_branches[_currentBranch] == null)
            { endOfLine = true; return; }

            endOfLine = false;

            //Depending on whos writin the next message, call a a diffrent "wait" method
            if (_branches[_currentBranch].NextSender() == Message.Sender.User)
            { TypeMessage(); }
            else
            { WaitForResponse(); }

            AddMessageToChatWindow(out bool awaitChoice);
            if (awaitChoice)
            {
                //If there were no choice to be made, endOfLine == true and the branch is over
                AddChoicesToInputWindow(out endOfLine);
                if (endOfLine) { return; }

                AwaitChoiceFromInputWindow();
            }
        }

        private void WaitForResponse()
        {
            //Simulates the character reading your message
            Kōkako.Wait((int)(Math.Sqrt(_branches[_currentBranch].PreviousMessageLength()) * 20));

            //Adds the is typing... text and renders it
            ScriptManager.GetScript<ChatWindow>().AddIsTyping(_branches[_currentBranch].CurrentMessage._Sender);
            RenderEngine.Render();

            //Simulates the character writing their message
            Kōkako.Wait((int)(Math.Sqrt(_branches[_currentBranch].NextMessageLength()) * 20));

            //If there was no previous message, wait anyway
            if (_branches[_currentBranch].PreviousMessageLength() == 0) { Kōkako.Wait(300); }

            //Remove the is typing... text
            ScriptManager.GetScript<ChatWindow>().RemoveIsTyping();
        }

        private void TypeMessage()
        {
            //Gives the player time to read the message
            Kōkako.Wait((int)(Math.Sqrt(_branches[_currentBranch].PreviousMessageLength()) * 20));

            //Types out the Message character by character
            ScriptManager.GetScript<InputWindow>().TypeMessage(_branches[_currentBranch].CurrentMessage);

            //Waits for the player to press enter before contiuning and sending the typed message
            while (!_keyEvents.Contains(KeyboardEvent.ENTER))
            {
                InputManager.InvokeInputEvents(null);
                RenderEngine.Render();
            }

            //Destroys the typed message before it's added to the chatwindow
            ScriptManager.GetScript<InputWindow>().ReomveTypedMessage();
        }

        private void AddMessageToChatWindow(out bool awaitChoice)
        {
            //Add the next message to the chatwindow
            ScriptManager.GetScript<ChatWindow>().DisplayMessage(_branches[_currentBranch].CurrentMessage);

            //Procedec to the next massage
            _branches[_currentBranch].NextMessage(out awaitChoice);
        }

        private void AddChoicesToInputWindow(out bool endOfLine)
        {
            //If the next choice doesn't exist, the branch is over
            if (_branches[_currentBranch].GetChoices() == null)
            { endOfLine = true; }
            else
            { endOfLine = false; ScriptManager.GetScript<InputWindow>().DisplayChoises(_branches[_currentBranch].GetChoices()); }
        }

        private void AwaitChoiceFromInputWindow()
        {
            //Get the index of the next branch depending on which slternative the player chooses
            _currentBranch = ScriptManager.GetScript<InputWindow>().AwaitChoice(_branches[_currentBranch].GetChoices());
        }

        public void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            _keyEvents = e.EventTypes;
        }
    }
}