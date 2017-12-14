using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.Collections;

namespace ShootShapesUp
{
    class PlayerShip : Entity
    {
        static int currentWeapon = 1;
        public static int death = 0;
        private static PlayerShip instance;
        public static PlayerShip Instance
        {
            get
            {
                if (instance == null)
                  
                    instance = new PlayerShip();

                return instance;
            }
        }
       
        const int cooldownFrames = 6;
        int cooldownRemaining = 0;

        int framesUntilRespawn = 0;
        public bool IsDead { get { return framesUntilRespawn > 0; } }
        public static bool dead;
        static Random rand = new Random();

        private PlayerShip()
        {
           
            image = GameRoot.Player;
            Position = GameRoot.ScreenSize / 2;
            Radius = 10;
        }

        public override void Update()
        {
            if (IsDead)
            {
                dead = true;                
                --framesUntilRespawn;
                return;
            }
            if (dead) { 
            Score.TakeScore();
            death++;
            }
            dead = false;

            bullet();
             
            
        }
        public  void bullet()
        {
            var aim = Input.GetMovementDirection();
            Texture2D weapon = null;
            if (currentWeapon == 1)
                weapon = weaponBurster();
            else if (currentWeapon == 2 && GameRoot.rocketEnable)
                weapon = weaponRocket();
            else if (currentWeapon == 3 && GameRoot.laserEnable)
                weapon = weaponLaser();
            if (Input.keyboardState.IsKeyDown(Keys.LeftShift) && Input.lastKeyboardState.IsKeyUp(Keys.LeftShift))
            {

               WeaponChoice();


            }
            if (Input.keyboardState.IsKeyDown(Keys.Space))
            {
                if ( cooldownRemaining <= 0)
                {
                    cooldownRemaining = cooldownFrames;
                    float aimAngle = Orientation;
                    Quaternion aimQuat = Quaternion.CreateFromYawPitchRoll(0, 0, aimAngle);

                    float randomSpread = rand.NextFloat(-0.04f, 0.04f) + rand.NextFloat(-0.04f, 0.04f);
                    Vector2 vel = 11f * new Vector2((float)Math.Cos(aimAngle + randomSpread), (float)Math.Sin(aimAngle + randomSpread));

                    Vector2 offset = Vector2.Transform(new Vector2(35, 0), aimQuat);
                    EntityManager.Add(new Bullet(Position + offset, vel, weapon));

                    //offset = Vector2.Transform(new Vector2(35, 8), aimQuat);
                    //EntityManager.Add(new Bullet(Position + offset, vel, weapon));

                    //GameRoot.Shot.Play(0.2f, rand.NextFloat(-0.2f, 0.2f), 0);
                }
            }
            
                if (cooldownRemaining > 0)
                cooldownRemaining--;

            const float speed = 8;
            Velocity = speed * Input.GetMovementDirection();
            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            if (Velocity.LengthSquared() > 0)
                Orientation = Velocity.ToAngle();
        }


        public void WeaponChoice()
        {

            ArrayList list = new ArrayList
            {1};

            if (GameRoot.rocketEnable)
                list.Add(2);
            if (GameRoot.laserEnable)
                list.Add(3);

            if (currentWeapon < list.Count)
                currentWeapon++;
            else
                currentWeapon = 1;           
        }
        public Texture2D weaponBurster()
        {
            currentWeapon = 1;
            damage = 10;
            return GameRoot.Bullet;
        }
        public Texture2D weaponRocket()
        {
            currentWeapon = 2;
            damage = 20;

            return GameRoot.Rocket;
        }
        public Texture2D weaponLaser()
        {
            damage = 30;

            currentWeapon = 3;
            return GameRoot.Laser;

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
            base.Draw(spriteBatch);
        }

        public void Kill()
        {
            framesUntilRespawn = 60;
        }

       
    }
}
