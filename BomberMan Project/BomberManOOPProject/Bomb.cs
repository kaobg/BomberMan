using System;

namespace BomberManOOPProject
{
    class Bomb : GameObject
    {
        private static readonly char[,] body = new char[,] { { '☼' } };
        private int lifetime;
        public event EventHandler OnExplode;

        public Bomb(int left, int top)
            : base(body, left, top)
        {
            lifetime = 20;
            this.Color = ConsoleColor.Red;
        }

        public int Lifetime
        {
            get
            {
                return this.lifetime;
            }
        }

        public void Update()
        {
            lifetime--;
            if (lifetime == 0)
            {
                Explode();
            }
        }

        public void Explode()
        {
            this.IsDestroyed = true;
            if (OnExplode != null)
            {
                OnExplode(this, new EventArgs());
            }
        }

        
    }
}
