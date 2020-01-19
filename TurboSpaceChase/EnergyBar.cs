/* Baseado no fonte de Kleber de Oliveira Andrade
 * Retirado do site PontoV
 * *******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Utils.Componentes.EnergyBar
{
    public class EnergyBar
    {
        #region [ Fields ]

        private Texture2D boxTexture = null;
        private Texture2D energyTexture = null;
        private Vector2 boxPosition;
        public Vector2 energyPosition;
        
        public float MaxEnergy
        {
            get { return this.maxEnergy; }
            set { this.maxEnergy = value; }
        }
        private float maxEnergy = 100.0f;

        public float Energy
        {
            get { return this.energy; }
            set
            {
                this.energy = MathHelper.Clamp(value, 0, maxEnergy);
            }
        }
        private float energy = 100.0f;

        private Color emptyEnergyColor = Color.White;

        private Color fullEnergyColor = Color.White;

        private bool flipEnergyReduce = false;

        private bool recoveryBar = false;

        private float recovery;

        private float recoveryFactor;

        private Color recoveryColor = Color.White;

        #endregion

        #region [ Constructor ]
        public EnergyBar(Texture2D boxTexture, Texture2D energyTexture, Vector2 boxPosition, Vector2 energyPosition)
        {
            this.boxTexture = boxTexture;
            this.energyTexture = energyTexture;
            this.boxPosition = boxPosition;
            this.energyPosition = energyPosition;
        }

        public EnergyBar(Texture2D boxTexture, Texture2D energyTexture, Vector2 boxPosition, Vector2 energyPosition, Color color)
        {
            this.boxTexture = boxTexture;
            this.energyTexture = energyTexture;
            this.boxPosition = boxPosition;
            this.energyPosition = energyPosition;
            this.emptyEnergyColor = color;
            this.fullEnergyColor = color;
        }

        public EnergyBar(Texture2D boxTexture, Texture2D energyTexture, Vector2 boxPosition, Vector2 energyPosition, Color emptyEnergyColor, Color fullEnergyColor)
        {
            this.boxTexture = boxTexture;
            this.energyTexture = energyTexture;
            this.boxPosition = boxPosition;
            this.energyPosition = energyPosition;
            this.emptyEnergyColor = emptyEnergyColor;
            this.fullEnergyColor = fullEnergyColor;
        }

        public EnergyBar(Texture2D boxTexture, Texture2D energyTexture, Vector2 boxPosition, Vector2 energyPosition, Color emptyEnergyColor, Color fullEnergyColor, bool flipEnergyReduce)
        {
            this.boxTexture = boxTexture;
            this.energyTexture = energyTexture;
            this.boxPosition = boxPosition;
            this.energyPosition = energyPosition;
            this.emptyEnergyColor = emptyEnergyColor;
            this.fullEnergyColor = fullEnergyColor;
            this.flipEnergyReduce = flipEnergyReduce;
        }

        public EnergyBar(Texture2D boxTexture, Texture2D energyTexture, Vector2 boxPosition, Vector2 energyPosition, bool recoveryBar, float recoveryFactor)
        {
            this.boxTexture = boxTexture;
            this.energyTexture = energyTexture;
            this.boxPosition = boxPosition;
            this.energyPosition = energyPosition;
            this.recoveryBar = recoveryBar;
            this.recoveryFactor = recoveryFactor;
        }

        public EnergyBar(Texture2D boxTexture, Texture2D energyTexture, Vector2 boxPosition, Vector2 energyPosition, Color emptyEnergyColor, Color fullEnergyColor, bool recoveryBar, float recoveryFactor, float maxEnergy)
        {
            this.boxTexture = boxTexture;
            this.energyTexture = energyTexture;
            this.boxPosition = boxPosition;
            this.energyPosition = energyPosition;
            this.emptyEnergyColor = emptyEnergyColor;
            this.fullEnergyColor = fullEnergyColor;
            this.recoveryBar = recoveryBar;
            this.recoveryFactor = recoveryFactor;
            this.recoveryColor = emptyEnergyColor;
            this.MaxEnergy = maxEnergy;
            this.Energy = maxEnergy;
        }

        public EnergyBar(Texture2D boxTexture, Texture2D energyTexture, Vector2 boxPosition, Vector2 energyPosition, Color emptyEnergyColor, Color fullEnergyColor, bool recoveryBar, float recoveryFactor, bool flipEnergyReduce)
        {
            this.boxTexture = boxTexture;
            this.energyTexture = energyTexture;
            this.boxPosition = boxPosition;
            this.energyPosition = energyPosition;
            this.emptyEnergyColor = emptyEnergyColor;
            this.fullEnergyColor = fullEnergyColor;
            this.recoveryBar = recoveryBar;
            this.recoveryFactor = recoveryFactor;
            this.recoveryColor = emptyEnergyColor;
            this.flipEnergyReduce = flipEnergyReduce;
        }

        #endregion

        #region [ Draw Method ]

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Trata a recuperação de energia
            if (recoveryBar)
            {
                // Controla o tempo para recuperar a energia
                recovery -= recoveryFactor * (float)gameTime.ElapsedGameTime.TotalSeconds;
                recovery = MathHelper.Clamp(recovery, energy, maxEnergy);
                
                spriteBatch.Draw(energyTexture,
                energyPosition,
                new Rectangle(0, 0, (int)(recovery * energyTexture.Width / maxEnergy), (int)energyTexture.Height),
                recoveryColor,
                flipEnergyReduce ? MathHelper.ToRadians(180) : 0.0f,
                flipEnergyReduce ? new Vector2(energyTexture.Width, energyTexture.Height) : Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);
            }

            spriteBatch.Draw(boxTexture,
                boxPosition,
                new Rectangle(0, 0, boxTexture.Width, boxTexture.Height),
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);

            spriteBatch.Draw(energyTexture,
                energyPosition,
                new Rectangle(0, 0, (int)(energy * energyTexture.Width / maxEnergy), (int)energyTexture.Height),
                Color.Lerp(emptyEnergyColor, fullEnergyColor, energy / maxEnergy),
                flipEnergyReduce ? MathHelper.ToRadians(180) : 0.0f,
                flipEnergyReduce ? new Vector2(energyTexture.Width, energyTexture.Height) : Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0.0f);
            spriteBatch.End();
        }

        #endregion

        #region [ Methods ]

        public void FullRecovery()
        {
            energy = maxEnergy;
            recovery = energy;
        }

        public void MaxRecovery()
        {
            recovery = energy;
        }

        public void MoveTo(Vector2 newPosition)
        {
            Vector2 diff = energyPosition - boxPosition;
            boxPosition = newPosition;
            energyPosition = boxPosition + diff;
        }

        #endregion
    }
}
