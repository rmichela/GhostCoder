GHOST CODER
===========
Ghost Coder is a tool for simulated live-coding on Windows. When using Ghost Coder, you can smash the keyboard arbitrarially
and be assured that your code will be properly typed. Ghost Coder works by installing a low-level Windows keyboard hook that,
when activated, replaces all your keystrokes with the next character from a script. Never mess up a coding demo again.

DOWNLOAD
===========
[Download the latest release](https://github.com/rmichela/GhostCoder/releases)

USAGE
===========
Ghost Coder runs as a Windows system tray application. After starting Ghost Coder, a ghost icon will appear in the system tray.

* To toggle Ghost Coder on and off, click the tray icon or press the `Pause` key.
* To add a deck of scripts to Ghost Coder, select the `Edit Decks...` menu item. A folder will open where you can create your deck.
 * Create a new folder for your deck. The folder name will show up in Ghost Coder.
 * Add .txt files to your deck folder, one for each script. These files will be loaded in alphabetic order.
 * Select `Refresh Decks` from the Ghost Coder menu after editing your deck.
* To activate a deck, select it from the Ghost Coder menu, then enable Ghost Coder. When you reach the end of a scrpt, press
`Tab` to advance to the next script.

_Note_: Most IDEs automatically assume indentation for you. You will have to compensate for this in your deck scripts. Ghost Coder
does not play well with Resharper.
