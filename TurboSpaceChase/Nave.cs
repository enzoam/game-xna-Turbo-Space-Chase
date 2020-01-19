using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Utils.Componentes.EnergyBar;

namespace TurboSpaceChase
{
    public abstract class Nave : God
    {
        private Texture2D imagemHidden;
        private antiRocket antiRocket;
        public int vidas;
        private float forcaPropulsao;
        private int angulo = 0;
        public Vector2 direcao;
        private int massa;
        private Vector2 destino;
        private Texture2D imagem;
        private Vector2 velocidade;
        private float MaxSpeed;
        private float MaxForce;
        private Keys botaoAcelera;
        private Keys botaoDireita;
        private Keys botaoEsquerda;
        private Keys botaoTiro;
        private Keys botaoEspecial;
        private int jogador;
        public bool apertouTiro;
        private Colisao colisao;

        public EnergyBar barraEnergia;
        public Vector2 posBarraEnergia;

        public bool isAlive;

        protected Nave(Game game) : base(game)
        {
            this.colisao = new Colisao(game);
        }

        public void CriarNave(
            int _vidas,
            int _massa,
            Vector2 _posicao,
            Texture2D _imagem,
            float _MaxSpeed,
            float _MaxForce,
            int _jogador,
            float _forcaPropulsao)
        {
            this.vidas = _vidas;
            this.massa = _massa;
            setPosition(_posicao);
            this.imagem = _imagem;
            this.MaxSpeed = _MaxSpeed;
            this.MaxForce = _MaxForce;
            this.forcaPropulsao = _forcaPropulsao;

            //Controles de cada jogador
            switch (jogador)
            {
                case 1:
                    botaoAcelera = Keys.Up;
                    botaoDireita = Keys.Right;
                    botaoEsquerda = Keys.Left;
                    botaoTiro = Keys.P;
                    botaoEspecial = Keys.Down;
                    this.posBarraEnergia = new Vector2(10, 420);
                    break;
                case 2:
                    botaoAcelera = Keys.W;
                    botaoDireita = Keys.E;
                    botaoEsquerda = Keys.Q;
                    botaoTiro = Keys.R;
                    botaoEspecial = Keys.T;
                    this.posBarraEnergia = new Vector2(600, 420);
                    break;
            }

            barraEnergia = new EnergyBar(Game.Content.Load<Texture2D>(@"EnergyBar/box"),
                        Game.Content.Load<Texture2D>(@"EnergyBar/energybar"),
                        posBarraEnergia,
                        posBarraEnergia,
                        Color.Red,
                        Color.Yellow,
                        false,
                        1.0f,
                        100);

            this.isAlive = true;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            getBatch().Begin();
            getBatch().DrawString(Game.Content.Load<SpriteFont>("Tela/menufont"), "Vidas: " + this.vidas.ToString(), posBarraEnergia + new Vector2(0, 20), Color.White);

            if (!isAlive)
            {
                if(this.getJogador().Equals(1))
                    getBatch().DrawString(Game.Content.Load<SpriteFont>("Tela/menufont"), "Jogador 1 Derrotado!", new Vector2(10, 10), Color.White);
                else
                    getBatch().DrawString(Game.Content.Load<SpriteFont>("Tela/menufont"), "Jogador 2 Derrotado!", new Vector2(550, 10), Color.White);

                getBatch().DrawString(Game.Content.Load<SpriteFont>("Tela/menufont"), "Pressione ESC para voltar ao Menu Inicial!", new Vector2(180, 200), Color.White);
            }

            getBatch().End();
        }

        public override void Initialize()
        {
            base.Initialize();            
        }

        public Keys getBotaoTiro()
        {
            return this.botaoTiro;
        }

        public Keys getBotaoEspecial()
        {
            return this.botaoEspecial;
        }


        public Texture2D getImagem()
        {
            return this.imagem;
        }

        public void setImagem(Texture2D imagem)
        {
            this.imagem = imagem;
        }

        public void setJogador(int jogador)
        {
            this.jogador = jogador;
        }

        public int getJogador()
        {
            return this.jogador;
        }

        public Texture2D getImagemHidden()
        {
            return this.imagemHidden;
        }

        public void setImagemHidden(Texture2D imagem)
        {
            this.imagemHidden = imagem;
        }

        public void setantiRocket(antiRocket antiRocket)
        {
            this.antiRocket = antiRocket;
        }

        public antiRocket getantiRocket()
        {
            return this.antiRocket;
        }

