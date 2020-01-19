using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurboSpaceChase
{
    // Municao defletora, atrai o tele-guiado para ela 
    public class defenseshipShip : Nave
    {
        private antiRocket _antiRocket;
        public defenseshipShip(Game game, int jogador)
            : base(game)
        {
            this.setJogador(jogador);
            Initialize();
        }
        
        public override void Initialize()
        {
            base.Initialize();
            posicaoInicialNaves(this.getJogador());

            setImagem(carregarImagem("imagens/defenseship"));
            setImagemHidden(carregarImagem("imagens/flare"));
            this.CriarNave(1, 1, getPosition(), getImagem(), 600.0f, 250.0f, this.getJogador(), 5); // NUMERO DE VIDAS E OUTRAS CARACTERISTICAS DA NAVE DO JOGADOR

            // Instancia a arma defletora da nave defensiva
            setantiRocket(new antiRocket(getGame(), this, getPosition(), TipoantiRocket.Tipo.Habilidade, TipoantiRocket.Habilidade.Defesa));
        }

        public override void acionatiro()
        {
            Tiro arma1 = new Tiro(Game, 1, this, 1);
            arma1.setBatch(getBatch());
            getGame().Components.Add(arma1);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.barraEnergia.Draw(gameTime, getBatch());
            getBatch().Begin();
            setPosition(getPosition());
            Texture2D _img = getImagem();
            Rectangle? rect = null;
            getBatch().Draw(_img, getPosition(), rect, Color.White,
                (float)Math.Atan2(direcao.Y, direcao.X),
                new Vector2(_img.Width / 2, _img.Height / 2),
                1.0f,
                SpriteEffects.None,
                0.0f);
            getBatch().End();

            if (_antiRocket != null)
            {
                _antiRocket.Update(gameTime);
                _antiRocket.Draw(gameTime);

                if (!_antiRocket.getSituacao())
                {
                    _antiRocket.Dispose();
                    _antiRocket = null;
                }    
            }   
        }

        public override void poderEspecial()
        {
            if (_antiRocket == null)
            {
                _antiRocket = getantiRocket();
                _antiRocket.setPosition(getPosition());
                _antiRocket.ativarantiRocket();
            }
        }

        public override void Update(GameTime gameTime)
        {

            // Alterar a tecla usada para ativar a arma defletora
            if (Keyboard.GetState().IsKeyDown(getBotaoEspecial()))
            {
                poderEspecial();
            }
            
            //Arma do tiro normal
            if (Keyboard.GetState().IsKeyDown(getBotaoTiro()) && !apertouTiro)
            {
                acionatiro();
                apertouTiro = true;
            }
            if (Keyboard.GetState().IsKeyUp(getBotaoTiro()))
            {
                apertouTiro = false;
            }

            base.Update(gameTime);
        }

    }
}
