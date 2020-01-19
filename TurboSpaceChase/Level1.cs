using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurboSpaceChase
{
    class Level1 : GameScreen
    {
        KeyboardState keyboardState;
        Texture2D image;
        Rectangle imageRectangle;
        Vector2[] posicaoNavesSelecionadas;
        Texture2D[] naves;
        bool carregou=false;
        public ConfiguraNaves configuranaves;

        defenseshipShip pShip;
        attackshiperShip sfShip;
        Limpeza limpa;
        Colisao colisao;

        float poscamx = 1.0f;
        float poscamy = 1.0f;
        float rotcam = 1;
        float moverotcam = 1;
        //float movezoomcam = 3;
        float zoomcam = 3;
        int padraozoom = 1;

        public bool acabouJogo;
        
        public Level1(Game game, SpriteBatch spriteBatch, Texture2D image, ConfiguraNaves _configuranaves)
            : base(game, spriteBatch)
        {
            this.image = image;
            imageRectangle = new Rectangle(
            0,
            0,
            Game.Window.ClientBounds.Width,
            Game.Window.ClientBounds.Height);

            posicaoNavesSelecionadas = new Vector2[2];
            posicaoNavesSelecionadas[0] = new Vector2(700,100);
            posicaoNavesSelecionadas[1] = new Vector2(700, 400);

            configuranaves = _configuranaves;

            naves = new Texture2D[2];

            acabouJogo = false;

            Initialize();
        }

        public override void Initialize()
        {

            base.Initialize();
            colisao = new Colisao(this.game);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }
        protected override void UnloadContent()
        {
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            keyboardState = Keyboard.GetState();

            if (configuranaves.navesSelecionadas != null && !carregou)
            {
                limpa = new Limpeza(this.game);
                carregou = true;
                int i = 0;
                foreach (TipoNave.Tipo tp in configuranaves.navesSelecionadas)
                {
                    switch (tp)
                    {
                        case TipoNave.Tipo.defenseship:
                            naves[i] = Game.Content.Load<Texture2D>("imagens/defenseship");
                            pShip = new defenseshipShip(this.game, i+1);
                            pShip.setBatch(spriteBatch);
                            Components.Add(pShip);
                            break;
                        case TipoNave.Tipo.attackship:
                            naves[i] = Game.Content.Load<Texture2D>("imagens/attackship");
                            sfShip = new attackshiperShip(this.game, i+1);
                            sfShip.setBatch(spriteBatch);
                            Components.Add(sfShip);
                            break;
                    }
                    i++;
                }

                // Gerencia os tiros e asteroides
                limpa.gerenciarObstaculos(this.spriteBatch);
            }

            limpa.Update(gameTime);

            // Gerencia o numero de asteroides na tela
            limpa.gerenciarObstaculos(this.spriteBatch);

            colisao.ocorreuColisaoTiroMeteoro();

            foreach (TipoNave.Tipo tp in configuranaves.navesSelecionadas)
            {
                switch(tp)
                {
                    case TipoNave.Tipo.defenseship:
                        if (pShip != null)
                            if (!pShip.isAlive)
                                acabouJogo = true;
                        break;
                    case TipoNave.Tipo.attackship:
                        if (sfShip != null)
                            if (!sfShip.isAlive)
                                acabouJogo = true;
                        break;
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            poscamx = 400.0f;
            poscamy = 300.0f;

            Camera2d cam = new Camera2d();
            cam.Pos = new Vector2(poscamx, poscamy);

            if (zoomcam > 6)
            {
                padraozoom = 2;
            }
            else if (zoomcam < 3)
            {
                padraozoom = 1;
            }

            if (padraozoom == 1) // altera padrao do zoom
            {
                zoomcam = zoomcam + 0.001f;
            }
            else
            {
                zoomcam = zoomcam - 0.001f;
            }

            rotcam++;
            if (rotcam == 5) // Delay
            {
                rotcam = 0;
                moverotcam = moverotcam + 0.001f; // Rotação do Planeta
            }

            cam.Zoom = zoomcam;
            cam.Rotation = moverotcam;

            spriteBatch.Begin(SpriteSortMode.BackToFront,
                                    BlendState.AlphaBlend,
                                    null,
                                    null,
                                    null,
                                    null,
                                    cam.get_transformation(GraphicsDevice));

            spriteBatch.Draw(image, imageRectangle, Color.White);

            spriteBatch.End();
            // Desenha a cena
            base.Draw(gameTime);

            List<Explosao> explosoesList = colisao.recuperaComponentesExplosao();
            foreach (Explosao exp in explosoesList) 
            {
                Console.WriteLine("Explodindooo");

                if (exp.explodindo)
                {
                    exp.update();
                    exp.Draw(gameTime, spriteBatch); // CHAMADA DA FUNÇÃO DE EXPLOSÃO
                }
                else
                {
                    exp.Dispose();
                }
            }
        }

        public void Limpar()
        {
            naves = new Texture2D[2];
            acabouJogo = false;

            keyboardState = new KeyboardState();
            carregou = false;
            limpa = new Limpeza(this.game);
            colisao = new Colisao(this.game);
            acabouJogo = false;
            Components.Remove(pShip);
            Components.Remove(sfShip);
        }

    }
}