        public override void Update(GameTime gameTime)
        {
            if (isAlive)
            {
                float secs = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 _posicao;
                _posicao = getPosition();
 
                Vector2 steering_force = CalculateArrive();
                if (steering_force.Length() > MaxForce)
                {
                    steering_force.Normalize();
                    steering_force *= MaxForce;
                }
                steering_force *= secs;

                Vector2 acceleration = steering_force / massa;

                velocidade += acceleration;
                if (velocidade.Length() > MaxSpeed)
                {
                    velocidade.Normalize();
                    velocidade *= MaxSpeed;
                }

                if (velocidade.Length() < 0.001)
                    velocidade = new Vector2();
                else
                    direcao = Vector2.Normalize(velocidade);

                if (Keyboard.GetState().IsKeyDown(botaoDireita))
                    angulo += 2;
                if (Keyboard.GetState().IsKeyDown(botaoEsquerda))
                    angulo -= 2;

                float radiano = MathHelper.ToRadians(angulo);
                direcao = new Vector2((float)Math.Cos(radiano), (float)Math.Sin(radiano));

                _posicao += velocidade * secs;

                int w = 800;
                int h = 600;

                int carw = imagem.Width;
                int carh = imagem.Height;

                if (_posicao.X < -carw) _posicao.X = w + carw - 1;
                else if (_posicao.X > w + carw) _posicao.X = -carw + 1;

                if (_posicao.Y < -carh) _posicao.Y = h + carh - 1;
                else if (_posicao.Y > h + carh) _posicao.Y = -carh + 1;

                setPosition(_posicao);

                if (colisao.ocorreuColisaoNaveMeteoro(this))
                {
                    Console.WriteLine("-10");
                    this.descontarDano(10);
                }
            }
            base.Update(gameTime);
        }

        public Vector2 CalculateArrive()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                destino = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            float decelaration = 0.0f;

            if (Keyboard.GetState().IsKeyDown(botaoAcelera))
            {
                destino += this.direcao * forcaPropulsao;
                decelaration += 2.0f;
            }

            if (decelaration.Equals(0.0f))
                destino = getPosition();

            Vector2 toTarget = destino - getPosition();
            float dist = toTarget.Length();
            if (Math.Equals(dist, 0.0f))
                return new Vector2();
            float speed = dist / decelaration;
            speed = speed < this.MaxSpeed ? speed : this.MaxSpeed;
            Vector2 desired = (toTarget / dist) * speed;
            return desired - this.velocidade;
        }

        public abstract void acionatiro();

        // Implementa o poder especial de cada nave
        public abstract void poderEspecial();

        public Vector2 getDirecao()
        {
            return direcao;
        }

        public Vector2 getVelocidade()
        {
            return velocidade;
        }

        public bool ocorreuColisao(Texture2D imagemObstaculo, Vector2 posicaoObstaculo)
        {
            Rectangle naveAlvo = new Rectangle((int)getPosition().X, (int)getPosition().Y, (int)imagem.Width, (int)imagem.Height);
            Rectangle obstaculo = new Rectangle((int)posicaoObstaculo.X, (int)posicaoObstaculo.Y, (int)imagemObstaculo.Width, (int)imagemObstaculo.Height);

            if (obstaculo.Intersects(naveAlvo))
            {
                return true;
            }
            return false;
        }

         public Vector2 getPosicaoTirosNave()
         {
             float tamanho = (imagem.Width/2) + 10;
             return calculaPosicaoTiros(tamanho);
         }

         private Vector2 calculaPosicaoTiros(float tamanho)
         {
             float x = (float)(Math.Cos(getAnguloEmRadianos()) * tamanho);
             float y = (float)(Math.Sin(getAnguloEmRadianos()) * tamanho);

             Vector2 direcao = new Vector2(x, y);
             return Vector2.Add(getPosition(), direcao);
         }

		 public Vector2 getPosicaoTirosNavetiro()
         {
             float tamanho = imagem.Width/2;
             return calculaPosicaoTiros(tamanho);
         }

         public float getAnguloEmRadianos()
         {
             return MathHelper.ToRadians(angulo);
         }

         protected List<Nave> recuperaComponentesNave()
         {
             return recuperaComponentesNave<Nave>();
         }

         public void descontarDano(int valorDano)
         {
             this.barraEnergia.Energy -= valorDano; //DIMINUI A VIDA DO JOGADOR CONFORME PRECONFIGURADO
             if (this.isAlive)
             {
                 if (this.barraEnergia.Energy <= 0)
                 {
                     this.vidas -= 1;
                     this.barraEnergia.Energy = 100;
                 }
             }
             if (this.vidas.Equals(0))
             {
                 isAlive = false;
             }
         }

         public bool isantiRocketAtivo()
         {
             if (getantiRocket() != null)
             {
                 return getantiRocket().getSituacao();
             }
             return false;
         }

         protected void posicaoInicialNaves(int jogador)
         {
             if (jogador == 1)
             {
                 setPosition(new Vector2(100, 100));
             }
             else
             {
                 setPosition(new Vector2(650, 300));
             }
         }
    }
}
