# Assembly-Definition-Generator
 A makeshift asmdef generator, it iterates through all your folders, those with .cs files but no .asmdef files are marked and asmdef files are created in them, these asmdefs do add the suffixes ".Editor" for Editor folders, taking the Assembly name from the parent, and ".Demo" for folders whose parent contains "Demo, Example or Sample" in its name. It can't make references or set excluded/included platforms as of yet/
 
 ##Usage
Tools/ASMDEF Generator
