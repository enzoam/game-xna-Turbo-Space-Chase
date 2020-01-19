using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TurboSpaceChase
{
    public class BehaviorWander
    {
        private Vector2 wanderTarget = new Vector2(1, 1);
        private float distance;
        private float radius;
        private float jitter;
        private Random random = new Random();

        public BehaviorWander(float distance, float radius, float jitter)
        {
            this.distance = distance;
            this.radius = radius;
            this.jitter = jitter;
        }

        private float RandomClamped()
        {
            return (float)((random.NextDouble() * 2) - 1);
        }

        public Vector2 Calculate(Obstaculo obstaculo, float secs)
        {
            float jitterTimeSlice = jitter * secs * 9;
            Vector2 tmp = new Vector2(
                RandomClamped() * jitterTimeSlice,
                RandomClamped() * jitterTimeSlice);

            wanderTarget += tmp;
            wanderTarget = Vector2.Normalize(wanderTarget) * radius;

            Vector2 target = obstaculo.getPosition();
            target += obstaculo.direcao * distance;
            target += wanderTarget;
            return new BehaviorSeek(target).Calculate(obstaculo, secs);
        }


        public Vector2 calculateWanderMissil(PursuitRocket missil, float secs)
        {
            float jitterTimeSlice = jitter * secs * 9;
            Vector2 tmp = new Vector2(
                RandomClamped() * jitterTimeSlice,
                RandomClamped() * jitterTimeSlice);

            wanderTarget += tmp;
            wanderTarget = Vector2.Normalize(wanderTarget) * radius;

            Vector2 target = missil.getPosition();
            target += missil.getDirecao() * distance;
            target += wanderTarget;
            return new BehaviorSeek(target).Calculate(missil, secs);
        }

    }
}
