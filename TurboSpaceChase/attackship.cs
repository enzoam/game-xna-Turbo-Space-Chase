using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace TurboSpaceChase
{
    public class attackshiperShip : Nave
    {
        bool apertouTiroEspecial = false;
        public attackshiperShip(Game game, int jogador)
            : base(game)
        {
            this.setJogador(jogador);
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            posicaoInicialNaves(this.getJogador());
            setImagem(carregarImagem("imagens/attackship"));
            this.CriarNave(1, 2, getPosition(), getImagem(), 600.0f, 450.0f, this.getJogador(), 5); // CONFIGURA VIDAS E OUTRAS CARACteRIStiCAS DA NAVE do jogador 2
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            this.barraEnergia.Draw(gameTime, getBatch());
            getBatch().Begin();
                setPosition(getPosition());
                Texture2D _img = getImagem();
                getBatch().Draw(_img, getPosition(), null, Color.White,
                    (float)Math.Atan2(direcao.Y, direcao.X), new Vector2(_img.Width / 2, _img.Height / 2),
                    1.0f, SpriteEffects.None, 0.0f);
            getBatch().End();

        }

        public override void poderEspecial()
        {
            // Máximo de Misseis que podem ser lançados 100 (diminui 1 a cada tiro)
            List<Nave> naves = recuperaComponentesNave();
            Colisao cl = new Colisao(getGame());
            int totalMisseisDestaNave = cl.recuperaComponentesMisseisTotal();
            if (totalMisseisDestaNave < 100)
            {
                foreach (Nave nave in naves)
                {
                    if (!(nave is attackshiperShip))
                    {
                        PursuitRocket missil = new PursuitRocket(Game, this, getPosicaoTirosNave(), 0.0f, 5.0f, 300.0f, 350.0f, nave);
                        missil.setBatch(getBatch());
                        getGame().Components.Add(missil);
                    }
                }
            }
        }
        public void tiro()
        {
            List<Nave> naves = recuperaComponentesNave();
            foreach (Nave nave in naves)
            {
                if (!(nave is attackshiperShip))
                {
                    Tiro arma1 = new Tiro(Game, 1, nave, 1);
                    getGame().Components.Add(arma1);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Tecla usada para atirar o defletor de tele-guiados
            if (Keyboard.GetState().IsKeyDown(getBotaoEspecial()) && !apertouTiroEspecial)
            {
                poderEspecial();
                apertouTiroEspecial = true;
            }
            if (Keyboard.GetState().IsKeyUp(getBotaoEspecial()))
            {
                apertouTiroEspecial = false;
            }

            //Tratamento do tiro Padrao
            if (Keyboard.GetState().IsKeyDown(getBotaoTiro()) && !apertouTiro)
            {
                acionatiro();
                apertouTiro = true;
            }
            if (Keyboard.GetState().IsKeyUp(getBotaoTiro()))
            {
                apertouTiro = false;
            }
        }
        // Aciona o tiro padrão
        public override void acionatiro()
        {
            Tiro arma1 = new Tiro(Game, 1, this, 1);
            arma1.setBatch(getBatch());
            getGame().Components.Add(arma1);
        }
    }
}
