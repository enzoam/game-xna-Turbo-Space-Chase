using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;


namespace TurboSpaceChase
{
    public class God : DrawableGameComponent
    {
        private Vector2 position;
        private SpriteBatch batch;
        private Game game;

        public God(Game game) : base(game)
        {
            this.game = game;
        }

        public God(Game game, Vector2 posicao) : base(game)
        {
            this.position = posicao;
            this.game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public Texture2D carregarImagem(String caminhoImagem)
        {
            return Game.Content.Load<Texture2D>(caminhoImagem);
        }

        /// <summary>
        /// Recupera os componentes de Game.Components do tipo especificado.
        /// </summary>

        public List<T> recuperaComponentes<T>()
        {
            return Game.Components.OfType<T>().ToList<T>();
        }

        public List<Nave> recuperaComponentesNave<Nave>()
        {
            return Game.Components.OfType<Level1>().ToList<Level1>()[0].Components.OfType<Nave>().ToList<Nave>();
        }

        public int recuperaComponentesTotal<T>()
        {
            return Game.Components.OfType<T>().Count();
        }

        public void setBatch(SpriteBatch _batch)
        {
            this.batch = _batch;
        }
        public SpriteBatch getBatch()
        {
            return this.batch;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(Vector2 posicao)
        {
            this.position = posicao;
        }

        public Game getGame()
        {
            return game;
        }
    }
}
