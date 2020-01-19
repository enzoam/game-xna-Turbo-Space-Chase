using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TurboSpaceChase
{
    class BehaviorFlee
    {

        //private Vector2 target;
        private float panic;

        public BehaviorFlee(float panic = 150.0f)
        {
            this.panic = panic;
        }
     
        public Vector2 Calculate(Arma arma, float secs, Vector2 target)
        {
            if (Vector2.Distance(arma.getPosition(), target) > panic)
            {
                return new Vector2();
            }

            Vector2 desired = arma.getPosition() - target;
            desired = Vector2.Normalize(desired) * arma.getMaxVelocidade();
            return desired - arma.getVelocidade();

        }
    }
}
