using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TurboSpaceChase
{
    public class BehaviorPursuit
    {
        private Nave alvo;
        public BehaviorPursuit(Nave alvo)
        {
            this.alvo = alvo;
        }

        public Vector2 Calcularpursuit(Arma arma, float secs)
        {
            Vector2 toTarget = alvo.getPosition() - arma.getPosition();
            float relative = Vector2.Dot(arma.getDirecao(), alvo.getDirecao());

            if (Vector2.Dot(toTarget, arma.getDirecao()) > 0 &&
                relative < -0.95)
            {
                return new BehaviorSeek(alvo.getPosition()).Calculate(arma, secs);
            }

            float lookAheadTime = toTarget.Length() / (arma.getMaxVelocidade() + alvo.getVelocidade().Length());
            return new BehaviorSeek(alvo.getPosition() + alvo.getVelocidade() * lookAheadTime).Calculate(arma, secs);

        }
    }
}
