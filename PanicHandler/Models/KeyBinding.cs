using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PanicHandler.Models
{
    /// <summary>
    /// This class stores the configuration of a shortcut
    /// </summary>
    public class KeyBinding
    {
        public string Name { get; set; }

        private string _shortcut;

        public string Shortcut
        {
            get => _shortcut;
            set
            {
                _shortcut = value;
                Split();
            }
        }

        public ProcessStartInfo ProcessToExec { get; set; }

        public List<string> Args { get; set; }

        public List<int> ShortcutRawKeys { get; }

        public KeyBinding()
        {
            ShortcutRawKeys = new();
        }

        private void Split()
        {
            var normalizedShortcut = _shortcut.ToUpper();

            var splitted = normalizedShortcut.Split('+')
                .Select(s => s.Trim());

            foreach (var element in splitted)
            {
                int key = element switch
                {
                    "D0" => 48,
                    "D1" => 49,
                    "D2" => 50,
                    "D3" => 51,
                    "D4" => 52,
                    "D5" => 53,
                    "D6" => 54,
                    "D7" => 55,
                    "D8" => 56,
                    "D9" => 57,
                    "A" => 65,
                    "B" => 66,
                    "C" => 67,
                    "D" => 68,
                    "E" => 69,
                    "F" => 70,
                    "G" => 71,
                    "H" => 72,
                    "I" => 73,
                    "J" => 74,
                    "K" => 75,
                    "L" => 76,
                    "M" => 77,
                    "N" => 78,
                    "O" => 79,
                    "P" => 80,
                    "Q" => 81,
                    "R" => 82,
                    "S" => 83,
                    "T" => 84,
                    "U" => 85,
                    "V" => 86,
                    "W" => 87,
                    "X" => 88,
                    "Y" => 89,
                    "Z" => 90,
                    "F1" => 112,
                    "F2" => 113,
                    "F3" => 114,
                    "F4" => 115,
                    "F5" => 116,
                    "F6" => 117,
                    "F7" => 118,
                    "F8" => 119,
                    "F9" => 120,
                    "F10" => 121,
                    "F11" => 122,
                    "F12" => 123,
                    "F13" => 124,
                    "F14" => 125,
                    "F15" => 126,
                    "F16" => 127,
                    "F17" => 128,
                    "F18" => 129,
                    "F19" => 130,
                    "F20" => 131,
                    "F21" => 132,
                    "F22" => 133,
                    "F23" => 134,
                    "F24" => 135,
                    "NONE" => 0,
                    "ADD" => 107,
                    "ALT" => 164,
                    "ALTKEY" => 18,
                    "APPS" => 93,
                    "ATTN" => 246,
                    "BACK" => 8,
                    "BROWSERBACK" => 166,
                    "BROWSERFAVORITES" => 171,
                    "BROWSERFORWARD" => 167,
                    "BROWSERHOME" => 172,
                    "BROWSERREFRESH" => 168,
                    "BROWSERSEARCH" => 170,
                    "BROWSERSTOP" => 169,
                    "CANCEL" => 3,
                    "CAPITAL" => 20,
                    "CAPSLOCK" => 20,
                    "CLEAR" => 12,
                    "CONTROL" => 131072,
                    "CONTROLKEY" => 17,
                    "CTRL" => 162,
                    "CRSEL" => 247,
                    "DECIMAL" => 110,
                    "DELETE" => 46,
                    "DIVIDE" => 111,
                    "DOWN" => 40,
                    "END" => 35,
                    "ENTER" => 13,
                    "ERASEEOF" => 249,
                    "ESCAPE" => 27,
                    "EXECUTE" => 43,
                    "EXSEL" => 248,
                    "FINALMODE" => 24,
                    "HANGUELMODE" => 21,
                    "HANGULMODE" => 21,
                    "HANJAMODE" => 25,
                    "HELP" => 47,
                    "HOME" => 36,
                    "IMEACCEPT" => 30,
                    "IMEACEEPT" => 30,
                    "IMECONVERT" => 28,
                    "IMEMODECHANGE" => 31,
                    "IMENONCONVERT" => 29,
                    "INSERT" => 45,
                    "JUNJAMODE" => 23,
                    "KANAMODE" => 21,
                    "KANJIMODE" => 25,
                    "KEYCODE" => 65535,
                    "LAUNCHAPPLICATION1" => 182,
                    "LAUNCHAPPLICATION2" => 183,
                    "LAUNCHMAIL" => 180,
                    "LBUTTON" => 1,
                    "LCONTROLKEY" => 162,
                    "LEFT" => 37,
                    "LINEFEED" => 10,
                    "LMENU" => 164,
                    "LSHIFTKEY" => 160,
                    "LWIN" => 91,
                    "MBUTTON" => 4,
                    "MEDIANEXTTRACK" => 176,
                    "MEDIAPLAYPAUSE" => 179,
                    "MEDIAPREVIOUSTRACK" => 177,
                    "MEDIASTOP" => 178,
                    "MENU" => 18,
                    "MULTIPLY" => 106,
                    "NEXT" => 34,
                    "NONAME" => 252,
                    "NUMLOCK" => 144,
                    "NUMPAD0" => 96,
                    "NUMPAD1" => 97,
                    "NUMPAD2" => 98,
                    "NUMPAD3" => 99,
                    "NUMPAD4" => 100,
                    "NUMPAD5" => 101,
                    "NUMPAD6" => 102,
                    "NUMPAD7" => 103,
                    "NUMPAD8" => 104,
                    "NUMPAD9" => 105,
                    "OEM1" => 186,
                    "OEM102" => 226,
                    "OEM2" => 191,
                    "OEM3" => 192,
                    "OEM4" => 219,
                    "OEM5" => 220,
                    "OEM6" => 221,
                    "OEM7" => 222,
                    "OEM8" => 223,
                    "OEMBACKSLASH" => 226,
                    "OEMCLEAR" => 254,
                    "OEMCLOSEBRACKETS" => 221,
                    "OEMCOMMA" => 188,
                    "OEMMINUS" => 189,
                    "OEMOPENBRACKETS" => 219,
                    "OEMPERIOD" => 190,
                    "OEMPIPE" => 220,
                    "OEMPLUS" => 187,
                    "OEMQUESTION" => 191,
                    "OEMQUOTES" => 222,
                    "OEMSEMICOLON" => 186,
                    "OEMTILDE" => 192,
                    "PA1" => 253,
                    "PACKET" => 231,
                    "PAGEDOWN" => 34,
                    "PAGEUP" => 33,
                    "PAUSE" => 19,
                    "PLAY" => 250,
                    "PRINT" => 42,
                    "PRINTSCREEN" => 44,
                    "PRIOR" => 33,
                    "PROCESSKEY" => 229,
                    "RBUTTON" => 2,
                    "RCONTROLKEY" => 163,
                    "RETURN" => 13,
                    "RIGHT" => 39,
                    "RMENU" => 165,
                    "RSHIFTKEY" => 161,
                    "RWIN" => 92,
                    "SCROLL" => 145,
                    "SELECT" => 41,
                    "SELECTMEDIA" => 181,
                    "SEPARATOR" => 108,
                    "SHIFT" => 65536,
                    "SHIFTKEY" => 16,
                    "SLEEP" => 95,
                    "SNAPSHOT" => 44,
                    "SPACE" => 32,
                    "SUBTRACT" => 109,
                    "TAB" => 9,
                    "UP" => 38,
                    "VOLUMEDOWN" => 174,
                    "VOLUMEMUTE" => 173,
                    "VOLUMEUP" => 175,
                    "XBUTTON1" => 5,
                    "XBUTTON2" => 6,
                    "ZOOM" => 251,
                    _ => 0
                };

                ShortcutRawKeys.Add(key);
            }
        }
    }
}