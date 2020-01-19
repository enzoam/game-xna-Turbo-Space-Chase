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
        public int vidas; //Quantidade de vidas da nave
        private float forcaPropulsao; //Força de propulsão da aceleração da nave
        private int angulo = 0; //Angulo usado para a rotação da nave
        public Vector2 direcao; //Direção da nave de acordo com o angulo escolhido
        private int massa; //Massa da nave
        private Vector2 destino; //Posição destino da nave
        private Texture2D imagem; //Sprite da nave
        private Vector2 velocidade; //Velocidade da nave
        private float MaxSpeed; //Velocidade máxima da nave
        private float MaxForce; //Força máxima da nave
        private Arma armaEspecial;
        private Arma tiro;
        private Keys botaoAcelera; //Tecla para identificar a aceleração
        private Keys botaoDireita; //Tecla para identificar a rotação para a direita
        private Keys botaoEsquerda; //Tecla para identificar a rotação para a esquerda
        private Keys botaoTiro; //Tecla para identificar o tiro basico
        private Keys botaoEspecial; //Tecla para poder especial das naves
        private int jogador; // 1, 2, 3 ou 4...
        public bool apertouTiro;
        private Colisao colisao;

        public EnergyBar barraEnergia;
        public Vector2 posBarraEnergia;

        public bool isAlive;

        protected Nave(Game game) : base(game)
        {
            this.jogador = jogador;
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
                case 1: //Jogador 1
                    botaoAcelera = Keys.Up;
                    botaoDireita = Keys.Right;
                    botaoEsquerda = Keys.Left;
                    botaoTiro = Keys.RightShift;
                    botaoEspecial = Keys.Down;
                    this.posBarraEnergia = new Vector2(10, 420);
                    break;
                case 2: //Jogador 2
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
            getBatch().DrawString(Game.Content.Load<SpriteFont>("Tela/menufont"), "vidas: " + this.vidas.ToString(), posBarraEnergia + new Vector2(0, 20), Color.White);

            if (!isAlive)
            {
                if(this.getJogador().Equals(1))
                    getBatch().DrawString(Game.Content.Load<SpriteFont>("Tela/menufont"), "Player Two WINS!", new Vector2(100, 350), Color.White);
                else
                    getBatch().DrawString(Game.Content.Load<SpriteFont>("Tela/menufont"), "Player One WINS!", new Vector2(100, 350), Color.White);

                getBatch().DrawString(Game.Content.Load<SpriteFont>("Tela/menufont"), "Press Enter to go to menu!", new Vector2(450, 350), Color.White);
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
 
                // Utilizacao do "Arrive"
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

                //Muda a direcao
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

                //Circula pela tela
                int w = 800;
                int h = 480;

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

        // Colisão entre a nave e obstáculo/arma. 

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

         // Retorna a posição e o ponto de ação da saída do tiro

         public Vector2 getPosicaoTirosNave()
         {
             //Descobre onde está o ponto de ação
             float tamanho = (imagem.Width/2) + 10;
             return calculaPosicaoTiros(tamanho);
         }

         private Vector2 calculaPosicaoTiros(float tamanho)
         {
             //calcula o vetor de offset
             float x = (float)(Math.Cos(getAnguloEmRadianos()) * tamanho);//Exemplo PontoV: X = co-seno(ângulo) * tamanho; 
             float y = (float)(Math.Sin(getAnguloEmRadianos()) * tamanho);//Exemplo PontoV: Y = seno(ângulo) * tamanho

             //direção do canhão
             Vector2 direcao = new Vector2(x, y);
             //Soma os vetores à posição da nave para obter a posição real dos canhões.
             return Vector2.Add(getPosition(), direcao);
         }

		public Vector2 getPosicaoTirosNavetiro()
         {
             //Descobre onde está a posição aproximada do nariz da nave
             float tamanho = imagem.Width/2;
             return calculaPosicaoTiros(tamanho);
         }

         // Transforma de ângulo para radianos
          public float getAnguloEmRadianos()
         {
             //converte de graus para radianos.
             return MathHelper.ToRadians(angulo);
         }

        // Recupera os componentes de Game.Components do tipo Nave

         protected List<Nave> recuperaComponentesNave()
         {
             return recuperaComponentesNave<Nave>();
         }

        // Desconta das vidas da nave o valor informado no parâmetro.

         public void descontarDano(int valorDano)
         {
             this.barraEnergia.Energy -= valorDano;
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
