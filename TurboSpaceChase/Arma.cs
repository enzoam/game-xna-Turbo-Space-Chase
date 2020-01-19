using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TurboSpaceChase
{
    public abstract class Arma : God
    {
        private int forcaDano;
        //private float distanciaVidaArma;
        protected Vector2 velocidade;
        private Texture2D imagem;
        private float maxVelocidade;
        private Vector2 direcao;

        public Arma(Game game)
            : base(game)
        {

        }

        public Arma(Game game, Vector2 velocidade, Vector2 posicao, float maxVelocidade)
            : base(game, posicao)
        {
            this.velocidade = velocidade;
            this.maxVelocidade = maxVelocidade;
        }

        public abstract bool ocorreuColisaoArma();



        public void setForcaDano(int forcaDano)
        {
            this.forcaDano = forcaDano;
        }

        public int getForcaDano()
        {
            return forcaDano;
        }

        public Vector2 getVelocidade()
        {
            return velocidade;
        }

        public Texture2D getImagem()
        {
            return this.imagem;
        }

        public void setImagem(Texture2D imagem)
        {
            this.imagem = imagem;
        }

        public float getMaxVelocidade()
        {
            return maxVelocidade;
        }

        public Vector2 getDirecao()
        {
            return direcao;
        }

        public void setDirecao(Vector2 direcao)
        {
            this.direcao = direcao;
        }
    }
}
