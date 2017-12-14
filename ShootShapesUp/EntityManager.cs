﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    static class EntityManager
    {
        static List<Entity> entities = new List<Entity>();
        static List<Enemy> enemies = new List<Enemy>();
        static List<Bullet> bullets = new List<Bullet>();
        static List<PowerUps> powerUps = new List<PowerUps>();

        static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();
        public static int Count { get { return entities.Count; } }
        public static void Add(Entity entity)
        {
            

            if (!isUpdating)
                AddEntity(entity);
            else
                addedEntities.Add(entity);
        }

        private static void AddEntity(Entity entity)
        {
           
            
            entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);
            else if (entity is Enemy)
                enemies.Add(entity as Enemy);
            else if (entity is PowerUps)
                powerUps.Add(entity as PowerUps);
        }

        public static void Update()
        {
            isUpdating = true;
            HandleCollisions();

            foreach (var entity in entities)
                entity.Update();

            isUpdating = false;

            foreach (var entity in addedEntities)
                AddEntity(entity);

            addedEntities.Clear();

            entities = entities.Where(x => !x.IsExpired).ToList();
            bullets = bullets.Where(x => !x.IsExpired).ToList();
            enemies = enemies.Where(x => !x.IsExpired).ToList();
            powerUps = powerUps.Where(x => !x.IsExpired).ToList();

        }

        static void HandleCollisions()
        {
            // handle collisions between enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = i + 1; j < enemies.Count; j++)
                {
                    if (IsColliding(enemies[i], enemies[j]))
                    {
                        enemies[i].HandleCollision(enemies[j]);
                        enemies[j].HandleCollision(enemies[i]);
                    }
                }
            if (!PowerUps.invis) { 
            // handle collisions between bullets and enemies
            for (int i = 0; i < enemies.Count; i++)
                for (int j = 0; j < bullets.Count; j++)
                {
                    if (IsColliding(enemies[i], bullets[j]))
                    {
                            //NEEDS TO TEST WILL BE USED FOR NUKE
                            //if (enemies[i].PointValue == 1)
                            //{
                            //    enemies.ForEach(x => x.WasShot());
                            //    EnemySpawner.Reset();

                            //}
                            enemies[i].WasShot();
                        bullets[j].IsExpired = true;
                            }
                    
                }
            }
            // handle collisions between the player and enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].IsActive && IsColliding(PlayerShip.Instance, enemies[i]))
                {
                    PlayerShip.Instance.Kill();
                    enemies.ForEach(x => x.WasShot());
                    EnemySpawner.Reset();
                    break;
                }
            }

            //Collison between player and powerup
            for (int i = 0; i < powerUps.Count; i++)
            {
                if (powerUps[i].IsActive && IsColliding(PlayerShip.Instance, powerUps[i]))
                {
                    PlayerShip.Instance.Kill();
                    powerUps.ForEach(x => x.WasShot());
                    EnemySpawner.Reset();
                    break;
                }
            }
        }

        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(a.Position, b.Position) < radius * radius;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
                entity.Draw(spriteBatch);
        }
    }
}
