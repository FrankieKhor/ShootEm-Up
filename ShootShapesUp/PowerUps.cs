using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ShootShapesUp
{
    class PowerUps : Entity
    {
        enum PowerUp{
            Invincibility,
            AddHealth,
            DoubleMode,
            Nuke,
        }
        GameTime gameTime;
        PowerUp _powerUp;
        static Random rand = new Random();
        double probSpawn = 0.8;
        double choice;
        Texture2D image;
        SpriteBatch spriteBatch;
        public static bool invis = false;
        static int time = 10;

        public bool IsActive { get; internal set; }

        public override void Update()
        {
            
            if (rand.NextDouble() * 1 > probSpawn)
            choice = rand.NextDouble() * 1 ;
            if (choice > 0  && choice < 0.25)
                _powerUp = PowerUp.AddHealth;
            else if (choice > 0.25 && choice < 0.5)
                _powerUp = PowerUp.DoubleMode;
            else if (choice > 0.5 && choice < 0.75)
                _powerUp = PowerUp.Invincibility;
            else if (choice > 0.75 && choice < 1)
                _powerUp = PowerUp.Nuke;

            switch (_powerUp)
                {
                    case PowerUp.Invincibility:
                        UpdateInvincibility(gameTime);
                        break;
                    case PowerUp.AddHealth:
                        UpdateAddHealth(gameTime);
                        break;
                    case PowerUp.DoubleMode:
                        UpdateDoubleMode(gameTime);
                        break;
                    case PowerUp.Nuke:
                        UpdateNuke(gameTime);
                        break;
                }          
        }

        public void UpdateInvincibility(GameTime gameTime)
        {
            invis = true;
        }
        public void UpdateAddHealth(GameTime gameTime)
        {
            Health.AddHealth();
        }
        public void UpdateDoubleMode(GameTime gameTime)
        {
            //Vector2 offset = Vector2.Transform(new Vector2(35, 0), aimQuat);
            //EntityManager.Add(new Bullet(Position + offset, vel));

            //offset = Vector2.Transform(new Vector2(35, 8), aimQuat);
            //EntityManager.Add(new Bullet(Position + offset, vel));
        }
        public void UpdateNuke(GameTime gameTime)
        {

        }


        public void DrawInvincibility(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GameRoot.PowerUpInvis, new Vector2(200, 400), Color.White);
            spriteBatch.End();
        }
        public void DrawAddHealth(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GameRoot.PowerUpAddHealth, new Vector2(200, 400), Color.White);
            spriteBatch.End();
        }
        public void DrawDoubleMode(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GameRoot.PowerUpDoubleMode, new Vector2(200, 400), Color.White);
            spriteBatch.End();
        }
        public void DrawNuke(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GameRoot.PowerUpNuke, new Vector2(200, 400), Color.White);
            spriteBatch.End();
        }
        public void WasShot()
        {
            IsExpired = true;
            // GameRoot.Explosion.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0);

        }

       
    }
}
