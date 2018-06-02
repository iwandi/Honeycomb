using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycomb
{
    class Program
    {
        static void Main(string[] args)
        {
            Context ctx = new Context();
            IPlatformSystem platformSystem = ctx.Add<Sdl.SdlPlatformSystem>();
            JobSystem jobSystem = ctx.Add<JobSystem>();
            ctx.Add<Sdl.SdlInputSystem>();
            ctx.Add<Sdl.SdlSwapChainSystem>();
            Sdl.SdlSpriteRenderSystem spriteRenderSyetem = ctx.Add<Sdl.SdlSpriteRenderSystem>();

            ctx.Init();

            Sdl.Sprite player = spriteRenderSyetem.LoadSprite("Content/Player.png");

            while (platformSystem.Run)
            {
                jobSystem.BeginFrame();

                spriteRenderSyetem.PlaceSprite(player);

                jobSystem.CompliteFrame();
            }
        }
    }
}
