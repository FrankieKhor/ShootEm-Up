using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended;
using System.Threading;
using System.Xml.Serialization;

namespace ShootShapesUp
{
    public class GameRoot : Game
    {
        
        // some helpful static properties
        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }
        public static Texture2D Player { get; private set; }
        public static Texture2D Seeker { get; private set; }
        public static Texture2D Bullet { get; private set; }
        public static Texture2D Rocket { get; private set; }
        public static Texture2D Laser { get; private set; }
        public static Texture2D Health1 { get; private set; }

        public static Texture2D Cursor { get; private set; }
        public static Vector2 CursorPos { get; private set; }
        public static Texture2D Background { get; private set; }
        public static Texture2D Earth1 { get; private set; }
        public static Texture2D Earth2 { get; private set; }
        public static Texture2D Earth3 { get; private set; }
        public static Texture2D Moon { get; private set; }
        public static Texture2D Alien { get; private set; }
        public static Texture2D AlienRare { get; private set; }
        public static Texture2D Firework { get; private set; }
        public static Texture2D PowerUpInvis { get; private set; }
        public static Texture2D PowerUpAddHealth { get; private set; }
        public static Texture2D PowerUpDoubleMode { get; private set; }
        public static Texture2D PowerUpNuke { get; private set; }


        public static SpriteFont Font { get; private set; }
        public static BitmapFont Font1 { get; private set; }

        // public static Song Music { get; private set; }


        //private static SoundEffect[] explosions;
        // return a random explosion sound
        // public static SoundEffect Explosion { get { return explosions[rand.Next(explosions.Length)]; } }

        // private static SoundEffect[] shots;
        //public static SoundEffect Shot { get { return shots[rand.Next(shots.Length)]; } }

        // private static SoundEffect[] spawns;
        // public static SoundEffect Spawn { get { return spawns[rand.Next(spawns.Length)]; } }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameState _state;
        Texture2D texture;
        Rectangle play;
        public static bool rocketEnable = true;
        public static bool laserEnable = false;
        public static bool doubleMode = false;
        Leaderboard a = new Leaderboard();
        Rectangle leaderboard;
        public static bool playerDied;
        bool levelComplete;
        int completion;
        String name;
        String level;
        bool menuPlay;
        public static int enemyCount;
        public static KeyboardState keyboardState;
        bool menu;
        public GameRoot()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content";

