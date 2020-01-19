using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TurboSpaceChase
{
    /// <summary>
    /// Programa principal com a logica do jogo
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;
        GameScreen activeScreen;
        MenuInicial menuinicial;
        Level1 level1;
        ConfiguraNaves configuranaves;
        SoundEffect somtiro;
        SoundEffect rocket;
        SoundEffect colisao;
        SoundEffect explode;
        SoundEffect flare;
        Song musica;
        Texture2D comandosp1;
        Texture2D comandosp2;
        int playsong = 0;

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Inicializacoes do programa antes de comecar a execucao do jogo
        /// </summary>
        protected override void Initialize()
        {
           base.Initialize();
        }

        /// <summary>
        /// Carga inicial dos elementos e resources do jogo
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            menuinicial = new MenuInicial(
                this,
                spriteBatch,
                Content.Load<SpriteFont>("Tela/menufont"),
                Content.Load<Texture2D>("Tela/01_menu"));
                comandosp1 = Content.Load<Texture2D>("imagens/posp1");
                comandosp2 = Content.Load<Texture2D>("imagens/posp2");
                somtiro = Content.Load<SoundEffect>("audios/somtiro");
                rocket = Content.Load<SoundEffect>("audios/rocket");
                colisao = Content.Load<SoundEffect>("audios/colisao");
                explode = Content.Load<SoundEffect>("audios/explode");
                flare = Content.Load<SoundEffect>("audios/flare");
                musica = Content.Load<Song>("audios/musica");

                Components.Add(menuinicial);
            menuinicial.Hide();

            configuranaves = new ConfiguraNaves(
                this,
                spriteBatch,
                Content.Load<Texture2D>("Tela/02_telaEscolha"));
            Components.Add(configuranaves);
            configuranaves.Hide();

            level1 = new Level1(
                this,
                spriteBatch,
                Content.Load<Texture2D>("Tela/03_jogo"),
                configuranaves);
            Components.Add(level1);
            level1.Hide();
          
            activeScreen = menuinicial;
            activeScreen.Show();
        }

        /// <summary>
        /// Descarregar o jogo da memoria
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Logica principal do jogo
        /// </summary>
        /// <param name="gameTime">Imagem do tempo de jogo</param>
        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (playsong == 0)
            {
                MediaPlayer.Play(musica);
                playsong = 1;
            }

            if (activeScreen == menuinicial)
            {
                if (CheckKey(Keys.Enter))
                {
                    explode.Play();
                    configuranaves.Limpar();
                    if (menuinicial.SelectedIndex == 0)
                    {
                        activeScreen.Hide();
                        activeScreen = configuranaves;
                        activeScreen.Show();
                    }
                    if (menuinicial.SelectedIndex == 1)
                    {
                        this.Exit();
                    }
                }
                // Efeito sonoro 
                if (CheckKey(Keys.Up))
                    flare.Play();
                if (CheckKey(Keys.Down))
                    flare.Play();
            }
            if (activeScreen == configuranaves)
             {
                if (configuranaves.iniciolistanave > 1)
                {
                    level1.Limpar();
                    level1.configuranaves = this.configuranaves;
                    activeScreen.Hide();
                    activeScreen = level1;
                    activeScreen.Show();
                }
            }
            if (activeScreen == level1)
            {
                if (CheckKey(Keys.Enter) && level1.acabouJogo)
                {  
                    configuranaves.Hide();
                    activeScreen = menuinicial;
                    activeScreen.Show();
                }
                // Efeito sonoro som dos tiros
                if (CheckKey(Keys.RightShift))
                    somtiro.Play();
                if (CheckKey(Keys.R))
                    somtiro.Play();
                if (CheckKey(Keys.T))
                    rocket.Play();
                if (CheckKey(Keys.Down))
                    flare.Play();  
            }

           base.Update(gameTime);
            oldKeyboardState = keyboardState;
        }

        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }

        /// <summary>
        /// Chamado para desenhar os sprites atualizados
        /// </summary>
        /// <param name="gameTime">Imagem do tempo de loop do jogo</param>
        protected override void Draw(GameTime gameTime)
        {
            // Desenha o cenario
            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
