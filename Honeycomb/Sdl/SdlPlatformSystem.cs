using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using Honeycomb;

namespace Honeycomb.Sdl
{
    public class SdlPlatformSystem : System, IPlatformSystem, IJobSource
    {
        [Import]
        JobSystem jobSystem;

        IntPtr window = IntPtr.Zero;
        internal IntPtr Window { get { return window; } }
        IntPtr renderder = IntPtr.Zero;
        internal IntPtr Renderer {  get { return renderder; } }

        public bool Run { get; internal set; } = true;

        public override void Init()
        {
            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
            
            SDL.SDL_CreateWindowAndRenderer(1280, 720, SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI | SDL.SDL_WindowFlags.SDL_WINDOW_MOUSE_CAPTURE | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE, out window, out renderder);

            PlatformUpdate = new PlatformUpdateJob(this);
            PlatformUpdateRegJob = jobSystem.Register(PlatformUpdate);
        }

        public PlatformUpdateJob PlatformUpdate { get; protected set; }
        public RegisterdJob PlatformUpdateRegJob { get; protected set; }

        public void SheduleJobs(JobSystem jobSystem, ScheduleScope scope)
        {
            jobSystem.Schedule(PlatformUpdate, scope);
        }

        public void Shutdown()
        {
            if (window != IntPtr.Zero)
            {
                SDL.SDL_DestroyRenderer(renderder);
                SDL.SDL_DestroyWindow(window);
                renderder = IntPtr.Zero;
                window = IntPtr.Zero;
            }

            SDL.SDL_Quit();
        }

        public class PlatformUpdateJob : IJob
        {
            public RegisterdJob Register { get; set; }
            public RegisterdJob[] Dependencys { get { return new RegisterdJob[0]; } }

            SdlPlatformSystem platformSystem;

            public PlatformUpdateJob(SdlPlatformSystem platformSystem)
            {
                this.platformSystem = platformSystem;
            }

            public void Execute(ScheduledJob[] dependencys)
            {
                SDL.SDL_Event e;
                while(SDL.SDL_PollEvent(out e) != 0)
                {
                    SDL.SDL_RenderPresent(platformSystem.Renderer);
                    if(e.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        platformSystem.Run = false;
                    }
                    SDL.SDL_RenderClear(platformSystem.Renderer);
                }
            }
        }
    }
}
