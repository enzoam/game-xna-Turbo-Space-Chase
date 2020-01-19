using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace TurboSpaceChase
{
    public class Obstaculo : God
    {
        private int vida;
	    private int massa = 5;
	    private int poderDano;
        //private Nave owner;
        public Texture2D imagem;
        private String imagemDesc;
        public Vector2 posicao;
        public Vector2 direcao;
        protected Vector2 velocidade;
        private float maxVelocidade=100.0f;
        private float maxForca=100.0f;
        private float angulo = 180.0f;

        public Obstaculo(Game game, int _vida, int _massa, int _poderDano, String _imagemDesc, Vector2 _posicao, SpriteBatch _batch)
            : base(game)
        {
            this.vida = _vida;
            this.massa = _massa;
            this.poderDano = _poderDano;
            this.imagemDesc = _imagemDesc;
            this.posicao = _posicao;
            this.setBatch(_batch);
	    }

        public override void Initialize()
        {
            base.Initialize();

            this.imagem = carregarImagem(imagemDesc);
            setPosition(posicao);
            direcao = new Vector2((float)Math.Cos(0.0f), (float)Math.Sin(0.0f));
        }

        public bool ocorreuColisaoNave(){
            return false;
	    }

        public bool ocorreuColisao(Texture2D imagemObstaculo, Vector2 posicaoObstaculo)
        {
            Rectangle asteroide = new Rectangle((int)getPosition().X, (int)getPosition().Y, (int)imagem.Width, (int)imagem.Height);
            Rectangle obstaculo = new Rectangle((int)posicaoObstaculo.X, (int)posicaoObstaculo.Y, (int)imagemObstaculo.Width, (int)imagemObstaculo.Height);

            if (obstaculo.Intersects(asteroide))
            {
                return true;
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            float secs = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // "Calcula movimento"
            Vector2 steering_force = new BehaviorWander(100.0f, 180.0f, 10.0f).Calculate(this, secs);
            if (steering_force.Length() > maxForca)
            {
                steering_force.Normalize();
                steering_force *= maxForca;
            }
            steering_force *= secs;

            Vector2 acceleration = steering_force / massa;

            velocidade += acceleration;
            if (velocidade.Length() > maxVelocidade)
            {
                velocidade.Normalize();
                velocidade *= maxVelocidade;
            }

            //Calcula a nova direcao
            if (velocidade.Length() < 0.001)
                velocidade = new Vector2();
            else
                direcao = Vector2.Normalize(velocidade);


            // "Circula pela tela"
            int w = Game.Window.ClientBounds.Width;
            int h = Game.Window.ClientBounds.Height;

            int obs_w = imagem.Width;
            int obs_h = imagem.Height;
            Random randon = new Random();

            Vector2 posicaoAux = getPosition();

            if (getPosition().X < -obs_w)
            {
                posicaoAux.X = randon.Next(w);
                posicaoAux.Y = -obs_h + 1;
            }
            else if (getPosition().X > w + obs_w)
            {
                posicaoAux.X = randon.Next(w);
                posicaoAux.Y = -obs_h + 1;
            }

            if (getPosition().Y < -obs_h)
            {
                posicaoAux.Y = (float)randon.Next((int)(h + obs_h - 1));
            }
            else if (getPosition().Y > h + obs_h)
            {
                posicaoAux.Y = -obs_h + 1;
                posicaoAux.X = randon.Next(w);
            }

            float radiano = MathHelper.ToRadians(angulo);
            this.setDirecao(new Vector2((float)Math.Cos(radiano), (float)Math.Sin(radiano)));

            setPosition(posicaoAux + velocidade * secs);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            getBatch().Begin();
            getBatch().Draw(imagem, getPosition(), null, Color.White, (float)Math.Atan2(getDirecao().Y, getDirecao().X),
                    new Vector2(imagem.Width / 2, imagem.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            getBatch().End();
        }
   
        public void descontarDano(int valorDano)
        {
            this.vida -= valorDano;
        }

        public Vector2 getVelocidade()
        {
            return velocidade;
        }
        public float getMaxVelocidade()
        {
            return maxVelocidade;
        }

        public Vector2 getDirecao()
        {
            return direcao;
        }

        public Texture2D getImagem()
        {
            return this.imagem;
        }

        public void setDirecao(Vector2 direcao)
        {
            this.direcao = direcao;
        }

    }
}
