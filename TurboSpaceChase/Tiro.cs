using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurboSpaceChase
{
    public class Tiro : Arma
    {
        public Vector2 direcao;
        private float angulo = 180.0f;
        private bool isAlive;
        private Nave nave;
        private Vector2 Momentum;
        private int tipoArma;

        public Tiro(Game game, int forcaDano, Nave _nave, int _tipoArma)
            : base(game)
        {
            this.setForcaDano(forcaDano);
            this.isAlive = true;
            setPosition(_nave.getPosicaoTirosNavetiro());
            this.nave = _nave;
            Momentum = new Vector2((float)Math.Cos(angulo) * 5, (float)Math.Sin(angulo) * 5);
            this.tipoArma = _tipoArma;
        }

        public Tiro(Game game, int forcaDano, Nave _nave, int _tipoArma, Vector2 direcao_2)
            : base(game)
        {
            this.direcao = direcao_2;
            this.setForcaDano(forcaDano);
            this.isAlive = true;
            setPosition(_nave.getPosicaoTirosNavetiro());
            this.nave = _nave;
            Momentum = new Vector2((float)Math.Cos(angulo) * 5, (float)Math.Sin(angulo) * 5);
            this.tipoArma = _tipoArma;
        }

        public override void Initialize()
        {
            base.Initialize();
            if(tipoArma.Equals(2))
                setImagem(carregarImagem("imagens/laser2"));
            else
                setImagem(carregarImagem("imagens/laser"));
            // ARRUMA A DIRECAO DO TIRO
            if (direcao == new Vector2(0,0))
                direcao = nave.getDirecao();
        }

        public override void Update(GameTime gameTime)
        {
            float secs = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //VELOCIDADE DO TIRO
            this.setPosition(this.getPosition() + direcao*10);

            //COLISAO DOS TIROS
            ocorreuColisaoArma();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (isAlive)
            {
                getBatch().Begin();
                getBatch().Draw(getImagem(), getPosition(), null, Color.White, (float)Math.Atan2(getDirecao().Y, getDirecao().X),
                    new Vector2(getImagem().Width / 2, getImagem().Height / 2), 1.0f, SpriteEffects.None, 0.0f);

                getBatch().End();
            }
        }

        // COLISAO DA ARMA COM A PROPRIA NAVE
        public override bool ocorreuColisaoArma()
        {
            Rectangle recTiro = new Rectangle((int)getPosition().X, (int)getPosition().Y,
                                          (int)getImagem().Width, (int)getImagem().Height);

            // REINICIA AS NAVES
            List<Nave> listNaves = recuperaComponentesNave<Nave>();

            foreach (Nave n in listNaves)
            {
                if (nave != n)
                {
                    Rectangle obstaculo = new Rectangle((int)n.getPosition().X - ((int)n.getImagem().Width / 2),
                                                        (int)n.getPosition().Y - ((int)n.getImagem().Height / 2),
                                                        (int)n.getImagem().Width, (int)n.getImagem().Height);
                    if (obstaculo.Intersects(recTiro))
                    {
                        n.descontarDano(getForcaDano());
                        this.Dispose();
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
