using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace TurboSpaceChase
{
    public class antiRocket : God
    {
        //private float vida;
        private Nave owner;
        private Texture2D imagem;
        public BoundingBox boxantiRocket;
        private TimeSpan timeOut;
        public bool antiRocketAtivo;
        TipoantiRocket.Tipo tipoantiRocket;
        TipoantiRocket.Habilidade habilidadeantiRocket;
        private float angle;
        private Vector2 offset;

        private Vector2 direcao_;

        public antiRocket(Game game, Nave owner, Vector2 posicao)
            : base(game)
        {
            this.owner = owner;
            setPosition(posicao);
            offset = new Vector2(0, 50);
            Initialize();
        }

        public antiRocket(Game game, Nave owner, Vector2 posicao,
            TipoantiRocket.Tipo tipoantiRocket, TipoantiRocket.Habilidade habilidadeantiRocket)
            : base(game)
        {
            this.owner = owner;
            setPosition(posicao);
            this.tipoantiRocket = tipoantiRocket;
            this.habilidadeantiRocket = habilidadeantiRocket;
            offset = new Vector2(0, 50);
            Initialize();
        }

        public Texture2D getImagem()
        {
            return this.imagem;
        }

        public Nave getOwner()
        {
            return this.owner;
        }

        public void setImagem(Texture2D imagem)
        {
            this.imagem = imagem;
        }

        public override void Initialize()
        {
            zerarantiRocket();
        }

        private void zerarantiRocket()
        {
            timeOut = new TimeSpan(0, 0, 10);
            antiRocketAtivo = false;
        }

        public bool ocorreuColisao()
        {
            return false;
        }

        public bool getSituacao()
        {
            return this.antiRocketAtivo;
        }

        public void ativarantiRocket()
        {
            zerarantiRocket();
            antiRocketAtivo = true;
            setImagem(owner.getImagemHidden());
            setPosition(owner.getPosition());
            direcao_ = owner.getDirecao();
        }

        public override void Update(GameTime gameTime)
        {
            if (antiRocketAtivo)
            {
                // Destroi arma especial (míssel tele-guiado) depois de um tempo ativo (timeout)
                if (!tipoantiRocket.Equals(TipoantiRocket.Tipo.Fisico))
                {
                    timeOut -= gameTime.ElapsedGameTime;
                    if (timeOut <= TimeSpan.Zero)
                    {
                        antiRocketAtivo = false;
                    }
                }
                // Tipo de arma especial de defesa refletor do tele-guiado
                if (tipoantiRocket.Equals(TipoantiRocket.Tipo.Habilidade))
                {
                    if (habilidadeantiRocket.Equals(TipoantiRocket.Habilidade.Defesa))
                    {
                        setPosition(getPosition() + direcao_);
                    }
                }
                
                else
                {
                    angle += (float)(MathHelper.ToRadians(270) * gameTime.ElapsedGameTime.TotalSeconds);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // Desenha o tele-guiado enquanto esta ativo
            if (antiRocketAtivo)
            {
                if (!tipoantiRocket.Equals(TipoantiRocket.Tipo.Fisico))
                {
                    // Caso ele seja refletor
                    if (habilidadeantiRocket.Equals(TipoantiRocket.Habilidade.Defesa))
                    {
                        owner.getBatch().Begin();
                        Vector2 _direcao = owner.getDirecao();
                        owner.getBatch().Draw(imagem, getPosition(),
                            new Rectangle(0, 0, imagem.Width, imagem.Height), Color.White,
                            (float)Math.Atan2(_direcao.Y, _direcao.X),
                            new Vector2(imagem.Width / 2, imagem.Height / 2),
                            1.0f, SpriteEffects.None, 0.0f);
                        owner.getBatch().End();
                    }
                }
              
            }

        }

        public bool isantiRocketAtivo()
        {
            return antiRocketAtivo;
        }

    }
}
