using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
     class Enemy : Entity
    {
        static Random rand = new Random();

        private List<IEnumerator<int>> behaviours = new List<IEnumerator<int>>();
        public int timeUntilStart = 60;
        public bool IsActive { get { return timeUntilStart <= 0; } }
        public int nameIdentifier { get; private set; }
        public int health; 
        public static float enemySpeed = 0.9f;
        public Enemy(Texture2D image, Vector2 position)
        {
            this.image = image;
            Position = position;
            Radius = image.Width / 2f;
            color = Color.Transparent;
            
        }

        public static Enemy CreateEnemy(Vector2 position)
        {
            
            var enemy = new Enemy(GameRoot.Alien, position);
            enemy.AddBehaviour(enemy.FollowPlayer());
            enemy.health =100;
            enemy.nameIdentifier = 1;

            return enemy;
        }
        public static Enemy CreateEnemyRare(Vector2 position)
        {
            
            var enemy = new Enemy(GameRoot.AlienRare, position);
            enemy.AddBehaviour(enemy.FollowPlayer());
            enemy.health = 200;
            enemy.nameIdentifier = 2;
            return enemy;
        }

        public override void Update()
        {
            if (timeUntilStart <= 0)
                ApplyBehaviours();
            else
            {
                timeUntilStart--;
                color = Color.White * (1 - timeUntilStart / 60f);
            }


            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, GameRoot.ScreenSize - Size / 2);

            Velocity *= enemySpeed;
        }

        private void AddBehaviour(IEnumerable<int> behaviour)
        {
            behaviours.Add(behaviour.GetEnumerator());
        }

        private void ApplyBehaviours()
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (!behaviours[i].MoveNext())
                    behaviours.RemoveAt(i--);
            }
        }

        public void HandleCollision(Enemy other)
        {
            var d = Position - other.Position;
            Velocity += 1 * d / (d.LengthSquared() + 1);
        }

        public void WasShot()
        {
            Console.WriteLine("health - "+ health + " point - "+nameIdentifier + " damage - "+ damage);
          
            health -= Bullet.damage;
            if (health < 0) {
                GameRoot.enemyCount -= 1;
                Console.WriteLine("Enemy Dead" );
                if (nameIdentifier == 1) {
                    Score.score += 1000;}
                else if (nameIdentifier == 2)
                    Score.score += 5000;
                IsExpired = true;
            }
           // GameRoot.Explosion.Play(0.5f, rand.NextFloat(-0.2f, 0.2f), 0);

        }

        #region Behaviours
        IEnumerable<int> FollowPlayer(float acceleration = 1f)
        {
            while (true)
            {
                if (!PlayerShip.Instance.IsDead)
                    Velocity += (PlayerShip.Instance.Position - Position) * (acceleration / (PlayerShip.Instance.Position - Position).Length());

                if (Velocity != Vector2.Zero)
                    Orientation = Velocity.ToAngle();

                yield return 0;
            }
        }
        #endregion
        
    }
}
