using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootShapesUp
{
    class Score : Entity
    {
        public static int score = 0;

        public Score()
        {
          
        }
        public void TotalScore()
        {
            Console.WriteLine("Score: "+ score);
        }
        public static void TakeScore()
        {
            if (score - 100 < 0)
            {
                score = 0;
            }
            else
            {
                score -= 100;
            }
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
