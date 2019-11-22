using System;
using System.Collections.Generic;
using System.Text;

namespace Kōkako
{
    class Game : GameObject
    {
        private Conversation _conversation;
        private string _currentScene;

        //Vårt spel undersöker påverkan av ordningen karaktärerna presenteras
        private bool _tomFirst = true;

        public void Init()
        {
            if (_tomFirst)
            {
                _currentScene = "T1";
                _conversation = new Conversation(GetPartT1());
            } 
            else
            {
                _currentScene = "C1";
                _conversation = new Conversation(GetPartC1());
            }

            //Lägg till scen
            SceneManager.AddScene(_currentScene);
            ScriptManager.InitScripts(_currentScene);
            Kōkako.AddEvents();
        }

        public void Update()
        {
            _conversation.Update(out bool endOfLine);

            if (endOfLine)
            {
                Transition();
            }
        }

        //Lägger till en Scen med en "stäng av" Animation i och animerar den
        private void AnimateShutDown()
        {

            SceneManager.AddScene("CRT");
            GameObject crt = FindAll("CRT")[0];

            while (crt.GetComponent<AnimationController>().CurrentAnimation.FristLoop.Value == true)
            {
                RenderEngine.Render();
                InputManager.InvokeInputEvents(null);
                crt.GetComponent<AnimationController>().Animate();
            }

            SceneManager.RemoveScene("CRT");
        }

        //Tar bort Den förra scenen och lägger till en ny
        private void Transition()
        {
            AnimateShutDown();

            SceneManager.RemoveScene(_currentScene);

            if (_currentScene == "T1")
            { 
                if (_tomFirst) { _currentScene = "C1"; } 
                else { _currentScene = "T2"; } 
            }
            if (_currentScene == "C1")
            { 
                if (_tomFirst) { _currentScene = "T2"; } 
                else { _currentScene = "T1"; }
            }
            if (_currentScene == "C2")
            { _currentScene = "T2"; }
            if (_currentScene == "T2")
            { _currentScene = "T3"; }
            if (_currentScene == "T3")
            { _currentScene = "C4"; }
            if (_currentScene == "C4")
            { 
                OpenWebPage(); 
                Kōkako.Exit(); 
            }

            if (_currentScene == "T1")
            { _conversation = new Conversation(GetPartT1()); }
            if (_currentScene == "C1")
            { _conversation = new Conversation(GetPartC1()); }
            if (_currentScene == "C2")
            { _conversation = new Conversation(GetPartC2()); }
            if (_currentScene == "T2")
            { _conversation = new Conversation(GetPartT2()); }
            if (_currentScene == "T3")
            { _conversation = new Conversation(GetPartT3()); }
            if (_currentScene == "C4")
            { _conversation = new Conversation(GetPartC4()); }

            //Lägg till svart Skärm
            SceneManager.AddScene("Black_screen");
            GameObject blackScreen = FindAll("Black_screen")[0];
            Kōkako.Wait(50);

            //Lägg till text
            GameObject date = new GameObject("Black_screen_" + _currentScene);
            SceneManager.GetScene("Black_screen").AddGameObject(date);

            RenderEngine.Render();
            Kōkako.Wait(300);

            //Lägg till scen
            SceneManager.AddScene(_currentScene);
            ScriptManager.InitScripts(_currentScene);
            Kōkako.AddEvents();

            //Ta bort text
            date.Dispose();
            RenderEngine.Render();
            Kōkako.Wait(100);

            //Ta bort svart skräm
            SceneManager.RemoveGameObject(blackScreen);
            SceneManager.RemoveScene("Black_screen");
        }

        public void OpenWebPage()
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.UseShellExecute = true;
            if (_tomFirst == true) { proc.StartInfo.FileName = "http://bit.ly/Gymnasiearbete_T1"; }
            else { proc.StartInfo.FileName = "http://bit.ly/Gymnasiearbete_C1"; }
            proc.Start();
        }

