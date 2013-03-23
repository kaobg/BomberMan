using System;

namespace BomberManOOPProject
{
    class BomberMain
    {
        static void Main(string[] args)
        {
            Engine.Instance.Renderer = new ConsoleRenderer();
            Engine.Instance.UserInterface = new KeyboardInterface();
            LoadDefaultLevel();

            Engine.Instance.Run();

        }

        static void LoadDefaultLevel()
        {
            Random rand = new Random();
            for (int row = 1; row < Console.WindowHeight; row+=3)
            {
                for (int col = 0; col < Console.WindowWidth - 2; col+=4)
                {
                    int randomInt = rand.Next();
                    randomInt %= 10;
                    if (randomInt < 4)
                    {
                        Engine.Instance.AddObject(new IndestructibleBlock(col, row));
                    }
                    else if (randomInt < 8)
                    {
                        Engine.Instance.AddObject(new DestructibleBlock(col, row));
                    }
                }
            }
        }
    }
}
