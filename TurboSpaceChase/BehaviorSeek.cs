using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TurboSpaceChase
{
    public class BehaviorSeek
    {
        private Vector2 alvo;
        private float panico;

        public BehaviorSeek(Vector2 alvo, float panico = 100.0f)
        {
            this.alvo = alvo;
            this.panico = panico;
        }

        public Vector2 Calculate(Arma arma, float secs)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                alvo = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }

            Vector2 desired = alvo - arma.getPosition();
            desired = Vector2.Normalize(desired) * arma.getMaxVelocidade();
            return desired - arma.getVelocidade();
        }

        public Vector2 Calculate(Obstaculo obstaculo, float secs)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                alvo = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }

            Vector2 desired = alvo - obstaculo.getPosition();
            desired = Vector2.Normalize(desired) * obstaculo.getMaxVelocidade();
            return desired - obstaculo.getVelocidade();
        }
    }
}
