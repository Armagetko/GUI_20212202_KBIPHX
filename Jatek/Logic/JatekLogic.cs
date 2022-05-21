using System;
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
        bullet, player, floor, bulletfish, garbage, hpfish,
        ice, ice1, ice2, ice3, ice4, ice5, seal
    }
    public class JatekLogic : IGameControl, IGameModel
    {
        private Queue<string> levels;
        public JatekElements[,] GameMatrix { get; set; }
        public List<Seal> Seals { get; set; }
        public static Random r;
        public int BulletfishesOnMap { get; set; }
        public int Lives { get; set; }
        public Penguin Penguin { get; set; }
        public int BulletNumber { get; set; }
        public List<Bullet> Bullets { get; set; }
        public event EventHandler Changed;
        public event EventHandler LifeLost;
        public event EventHandler GameOver;
        public event EventHandler GameWon;
        public event EventHandler GamePaused;
        //fóka léphessen halra, ha meghal dobja vissza, de eredetileg nem dob halat
        //ha minden fóka meghalt, jöjjön a kardszárnyú delfin 3 élettel, fóka mozgás, áthalad a falakon, 5 halat dob
        //legyen szép
        private KeyValuePair<string, int> SavedStats { get; set; }
        private int Difficulty;

        public void SetupMap(int diff)
        {
            r = new Random();
            levels = new Queue<string>();
            Bullets = new List<Bullet>();
            Penguin = new Penguin();
            BulletNumber = 0;
            Difficulty = diff;
            Lives = diff;
            var lvls = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), $"Levels{diff}"),
                "*.lvl");
            foreach (var item in lvls)
            {
                levels.Enqueue(item);
            }
            LoadNext(levels.Dequeue());
        }
        public void RestartLevel()
        {
            Lives--;
            BulletNumber = SavedStats.Value;
            Seals.Clear();
            Seals = null;
            BulletfishesOnMap = 0;
            LoadNext(SavedStats.Key);
            Changed?.Invoke(this, null);
        }
        private void LoadNext(string path)
        {
            SavedStats = new KeyValuePair<string, int>(path, BulletNumber);
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
        private char ConvertToChar(JatekElements v)
        {
            switch (v)
            {
                case JatekElements.ice: return 'e';
                case JatekElements.ice1: return '1';
                case JatekElements.ice2: return '2';
                case JatekElements.ice3: return '3';
                case JatekElements.ice4: return '4';
                case JatekElements.ice5: return '5';
                case JatekElements.garbage: return 'S';
                case JatekElements.hpfish: return 'H';
                case JatekElements.bulletfish: return 'L';
                case JatekElements.player: return 'P';
                case JatekElements.seal: return 'o';
                default: return ' ';
            }

        }
        public void LoadGame(string path)
        {
            StreamReader sr = new StreamReader(path);
            r = new Random();
            levels = new Queue<string>();
            Bullets = new List<Bullet>();
            Penguin = new Penguin();
            BulletNumber = int.Parse(sr.ReadLine());
            Lives = int.Parse(sr.ReadLine());
            Difficulty = int.Parse(sr.ReadLine());
            while (!sr.EndOfStream)
            {
                string tmp= Path.Combine(Directory.GetCurrentDirectory(), $"Levels{Difficulty}",sr.ReadLine());
                levels.Enqueue(tmp);
            }
            LoadNext(levels.Dequeue());
        }
        public string SaveGame()
        {
            Queue<string> tmpLevels = levels;

            int k = 0;
            if (!Directory.Exists("Saves"))
                Directory.CreateDirectory("Saves");
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Saves");
            while (File.Exists($@"{path}\save{k}.txt"))
                k++;
            StreamWriter sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(),"Saves", $"save{k}.txt"));
            //C:\Users\bodib\source\repos\Jatek\Jatek\bin\Debug\net5.0-windows\Levels3\LVL00.lvl
            sw.WriteLine(BulletNumber);
            sw.WriteLine(Lives);
            sw.WriteLine(Difficulty);
            sw.WriteLine(SavedStats.Key);

            while (tmpLevels.Count > 0)
                sw.WriteLine(tmpLevels.Dequeue().Split(@"\").Last());
            sw.Close();
            return $"save{k}";
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
            else if (GameMatrix[i, j] == JatekElements.seal)
            {
                if (Lives > 0)
                    LifeLost?.Invoke(this, null);
                else
                    GameOver?.Invoke(this, null);
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
                    LoadNext(levels.Dequeue());
                else
                    GameWon?.Invoke(this, null);
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
        public void MoveSeals()
        {
            //fókák
            foreach (var item in Seals)
            {
                int i = item.Position[0];
                int j = item.Position[1];
                var prevElement = new JatekElements();
                var newPos = SealGetNextPosition(i, j, item.currentDirection, item.KeptSameDirection);
                if (newPos == item.currentDirection)
                    item.KeptSameDirection++;
                else
                    item.KeptSameDirection = 0;
                item.currentDirection = newPos;
                switch (newPos)
                {
                    case Directions.up:
                        GameMatrix[i, j] = JatekElements.floor;
                        prevElement = GameMatrix[i - 1, j];
                        GameMatrix[i - 1, j] = JatekElements.seal;
                        i--;
                        break;
                    case Directions.left:
                        GameMatrix[i, j] = JatekElements.floor;
                        prevElement = GameMatrix[i, j - 1];
                        GameMatrix[i, j - 1] = JatekElements.seal;
                        j--;
                        break;
                    case Directions.down:
                        GameMatrix[i, j] = JatekElements.floor;
                        prevElement = GameMatrix[i + 1, j];
                        GameMatrix[i + 1, j] = JatekElements.seal;
                        i++;
                        break;
                    case Directions.right:
                        GameMatrix[i, j] = JatekElements.floor;
                        prevElement = GameMatrix[i, j + 1];
                        GameMatrix[i, j + 1] = JatekElements.seal;
                        j++;
                        break;
                }
                item.Killed = CheckBullet(item.Position[0], item.Position[1]);
                if (item.Killed == false)
                {
                    item.SealMovedTo(i, j);
                    item.Killed = CheckBullet(item.Position[0], item.Position[1]);
                    if (prevElement == JatekElements.player)
                    {
                        if (Lives > 0)
                            LifeLost?.Invoke(this, null);
                        else
                            GameOver?.Invoke(this, null);
                        break;
                    }
                }

            }
            Seals.RemoveAll(t => t.Killed == true);

            Changed?.Invoke(this, null);
        }
        public void MoveBullets()
        {

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
                        }
                        break;
                    case Directions.left:
                        if (item.Origin[1] - 1 >= 0)
                        {
                            if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.player)
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.floor;
                            item.newOrig(item.Origin[0], item.Origin[1] - 1);
                        }
                        break;
                    case Directions.down:
                        if (item.Origin[0] + 1 < GameMatrix.GetLength(0))
                        {
                            if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.player)
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.floor;
                            item.newOrig(item.Origin[0] + 1, item.Origin[1]);
                        }
                        break;
                    case Directions.right:
                        if (item.Origin[1] + 1 < GameMatrix.GetLength(1))
                        {
                            if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.player)
                                GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.floor;
                            item.newOrig(item.Origin[0], item.Origin[1] + 1);
                        }
                        break;
                }
                if (GameMatrix[item.Origin[0], item.Origin[1]] != JatekElements.floor)
                {
                    item.CollisionHappened = true;
                    if (GameMatrix[item.Origin[0], item.Origin[1]] == JatekElements.seal)
                    {
                        Seals.First(t => t.Position[0] == item.Origin[0] && t.Position[1] == item.Origin[1]).Killed = true;
                        GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.bulletfish;
                        BulletfishesOnMap++;
                    }
                }
                if (item.CollisionHappened == false)
                    GameMatrix[item.Origin[0], item.Origin[1]] = JatekElements.bullet;
            }
            Bullets.RemoveAll(t => t.CollisionHappened == true);
            Seals.RemoveAll(t => t.Killed == true);

            Changed?.Invoke(this, null);
        }
        private Directions SealGetNextPosition(int i, int j, Directions prevDirection, int keptSameDirection)
        {
            List<int> possibleDirections = new List<int>();

            if (i - 1 > 0 && (int)GameMatrix[i - 1, j] <= 2)
                possibleDirections.Add(0);
            if (i + 1 < GameMatrix.GetLength(0) && (int)GameMatrix[i + 1, j] <= 2)
                possibleDirections.Add(2);
            if (j - 1 > 0 && (int)GameMatrix[i, j - 1] <= 2)
                possibleDirections.Add(1);
            if (j + 1 < GameMatrix.GetLength(1) && (int)GameMatrix[i, j + 1] <= 2)
                possibleDirections.Add(3);

            var selectedPos = r.Next(0, 4);
            if (keptSameDirection < 8)
                selectedPos = (int)prevDirection;
            while (!possibleDirections.Contains(selectedPos))
                selectedPos = r.Next(0, 4);
            return (Directions)selectedPos;
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

        public void PauseGame()
        {
            GamePaused?.Invoke(this, null);
        }
    }
}
