using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class Health : Entity
    {
        static int healthLimit =2;
        public static int actualHealth;
        public Health()
        {
            
            //Position = GameRoot.ScreenSize/3;
        }
        
        public static int RemainHealth()
        {
            actualHealth = healthLimit - PlayerShip.death;
            if (actualHealth < 1)
            {
                GameRoot.playerDied = true;
            }
            else
            {
                GameRoot.playerDied = false;

            }
            return actualHealth;
        }
        public static int AddHealth()
        {
            if (actualHealth < healthLimit)
            {
                healthLimit++;
            }
            return actualHealth;
        }
        

        public static void reset()
        {
            //GameRoot.playerDied = false;
            PlayerShip.death = 0;
            actualHealth = healthLimit;

        }


        public void draw()
        {
            //spriteBatch.Draw(Rocket, new Vector2(ScreenSize.X - 40, ScreenSize.Y - 30), Color.White);

        }

        public override void Update()
        {
            //spriteBatch.Draw(Rocket, new Vector2(ScreenSize.X - 40, ScreenSize.Y - 30), Color.White);

        }
      
    }


}

