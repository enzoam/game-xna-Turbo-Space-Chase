using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurboSpaceChase
{
    class ConfiguraNaves : GameScreen
    {
        public TipoNave.Tipo[] navesSelecionadas;
        public int iniciolistanave = 0;

        public ConfiguraNaves(Game game, SpriteBatch spriteBatch, Texture2D image)
            : base(game, spriteBatch) { }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            setlistatiponave(0);
            navesSelecionadas[getlistatiponave()] = TipoNave.Tipo.defenseship;
            setlistatiponave(1);
            navesSelecionadas[getlistatiponave()] = TipoNave.Tipo.attackship;
            setlistatiponave(2);
        }

        public void Limpar()
        {
            setlistatiponave(0);
            this.navesSelecionadas = new TipoNave.Tipo[2];
        }
        public int getlistatiponave()
        {
            return this.iniciolistanave;
        }
        public void setlistatiponave(int valor)
        {
            this.iniciolistanave = valor;
        }
    }
}
