# PanicHandler

Windows utility to bind shortcut to custom executable, just add to configuration in the KeyBindings section your custom shortcuts:

you can get the configuration by clicking on the **program notification icon at startup -> right click -> Show configuration** <br>
or opening the **appsettings.Production.json** files in the app folder.

Now add a block like this with the appropriate customizations:

```json
  "KeyBindings": [
     {
      "Name": "BindingNameYouLike",
      "Shortcut": "CTRL+ALT+END",
      "ProcessToExec": {
      "FileName": "FullNameExeToLaunch",
      "Arguments": "ExampleArg1 ExampleArg2",
      "CreateNoWindow": true,
      "WindowStyle": 1,
      "RedirectStandardOutput": false,
      "UseShellExecute": false
     }
  ]
```

or more than one:

```json
  "KeyBindings": [
     {
      "Name": "BindingNameYouLike1",
      "Shortcut": "CTRL+ALT+END",
      "ProcessToExec": {
      "FileName": "FullNameExeToLaunch",
      "Arguments": "ExampleArg1 ExampleArg2",
      "CreateNoWindow": true,
      "WindowStyle": 1,
      "RedirectStandardOutput": false,
      "UseShellExecute": false
     },
     {
      "Name": "BindingNameYouLike2",
      "Shortcut": "CTRL+ALT+F12",
      "ProcessToExec": {
      "FileName": "FullNameExeToLaunch",
      "Arguments": "ExampleArg1 ExampleArg2",
      "CreateNoWindow": true,
      "WindowStyle": 1,
      "RedirectStandardOutput": false,
      "UseShellExecute": false
     }
  ]
```
You should change at least "Name", "Shortcut", "FileName".

<details>
  <summary>Shortcut parsable keys are:</summary>
  
```csharp
  D0  D1  D2  D3  D4  D5  D6  D7  D8  D9
  
  A  B  C  D  E  F  G  H  I  J  K  L  M  N  O  P  Q  R  S  T  U  V  W  X  Y  Z
  
  F1  F2  F3  F4  F5  F6  F7  F8  F9  F10  F11  F12  F13  F14  F15  F16  F17  F18  F19  F20  F21  F22  F23  F24
  
  NUMPAD0  NUMPAD1  NUMPAD2  NUMPAD3  NUMPAD4  NUMPAD5  NUMPAD6  NUMPAD7  NUMPAD8  NUMPAD9
  
  OEM1  OEM102  OEM2  OEM3  OEM4  OEM5  OEM6  OEM7  OEM8
  
  ALT CTRL DELETE ALTKEY CONTROL CONTROLKEY SHIFT SHIFTKEY HOME END ENTER ESCAPE INSERT LWIN BACK CANCEL SNAPSHOT PRINTSCREEN TAB SPACE
  
  LEFT RIGHT UP DOWN PAGEDOWN PAGEUP
```
</details>
Other reference can be found in **KeyBinding** file.<br><br>


If you want you can make the app bootable with windows:

**program notification icon at startup -> right click -> Start at startup**

remember that the app will be bootable for the current user, but the configuration will be shared. The simplest way to avoid it,
if the machines are shared by multiple users, it is to NOT put the app in "program files" or any other shared folder, but in the user's folder/subfolder.
