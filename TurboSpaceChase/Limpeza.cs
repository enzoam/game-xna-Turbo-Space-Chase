using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurboSpaceChase 
{
    class Limpeza : God
    {
        int numMaxObstaculos;
        Obstaculo obstaculo;

        public Limpeza(Game game) : base(game){
            numMaxObstaculos = 5;  //QUANTIDADE MAXIMA DE ASTEROIDS PERMITIDA POR TELA
        }

        protected List<Tiro> recuperaComponentesBalas()
        {

            return recuperaComponentes<Tiro>();
        }

        protected List<Obstaculo> recuperaComponentesObstaculos()
        {

            return recuperaComponentes<Obstaculo>();
        }

        // Atualiza os frames com as balas disparadas
        public override void Update(GameTime gameTime)
        {
            int w = Game.Window.ClientBounds.Width;
            int h = Game.Window.ClientBounds.Height;

            // Verifica a colisao dos tiros com Meteoros
            List<Tiro> balas_disparadas = recuperaComponentesBalas();
            foreach (Tiro bala in balas_disparadas) 
            {
                if (bala.getPosition().X > w || bala.getPosition().Y > h
                    || bala.getPosition().X < 0 || bala.getPosition().Y < 0)
                {
                    bala.Dispose();
                    Console.WriteLine("Limpou bala ");
                }
            }
            base.Update(gameTime);
        }

        // Cria asteroides
        public void gerenciarObstaculos(SpriteBatch _batch)
        {
            int n_obs = recuperaComponentesTotal<Obstaculo>();
            Random r = new Random();
            while (n_obs < numMaxObstaculos)
            {
                n_obs++;
                obstaculo = new Obstaculo(getGame(), 5, 10, 10, "imagens/asteroid", new Vector2(r.Next(0, 780), -50), _batch);
                getGame().Components.Add(obstaculo);
            }
            
        }
    }
}
