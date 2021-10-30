using System;
using System.Linq;
using GlobalHook.Core;
using GlobalHook.Core.Keyboard;
using GlobalHook.Core.Windows.Keyboard;
using SimpleInjector;

namespace PanicHandler.Data
{
    /// <summary>
    /// This class handles device-related events
    /// </summary>
    internal class KeyInterceptor
    {
        internal HookEventsBinder _hookEventsBinder;

        private readonly IKeyboardHook _keyboardHook;
        private readonly Container _container;

        public KeyInterceptor(Container container)
        {
            _container = container;

            _keyboardHook = new KeyboardHook();
            _keyboardHook.Install(true);

            _hookEventsBinder = _container.GetInstance<HookEventsBinder>();
            _hookEventsBinder.BindKeyboardEvents(_keyboardHook);
        }
    }
}