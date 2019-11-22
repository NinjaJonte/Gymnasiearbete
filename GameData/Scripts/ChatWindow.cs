using System.Collections.Generic;

namespace Kōkako
{
    class ChatWindow : GameObject
    {
        protected List<GameObject> _textMessages;
        private GameObject _isTyping;
        private int _textMarginY = 200;
        private int _textMarginX = 150;

        public void Init()
        {
            _textMessages = new List<GameObject>();

            //Genererar sender is Typing GameObjectet (ser alltid lika dant ut)
            /*-------------------------------------------------------------------*/
          
            _isTyping = new GameObject("text");
            _isTyping._transform.y = GetComponent<Sprite>()._transform.h;
            _isTyping._transform.x =  _textMarginX;
            _isTyping.SetParent(this);
            _isTyping.SetParentScene(ParentScene);
            ParentScene.AddGameObject(_isTyping);

            /*-------------------------------------------------------------------*/
        }

        public void Update() { }

        //lägger till sender is typing... text
        public void AddIsTyping(Message.Sender sender)
        {
            Text textComponent = _isTyping.GetComponent<Text>();

            textComponent.Message = sender.ToString() + " is typing...";
            textComponent.Wrapper = (uint)((GetComponent<Sprite>()._transform.w * 2 / 3) - (_textMarginX * 2));

            textComponent.SetTransperancy(125);
        }

        //Tar bort sender is typing... text
        public void RemoveIsTyping()
        {
            Text textComponent = _isTyping.GetComponent<Text>();
            textComponent.SetTransperancy(0);
        }


        public void DisplayMessage(Message message)
        {
            //Skapar ett GameObject av medelandet
            /*-------------------------------------------------------------------*/

            GameObject textGameObject = new GameObject("text");

            Text textComponent = textGameObject.GetComponent<Text>();

            textComponent.Wrapper = (uint)((GetComponent<Sprite>()._transform.w * 2 / 3) - (_textMarginY * 2));
            textComponent.Message = message.Text;

            textGameObject.SetParent(this);
            textGameObject.SetParentScene(ParentScene);

            textGameObject._transform.y = GetComponent<Sprite>()._transform.h - textComponent._transform.h - (_textMarginY / 2);
            if (message._Sender != Message.Sender.User) { textGameObject._transform.x = _textMarginX; }
            else { textGameObject._transform.x = GetComponent<Sprite>()._transform.w - textComponent._transform.w - _textMarginX; }


            //Skapar ett GameObject av textrutan
            /*-------------------------------------------------------------------*/

            GameObject textboxGameObject = new GameObject("textbox");
            Sprite TextboxSprite = textboxGameObject.GetComponent<Sprite>();

            textboxGameObject.SetParent(textGameObject);
            textboxGameObject.SetParentScene(ParentScene);

            //Får sin position av Parent, ändrar marginal
            textboxGameObject._transform.x = -50;
            textboxGameObject._transform.y = -40;
            TextboxSprite._transform.w = textComponent.TextureSize.w + 100;
            TextboxSprite._transform.h = textComponent.TextureSize.h + 80;

            //Skapar ett GameObject av datorns tid
            /*-------------------------------------------------------------------*/

            GameObject timeGameObject = new GameObject("text");
            Text timeText = timeGameObject.GetComponent<Text>();
            timeText.FontSize = 45;
            timeText.Message = GetTime();
            timeText.SetTransperancy(125);

            timeGameObject.SetParent(textGameObject);
            timeGameObject.SetParentScene(ParentScene);

            if (message._Sender == Message.Sender.User) { timeGameObject._transform.x = textComponent.TextureSize.w - timeText.TextureSize.w; }
            else { timeGameObject._transform.x = 0; }
            timeGameObject._transform.y = -50 - (timeText.TextureSize.h);

            //Bara medelandet behöver ligga i _textMessages eftersom att Tiden och textrutan flyttas automatiskt pga de har medelandet som parent
            //Lägger till alla GameObjects i Scenen
            /*-------------------------------------------------------------------*/

            ScrollMessages(textComponent._transform.h);
            _textMessages.Add(textGameObject);
            ParentScene.AddGameObject(textboxGameObject);
            ParentScene.AddGameObject(textGameObject);
            ParentScene.AddGameObject(timeGameObject);
        }

        //Flytta alla medelanden i _textMessages så att det nya får plats
        private void ScrollMessages(float amount)
        {
            foreach (GameObject gameObject in _textMessages)
            {
                gameObject._transform.y -= amount + _textMarginY;
            }
        }

        private string GetTime()
        {
            string t = "";
             
            if(System.DateTime.Now.TimeOfDay.Hours < 10)
            { t += "0" + System.DateTime.Now.TimeOfDay.Hours; }
            else { t += System.DateTime.Now.TimeOfDay.Hours; }
            t += ":";
            if (System.DateTime.Now.TimeOfDay.Minutes < 10)
            { t += "0" + System.DateTime.Now.TimeOfDay.Minutes; }
            else { t += System.DateTime.Now.TimeOfDay.Minutes; }

            return t;
        }
    }
}