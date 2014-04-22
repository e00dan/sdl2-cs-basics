using System;
using SDL2;

namespace SDLApp
{
	public sealed class App
	{
		private static App Instance { get { return Nested.instance; } }
		private bool Running = true;

		private IntPtr Window = IntPtr.Zero;
		private IntPtr Renderer = IntPtr.Zero;
		private IntPtr PrimarySurface;

		private const int WindowWidth = 1024;
		private const int WindowHeight = 768;

		public App ()
		{
		}

		private class Nested
		{
			static Nested()
			{
			}

			internal static readonly App instance = new App();
		}

		public int Execute (String[] args)
		{
			if (!Init())
				return 0;

			SDL.SDL_Event Event;
			while (Running) {
				while (SDL.SDL_PollEvent(out Event) != 0) {
					OnEvent (Event);
					if(Event.type == SDL.SDL_EventType.SDL_QUIT) {
						Running = false;
					}
				}
				Loop ();
				Render ();

				SDL.SDL_Delay (1);
			}
			Cleanup ();

			return 1;
		}

		static int Main(String[] args)
		{
			return App.GetInstance ().Execute(args);
		}

		public bool Init ()
		{
			if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
				return false;

			if(SDL.SDL_SetHint(SDL.SDL_HINT_RENDER_SCALE_QUALITY, "1") == SDL.SDL_bool.SDL_FALSE) {
				// some problem
			}

			if ((Window = SDL.SDL_CreateWindow ("Moja nazwa", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, WindowWidth, WindowHeight, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN)) == IntPtr.Zero) {
				return false;
			}

			PrimarySurface = SDL.SDL_GetWindowSurface (Window);

			if((Renderer = SDL.SDL_CreateRenderer(Window, -1, 2)) == IntPtr.Zero) {
				return false;
			}

			SDL.SDL_SetRenderDrawColor (Renderer, 0xae, 0xFF, 0x00, 0xFF);

			return true;
		}

		public void OnEvent (SDL.SDL_Event Event)
		{
		}

		public void Loop ()
		{
		}

		public void Render ()
		{
			SDL.SDL_RenderClear (Renderer);
			SDL.SDL_RenderPresent (Renderer);
		}

		public void Cleanup ()
		{
			if (Renderer != IntPtr.Zero) {
				SDL.SDL_DestroyRenderer (Renderer);
				Renderer = IntPtr.Zero;
			}

			if(Window != IntPtr.Zero) {
				SDL.SDL_DestroyWindow (Window);
				Window = IntPtr.Zero;
			}

			SDL.SDL_Quit();
		}

		public static App GetInstance ()
		{
			return App.Instance;
		}

		public static int GetWindowWidth ()
		{
			return WindowWidth;
		}

		public static int GetWindowHeight ()
		{
			return WindowHeight;
		}
	}
}

