using System;
using System.Collections.Generic;
using System.Linq;

namespace BomberManOOPProject
{
    class Engine
    {
        private IRenderer renderer;
        private IUserInterface userInterface;
        private List<GameObject> renderedObjects;
        private List<GameObject> bombs;
        private HumanPlayer player;
        private List<ComputerPlayer> enemies;
        private int sleepTime = 150;
        private static Engine instance;
        private MoveHandler moveHandler;
        private ExplosionHandler expHandler;
        private List<Explosion> explosions;
        private static List<CoordinateArgs> spawnLocations = new List<CoordinateArgs>() {
            new CoordinateArgs(21, 5),
            new CoordinateArgs(0, Console.WindowWidth - 1),
        };
        

        private static int bombLimit = 3;

        static Engine()
        {
            instance = new Engine();
        }

        private Engine() 
        {
            this.renderedObjects = new List<GameObject>();
            this.bombs = new List<GameObject>();
            this.moveHandler = new MoveHandler(Console.WindowWidth, Console.WindowHeight - 1, renderedObjects);
            this.expHandler = new ExplosionHandler();
            this.expHandler.KilledComputerPlayer += RemoveEnemy;
            this.explosions = new List<Explosion>();
            this.enemies = new List<ComputerPlayer>();
            this.player = new HumanPlayer(0, 0, moveHandler);
            this.player.OnAction += OnActionPressed;
        }

        public static Engine Instance
        {
            get
            {
                return instance;
            }
        }

        public IRenderer Renderer
        {
            get
            {
                return this.renderer;
            }
            set
            {
                this.renderer = value;
                this.player.OnMove += renderer.CleanUp;
                this.player.OnMove += ((ConsoleRenderer)renderer).RenderObject;
            }
        }

        public void OnActionPressed(object sender, EventArgs e) // used to add bombs to the list
        {
            GameObjectArgs args = e as GameObjectArgs;
            if (args != null)
            {
                AddObject(args.GameObject);
            }
        }

        public IUserInterface UserInterface
        {
            get
            {
                return this.userInterface;
            }
            set
            {
                this.userInterface = value;
                this.userInterface.OnKeyPressed += player.HandleInput;
                this.userInterface.OnKeyPressed += AddEnemy;
                this.userInterface.OnKeyPressed += RemoveEnemy;
            }
        }

        public int SleepTime
        {
            get
            {
                return this.sleepTime;
            }
            set
            {
                this.sleepTime = value;
            }
        }

        
        public void AddObject(GameObject obj)
        {
            if (obj is Bomb)
            {
                if (bombs.Count < bombLimit)
                {
                    Bomb bomb = obj as Bomb;
                    bomb.OnExplode += CatchExplosion;
                    this.bombs.Add(bomb);
                    this.renderedObjects.Add(bomb);
                }
            }
            else if (obj is IndestructibleBlock)
            {
                this.renderedObjects.Add(new IndestructibleBrick(obj.Left, obj.Top));
                this.renderedObjects.Add(new IndestructibleBrick(obj.Left+1, obj.Top));
                this.renderedObjects.Add(new IndestructibleBrick(obj.Left + 2, obj.Top));
                this.renderedObjects.Add(new IndestructibleBrick(obj.Left, obj.Top+1));
                this.renderedObjects.Add(new IndestructibleBrick(obj.Left+1, obj.Top+1));
                this.renderedObjects.Add(new IndestructibleBrick(obj.Left + 2, obj.Top + 1));

            }
            else if (obj is DestructibleBlock)
            {
                this.renderedObjects.Add(new DestructibleBrick(obj.Left, obj.Top));
                this.renderedObjects.Add(new DestructibleBrick(obj.Left + 1, obj.Top));
                this.renderedObjects.Add(new DestructibleBrick(obj.Left + 2, obj.Top));
                this.renderedObjects.Add(new DestructibleBrick(obj.Left, obj.Top + 1));
                this.renderedObjects.Add(new DestructibleBrick(obj.Left + 1, obj.Top + 1));
                this.renderedObjects.Add(new DestructibleBrick(obj.Left + 2, obj.Top + 1));
            }
            else
            {
                this.renderedObjects.Add(obj);
            }
        }

        public void AddEnemy(object sender, EventArgs e)
        {
            KeyInfoArgs args = (KeyInfoArgs)e;
            if (args.Key == ConsoleKey.Add)
            {
                Random rand = new Random();
                int index = rand.Next(0, spawnLocations.Count);
                CoordinateArgs coords = spawnLocations[index];
                ComputerPlayer computer = new ComputerPlayer(coords.Top, coords.Left, moveHandler);
                this.player.OnMove += computer.UpdateTargetCoords;
                computer.UpdateTargetCoords(player, new EventArgs());
                computer.OnMove += renderer.CleanUp;
                computer.OnMove += ((ConsoleRenderer)renderer).RenderObject;
                enemies.Add(computer);
                renderedObjects.Add(computer);   
            }
        }

        public void RemoveEnemy(object sender, EventArgs e)
        {
            if (sender is ComputerPlayer)
            {
                ComputerPlayer computer = (ComputerPlayer)sender;
                renderer.CleanUp(computer, new CoordinateArgs(computer.Left, computer.Top));
                renderedObjects.Remove(computer);
                enemies.Remove(computer);
            }
            else
            {
                KeyInfoArgs args = (KeyInfoArgs)e;
                if (args.Key == ConsoleKey.Subtract)
                {
                    if (enemies.Count > 0)
                    {
                        renderer.CleanUp(enemies[0], new CoordinateArgs(enemies[0].Left, enemies[0].Top));
                        renderedObjects.Remove(enemies[0]);
                        enemies.Remove(enemies[0]);
                    }
                }
            }
        }

        public void CatchExplosion(object sender, EventArgs e)
        {
            Bomb bomb = (Bomb)sender;
            Explosion newExp = new Explosion(bomb.Left, bomb.Top);
            newExp.OnUpdate += delegate(object _sender, EventArgs _e)
            {
                expHandler.Collide((Explosion)_sender, renderedObjects);
            };
            newExp.OnUpdate += expHandler.UpdateBlast;
            
            newExp.OnDissappear += delegate(object _sender, EventArgs _e)
            {
                Explosion exp = (Explosion)_sender;
                foreach (CoordinateArgs coords in exp.CleanUpCoords)
                {
                    renderer.CleanUp(this, coords);
                }
            };
            this.explosions.Add(newExp);
        }


        public void Run()
        {
            foreach (GameObject obj in this.renderedObjects)
            {
                renderer.RenderObject(obj);
            }

            foreach (ComputerPlayer enemy in enemies)
            {
                renderer.RenderObject(enemy);
            }
            
            while (true)
            {
                foreach (Bomb bomb in bombs)
                {
                    bomb.Update();
                    if (bomb.IsDestroyed)
                    {
                        renderer.CleanUp(this, new CoordinateArgs(bomb.Left, bomb.Top));
                    }
                    else
                    {
                        renderer.RenderObject(bomb);
                    }
                }

                bombs.RemoveAll(bomb => bomb.IsDestroyed);
                renderedObjects.RemoveAll(obj => obj.IsDestroyed);

                for (int i = 0; i < explosions.Count; i++)
                {
                    explosions[i].Update();
                }
                explosions.RemoveAll(exp => exp.IsDestroyed);
                foreach (ComputerPlayer enemy in enemies)
                {
                    enemy.DecideMove();
                }
                renderer.RenderObject(player);
                
                System.Threading.Thread.Sleep(sleepTime);
                this.userInterface.ProcessInput();
            }
        }
    }
}
