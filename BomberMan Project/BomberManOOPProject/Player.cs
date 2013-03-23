using System;

namespace BomberManOOPProject
{
    abstract class Player : GameObject, IMoveable
    {
        protected static readonly char[,] body = new char[,] { { '☺' } };
        protected MoveHandler moveHandler;

        public Player(int top, int left, MoveHandler moveHandler)
            : base(body, top, left)
        {
            this.moveHandler = moveHandler;
        }

        public event EventHandler OnMove;
        public event EventHandler OnAction;


        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    if (moveHandler.IsValidMove(this.Left, this.Top - 1))
                    {
                        if (this.OnMove != null)
                        {
                            OnMove(this, new CoordinateArgs(this.Left, this.Top));
                        }
                        this.Top--;
                    }
                    break;

                case Direction.Down:
                    if (moveHandler.IsValidMove(this.Left, this.Top + this.Height))
                    {
                        if (this.OnMove != null)
                        {
                            OnMove(this, new CoordinateArgs(this.Left, this.Top));
                        }
                        this.Top++;
                    }
                    break;

                case Direction.Left:
                    if (moveHandler.IsValidMove(this.Left - 1, this.Top))
                    {
                        if (this.OnMove != null)
                        {
                            OnMove(this, new CoordinateArgs(this.Left, this.Top));
                        }
                        this.Left--;
                    }
                    break;

                case Direction.Right:
                    if (moveHandler.IsValidMove(this.Left + this.Width, this.Top))
                    {
                        if (this.OnMove != null)
                        {
                            OnMove(this, new CoordinateArgs(this.Left, this.Top));
                        }
                        this.Left++;
                    }
                    break;
            }

            if (this.OnMove != null)
            {
                OnMove(this, new GameObjectArgs(this));
            }
        }

        protected void OnMoveEvent(CoordinateArgs args)
        {
            EventHandler handler = OnMove;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        protected void OnActionEvent(GameObjectArgs args)
        {
            EventHandler handler = OnAction;
            if (handler != null)
            {
                handler(this, args);
            }
        }
       
    }
}
//test for updates
