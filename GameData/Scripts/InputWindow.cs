using System;
using System.Collections.Generic;
using Kōkako;

namespace Kōkako
{
    class InputWindow : GameObject
    {
        protected List<KeyboardEvent> _keyEvents;
        protected List<GameObject> _gameObjects;
        private int _textMargin = 150;

        public void Init()
        {
            List<KeyboardEvent> _keyEvents = new List<KeyboardEvent>();
            InputManager.KeyDown += OnKeyDown;
            _gameObjects = new List<GameObject>();
        }

        public void Update() { }


        //Väntar tills spelaren vallt sitt alternativ och returnerar vilke branch det valet leder till.
        //KeyboardEvent enumet representerar tangentbordets knappar. siffrer-knappars plats i enumet 
        //korresponderar med siffrans värde och kan därför castas till respetive värde. Essentially
        //Om Listan av KeyboardEvents innehåller 1 returnera nästa branch av det första valet
        public int AwaitChoice(Choice[] choices)
        {
            bool choiceMade = false;
            int choice = 0;

            while (!choiceMade)
            {
                InputManager.InvokeInputEvents(this);
                for (int i = 0; i < choices.Length; i++)
                {
                    if (_keyEvents.Contains((KeyboardEvent)i + 1))
                    {
                        choice = i; 
                        choiceMade = true; 
                    }
                }
            }

            return choices[choice].NextBranch;
        }

        //Lägger till alla alternativ som GameObjects i Inputfönstret, renderar dem och tar bort dem 
        //(de behövs bara renderas engång, efter som spelaren gör sitt val innan nästa rendering)
        public void DisplayChoises(Choice[] choices)
        {
            for (int i = 0; i < choices.Length; i++)
            { 
                GameObject gameObject = new GameObject("text");

                Text textComponent = gameObject.GetComponent<Text>();

                textComponent.Wrapper = (uint)(GetComponent<Sprite>()._transform.w - (_textMargin * 2));
                textComponent.Message = (i + 1) + ". " + choices[i].Text;

                gameObject.SetParent(this);
                gameObject.SetParentScene(ParentScene);

                gameObject._transform.y = _textMargin + (i * textComponent._transform.h);
                gameObject._transform.x = _textMargin;

                _gameObjects.Add(gameObject);
                ParentScene.AddGameObject(gameObject);
            }

            RenderEngine.Render();

            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Dispose();
                SceneManager.RemoveGameObject(gameObject);
            }
        }

        public void TypeMessage(Message message)
        {
            //Skapar ett GameObject av medelandet
            /*-------------------------------------------------------------------*/

            GameObject gameObject = new GameObject("text");

            Text textComponent = gameObject.GetComponent<Text>();

            textComponent.Wrapper = (uint)(GetComponent<Sprite>()._transform.w - (_textMargin * 2));

            gameObject.SetParent(this);
            gameObject.SetParentScene(ParentScene);

            gameObject._transform.y = _textMargin;
            gameObject._transform.x = _textMargin;

            //Lägger till GameObjectet i Scenen
            /*-------------------------------------------------------------------*/

            _gameObjects.Add(gameObject);
            ParentScene.AddGameObject(gameObject);

            //Lägger till en karaktär i medelandet, väntar och renderar
            /*-------------------------------------------------------------------*/

            for (int i = 0; i < message.Text.Length; i++)
            {
                textComponent.Message += message.Text[i];
                RenderEngine.Render();
                InputManager.InvokeInputEvents(null);
                Kōkako.Wait(Kōkako.random.Next(1, 7));
            }

            /*-------------------------------------------------------------------*/
        }

        //Tar bort medelandet ur input fönstret
        public void ReomveTypedMessage()
        {
            foreach (GameObject gameObject in _gameObjects) 
            {
                gameObject.Dispose();
            }
            _gameObjects.Clear();
        }

        public void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            _keyEvents = e.EventTypes;
        }
    }
}