        private List<Branch> GetPartT1()
        {
            List<Branch> branches = new List<Branch>();
            Message.Sender sender = Message.Sender.Thomas;

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #0
                new Message[] {
                    new Message("You know that game you wanted me to play? Twilight Princess? Just finished it.", sender),
                    new Message("Finally!!! :D What did you think of it?", Message.Sender.User),
                    new Message("I didn't really like it that much, it was too bland.", sender),
                    new Message("What!?", Message.Sender.User),
                    new Message("Nah i’m just kidding. Loved it! :))))))", sender),
                    new Message("I was a bit sceptical about the dark tone of the game, It’s such a jump from Wind Waker, but it really played to it’s strengths.", sender) },
                new Choice[] {
                    new Choice("Wind waker was such a good game.", 1), //Branch #1
                    new Choice("Wind waker didn’t tickle my fancy.", 2) }) //Branch #2
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #1
                new Message[] {
                    new Message("Wind waker was such a good game.", Message.Sender.User),
                    new Message("Honestly I didn’t like it too much. I mostly played it for Charlie’s sake.", sender),
                    new Message("He’s really excited to play Ocarina of Time with me! I heard that game is supposed to be really good.", sender) },
                new Choice[] {
                    new Choice("Oh yeah! How’s Charlie doing?", 3), //Branch #3
                    new Choice("Is everything okay with Charlie?", 4) }) //Branch #4
                );
            branches.Add(new Branch( //Branch #2
                new Message[] {
                    new Message("Wind waker didn’t tickle my fancy.", Message.Sender.User),
                    new Message("Honestly I didn’t like it too much. I mostly played it for Charlie’s sake.", sender),
                    new Message("He’s really excited to play Ocarina of Time with me! I heard that game is supposed to be really good.", sender) },
                new Choice[] {
                    new Choice("Oh yeah! How’s Charlie doing?", 3), //Branch #3
                    new Choice("Is everything okay with Charlie?", 4) }) //Branch #4
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #3
               new Message[] {
                    new Message("Oh yeah! How’s Charlie doing?", Message.Sender.User),
                    new Message("He didn't get accepted into his college so he’s not really doing anything right now.", sender),
                    new Message("Speaking of which, he’s mostly at home replaying all of the zelda games.", sender) },
               new Choice[] {
                    new Choice("Sad to hear about college.", 5), //Branch #5
                    new Choice("Doesn't sound too bad.", 6), //Branch #6
                    new Choice("Wish I could play zelda games instead of school.", 7) }) //Branch #7
               );
            branches.Add(new Branch( //Branch #4
               new Message[] {
                    new Message("Is everything okay with Charlie?", Message.Sender.User),
                    new Message("He didn't get accepted into his college so he’s not really doing anything right now.", sender),
                    new Message("Speaking of which, he’s mostly at home replaying all of the zelda games.", sender) },
               new Choice[] {
                    new Choice("Sad to hear about college.", 5), //Jump to Branch #5
                    new Choice("Doesn't sound too bad.", 6), //Jump to Branch #6
                    new Choice("Wish I could play zelda games instead of school.", 7) }) //Jump to Branch #7
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #5
               new Message[] {
                    new Message("Sad to hear about college.", Message.Sender.User),
                    new Message("I almost forgot! You live together now, right? Sorry I didn't help out with the moving part...", Message.Sender.User) },
               new Choice[] {
                    new Choice("I was at the hospital with my mom celebrating her birthday.", 8), //Jump to Branch #8
                    new Choice("I had my finals that week.", 9), //Jump to Branch #9
                    new Choice("I was at my second cousin's funeral.", 10) }) //Jump to Branch #10
               );
            branches.Add(new Branch( //Branch #6
               new Message[] {
                    new Message("Doesn't sound too bad.", Message.Sender.User),
                    new Message("I almost forgot! You live together now, right? Sorry I didn't help out with the moving part...", Message.Sender.User) },
               new Choice[] {
                    new Choice("I was at the hospital with my mom celebrating her birthday.", 8), //Jump to Branch #8
                    new Choice("I had my finals that week.", 9), //Jump to Branch #9
                    new Choice("I was at my second cousin's funeral.", 10) }) //Jump to Branch #10
               );
            branches.Add(new Branch( //Branch #7
               new Message[] {
                    new Message("Wish I could play zelda games instead of school.", Message.Sender.User),
                    new Message("I almost forgot! You live together now, right? Sorry I didn't help out with the moving part...", Message.Sender.User) },
               new Choice[] {
                    new Choice("I was at the hospital with my mom celebrating her birthday.", 8), //Jump to Branch #8
                    new Choice("I had my finals that week.", 9), //Jump to Branch #9
                    new Choice("I was at my second cousin's funeral.", 10) }) //Jump to Branch #10
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #8
               new Message[] {
                    new Message("I was at the hospital with my mom celebrating her birthday.", Message.Sender.User),
                    new Message("I’ll never forgive you, you monster.", sender),
                    new Message("No, but seriously it's alright but my parents are not thrilled you know.", sender) ,
                    new Message("They weren't really expecting us to move this early in life...", sender),
                    new Message("Anyways, you should visit us sometime! The apartment is really nice actually. Kinda small but that's alright.", sender) },
               new Choice[] {
                    new Choice("Sounds like a great plan, how about next weekend?", 11), //Jump to Branch #11
                    new Choice("I’m a bit claustrophobic, you’ll have to get a bigger apartment if you want me to visit :)", 12) }) //Jump to Branch #12
               );
            branches.Add(new Branch( //Branch #9
                new Message[] {
                    new Message("I had my finals that week.", Message.Sender.User),
                    new Message("I’ll never forgive you, you monster.", sender),
                    new Message("No, but seriously it's alright but my parents are not thrilled you know.", sender) ,
                    new Message("They weren't really expecting us to move this early in life...", sender),
                    new Message("Anyways, you should visit us sometime! The apartment is really nice actually. Kinda small but that's alright.", sender) },
                new Choice[] {
                    new Choice("Sounds like a great plan, how about next weekend?", 11), //Jump to Branch #11
                    new Choice("I’m a bit claustrophobic, you’ll have to get a bigger apartment if you want me to visit :)", 12) }) //Jump to Branch #12
                );
            branches.Add(new Branch( //Branch #10
                new Message[] {
                    new Message("I was at the hospital with my mom celebrating her birthday.", Message.Sender.User),
                    new Message("I’ll never forgive you, you monster.", sender),
                    new Message("No, but seriously it's alright but my parents are not thrilled you know.", sender) ,
                    new Message("They weren't really expecting us to move this early in life...", sender),
                    new Message("Anyways, you should visit us sometime! The apartment is really nice actually. Kinda small but that's alright.", sender) },
                new Choice[] {
                    new Choice("Sounds like a great plan, how about next weekend?", 11), //Jump to Branch #11
                    new Choice("I’m a bit claustrophobic, you’ll have to get a bigger apartment if you want me to visit :)", 12) }) //Jump to Branch #12
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #11
               new Message[] {
                    new Message("Sounds like a great plan, how about next weekend?", Message.Sender.User),
                    new Message("Sounds good! It’s a bit of a mess though, if you’re unfortunate you might get eaten alive by the moving boxes. ", sender),
                    new Message("Haha, yeah. See you around!", Message.Sender.User),
                    new Message("See ya!", sender) },
               null)
               );
            branches.Add(new Branch( //Branch #12
               new Message[] {
                    new Message("I’m a bit claustrophobic, you’ll have to get a bigger apartment if you want me to visit :)", Message.Sender.User),
                    new Message("Haha, maybe you can come over once we’ve got everything packed up, i’m sure we’ll all fit :)", sender),
                    new Message("I’m just messing with ya. Bye!", Message.Sender.User),
                    new Message("See ya!", sender) },
               null)
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            return branches;
        }

        private List<Branch> GetPartC1()
        {
            List<Branch> branches = new List<Branch>();
            Message.Sender sender = Message.Sender.Charlie;

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #0
                new Message[] {
                    new Message("Hi!", sender),
                    new Message("Hello!", Message.Sender.User),
                    new Message("How was your mother’s birthday party?", sender) },
                new Choice[] {
                    new Choice("It was great!", 1), //Branch #1
                    new Choice("Eh, not that great, i wish I could’ve helped you with the moving instead.", 2) }) //Branch #2
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #1
                new Message[] {
                    new Message("It was great!", Message.Sender.User),
                    new Message("Neat, too bad you couldn’t see our new apartment though...", sender),
                    new Message("But look on the bright side, at least you missed Toms father's reaction when Tom told him he's moving out.", sender),
                    new Message("I wish I could unsee that...", sender),
                    new Message("Aaaaaanyway, did you hear about that new Zelda game they announced?", sender) },
                new Choice[] {
                    new Choice("No?", 3), //Branch #3
                    new Choice("Yeah! Skyward Sword was it?", 4) }) //Branch #4
                );
            branches.Add(new Branch( //Branch #2
                new Message[] {
                    new Message("Eh, not that great, I wish I could’ve helped you with the moving instead.", Message.Sender.User),
                    new Message("Aw, that’s so nice of you. My dad came to help us out so we managed just fine.", sender),
                    new Message("It’s sad that Tom’s parents aren’t as thrilled to have us move out...", sender),
                    new Message("Aaaaaanyway, did you hear about that new Zelda game they announced?", sender) },
                new Choice[] {
                    new Choice("No?", 3), //Branch #3
                    new Choice("Yeah! Skyward Sword was it?", 4) }) //Branch #4
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #3
               new Message[] {
                    new Message("No?", Message.Sender.User),
                    new Message("Apparently they’ve added real motion controls to the sword fighting.", sender),
                    new Message("You have to hit monsters form a certain angle, and you can roll bombs! Finally!!!", sender) },
               new Choice[] {
                    new Choice("That’s so awesome!", 5), //Branch #5
                    new Choice("I just wish I could play with a standard controller.", 6) }) //Branch #6
               );
            branches.Add(new Branch( //Branch #4
               new Message[] {
                    new Message("Yeah! Skyward Sword was it?", Message.Sender.User),
                    new Message("Yes! It looks absolutely stunning, right!", sender),
                    new Message("Finally a Zelda game with good motions controls!", sender),
                    new Message("I’m never going back to Twilight Princess ever again.", sender) },
               new Choice[] {
                    new Choice("That’s so awesome!", 5), //Branch #5
                    new Choice("I just wish I could play with a standard controller.", 6) }) //Branch #6
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #5
               new Message[] {
                    new Message("That’s so awesome!", Message.Sender.User),
                    new Message("So, now school's over.", Message.Sender.User),
                    new Message("What are you doing apart from obsessing over new game releases", Message.Sender.User),
                    new Message("Haha, I wouldn't call it obsessing, only watching every single move they make extremely closely", sender),
                    new Message("No but it gets kind of boring after a while you know.", sender),
                    new Message("Feels like nothing you do has a purpose when you have nowhere to go...", sender),
                    new Message("I never thought I would miss school but now I kind of feel like I need that every-day structure in my life...", sender),
                    new Message("You could come over to my place this weekend and have a movie night!", Message.Sender.User),
                    new Message("My dad just bought a new projector, he will probably be fine lending it to us.", Message.Sender.User),
                    new Message("For real!? That’d be very nice. Which movie did you have in mind.", sender) },
               new Choice[] {
                    new Choice("Scott Pilgrim vs the World?", 7), //Jump to Branch #7
                    new Choice("How to train your dragon", 8), //Jump to Branch #8
                    new Choice("Kick-Ass", 9), //Jump to Branch #9
                    new Choice("Alice in Wonderland", 10) }) //Jump to Branch #10
               );
            branches.Add(new Branch( //Branch #6
               new Message[] {
                    new Message("I just wish I could play with a standard controller.", Message.Sender.User),
                    new Message("So, now school's over.", Message.Sender.User),
                    new Message("What are you doing apart from obsessing over new game releases", Message.Sender.User),
                    new Message("Haha, I wouldn't call it obsessing, only watching every single move they make extremely closely", sender),
                    new Message("No but it gets kind of boring after a while you know.", sender),
                    new Message("Feels like nothing you do has a purpose when you have nowhere to go...", sender),
                    new Message("I never thought I would miss school but now I kind of feel like I need that every-day structure in my life...", sender),
                    new Message("You could come over to my place this weekend and have a movie night!", Message.Sender.User),
                    new Message("My dad just bought a new projector, he will probably be fine lending it to us.", Message.Sender.User),
                    new Message("For real!? That’d be very nice. Which movie did you have in mind.", sender) },
               new Choice[] {
                    new Choice("Scott Pilgrim vs the World?", 7), //Jump to Branch #7
                    new Choice("How to train your dragon", 8), //Jump to Branch #8
                    new Choice("Kick-Ass", 9), //Jump to Branch #9
                    new Choice("Alice in Wonderland", 10) }) //Jump to Branch #10
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #7
               new Message[] {
                    new Message("Has Scott Pilgrim vs the World come out on dvd yet?", Message.Sender.User),
                    new Message("No... But I don’t think there’s much time left before it releases.", sender) ,
                    new Message("Thank you for cheering me up, finally I have something to look forward to! :)", sender) },
               new Choice[] {
                    new Choice("Okay, see you friday then!", 11), //Jump to Branch #11
                    new Choice("Smell you later.", 12) }) //Jump to Branch #12
               );
            branches.Add(new Branch( //Branch #8
               new Message[] {
                    new Message("Honestly, I wanna see the new How to train your dragon movie.", Message.Sender.User),
                    new Message("And don't tell me its a children's movie, I will kill you.", Message.Sender.User),
                    new Message("It is a children's movie. Doesn’t mean I wont watch it. (Please don't kill me)", sender),
                    new Message("Thank you for cheering me up, finally I have something to look forward to! :)", sender) },
               new Choice[] {
                    new Choice("Okay, see you friday then!", 11), //Jump to Branch #11
                    new Choice("Smell you later.", 12) }) //Jump to Branch #12
               );
            branches.Add(new Branch( //Branch #9
               new Message[] {
                    new Message("What about Kick-Ass?", Message.Sender.User),
                    new Message("Aw yeah, I heard that movie was really K I C K - A S S.", sender),
                    new Message("Thank you for cheering me up, finally I have something to look forward to! :)", sender) },
               new Choice[] {
                    new Choice("Okay, see you friday then!", 11), //Jump to Branch #11
                    new Choice("Smell you later.", 12) }) //Jump to Branch #12
               );
            branches.Add(new Branch( //Branch #10
               new Message[] {
                    new Message("I read Alice in Wonderland a once, actually maybe just half the book really...", Message.Sender.User),
                    new Message("Have you seen the movie yet?", Message.Sender.User),
                    new Message("Nope, but I really like Tim burton so that would indeed be a great choice.", sender),
                    new Message("Thank you for cheering me up, finally I have something to look forward to! :)", sender) },
               new Choice[] {
                    new Choice("Okay, see you friday then!", 11), //Jump to Branch #11
                    new Choice("Smell you later.", 12) }) //Jump to Branch #12
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #11
               new Message[] {
                    new Message("Okay, see you friday then!", Message.Sender.User),
                    new Message("Bye!", sender) },
               null)
               );

            branches.Add(new Branch( //Branch #12
               new Message[] {
                    new Message("Smell you later.", Message.Sender.User),
                    new Message("Bye!", sender) },
               null)
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            return branches;
        }


        private List<Branch> GetPartC2()
        {
            List<Branch> branches = new List<Branch>();
            Message.Sender sender = Message.Sender.Charlie;

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #0
                new Message[] {
                    new Message("Dude...", sender),
                    new Message("Something awful just happened...", sender),
                    new Message("It all just happened so fast, and it was all my fault.", sender),
                    new Message("Hey calm down, what are you talking about!?", Message.Sender.User),
                    new Message("What's going on, are you OK?", Message.Sender.User),
                    new Message("Tom’s at the hospital and I have no idea if he's gonna make it.", sender),
                    new Message("It’s all my fault.", sender),
                    new Message("What if he doesn't make it?", sender),
                    new Message("What am I supposed to do?", sender) },
                new Choice[] {
                    new Choice("What happened? What did you do Charlie?", 1), //Branch #1
                    new Choice("Whatever you did I am sure you didn't mean to do it.", 2), //Branch #2
                    new Choice("What have you done to Tom?", 3) }) //Branch #3
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #1
                new Message[] {
                    new Message("What happened? What did you do Charlie?", Message.Sender.User),
                    new Message("We were driving home from Tom’s summer house", sender),
                    new Message("I wish I could unsee that...", sender),
                    new Message("I looked at my phone for just one second, I swear. I mean, who wouldn't!?", sender),
                    new Message("The screen flashed and I got distracted.", sender),
                    new Message("I didn't mean to...", sender),
                    new Message("And now he’s dying because of me.", sender) },
                new Choice[] {
                    new Choice("Oh my god... That sounds awful, do you want me to come over?", 4), //Branch #4
                    new Choice("I really wish that I knew how to respond to that.", 5), //Branch #5
                    new Choice("You got distracted by your phone!? Your better than that!", 6) }) //Branch #6
                );
            branches.Add(new Branch( //Branch #2
                new Message[] {
                    new Message("Whatever you did I am sure you didn't mean to do it", Message.Sender.User),
                    new Message("But you’ll have to tell me what happened.", Message.Sender.User),
                    new Message("We were driving home from Tom’s summer house", sender),
                    new Message("I wish I could unsee that...", sender),
                    new Message("I looked at my phone for just one second, I swear. I mean, who wouldn't!?", sender),
                    new Message("The screen flashed and I got distracted.", sender),
                    new Message("I didn't mean to...", sender),
                    new Message("And now he’s dying because of me.", sender) },
                new Choice[] {
                    new Choice("Oh my god... That sounds awful, do you want me to come over?", 4), //Branch #4
                    new Choice("I really wish that I knew how to respond to that.", 5), //Branch #5
                    new Choice("You got distracted by your phone!? Your better than that!", 6) }) //Branch #6
                );
            branches.Add(new Branch( //Branch #3
               new Message[] {
                    new Message("What have you done to Tom?", Message.Sender.User),
                    new Message("We were driving home from Tom’s summer house", sender),
                    new Message("I wish I could unsee that...", sender),
                    new Message("I looked at my phone for just one second, I swear. I mean, who wouldn't!?", sender),
                    new Message("The screen flashed and I got distracted.", sender),
                    new Message("I didn't mean to...", sender),
                    new Message("And now he’s dying because of me.", sender) },
                new Choice[] {
                    new Choice("Oh my god... That sounds awful, do you want me to come over?", 4), //Branch #4
                    new Choice("I really wish that I knew how to respond to that.", 5), //Branch #5
                    new Choice("You got distracted by your phone!? Your better than that!", 6) }) //Branch #6
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #4
               new Message[] {
                    new Message("Oh my god... That sounds awful, do you want me to come over?", Message.Sender.User),
                    new Message("That's very nice of you but no, I don’t feel like seeing anyone right now... Besides, they wouldn't let more than one person at a time see him if he woke up.", sender),
                    new Message("TAnd if he does, I want to be there for him.", sender),
                    new Message("There’s nothing I can do to make up for what I’ve done but I can at least be there by his side.", sender),
                    new Message("If he ever wakes up...", sender),
                    new Message("I don’t even think i’m going to be able to look him in the eyes if he did...", sender),
                    new Message("The doctors coming now! I need to go.", sender) },
               null)
               );

            branches.Add(new Branch( //Branch #5
               new Message[] {
                    new Message("I really wish that I knew how to respond to that but I really don't...", Message.Sender.User),
                    new Message("Are you sure he’s really that badly injured?", Message.Sender.User),
                    new Message("I don't know, they wont let me see him.", sender),
                    new Message("But I know he’s bad off, I stayed conscious during the whole thing and he just crashed into the windshield.", sender),
                    new Message("head first.", sender),
                    new Message("And it shattered.", sender),
                    new Message("And his face, it got all crushed and he fell to the floor and it is my fault.", sender),
                    new Message("I don’t even think i’m going to be able to look him in the eyes if he ever wakes up...", sender),
                    new Message("The doctors coming now! I need to go.", sender),
                    new Message("I don't know how he ever would be fine after that.", sender) },
               null)
               );
            branches.Add(new Branch( //Branch #5
                new Message[] {
                    new Message("You got distracted by your phone!? Your better than that!", Message.Sender.User),
                    new Message("You’re right,i’m just so fucking worthless.", sender),
                    new Message("I just wish i’d be more FUCKING CAREFUL. HOW DID IT HAPPEN SO QUICKLY!? ", sender),
                    new Message("...", sender),
                    new Message("I don’t even think i’m going to be able to look him in the eyes if he ever wakes up...", sender) },
                null)
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            return branches;
        }

        private List<Branch> GetPartT2()
        {
            List<Branch> branches = new List<Branch>();
            Message.Sender sender = Message.Sender.Thomas;

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #0
                new Message[] {
                    new Message("Hey there partner!", sender),
                    new Message("I’ve returned from the dead!", sender),
                    new Message("Tom!?", Message.Sender.User) },
                new Choice[] {
                    new Choice("Oh my god! Are you OK???", 1), //Branch #1
                    new Choice("Hey! Stop joking around! I was actually worried!!! Are you alright?!", 2), //Branch #2
                    new Choice("Holy shit Tom, don’t ever die again or I’ll kill you. How are you feeling?", 3) }) //Branch #3
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #1
                new Message[] {
                    new Message("Oh my god! Are you OK???", Message.Sender.User),
                    new Message("I’m feeling, not great, but fine if you take my condition into account.", sender),
                    new Message("The doctors believe that i’m paralyzed from the waste down, but it too soon to say, They’re just astonished that i made out alive...", sender),
                    new Message("Well, I’m glad you did, I’ve been very worried.", Message.Sender.User),
                    new Message("Yeah, I’d be very disappointed if you weren't, haha.", sender),
                    new Message("Anyway, even though dislike laying around doing nothing I should really be resting.", sender),
                    new Message("should really be resting. I suspect the doctors will force me if I don’t. See ya!", sender),
                    new Message("Hope you get better soon!", Message.Sender.User) },
                new Choice[] {
                    null })
                );
            branches.Add(new Branch( //Branch #2
                new Message[] {
                    new Message("Hey! Stop joking around! I was actually worried!!! Are you alright?!", Message.Sender.User),
                    new Message("I’m feeling, not great, but fine if you take my condition into account.", sender),
                    new Message("The doctors believe that i’m paralyzed from the waste down, but it too soon to say, They’re just astonished that i made out alive...", sender),
                    new Message("Well, I’m glad you did, I’ve been very worried.", Message.Sender.User),
                    new Message("Yeah, I’d be very disappointed if you weren't, haha.", sender),
                    new Message("Anyway, even though dislike laying around doing nothing I should really be resting.", sender),
                    new Message("should really be resting. I suspect the doctors will force me if I don’t. See ya!", sender),
                    new Message("Hope you get better soon!", Message.Sender.User) },
                new Choice[] {
                    null })
                );
           branches.Add(new Branch( //Branch #3
                new Message[] {
                    new Message("Holy shit Tom, don’t ever die again or I’ll kill you. How are you feeling?", Message.Sender.User),
                    new Message("I’m feeling, not great, but fine if you take my condition into account.", sender),
                    new Message("The doctors believe that i’m paralyzed from the waste down, but it too soon to say, They’re just astonished that i made out alive...", sender),
                    new Message("Well, I’m glad you did, I’ve been very worried.", Message.Sender.User),
                    new Message("Yeah, I’d be very disappointed if you weren't, haha.", sender),
                    new Message("Anyway, even though dislike laying around doing nothing I should really be resting.", sender),
                    new Message("should really be resting. I suspect the doctors will force me if I don’t. See ya!", sender),
                    new Message("Hope you get better soon!", Message.Sender.User) },
                new Choice[] {
                    null })
                );
            
            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            return branches;
        }


        private List<Branch> GetPartT3()
        {
            List<Branch> branches = new List<Branch>();
            Message.Sender sender = Message.Sender.Thomas;

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #0
                new Message[] {
                    new Message("Hello Tom!",  Message.Sender.User),
                    new Message("Hi!", sender) },
                new Choice[] {
                    new Choice("What’s up?", 1), //Branch #1
                    new Choice("How are you?", 2) }) //Branch #2
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #1
                new Message[] {
                    new Message("What’s up?", Message.Sender.User),
                    new Message("Not much.", sender),
                    new Message("Charlie’s been shut in his room playing Ocarina of Time all weekend.", sender),
                    new Message("But weren’t you supposed to play it together?", Message.Sender.User),
                    new Message("You guys always play together.", Message.Sender.User),
                    new Message("I dunno, it’s just not the same anymore.", sender),
                    new Message("Did something happened?", Message.Sender.User),
                    new Message("Not really.", sender),
                    new Message("It’s just that Charlie has started acting really strange ever since the accident.", sender),
                    new Message("I think he feels guilty about what happened, but he doesn’t want to talk about it.", sender),
                    new Message("I try to tell him that it’s alright, and that I don’t blame him a bit, but it doesn’t seem to believe it.", sender) },
                new Choice[] {
                    new Choice("That’s a bummer...", 3), //Branch #3
                    new Choice("He’ll come around!", 4) }) //Branch #4
                );
            branches.Add(new Branch( //Branch #2
                new Message[] {
                    new Message("How are you?", Message.Sender.User),
                    new Message("Not to great actually", sender),
                    new Message("Did something happened?", Message.Sender.User),
                    new Message("Not really.", sender),
                    new Message("It’s just that Charlie has started acting really strange ever since the accident.", sender),
                    new Message("I think he feels guilty about what happened, but he doesn’t want to talk about it.", sender),
                    new Message("I try to tell him that it’s alright, and that I don’t blame him a bit, but it doesn’t seem to believe it.", sender) },
                new Choice[] {
                    new Choice("That’s a bummer...", 3), //Branch #3
                    new Choice("He’ll come around!", 4) }) //Branch #4
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #3
               new Message[] {
                    new Message("That’s a bummer... ", Message.Sender.User),
                    new Message("Yeah...", sender),
                    new Message("Anyway, Since I’ve never actually played Ocarina of Time, maybe i should play it on my own.", sender) },
               new Choice[] {
                    new Choice("Don’t waste your Time.", 5), //Branch #5
                    new Choice("Sounds fun!", 6), //Branch #6
                    new Choice("That game is really overrated.", 7) }) //Branch #7
               );
            branches.Add(new Branch( //Branch #4
               new Message[] {
                    new Message("He’ll come around!", Message.Sender.User),
                    new Message("I guess...", sender),
                    new Message("Well i’m not gonna help by being bummed out!", sender),
                    new Message("I need to support im the best i can.", sender),
                    new Message("Anyway, Since I’ve never actually played Ocarina of Time, maybe i should play it on my own..", sender) },
               new Choice[] {
                    new Choice("Don’t waste your Time.", 5), //Branch #5
                    new Choice("Sounds fun!", 6), //Branch #6
                    new Choice("That game is really overrated.", 7) }) //Branch #7
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #5
               new Message[] {
                    new Message("Don’t waste your Time.", Message.Sender.User),
                    new Message("Really? Maybe i should just play Majora's mask instead.", sender),
                    new Message("Hey, Sam", sender),
                    new Message("I really appreciate that i can talk to you about this stuff. ", sender),
                    new Message("I’ve been fearing that our relationship might take a turn for the worse after the accident, but i think we’ve might have been able to avoid it.", sender),
                    new Message("Honestly i didn’t expect Charlie to take it as well as he did. It is only natural that he would be shook by it.", sender) },
               new Choice[] {
                    new Choice("That’s a relief, you know you can always count on me!", 8), //Jump to Branch #8
                    new Choice("You’ve got to be careful though, Charlie is a very sensitive person you know.", 9) }) //Jump to Branch #9
               );
            branches.Add(new Branch( //Branch #6
               new Message[] {
                    new Message("Sounds fun!", Message.Sender.User),
                    new Message("Yeah, i’m actually really excited!", sender),
                    new Message("Hey, Sam", sender),
                    new Message("I really appreciate that i can talk to you about this stuff. ", sender),
                    new Message("I’ve been fearing that our relationship might take a turn for the worse after the accident, but i think we’ve might have been able to avoid it.", sender),
                    new Message("Honestly i didn’t expect Charlie to take it as well as he did. It is only natural that he would be shook by it.", sender) },
               new Choice[] {
                    new Choice("That’s a relief, you know you can always count on me!", 8), //Jump to Branch #8
                    new Choice("You’ve got to be careful though, Charlie is a very sensitive person you know.", 9) }) //Jump to Branch #9
               );
            branches.Add(new Branch( //Branch #7
               new Message[] {
                    new Message("That game is really overrated.", Message.Sender.User),
                    new Message("So i’ve heard. Still, i wanna hear what all the fuss is about.", sender),
                    new Message("Hey, Sam", sender),
                    new Message("I really appreciate that i can talk to you about this stuff. ", sender),
                    new Message("I’ve been fearing that our relationship might take a turn for the worse after the accident, but i think we’ve might have been able to avoid it.", sender),
                    new Message("Honestly i didn’t expect Charlie to take it as well as he did. It is only natural that he would be shook by it.", sender) },
               new Choice[] {
                    new Choice("That’s a relief, you know you can always count on me!", 8), //Jump to Branch #8
                    new Choice("You’ve got to be careful though, Charlie is a very sensitive person you know.", 9) }) //Jump to Branch #9
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #8
               new Message[] {
                    new Message("That’s a relief, you know you can always count on me!", Message.Sender.User),
                    new Message("That means alot to me, thank you. i’m gonna go give Charlie a hug, i think he needs it.", sender),
                    new Message("Take care!", sender),
                    new Message("Bye!", Message.Sender.User) },
               new Choice[] {
                    null })
               );
            branches.Add(new Branch( //Branch #9
               new Message[] {
                    new Message("You’ve got to be careful though, Charlie is a very sensitive person you know.", Message.Sender.User),
                    new Message("Yeah, i should go give Charlie a hug, i think he needs it.", sender),
                    new Message("Take care!", sender),
                    new Message("Bye!", Message.Sender.User) },
               new Choice[] {
                    null })
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            return branches;
        }


        private List<Branch> GetPartC4()
        {
            List<Branch> branches = new List<Branch>();
            Message.Sender sender = Message.Sender.Charlie;

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #0
                new Message[] {
                    new Message("Hello?", sender),
                    new Message("Hi?",  Message.Sender.User),
                    new Message("I really need to talk, do you have a minute?", sender) },
                new Choice[] {
                    new Choice("Of course", 1), //Branch #1
                    new Choice("I’m kind of busy", 2) }) //Branch #2
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #1
                new Message[] {
                    new Message("Of course, what’s bothering you?", Message.Sender.User),
                    new Message("Have you been seeing Tom lately?", sender) },
                new Choice[] {
                    new Choice("Yes?", 3), //Branch #3
                    new Choice("No?", 4) }) //Branch #4
                );
            branches.Add(new Branch( //Branch #2
                new Message[] {
                    new Message("Actually I’m kind of busy, can we talk later?", Message.Sender.User),
                    new Message("Don't worry, It won't take long.", sender),
                    new Message("Have you been seeing Tom lately?", sender) },
                new Choice[] {
                    new Choice("Yes?", 3), //Branch #3
                    new Choice("No?", 4) }) //Branch #4
                );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #3
               new Message[] {
                    new Message("Yes?", Message.Sender.User),
                    new Message("I feel like he’s changed a lot over the past year, as in, he’s not the same anymore.", sender),
                    new Message("I don't know...", sender),
                    new Message("Is it just me?", sender) },
               new Choice[] {
                    new Choice("No, I agree, he’s indeed been acting strange...", 5), //Branch #5
                    new Choice("I don’t think I follow...", 6) }) //Branch #6
               );
            branches.Add(new Branch( //Branch #4
               new Message[] {
                    new Message("No?", Message.Sender.User),
                    new Message("I feel like he’s changed a lot over the past year, as in, he’s not the same anymore.", sender),
                    new Message("I don't know...", sender),
                    new Message("Is it just me?", sender) },
               new Choice[] {
                    new Choice("No, I agree, he’s indeed been acting strange...", 5), //Branch #5
                    new Choice("I don’t think I follow...", 6) }) //Branch #6
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #5
               new Message[] {
                    new Message("No, I agree, he’s indeed been acting strange...", Message.Sender.User),
                    new Message("Ever since the accident it has been that way and I can't help but think that the injuries maybe affected more than his motor skills.", sender),
                    new Message("I looked it up and it's not an uncommon thing.", sender),
                    new Message("Apparently traumatic brain injuries can alter personality. And it is one of the most common injuries in car accidents.", sender),
                    new Message("Sometimes it is not even the injury itself that does it. Even when someone's brain is not injured, their personality can change because of the trauma.", sender),
                    new Message("What if the accident did that to Tom?", sender),
                    new Message("That means I did it.", sender) },
               new Choice[] {
                    new Choice("You didn't hurt him intentionally, don’t blame yourself", 7), //Jump to Branch #7
                    new Choice("You should speak to him instead of me.", 8) }) //Jump to Branch #8
               );
            branches.Add(new Branch( //Branch #6
               new Message[] {
                    new Message("I don’t think I follow...", Message.Sender.User),
                    new Message("Ever since the accident it has been that way and I can't help but think that the injuries maybe affected more than his motor skills.", sender),
                    new Message("I looked it up and it's not an uncommon thing.", sender),
                    new Message("Apparently traumatic brain injuries can alter personality. And it is one of the most common injuries in car accidents.", sender),
                    new Message("Sometimes it is not even the injury itself that does it. Even when someone's brain is not injured, their personality can change because of the trauma.", sender),
                    new Message("What if the accident did that to Tom?", sender),
                    new Message("That means I did it.", sender) },
               new Choice[] {
                    new Choice("You didn't hurt him intentionally, don’t blame yourself", 7), //Jump to Branch #7
                    new Choice("You should speak to him instead of me.", 8) }) //Jump to Branch #8
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
            
            branches.Add(new Branch( //Branch #7
               new Message[] {
                    new Message("Hey, calm down.", Message.Sender.User),
                    new Message("You didn't hurt him intentionally, don’t blame yourself", Message.Sender.User),
                    new Message("I Actually spoke to Tom not too long ago and he's worried about you.", Message.Sender.User),
                    new Message("He really doesn't blame you a bit, you know.", Message.Sender.User),
                    new Message("You should let it go, it's better for both of you.", Message.Sender.User),
                    new Message("I know I should but I can’t, especially now when nothing is as it used to be.", sender),
                    new Message("I don't know, maybe I’m just overreacting...", sender),
                    new Message("Am I acting differently? ", sender) },
               new Choice[] {
                    new Choice("Yeah, you seem unusually stressed.", 9), //Jump to Branch #9
                    new Choice("Not really...", 10) }) //Jump to Branch #10
               );


            branches.Add(new Branch( //Branch #8
               new Message[] {
                    new Message("In that case he really needs you right now.", Message.Sender.User),
                    new Message("You should speak to him instead of me.", Message.Sender.User),
                    new Message("He’s worried about you too you know.", Message.Sender.User),
                    new Message("I know I should but I can’t, especially now when nothing is as it used to be.", sender),
                    new Message("I don't know, maybe I’m just overreacting...", sender),
                    new Message("Am I acting differently? ", sender) },
               new Choice[] {
                    new Choice("Yeah, you seem unusually stressed.", 9), //Jump to Branch #9
                    new Choice("Not really...", 10) }) //Jump to Branch #10
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #9
                new Message[] {
                    new Message("Yeah, you seem unusually stressed.", Message.Sender.User),
                    new Message("I mean, the accident happened more than one year ago, and you still are pretty worked up...", Message.Sender.User),
                    new Message("Are you sure that is the only thing that has been bothering you lately?", Message.Sender.User),
                    new Message("I don't know anymore, my feelings are all mixed up.", sender),
                    new Message("Let me ask you the same question you asked me.", Message.Sender.User),
                    new Message("Have you been seeing Tom lately?", Message.Sender.User),
                    new Message("What are you on about?", sender),
                    new Message("Of course I have, I live with him you know? ", sender),
                    new Message("Yes, but have you been seeing him.", Message.Sender.User),
                    new Message("Like in spending time with him?", Message.Sender.User),
                    new Message("You used to play games together all the time and he told me you hardly never do that anymore.", Message.Sender.User),
                    new Message("Well, as I said, it’s not the same as it used to be...", sender),
                    new Message("At least it doesn't feel the same.", sender),
                    new Message("I used to be happy when we did those kind of things but i've been feeling kind of numb lately.", sender),
                    new Message("Nothing is as fun as it used to. I’m not happy anymore.", sender) },
                new Choice[] {
                    new Choice("Are you telling me you don't like Tom anymore?", 11), //Jump to Branch #11
                    new Choice("If you feel that way, maybe you should break up? ", 12) }) //Jump to Branch #12
                );


            branches.Add(new Branch( //Branch #10
               new Message[] {
                    new Message("Not really...", Message.Sender.User),
                    new Message("I didn't use to feel this stressed...", sender),
                    new Message("I mean, I worried a lot about things but most of the time I would let it go.", sender),
                    new Message("The accident happened more than one year ago and still can't let it go.", sender),
                    new Message("I don't even know if it really is the accident that has gotten me all worked up.", sender),
                    new Message("Maybe it’s something else...", sender),
                    new Message("Let me ask you the same question you asked me.", Message.Sender.User),
                    new Message("Have you been seeing Tom lately?", Message.Sender.User),
                    new Message("What are you on about?", sender),
                    new Message("Of course I have, I live with him you know? ", sender),
                    new Message("Yes, but have you been seeing him.", Message.Sender.User),
                    new Message("Like in spending time with him?", Message.Sender.User),
                    new Message("You used to play games together all the time and he told me you hardly never do that anymore.", Message.Sender.User),
                    new Message("Well, as I said, it’s not the same as it used to be...", sender),
                    new Message("At least it doesn't feel the same.", sender),
                    new Message("I used to be happy when we did those kind of things but i've been feeling kind of numb lately.", sender),
                    new Message("Nothing is as fun as it used to. I’m not happy anymore.", sender) },
               new Choice[] {
                    new Choice("You don't like Tom anymore?", 11), //Jump to Branch #11
                    new Choice("Maybe you should break up?", 12) }) //Jump to Branch #12
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #11
                new Message[] {
                    new Message("Are you telling me you don't like Tom anymore?", Message.Sender.User),
                    new Message("To be honest, I’ve been considering breaking up, but that is not an option.", sender),
                    new Message("How could I ever do such a thing?", sender),
                    new Message("Harm him in that way and then leave him all by himself.", sender),
                    new Message("No, no way I could do that, leave him now when he’s paralyzed from the waist down and wheelchair-bound because of me.", sender),
                    new Message("Still, I can’t stand being around him anymore...",sender),
                    new Message("I can't look him in the eyes without thinking of what I did.", sender),
                    new Message("If I just wouldn't have looked at my phone. ", sender),
                    new Message("How could I do something that incredibly stupid?", sender),
                    new Message("Maybe I’ll get over it... I don’t know, everything feels hopeless right now.", sender),
                    new Message("I don't know what to do.", sender) },
                new Choice[] {
                    new Choice("Break up", 13), //Jump to Branch #13
                    new Choice("Don’t break up ", 14) }) //Jump to Branch #14
                );


            branches.Add(new Branch( //Branch #12
               new Message[] {
                    new Message("If you feel that way, maybe you should break up?", Message.Sender.User),
                    new Message("To be honest, I’ve been considering breaking up, but that is not an option.", sender),
                    new Message("How could I ever do such a thing?", sender),
                    new Message("Harm him in that way and then leave him all by himself.", sender),
                    new Message("No, no way I could do that, leave him now when he’s paralyzed from the waist down and wheelchair-bound because of me.", sender),
                    new Message("Still, I can’t stand being around him anymore...",sender),
                    new Message("I can't look him in the eyes without thinking of what I did.", sender),
                    new Message("If I just wouldn't have looked at my phone. ", sender),
                    new Message("How could I do something that incredibly stupid?", sender),
                    new Message("Maybe I’ll get over it... I don’t know, everything feels hopeless right now.", sender),
                    new Message("I don't know what to do.", sender) },
                new Choice[] {
                    new Choice("Break up", 13), //Jump to Branch #13
                    new Choice("Don’t break up", 14) }) //Jump to Branch #14
               );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            branches.Add(new Branch( //Branch #13
                new Message[] {
                    new Message("Break up", Message.Sender.User),
                    new Message("", Message.Sender.User) },
                null)
            );
            branches.Add(new Branch( //Branch #14
                new Message[] {
                    new Message("Don’t break up", Message.Sender.User),
                    new Message("", Message.Sender.User) },
                null)
            );

            /*------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            return branches;
        }
    }
}
