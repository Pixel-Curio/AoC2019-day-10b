using System;
using System.IO;
using System.Linq;
using AsteroidList = System.Collections.Generic.List<(int x, int y)>;

namespace Day_10b
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines(@"day10a-input.txt");

            //input = new[]
            //{
            //    ".#....#####...#..",
            //    "##...##.#####..##",
            //    "##...#...#.#####.",
            //    "..#.....#...###..",
            //    "..#.#.....#....##",
            //};

            AsteroidList asteroids = new AsteroidList();

            for (int y = 0; y < input.Length; y++)
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] == '#') asteroids.Add((x, y));
                }

            int destroyCount = 0;

            while (asteroids.Count > 1)
            {
                AsteroidList asteroidsToRemove = new AsteroidList();
                //20,18 //8,3
                int i = asteroids.FindIndex(x => x.x == 20 && x.y == 18);
                var homeBase = asteroids[i];

                for (int t = 0; t < asteroids.Count; t++)
                {
                    if (i == t) continue;
                    bool canSee = true;

                    for (int o = 0; o < asteroids.Count; o++)
                    {
                        if (o == i || o == t) continue;

                        //Exists on the same line.
                        if (IsCollinear((asteroids[i].x, asteroids[i].y),
                            (asteroids[t].x, asteroids[t].y),
                            (asteroids[o].x, asteroids[o].y)))
                        {
                            //Check if o is on the line segment.
                            if ((Math.Min(asteroids[i].x, asteroids[t].x) < asteroids[o].x &&
                                 asteroids[o].x < Math.Max(asteroids[i].x, asteroids[t].x)) ||
                                (Math.Min(asteroids[i].y, asteroids[t].y) < asteroids[o].y &&
                                 asteroids[o].y < Math.Max(asteroids[i].y, asteroids[t].y)))
                            {
                                canSee = false;
                                break;
                            }
                        }
                    }

                    if (canSee) asteroidsToRemove.Add(asteroids[t]);
                }

                foreach (var destroyed in asteroidsToRemove.OrderBy(x => GetAngle(homeBase, x)))
                {
                    Console.WriteLine($"Destroyed {destroyed.x}:{destroyed.y} #:{++destroyCount} angle:{GetAngle(homeBase, destroyed)}");
                    asteroids.Remove(destroyed);
                }
            }
        }

        static double GetAngle((int x, int y) p1, (int x, int y) p2)
        {
            float xDiff = p2.x - p1.x;
            float yDiff = p2.y - p1.y;
            var angle = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI + 90;
            return angle < 0 ? angle + 360 : angle;
        }

        static bool IsCollinear((int x, int y) p1, (int x, int y) p2, (int x, int y) p3)
        {
            int a = p1.x * (p2.y - p3.y) +
                    p2.x * (p3.y - p1.y) +
                    p3.x * (p1.y - p2.y);

            return a == 0;
        }
    }
}
