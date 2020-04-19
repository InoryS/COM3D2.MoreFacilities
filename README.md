# COM3D2.MoreFacilities

UPDATES
=======
# 2020-04-18:

Modification to how data is saved, creates an additional file with the additional facilities and casino dealer data.

Originally the plan was to just keep the relevant data in extra files, but the whole file needs to be saved because dealer data is not deserializing properly during load (but it does deserialize properly during save).

Maximum Facilty Count increased from 50 to 60

# 2020-04-15:

Fix for facility upgrades maid task

# 2020-04-14: 

First Alpha


WARNING
==========================================================
The following COM3D2 Plugin is an alpha build, it is for those brave enough to help test.

My plugin development is limited, by proceeding you agree that you will not hold me, 
or any of the plugin tool developers, or KISS responsible for anything that happens to your PC or game data.

It is always recommended that you make a backup of game files before testing, save frequently, and save to a new save slot in case of any bugs.
(I have had issues with saving/loading breaking, but believe I have resolved them)

REQUIREMENTS
==========================================================
COM3D2 Version 1.45.0
BepinEx Version 5.0.1

INSTALL
==========================================================
Place COM3D2.MoreFacilities.Plugin.dll in your BepinEx\plugins directory before starting your game (eg. C:\KISS\COM3D2\BepinEx\plugins)

USAGE DESCRIPTION
==========================================================
This plugin increases the maximum facility count to 60. 

This works in Facility Manager and Life Mode (GP01). It has not yet been tested Guest Mode yet.

On any page with the Facility Grid (my name for it, not official), use the Up/Down Arrow Keys or the Mouse's Scroll Wheel to browse all facilities. 
All of the facilities you add should be available to Maid Scheduling, as well as triggering end-of-day event unlocks.

[EDIT][2020-04-15]:
Fix to allow scrolling during the Facility Updgrades Maid Task, note that only the mouse wheel works here.

LEGAL
==========================================================


NOTES
==========================================================
Please do not steal or distribute my code/plugin without explicit permission

Please contact Guest4168 on the Discord to report any comments, suggestions, and errors (including in this readme).
Yes I know my readme writing sucks and this probably needs legal stuff/license added.

I am looking for suggestions on how I can make it more prominent that the Plugin is active, in case it ever gets added to an all in one installer. 

