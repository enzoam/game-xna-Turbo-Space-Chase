using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace TurboSpaceChase
{
    public class PursuitRocket : Arma
    {
        private const float TEMPO_MAX_VIDA_MISSIL = 15;
        private const int  VALOR_DANO = 5;
        private const int  NAVE_defenseship = 2;

        public static Vector2 posMouse;
        private float tempoInicioRocket;
        private float massa;
        private float maxForca;
        private float angulo = 0.0f;
        private BehaviorPursuit pursuit;
        private BehaviorWander wander;
        private BehaviorFlee flee;
        private Nave Owner;

        private bool missilAlive;
        private Nave naveAlvo;
        private List<God> listaObjetosColididos;
        private int comportamento;

        float orientacao;

        public PursuitRocket(Game game, Nave owner, Vector2 posicao, float orientacao, float massa, float maxVelocidade,
            float maxForca, Nave naveAlvo) : base(game, new Vector2(), posicao, maxVelocidade)
        {
            posMouse = new Vector2();
            this.Owner = owner;
            this.massa = massa;
            this.maxForca = maxForca;
            this.setDirecao(new Vector2((float)Math.Cos(orientacao), (float)Math.Sin(orientacao)));
            this.naveAlvo = naveAlvo;
            this.missilAlive = true;
            listaObjetosColididos = new List<God>();
        }

        public Nave getOwner()
        {
            return this.Owner;
        }

        public override void Initialize()
        {
            base.Initialize();
            orientacao = 0;
            setImagem(carregarImagem("imagens/Rocket"));
            travarAlvo(naveAlvo);

                if (naveAlvo is defenseshipShip)
                 {
                    comportamento = NAVE_defenseship;
                 }
        }

        private bool isMissilAlive(GameTime gameTime)
        {
            if (tempoInicioRocket == 0)
            {
                tempoInicioRocket = (float)gameTime.TotalGameTime.TotalSeconds;
            }

            float tempoVidaRocket = (float)gameTime.TotalGameTime.TotalSeconds - tempoInicioRocket;
            bool colisao = naveAlvo.ocorreuColisao(getImagem(), getPosition());

            if (tempoVidaRocket > TEMPO_MAX_VIDA_MISSIL || colisao)
            {
                listaObjetosColididos.Add(naveAlvo);
            }

            List<Obstaculo> listaObstaculos = recuperaComponentes<Obstaculo>();
            foreach (Obstaculo obstaculo in listaObstaculos)
            {
                if (obstaculo.ocorreuColisao(getImagem(), getPosition()))
                {
                    listaObjetosColididos.Add(obstaculo);
                }
            }
            if (listaObjetosColididos.Count > 0)
            {
                return false;
            }
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float secs = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 posAntes = getPosition();


            // Se o missil se tornar inativo, por colisao ou timeout não são feitos mais os calculos abaixo
            if (isMissilAlive(gameTime))
            {

                Vector2 steering_force = new Vector2();
                bool comportamentoPadrao = true;

                // se for defletor de tele-guiado
                if (naveAlvo is defenseshipShip)
                {
                    defenseshipShip nave = (defenseshipShip)naveAlvo;
                    if (nave.isantiRocketAtivo())
                    {
                        Vector2 pos = naveAlvo.getantiRocket().getPosition();
                        steering_force = flee.Calculate(this, secs, pos);
                        comportamentoPadrao = false;
                    }
                }

                if (comportamentoPadrao)
                {
                    steering_force = pursuit.Calcularpursuit(this, secs);
                }

                if (steering_force.Length() > maxForca)
                {
                    steering_force.Normalize();
                    steering_force *= maxForca;
                }
                steering_force *= secs;

                Vector2 acceleration = steering_force / massa;

                velocidade += acceleration;
                if (velocidade.Length() > getMaxVelocidade())
                {
                    velocidade.Normalize();
                    velocidade *= getMaxVelocidade();
                }

                if (velocidade.Length() < 0.001)
                {
                    velocidade = new Vector2();
                }
                else
                {
                    this.setDirecao(Vector2.Normalize(velocidade));
                }

                float radiano = MathHelper.ToRadians(angulo);
                this.setDirecao(new Vector2((float)Math.Cos(radiano), (float)Math.Sin(radiano)));

                setPosition(getPosition() + velocidade * secs);

                orientacao = (float)Math.Atan2(getPosition().Y - posAntes.Y,
                    getPosition().X - posAntes.X);

                int w = 800;
                int h = 600;

                int missilw = getImagem().Width;
                int missilh = getImagem().Height;

                Vector2 posicaoAux = getPosition();

                if (getPosition().X < -missilw)
                {
                    posicaoAux.X = w + missilw - 1;
                }
                else if (getPosition().X > w + missilw)
                {
                    posicaoAux.X = -missilw + 1;
                }

                if (getPosition().Y < -missilh)
                {
                    posicaoAux.Y = h + missilh - 1;
                }
                else
                    if (getPosition().Y > h + missilh)
                    {
                        posicaoAux.Y = -missilh + 1;
                    }
                setPosition(posicaoAux);
            }
            else
            {
                if (missilAlive)
                {
                   bool colisao = naveAlvo.ocorreuColisao(getImagem(), getPosition());
                   if (colisao)
                   {
                       naveAlvo.descontarDano(VALOR_DANO);
                   }
                }
                missilAlive = false;
            }
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            //Testa se o missil ainda esta ativo
            if (missilAlive)
            {
                getBatch().Begin();
                getBatch().Draw(getImagem(), getPosition(), null, Color.White, orientacao,
                    new Vector2(getImagem().Width / 2, getImagem().Height / 2), 1.0f, SpriteEffects.FlipHorizontally, 0.0f);
                getBatch().End();
            }
        }

        public void travarAlvo(Nave naveAlvo)
        {
            pursuit = new BehaviorPursuit(naveAlvo);
            wander = new BehaviorWander(50, new Random().Next(20, 180), 100);
            flee =  new BehaviorFlee();
        }

        public override bool ocorreuColisaoArma()
        {
            return false;
        }
              
    }
}
