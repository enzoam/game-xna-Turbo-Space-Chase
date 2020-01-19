using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace TurboSpaceChase
{
    class Explosao : God
    {
        int i=0;
        public bool explodindo;
        public bool off;
        private Texture2D textureExplosion;
        SoundEffect explode;

        public Explosao(Game game)
             : base(game)
        {   
        }

        public void setConfig(Vector2 position)
        {
            setPosition(position);
            off = false;
            textureExplosion = carregarImagem("imagens/exploding"); // CARREGA A ANIMACAO DA EXPLOSAO
            explodindo = false;
        }

        public void explodir()
        {
            explode = Game.Content.Load<SoundEffect>("audios/explode");// GARREGA O SOM DA EXPLOSAO
            explodindo = true;
        }

        public void update()
        {
            i++;
            if (i >= 25)
            {
                explodindo = false;
                off = true;
                explode.Play();
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(textureExplosion, new Vector2(getPosition().X, getPosition().Y), Color.White);
            spriteBatch.End();
        }
    }
}
