using System;

namespace TurboSpaceChase
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// Ponto de entrada da aplicacao
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

