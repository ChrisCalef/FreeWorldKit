IMPORTANT: Due to licensing restrictions, we cannot legally redistribute the RAIN binary files that make this project work. Without them, opening this project will cause Unity to CRASH. To download RAIN, go to the following URL:

    http://rivaltheory.com/rain/download/

and grab the current RAIN unity package. Then,

 1) Create a new, temporary blank project in Unity.

 2) Import the RAIN unity package into this project.

 3) Find the Assets/RAIN directory, and copy all of its contents into your FreeWorldKit/Assets/RAIN directory.

 4) Open the FreeWorldKit project in Unity.

 5) delete the temporary project if desired.

If you do not follow these instructions and instead try to open the FreeWorldKit project first, UNITY WILL CRASH, and it will most likely continue crashing every time you try to open it, because it is trying to put you back in the last project you were working on.

The way around this problem is to put the command line argument "-projectPath (some safe working project path)" after Unity.exe in the target of your Unity shortcut.

