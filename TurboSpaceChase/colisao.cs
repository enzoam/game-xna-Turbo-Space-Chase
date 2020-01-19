using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurboSpaceChase
{
    class Colisao : God
    {
        public Colisao(Game game)
            : base(game)
        {

        }

        protected List<Tiro> recuperaComponentestiro()
        {
            return recuperaComponentes<Tiro>();
        }

        protected List<Obstaculo> recuperaComponentesObstaculos()
        {
            return recuperaComponentes<Obstaculo>();
        }

        public List<PursuitRocket> recuperaComponentesMisseis()
        {
            return recuperaComponentes<PursuitRocket>();
        }

        public List<Explosao> recuperaComponentesExplosao()
        {
            return recuperaComponentes<Explosao>();
        }


        public int recuperaComponentesMisseisTotal()
        {
            return recuperaComponentes<PursuitRocket>().Count();
        }


        public bool ocorreuColisaoNaveMeteoro(Nave nave, bool especial=false)
        {
            Rectangle antiRocketRec = new Rectangle((int)nave.getPosition().X - ((int)nave.getImagem().Width/2),
                                                (int)nave.getPosition().Y - ((int)nave.getImagem().Height/2),
                                                (int)nave.getImagem().Width, (int)nave.getImagem().Height);

            // Trata a colisao da nave com os Meteoros
            List<Obstaculo> Meteoros = recuperaComponentesObstaculos();
            foreach (Obstaculo prime in Meteoros)
            {
                Rectangle obstaculo = new Rectangle((int)prime.getPosition().X - ((int)prime.getImagem().Width/2),
                                                    (int)prime.getPosition().Y - ((int)prime.getImagem().Height/2),
                                                    (int)prime.getImagem().Width, (int)prime.getImagem().Height);
                if (obstaculo.Intersects(antiRocketRec))
                {
                    prime.Dispose();
                    return true;
                }
            }
            return false;
        }

         public void ocorreuColisaoTiroMeteoro()
        {

            List<Tiro> listaArmas = recuperaComponentestiro();

            // colisão dos tiros com os Meteoros
            List<Obstaculo> obstaculos = recuperaComponentesObstaculos();

            foreach (Tiro ap in listaArmas)
            {
                Rectangle apRec = new Rectangle((int)ap.getPosition().X, (int)ap.getPosition().Y,
                                                        (int)ap.getImagem().Width, (int)ap.getImagem().Height);
                foreach (Obstaculo obstac in obstaculos)
                {
                    Rectangle obstaculoRec = new Rectangle((int)obstac.getPosition().X - ((int)obstac.getImagem().Width/2),
                                                           (int)obstac.getPosition().Y - ((int)obstac.getImagem().Height/2),
                                                           (int)obstac.getImagem().Width, (int)obstac.getImagem().Height);
                    if (obstaculoRec.Intersects(apRec))
                    {
                        Explosao exp = new Explosao(getGame());
                        exp.setConfig(new Vector2(obstac.getPosition().X-40, obstac.getPosition().Y-40)); // CONFIGURA A POSICAO DA EXPLOSAO NA COLISAO DO TIRO COM O OBJETO
                        exp.explodir();
                        getGame().Components.Add(exp);
                        Console.WriteLine("Explodaaa");
                        obstac.Dispose();
                        ap.Dispose();
                        obstaculos = recuperaComponentesObstaculos();
                    }
                }
            }
        }
    }
}
