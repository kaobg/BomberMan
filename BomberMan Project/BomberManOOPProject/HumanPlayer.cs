using System;

namespace BomberManOOPProject
{
    class HumanPlayer : Player
    {
        public HumanPlayer(int top, int left, MoveHandler moveHandler)
            : base(top, left, moveHandler)
        {
            this.Color = ConsoleColor.White;
        }

        public void HandleInput(object sender, EventArgs args)
        {
            KeyInfoArgs keyArgs = args as KeyInfoArgs;
            switch (keyArgs.Key)
            {
                case ConsoleKey.UpArrow:
                    Move(Direction.Up);
                    break;
                case ConsoleKey.DownArrow:
                    Move(Direction.Down);
                    break;

                case ConsoleKey.LeftArrow:
                    Move(Direction.Left);
                    break;

                case ConsoleKey.RightArrow:
                    Move(Direction.Right);
                    break;
                case ConsoleKey.Spacebar:
                    OnActionEvent(new GameObjectArgs(new Bomb(this.Left, this.Top)));
                    break;
            }
        }
    }
}
