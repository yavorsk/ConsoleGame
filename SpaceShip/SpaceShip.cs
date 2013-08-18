using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpaceShip
{
    class SpaceShip
    {
        const int PlayFieldHeight = 45;
        const int playFieldWidth = 45;

        static int playerPosition = 0;

        static int lives = 4;

        static char[] spaceship = new char[] {'<', 'W', '>'};
        static ConsoleColor spaceshipColor = ConsoleColor.Cyan;

        static char weapon = 'S'; // S - spreadgun, C -chaingun, D - double chaingun

        static char bulletCGSymbol = '^';
        static List<int[]> bulletsCG = new List<int[]>();
        static ConsoleColor bulletCGColor = ConsoleColor.DarkBlue;

        static char bulletDCGSymbol = '^';
        static List<int[]> bulletsDCG = new List<int[]>();
        static ConsoleColor bulletDCGColor = ConsoleColor.DarkRed;

        static char bulletSGSymbol = '.';
        static List<int[]> bulletsSG = new List<int[]>();
        static ConsoleColor bulletSGColor = ConsoleColor.Red;

        static void Main()
        {
            Console.BufferHeight = Console.WindowHeight = PlayFieldHeight;
            Console.BufferWidth = Console.WindowWidth = playFieldWidth+20;

            playerPosition = playFieldWidth / 2;

            while (lives > 0)
            {
                UpdateField();

                Draw();

                if (Console.KeyAvailable)   //commands from the console
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);

                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }

                    if (pressedKey.Key == ConsoleKey.RightArrow)    // move right
                    {
                        if (playerPosition + 1 < playFieldWidth - 1)
                        {
                            playerPosition++;
                        }
                    }

                    if (pressedKey.Key == ConsoleKey.LeftArrow)     // move left
                    {
                        if (playerPosition - 1 > 0)
                        {
                            playerPosition--;
                        }
                    }

                    if (pressedKey.Key == ConsoleKey.Z)     // shoot
                    {
                        switch (weapon)
                        {
                            case 'C': ShootChainGun(); break;
                            case 'D': ShootDoubleChainGun(); break;
                            case 'S': ShootSpreadGun(); break;
                        }

                    }
                }

                Thread.Sleep(100);

                Console.Clear();
            }
        }

        private static void ShootSpreadGun()    // add bullets to spread gun bullets list
        {                                       // на всеки изстрел добавяме по точно три куршума, т.е. bulletsSG.Count % 3 = 0 винаги
            bulletsSG.Add(new int[] { playerPosition - 1, PlayFieldHeight - 2 });
            bulletsSG.Add(new int[] { playerPosition, PlayFieldHeight - 2 });
            bulletsSG.Add(new int[] { playerPosition + 1, PlayFieldHeight - 2 });
        }

        private static void ShootDoubleChainGun()       // add bullets to double chaingun bullets list
        {
            bulletsDCG.Add(new int[] { playerPosition-1, PlayFieldHeight - 2 });
            bulletsDCG.Add(new int[] { playerPosition + 1, PlayFieldHeight - 2 });
        }

        private static void ShootChainGun()     // add bullets to chaingun bullets list
        {
            bulletsCG.Add(new int[] { playerPosition, PlayFieldHeight - 2 });
        }

        private static void UpdateField()   
        {
            UpdateShots();
        }

        private static void UpdateShots()
        {
            for (int i = 0; i < bulletsCG.Count; i++)   // на всяка стъпка преместваме куршума с една позиция нагоре, ако излезе извън екрана го премахваме от листа
            {
                if (bulletsCG[i][1] == 0)
                {
                    bulletsCG.RemoveAt(i);
                    break;
                }
                bulletsCG[i][1]--;
            }

            for (int i = 0; i < bulletsDCG.Count; i++)  // на всяка стъпка преместваме куршума с една позиция нагоре, ако излезе извън екрана го премахваме от листа
            {
                if (bulletsDCG[i][1] == 0)
                {
                    bulletsDCG.RemoveAt(i);
                    break;
                }
                bulletsDCG[i][1]--;
            }

            for (int i = 0; i < bulletsSG.Count; i++)   // тук имаме по три куршума на изстрел - по диагонал наляво и нагоре, диагонал надясно и нагоре и само нагоре
            {                                           // ако куршумът излезе извън екрана просто не го принтираме в DrawBulletsSG() по-долу
                if (bulletsSG.Count /3 > PlayFieldHeight)
                {
                    bulletsSG.RemoveAt(0);
                    bulletsSG.RemoveAt(1);
                    bulletsSG.RemoveAt(2);
                }

                if (i % 3 == 0)     // при true bulletsSG[i] е левият куршум - местим го нагоре и наляво
                {
                    bulletsSG[i][0]--;
                    bulletsSG[i][1]--;
                }

                if (i % 3 == 1)     // при true bulletsSG[i] е средният куршум - местим го само нагоре
                {

                    bulletsSG[i][1]--;
                }

                if (i % 3 == 2)     // при true bulletsSG[i] е средният куршум - местим го само надясно
                {
                    bulletsSG[i][0]++;
                    bulletsSG[i][1]--;
                }
            }
        }

        private static void Draw()
        {
            DrawField();
            DrawPlayer();
            DrawBulletsCG();
            DrawBulletsDCG();
            DrawBulletSG();
        }

        private static void DrawField()
        {
            for (int i = 0; i < PlayFieldHeight; i++)
            {
                PrintCharOnCoordinates(playFieldWidth + 1, i, '|', ConsoleColor.Yellow);
            }
        }

        private static void DrawBulletSG()
        {
            foreach (var bullet in bulletsSG)
            {
                if (bullet[0] >= 0 && bullet[0] <= playFieldWidth-1 && bullet[1] >= 0)
                {
                    PrintCharOnCoordinates(bullet[0], bullet[1], bulletSGSymbol, bulletSGColor);
                }
            }
        }

        private static void DrawBulletsDCG()
        {
            foreach (var bullet in bulletsDCG)
            {
                PrintCharOnCoordinates(bullet[0], bullet[1], bulletDCGSymbol, bulletDCGColor);
            }
        }

        private static void DrawBulletsCG()
        {
            foreach (var bullet in bulletsCG)
            {
                PrintCharOnCoordinates(bullet[0], bullet[1], bulletCGSymbol, bulletCGColor);
            }
        }

        private static void DrawPlayer()
        {
            int[] spaceshipCoordinates = new int[] {playerPosition, PlayFieldHeight-1};

            PrintCharOnCoordinates(spaceshipCoordinates[0], spaceshipCoordinates[1], spaceship[1], spaceshipColor);
            PrintCharOnCoordinates(spaceshipCoordinates[0]-1, spaceshipCoordinates[1], spaceship[0], spaceshipColor);
            PrintCharOnCoordinates(spaceshipCoordinates[0]+1, spaceshipCoordinates[1], spaceship[2], spaceshipColor);
        }

        static void PrintCharOnCoordinates(int coordX, int coordY, char symbol, ConsoleColor color)
        {
            Console.SetCursorPosition(coordX, coordY);
            Console.ForegroundColor = color;
            Console.Write(symbol);
        }
    }
}