            graphics.PreferredBackBufferWidth = 1100;
            graphics.PreferredBackBufferHeight = 750;
        }
        enum GameState
        {
            MainMenu,
            LevelOne,
            LevelTwo,
            LevelBoss,
            EndOfGame,
            WonGame,
            Leaderboard
        }

        protected override void Initialize()
        {
            enemyCount = 10;
            level = "";
            base.Initialize();
            EntityManager.Add(PlayerShip.Instance);
            completion = 0;
            play = new Rectangle((int)ScreenSize.X / 3, (int)ScreenSize.Y / 5, 200, 100);
            leaderboard = new Rectangle((int)ScreenSize.X / 3, (int)ScreenSize.Y / 2, 200, 100);
            playerDied = false;
            levelComplete = false;
            name = "Frankie";
            menuPlay = true;
            menu = false;
            
            a.Initialize();
            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Play(GameRoot.Music);
        }
        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player = Content.Load<Texture2D>("Art/Player");
            Bullet = Content.Load<Texture2D>("Art/Bullet");
            Rocket = Content.Load<Texture2D>("Art/Rocket");
            Laser = Content.Load<Texture2D>("Art/Laser");
            Health1 = Content.Load<Texture2D>("Art/Health2");
            Cursor = Content.Load<Texture2D>("Art/Pointer");
            Background = Content.Load<Texture2D>("Art/Background");
            Earth1 = Content.Load<Texture2D>("Art/Earth_1");
            Earth2 = Content.Load<Texture2D>("Art/Earth_2");
            Earth3 = Content.Load<Texture2D>("Art/Earth_3");
            Moon = Content.Load<Texture2D>("Art/Moon");
            Alien = Content.Load<Texture2D>("Art/Alien_enemy");
            AlienRare = Content.Load<Texture2D>("Art/Alien_enemy_special");
            PowerUpAddHealth = Content.Load<Texture2D>("Art/PowerUpHealth");
            PowerUpInvis = Content.Load<Texture2D>("Art/PowerUpInvis");
            PowerUpNuke = Content.Load<Texture2D>("Art/PowerUpNuke");
            Firework = Content.Load<Texture2D>("Art/Fireworks");

            Font = Content.Load<SpriteFont>("Font");
            //Font1 = Content.Load<BitmapFont>("VerFont");
            texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });

            // Music = Content.Load<Song>("Sound/Music");

            // These linq expressions are just a fancy way loading all sounds of each category into an array.
            //explosions = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/explosion-0" + x)).ToArray();
            //  shots = Enumerable.Range(1, 4).Select(x => Content.Load<SoundEffect>("Sound/shoot-0" + x)).ToArray();
            // spawns = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/spawn-0" + x)).ToArray();
        }
        protected override void Update(GameTime gameTime)
        {
            //Console.WriteLine("DEAD: "+playerDied + " "+ Health.RemainHealth() + " "+PlayerShip.dead);
            GameTime = gameTime;
            keyboardState = Keyboard.GetState();
            Input.Update();

            // Allows the game to exit
            if (Input.WasButtonPressed(Buttons.Back) || Input.WasKeyPressed(Keys.Escape))
                this.Exit();
            if (_state != GameState.MainMenu && _state != GameState.WonGame && _state != GameState.EndOfGame && _state != GameState.Leaderboard) { 
            EntityManager.Update();
            EnemySpawner.Update();
            if (playerDied) {
                    Console.WriteLine("Saving Highscore");
                    HighScoreData data = new HighScoreData(10);

                    data.PlayerName[6] = "Bob";
                    data.Score[6] = Score.score;
                    a.OrderSave();
            }
}

            base.Update(gameTime);
            switch (_state)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.LevelOne:
                    UpdateLevelOne(gameTime);
                    break;
                case GameState.LevelTwo:
                    UpdateLevelTwo(gameTime);
                    break;
                case GameState.LevelBoss:
                    UpdateLevelBoss(gameTime);
                    break;
                case GameState.WonGame:
                    UpdateWonGame(gameTime);
                    break;
                case GameState.EndOfGame:
                    UpdateEndOfGame(gameTime);
                    break;
                case GameState.Leaderboard:
                    UpdateLeaderboard(gameTime);
                    break;


            }
        }
        public void reset()
        {
            enemyCount = 10;
            level = "";
            base.Initialize();
            EntityManager.Add(PlayerShip.Instance);
            playerDied = false;
            levelComplete = false;
            name = "Frankie";
            menuPlay = true;
            menu = false;
        }
        public void UpdateMainMenu(GameTime gameTime)
        {
            if (completion>0)
            {
                //Health.reset();
            }
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                menuPlay = false;
            }
            else if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                menuPlay = true;
            }
            if (menuPlay && (keyboardState.IsKeyDown(Keys.Enter)))
                _state = GameState.LevelOne;
            else if(!menuPlay && (keyboardState.IsKeyDown(Keys.Enter)))
                _state = GameState.Leaderboard;

        }
        public void UpdateLevelOne(GameTime gameTime)
        {
            levelComplete = false;
            
            Enemy.enemySpeed = 0.1F;
            if (playerDied)
                _state = GameState.EndOfGame;
            
            if (enemyCount == 0)
                levelComplete = true;   

            if (levelComplete) { 
                enemyCount = 20;
                _state = GameState.LevelTwo;
            }
        }
        public void UpdateLevelTwo(GameTime gameTime)
        {
            levelComplete = false;
            rocketEnable = true;
            Enemy.enemySpeed = 0.7F;

            if (playerDied)
                _state = GameState.EndOfGame;
            if (enemyCount == 0)
            {
                Console.WriteLine("COMPLETE");
                levelComplete = true;
            }
            if (levelComplete) {
                enemyCount = 30;
            _state = GameState.LevelBoss;
        }
        }
        public void UpdateLevelBoss(GameTime gameTime)
        {
            levelComplete = false;
            laserEnable = true;
            Enemy.enemySpeed = 0.9F;

            if (playerDied)
                _state = GameState.EndOfGame;
            if (enemyCount == 0)
                levelComplete = true;
            if (levelComplete)
                _state = GameState.WonGame;
        }
        public void UpdateEndOfGame(GameTime gameTime)
        {
            completion++;
            if (keyboardState.IsKeyDown(Keys.Enter))
            menu = true;
            if(menu)
                _state = GameState.MainMenu;


        }
        public void UpdateWonGame(GameTime gameTime)
        {
            menu = true;
            if (menu)
                if (keyboardState.IsKeyDown(Keys.Enter))
                _state = GameState.MainMenu;
            menu = false;
        }

        public void UpdateLeaderboard(GameTime gameTime)
        {
            if (keyboardState.IsKeyDown(Keys.Back))
                _state = GameState.MainMenu;
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
             Random rand = new Random();
            
            // Draw user interface
            if (_state != GameState.MainMenu && _state != GameState.WonGame && _state != GameState.EndOfGame && _state != GameState.Leaderboard)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
                //DrawBorder(Bullet, 5, Color.Red);.
               
                spriteBatch.DrawString(Font, "Score: " + Score.score.ToString("000000"), new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(Font, level, new Vector2(ScreenSize.X-100, 0), Color.White);
                spriteBatch.DrawString(Font, "Enemy Left: "+enemyCount, new Vector2(ScreenSize.X /2, 0), Color.White);
                spriteBatch.Draw(Moon, new Vector2(ScreenSize.X-200, ScreenSize.Y - 700), Color.White);

                if (playerDied)
                {
                    spriteBatch.DrawString(Font, "NAME", new Vector2(0,0), Color.Aquamarine);
                }
                for (int i = 1; i <= Health.RemainHealth(); i++)
                {
                    spriteBatch.Draw(Health1, new Vector2(0 + (i * 30), ScreenSize.Y - 30), Color.White);
                }

                spriteBatch.End();
            }

            switch (_state)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.LevelOne:
                    level = "LEVEL 1";
                    DrawLevelOne(gameTime);
                    break;
                case GameState.LevelTwo:
                    level = "LEVEL 2";
                    DrawLevelTwo(gameTime);
                    break;
                case GameState.LevelBoss:
                    level = "LEVEL BOSS";
                    DrawLevelBoss(gameTime);
                    break;
                case GameState.WonGame:
                    level = "WON GAME";
                    DrawWonGame(gameTime);
                    break;
                case GameState.EndOfGame:
                    DrawEndOfGame(gameTime);
                    break;
                case GameState.Leaderboard:
                    DrawLeaderboard(gameTime);
                    break;
            }
        }
        public void DrawMainMenu(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            spriteBatch.Draw(Cursor, CursorPos, Color.Blue);
            spriteBatch.End();
            Rectangle play = new Rectangle(new Point((int)ScreenSize.X /3, (int)ScreenSize.Y / 6), new Point(200, 100));
            Rectangle leaderboard = new Rectangle(new Point((int)ScreenSize.X / 3, (int)ScreenSize.Y / 3), new Point(200, 100));

            //base.Draw(gameTime);
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "SHUMP GAME", new Vector2(ScreenSize.X / 3, ScreenSize.Y / 8), Color.White);

            spriteBatch.FillRectangle(play, Color.White);
            spriteBatch.DrawString(Font, "PLAY", new Vector2(play.X+80, play.Y+40), Color.Black);
            spriteBatch.FillRectangle(leaderboard, Color.White);
            spriteBatch.DrawString(Font, "LEADERBAORD", new Vector2(leaderboard.X + 40, leaderboard.Y + 40), Color.Black);


            if (menuPlay)
            {
                spriteBatch.DrawRectangle(play, Color.Red, 5);
            }
            else
            {
                spriteBatch.DrawRectangle(leaderboard, Color.Red, 5);
            }
            //if (Math.Abs(selection) % 2 == 0)
            //{
            //    spriteBatch.DrawRectangle(play, Color.Red, 5);
            //}
            //else
            //{
            //    spriteBatch.DrawRectangle(leaderboard, Color.Red, 5);

            //}

            spriteBatch.End();
        }
        
        public void DrawLevelOne(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(PowerUpAddHealth, new Vector2(200, 400), Color.White);
            spriteBatch.Draw(Earth1, new Vector2(ScreenSize.X / 2, ScreenSize.Y - 100), Color.White);
            spriteBatch.Draw(Bullet, new Vector2(ScreenSize.X - 40, ScreenSize.Y - 20), Color.White);
            //spriteBatch.Draw(Firework, new Vector2(ScreenSize.X/2, ScreenSize.Y /2), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(spriteBatch);
            spriteBatch.End();

            
            
          
        }
        public void DrawLevelTwo(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Earth2, new Vector2(ScreenSize.X / 2, ScreenSize.Y - 100), Color.White);
            spriteBatch.Draw(Bullet, new Vector2(ScreenSize.X - 40, ScreenSize.Y - 20), Color.White);
            spriteBatch.Draw(Rocket, new Vector2(ScreenSize.X - 40, ScreenSize.Y - 40), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(spriteBatch);
            spriteBatch.End();
        }
        public void DrawLevelBoss(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Earth3, new Vector2(ScreenSize.X / 2, ScreenSize.Y - 100), Color.White);
            spriteBatch.Draw(Bullet, new Vector2(ScreenSize.X - 40, ScreenSize.Y - 20), Color.White);
            spriteBatch.Draw(Rocket, new Vector2(ScreenSize.X - 40, ScreenSize.Y - 40), Color.White);
            spriteBatch.Draw(Laser, new Vector2(ScreenSize.X - 40, ScreenSize.Y - 60), Color.White);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            EntityManager.Draw(spriteBatch);
            spriteBatch.End();
        }
        public void DrawEndOfGame(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "You Died: "+ name , new Vector2(ScreenSize.X/2, ScreenSize.Y/7), Color.White);
            spriteBatch.DrawString(Font, "Score: " + Score.score, new Vector2(ScreenSize.X / 2, ScreenSize.Y / 6), Color.White);
            spriteBatch.DrawString(Font, "Press Enter to Main Menu: " , new Vector2(ScreenSize.X / 2, ScreenSize.Y/2 ), Color.White);
            spriteBatch.End();
        }
        public void DrawWonGame(GameTime gameTime)
        {
            spriteBatch.Begin();
            DrawRightAlignedString("dasdas", 32);
            spriteBatch.DrawString(Font, "You Won: " + name, new Vector2(ScreenSize.X / 2, ScreenSize.Y / 7), Color.White);
            spriteBatch.DrawString(Font, "Score: " + Score.score, new Vector2(ScreenSize.X / 2, ScreenSize.Y / 6), Color.White);
            spriteBatch.End();
        }
        public void DrawLeaderboard(GameTime gameTime)
        {
            Leaderboard.PrettyXml("D:/Year 3/CS3005 - Digital Media and Games/Visual Studio 2015/Projects/ShootShapesUp/ShootShapesUp/highscores.xml");
            int[] balance = new int[10];
            for (int i = 0; i < 10; i++)
            {
                balance[i] = i + 100;
            }
            spriteBatch.Begin();
            spriteBatch.DrawString(Font, "LEADERBOARD: " , new Vector2(ScreenSize.X / 2, 100), Color.White);
            for (int i = 0; i < 10; i++)
                spriteBatch.DrawString(Font, "Score: " + balance[i], new Vector2(ScreenSize.X / 2, (i+1)*100), Color.White);   
            spriteBatch.End();
        }
        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = GameRoot.Font.MeasureString(text).X;
            spriteBatch.DrawString(GameRoot.Font, text, new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }
        public void spawnPowerUp(Vector2 position)
        {

        }

    }
}

