using GlobalHook.Core;
using GlobalHook.Core.Keyboard;
using GlobalHook.Core.MessageLoop;
using GlobalHook.Core.Mouse;
using Serilog;
using SimpleInjector;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PanicHandler.Data
{
    /// <summary>
    /// This class handles device-related events
    /// </summary>
    internal class KeyInterceptor
    {
        internal HookEventsBinder _hookEventsBinder;

        private readonly IKeyboardHook _keyboardHook;
        private readonly IMouseHook _mouseHook;
        private readonly ILogger _logger;
        private readonly Container _container;
        private readonly IMessageLoop _loop;

        public KeyInterceptor(ILogger logger, Container container)
        {
            _container = container;
            _logger = logger;

            IHook[] hooks = IHook
                .Load(Environment.CurrentDirectory)
                .Where(x => x.CanBeInstalled && x.CanPreventDefault)
                .ToArray();

            /*mouse possible future*/
            /*_mouseHook = hooks.OfType<IMouseHook>().First();*/
            /*_hookEventsBinder.BindMouseEvents(_mouseHook);*/

            _keyboardHook = hooks.OfType<IKeyboardHook>().First();

            _hookEventsBinder = _container.GetInstance<HookEventsBinder>();

            _hookEventsBinder.BindKeyboardEvents(_keyboardHook);

            _loop = IMessageLoop.Load(Environment.CurrentDirectory)
                    .First(x => x.CanBeRunned);
        }

        public void Start()
        {
            CancellationTokenSource source = new();

            Task.Run(() => _loop.Run(source.Token, _keyboardHook));
        }
    }
}