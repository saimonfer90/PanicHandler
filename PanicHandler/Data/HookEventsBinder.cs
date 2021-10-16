using GlobalHook.Core.Keyboard;
using GlobalHook.Core.Mouse;
using PanicHandler.Models;
using Serilog;
using Serilog.Events;
using SimpleInjector;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PanicHandler.Data
{
    internal class HookEventsBinder
    {
        private readonly HookConfig _hookConfig;
        private readonly Container _container;
        private readonly ILogger _logger;
        private readonly LogEventLevel _loggerLevelSet;
        private readonly KeyBindingsCollection _keyBindingsCollection;
        private readonly ConcurrentBag<int> _keyPressed;
        private readonly object _locked = new();

        public HookEventsBinder(ILogger logger, Container container)
        {
            _hookConfig = new HookConfig();
            _container = container;

            _logger = logger;
            _loggerLevelSet = Enum.GetValues(typeof(LogEventLevel))
                .Cast<LogEventLevel>()
                .Where(_logger.IsEnabled)
                .Min();

            _keyBindingsCollection = _container.GetInstance<KeyBindingsCollection>();
            _keyPressed = new();

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);

                    lock (_locked)
                    {
                        _keyPressed.Clear();
                    }
                }
            });
        }

        internal void BindKeyboardEvents(IKeyboardHook keyboardHook)
        {
            if (_hookConfig.IsKeyboardHookEnabled)
            {
                keyboardHook.OnEvent += (_, e)
                    => ManageKeyboardEvent(e);
            }
            else
            {
                keyboardHook.OnEvent -= (_, e)
                    => ManageKeyboardEvent(e);
            }
        }

        internal void BindMouseEvents(IMouseHook mouseHook)
        {
            if (_hookConfig.IsMouseHookEnabled)
            {
                mouseHook.OnEvent += (_, e)
                    => ManageMouseEvent(e);
            }
            else
            {
                mouseHook.OnEvent -= (_, e)
                    => ManageMouseEvent(e);
            }
        }

        /// <summary>
        /// Keyboard event handler: check the keys pressed and do as set by configuration
        /// </summary>
        /// <param name="e"></param>
        private void ManageKeyboardEvent(IKeyboardEventArgs e)
        {
            void logInfo(string message, IEnumerable<int> array)
            {
                _logger.Information(message);

                /*in case of a lot bindings set don't iterate..*/
                if (_loggerLevelSet <= LogEventLevel.Information)
                {
                    var keys = string.Join(" - ", _keyPressed.ToList());

                    _logger.Information(keys);
                }
            }

            if (e.KeyState != KeyState.Down && e.KeyState != KeyState.SysDown)
                return;

            _keyPressed.Add(e.RawKey);

            foreach (var binding in _keyBindingsCollection.KeyBindings)
            {
                if (binding.ShortcutRawKeys.Count != _keyPressed.Count)
                    continue;

                lock (_locked)
                {
                    logInfo($"Binding processed ({binding.Name}):", binding.ShortcutRawKeys);

                    if (binding.ShortcutRawKeys.TrueForAll(k => _keyPressed.Contains(k)))
                    {
                        logInfo("Got it! pressed keys: ", _keyPressed);

                        try
                        {
                            _logger.Information($"Start process {binding.ProcessToExec.FileName} {binding.ProcessToExec.Arguments}");

                            Process p = new()
                            {
                                StartInfo = binding.ProcessToExec
                            };

                            p.Start();

                            _logger.Information($"End call to process");
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Binding {binding.Name}: errore lancio processo {binding.ProcessToExec.FileName}: ");
                            _logger.Error(ex.Message);
                        }
                    }
                    else
                    {
                        logInfo("no match for this binding, pressed keys:", _keyPressed.ToList());
                    }

                    _keyPressed.Clear();
                }
            }
        }

        /// <summary>
        /// Mouse event handler: check the keys pressed and do as set by configuration
        /// </summary>
        /// <param name="e"></param>
        private void ManageMouseEvent(IMouseEventArgs e)
        {
            //if (e.MouseEventType == MouseEventType.Key && e.Key == MouseButtons.Middle)
            //    e.PreventDefault();
        }
    }
}