using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Honeycomb;
using SDL2;

namespace Honeycomb.Sdl
{
    public class Sprite
    {
        internal IntPtr Handle;


    }

    public class SdlSpriteRenderSystem : System
    {
        [Import]
        SdlPlatformSystem platformSystem;

        public Sprite LoadSprite(string path)
        {
            IntPtr handle = SDL_image.IMG_LoadTexture(platformSystem.Renderer, path);

            return new Sprite
            {
                Handle = handle,
            };
        }

        public void PlaceSprite(Sprite sprite)
        {
            SDL.SDL_RenderCopy(platformSystem.Renderer, sprite.Handle, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
