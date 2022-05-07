﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public class JatekLogic : IGameControl, IGameModel
    {
        public enum JatekElements
        {
            player, floor, garbage, hpfish, bulletfish, 
            ice,ice1,ice2,ice3,ice4,ice5
        }
        public enum Directions
        {
            up, down, left, right
        }
        private Queue<string> levels;
        public event EventHandler Changed;
        public JatekElements[,] GameMatrix { get; set; }
        public Penguin Penguin { get; set; }
        public List<Seal> Seals { get; set; }
        public List<Bulletfish> Bulletfishes { get; set; }
        public List<Bullet> Bullets { get; set; }

        public JatekLogic()
        {
            levels = new Queue<string>();
            var lvls = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Levels"), "*.lvl");
            foreach (var item in lvls)
            {
                levels.Enqueue(item);
            }
            LoadNext(levels.Dequeue());
        }

        private void LoadNext(string path)
        {
            string[] lines = File.ReadAllLines(path);
            GameMatrix = new JatekElements[int.Parse(lines[1]), int.Parse(lines[0])];
            for (int i = 0; i < GameMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < GameMatrix.GetLength(1); j++)
                {
                    GameMatrix[i, j] = ConvertToEnum(lines[i + 2][j]);
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
                case 'H': return JatekElements.hpfish;
                case 'L': return JatekElements.bulletfish;
                case 'P': return JatekElements.player;
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

        }
        public  int[] WhereAmI()
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
        public void Shoot()
        {
            throw new NotImplementedException();
        }
        public void Turn(Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}
