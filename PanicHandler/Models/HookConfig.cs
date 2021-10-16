namespace PanicHandler.Models
{
    /// <summary>
    /// This class stores the configuration of enabling or disabling device-related events
    /// </summary>
    public class HookConfig
    {
        public bool IsKeyboardHookEnabled { get; set; }
        public bool IsMouseHookEnabled { get; set; }

        public HookConfig()
        {
            IsKeyboardHookEnabled = true;
            IsMouseHookEnabled = false;
        }
    }
}