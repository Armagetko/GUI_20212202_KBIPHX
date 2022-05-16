﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public enum Directions
    {
        up = 0, left = 1, down = 2, right = 3
    }
    public enum JatekElements
    {
        player, floor, garbage, hpfish, bulletfish,
        ice, ice1, ice2, ice3, ice4, ice5, seal, bullet
    }
    public class JatekLogic : IGameControl, IGameModel
    {
        private Queue<string> levels;
        public JatekElements[,] GameMatrix { get; set; }
        public List<Seal> Seals { get; set; }
        public int BulletfishesOnMap { get; set; }
        public int Lives { get; set; }
        public Penguin Penguin { get; set; }
        public int BulletNumber { get; set; }
        public List<Bullet> Bullets { get; set; }
        public event EventHandler Changed;

        public void SetupMap()
        {

            levels = new Queue<string>();
            Bullets = new List<Bullet>();
            Penguin = new Penguin();
            BulletNumber = 3;
            Lives = 3;
            var lvls = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Levels"),
                "*.lvl");
            foreach (var item in lvls)
            {
                levels.Enqueue(item);
            }
            LoadNext(levels.Dequeue());

        }

        private void LoadNext(string path)
        {
            Seals = new List<Seal>();
            string[] lines = File.ReadAllLines(path);
            GameMatrix = new JatekElements[int.Parse(lines[1]), int.Parse(lines[0])];
            for (int i = 0; i < GameMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < GameMatrix.GetLength(1); j++)
                {
                    GameMatrix[i, j] = ConvertToEnum(lines[i + 2][j]);
                    if (GameMatrix[i, j] == JatekElements.seal)
                        Seals.Add(new Seal(i, j));
                }
            }

        }
        private JatekElements ConvertToEnum(char v)
        {
            switch (v)
            {

                case 'e': return JatekElements.ice;
                case '1': return JatekElements.ice1;
                case '2': return JatekElements.ice2;
                case '3': return JatekElements.ice3;
                case '4': return JatekElements.ice4;
                case '5': return JatekElements.ice5;
                case 'S': return JatekElements.garbage;
                case 'H':
                    return JatekElements.hpfish;
                case 'L':
                    BulletfishesOnMap++;
                    return JatekElements.bulletfish;
                case 'P': return JatekElements.player;
                case 'o':
                    return JatekElements.seal;
                default:
                    return JatekElements.floor;
            }
        }

        public void Move(Directions direction)
        {
            var coords = WhereAmI();
            int i = coords[0];
            int j = coords[1];
            int old_i = i;
            int old_j = j;
            switch (direction)
            {
                case Directions.up:
                    if (i - 1 >= 0)
                    {
                        i--;
                    }
                    break;
                case Directions.down:
                    if (i + 1 < GameMatrix.GetLength(0))
                    {
                        i++;
                    }
                    break;
                case Directions.left:
                    if (j - 1 >= 0)
                    {
                        j--;
                    }
                    break;
                case Directions.right:
                    if (j + 1 < GameMatrix.GetLength(1))
                    {
                        j++;
                    }
                    break;
                default:
                    break;
            }
            if (GameMatrix[i, j] == JatekElements.floor)
            {
                GameMatrix[i, j] = JatekElements.player;
                GameMatrix[old_i, old_j] = JatekElements.floor;
            }
            else if (GameMatrix[i, j] == JatekElements.bulletfish)
            {
                BulletfishesOnMap--;
                BulletNumber++;
                GameMatrix[i, j] = JatekElements.player;
                GameMatrix[old_i, old_j] = JatekElements.floor;
            }
            else if (GameMatrix[i, j] == JatekElements.hpfish)
            {
                Lives++;
                GameMatrix[i, j] = JatekElements.player;
                GameMatrix[old_i, old_j] = JatekElements.floor;
            }
            else if (GameMatrix[i, j] == JatekElements.garbage)
            {
                switch (direction)
                {
                    case Directions.up:
                        if (i - 1 >= 0 && GameMatrix[i - 1, j] == JatekElements.floor)
                        {
                            GameMatrix[i - 1, j] = JatekElements.garbage;
                            GameMatrix[i, j] = JatekElements.player;
                            GameMatrix[old_i, old_j] = JatekElements.floor;
                        }
                        break;
                    case Directions.down:
                        if (i + 1 < GameMatrix.GetLength(0) && GameMatrix[i + 1, j] == JatekElements.floor)
                        {
                            GameMatrix[i + 1, j] = JatekElements.garbage;
                            GameMatrix[i, j] = JatekElements.player;
                            GameMatrix[old_i, old_j] = JatekElements.floor;
                        }
                        break;
                    case Directions.left:
                        if (j - 1 >= 0 && GameMatrix[i, j - 1] == JatekElements.floor)
                        {
                            GameMatrix[i, j - 1] = JatekElements.garbage;
                            GameMatrix[i, j] = JatekElements.player;
                            GameMatrix[old_i, old_j] = JatekElements.floor;
                        }
                        break;
                    case Directions.right:
                        if (j + 1 < GameMatrix.GetLength(1) && GameMatrix[i, j + 1] == JatekElements.floor)
                        {
                            GameMatrix[i, j + 1] = JatekElements.garbage;
                            GameMatrix[i, j] = JatekElements.player;
                            GameMatrix[old_i, old_j] = JatekElements.floor;
                        }
                        break;
                    default:
                        break;
                }
            }
            if (BulletfishesOnMap == 0)
            {
                if (levels.Count > 0)
                {
                    LoadNext(levels.Dequeue());
                }

            }
            Penguin.NewCenter(new Point(i, j));
            Changed?.Invoke(this, null);

        }
        public int[] WhereAmI()
        {
            for (int i = 0; i < GameMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < GameMatrix.GetLength(1); j++)
                {
                    if (GameMatrix[i, j] == JatekElements.player)
                    {
                        return new int[] { i, j };
                    }
                }
            }
            return new int[] { -1, -1 };
        }
        public void MoveGameItems()
        {
            //fókák
            foreach (var item in Seals)
            {
                item.Killed = CheckBullet(item.Position[0], item.Position[1] + item.Distances[item.CurrentDistance]);
                if (item.Killed == false)
                {
                    if (item.CurrentDistance < 7)
                    {
                        GameMatrix[item.Position[0], item.Position[1]] = JatekElements.floor;
                        item.CurrentDistance++;
                        item.Position[1] += item.Distances[item.CurrentDistance];
                        GameMatrix[item.Position[0], item.Position[1]] = JatekElements.seal;
                    }
                    else
                    {
                        GameMatrix[item.Position[0], item.Position[1]] = JatekElements.floor;
                        item.Position[1] += item.Distances[7];
                        item.CurrentDistance = 0;
                        GameMatrix[item.Position[0], item.Position[1]] = JatekElements.seal;
                    }
                    item.Killed = CheckBullet(item.Position[0], item.Position[1] + item.Distances[item.CurrentDistance]);
                }
                if (item.Killed == true)
                {
                    GameMatrix[item.Position[0], item.Position[1]] = JatekElements.bulletfish;
                    BulletfishesOnMap++;
                }
            }
            Seals.RemoveAll(t => t.Killed == true);


            //lövedékek
            foreach (var item in Bullets)
            {
                switch (item.direction)
                {
                    case Directions.up:
                        if (item.Origin[0] - 1 >= 0)
                        {
                            if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.player)
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.floor;
                            item.newOrig(item.Origin[0] - 1, item.Origin[1]);
                            if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.floor)
                                item.CollisionHappened = true;
                            else if (GameMatrix[item.Origin[0], item.Origin[1]] == JatekElements.seal)
                            {
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.bullet;
                                item.CollisionHappened = true;
                            }
                            else
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.bullet;
                        }
                        break;
                    case Directions.left:
                        if (item.Origin[1] - 1 >= 0)
                        {
                            if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.player)
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.floor;
                            item.newOrig(item.Origin[0], item.Origin[1] - 1);
                            if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.floor)
                                item.CollisionHappened = true;
                            else if (GameMatrix[item.Origin[0], item.Origin[1]] == JatekElements.seal)
                            {
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.bullet;
                                item.CollisionHappened = true;
                            }
                            else
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.bullet;
                        }
                        break;
                    case Directions.down:
                        if (item.Origin[0] + 1 < GameMatrix.GetLength(0))
                        {
                            if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.player)
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.floor;
                            item.newOrig(item.Origin[0] + 1, item.Origin[1]);
                            if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.floor)
                                item.CollisionHappened = true;
                            else if (GameMatrix[item.Origin[0], item.Origin[1]] == JatekElements.seal)
                            {
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.bullet;
                                item.CollisionHappened = true;
                            }
                            else
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.bullet;
                        }
                        break;
                    case Directions.right:
                        if (item.Origin[1] + 1 < GameMatrix.GetLength(1))
                        {
                            if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.player)
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.floor;
                            item.newOrig(item.Origin[0], item.Origin[1] + 1);
                            if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.floor)
                                item.CollisionHappened = true;
                            else if (GameMatrix[item.Origin[0], item.Origin[1]] == JatekElements.seal)
                            {
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.bullet;
                                item.CollisionHappened = true;
                            }
                            else
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.bullet;
                        }
                        break;
                    default:
                        break;
                }
            }
            Bullets.RemoveAll(t => t.CollisionHappened == true);

            Changed?.Invoke(this, null);
        }
        public void Shoot()
        {
            if (BulletNumber > 0)
            {
                var orig = WhereAmI();
                Bullets.Add(new Bullet(orig, this.Penguin.direction));
                Changed?.Invoke(this, null);
                BulletNumber--;
            }
        }

        public void Rotate(int uj)
        {
            Penguin.Rotate(uj);
        }
        private bool CheckBullet(int i, int j)
        {
            return GameMatrix[i, j] == JatekElements.bullet;
        }
    }
}
