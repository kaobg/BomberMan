using System;
using System.Collections.Generic;

namespace BomberManOOPProject
{
    class ComputerPlayer : Player
    {
        private int targetTop;
        private int targetLeft;
        private Direction moveDirection;
        private static List<Direction> allDirections = new List<Direction>() { Direction.Up, 
            Direction.Right, Direction.Down, Direction.Left };
        private bool skipMove;

        public ComputerPlayer(int top, int left, MoveHandler moveHandler)
            : base(top, left, moveHandler) 
        {
            this.Color = ConsoleColor.Magenta;
            this.moveDirection = Direction.Left;
        }

        public void UpdateTargetCoords(object sender, EventArgs e)
        {
            Player target = (Player)sender;
            this.targetTop = target.Top;
            this.targetLeft = target.Left;
        }

        public void DecideMove()
        {
            bool isStuck = true;
            Random rand = new Random();

            if (!skipMove)
            {
                int chance = rand.Next() % 10;
                if (chance < 2)
                {
                    List<Direction> availableDirections = new List<Direction>(allDirections);
                    for (int i = 0; i < availableDirections.Count; i++)
                    {
                        chance = rand.Next() % availableDirections.Count;
                        if (moveHandler.IsValidMove(this, availableDirections[chance]))
                        {
                            moveDirection = availableDirections[chance];
                            isStuck = false;
                        }
                        else
                        {
                            availableDirections.Remove(availableDirections[chance]);
                        }
                    }
                }
                else
                {

                    if (moveHandler.IsValidMove(this, moveDirection))
                    {
                        Move(moveDirection);
                        isStuck = false;
                    }
                    else
                    {
                        List<Direction> availableDirections = new List<Direction>(allDirections);
                        for (int i = 0; i < availableDirections.Count; i++)
                        {
                            chance = rand.Next() % availableDirections.Count;
                            if (moveHandler.IsValidMove(this, availableDirections[chance]))
                            {
                                moveDirection = availableDirections[chance];
                                isStuck = false;
                            }
                            else
                            {
                                availableDirections.Remove(availableDirections[chance]);
                            }
                        }
                    }
                }

                if (!isStuck)
                {
                    Move(moveDirection);
                }
            }
            skipMove = !skipMove;
            
        }
        //public void DecideMove()
        //{
        //    if (this.Top > targetTop)
        //    {
        //        if (moveHandler.IsValidMove(this, Direction.Up))
        //        {
        //            this.Move(Direction.Up);
        //        }
        //        else
        //        {
        //            if (this.Left < targetLeft)
        //            {
        //                this.Move(Direction.Right);
        //            }
        //            else if (this.Left > targetLeft)
        //            {
        //                this.Move(Direction.Left);
        //            }
        //        }
        //    }
        //    else if (this.Top < targetTop)
        //    {
        //        if (moveHandler.IsValidMove(this, Direction.Down))
        //        {
        //            this.Move(Direction.Down);
        //        }
        //        else
        //        {
        //            if (this.Left < targetLeft)
        //            {
        //                this.Move(Direction.Right);
        //            }
        //            else if (this.Left > targetLeft)
        //            {
        //                this.Move(Direction.Left);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (this.Left < targetLeft)
        //        {
        //            this.Move(Direction.Right);
        //        }
        //        else if (this.Left > targetLeft)
        //        {
        //            this.Move(Direction.Left);
        //        }
        //    }
        //}
       
    }
}
