# Assembly Definition Generator

![Unity Version](https://img.shields.io/badge/Unity-2018.2%2B-blue?style=plastic) ![License](https://img.shields.io/github/license/ParkingLotGames/Assembly-Definition-Generator?style=plastic) ![Size](https://img.shields.io/github/repo-size/ParkingLotGames/Assembly-Definition-Generator?style=plastic) ![package.json version (branch)](https://img.shields.io/github/package-json/v/ParkingLotGames/Assembly-Definition-Generator/main?style=plastic) ![Last commit](https://img.shields.io/github/last-commit/ParkingLotGames/Assembly-Definition-Generator?style=plastic)

![package.json dynamic](https://img.shields.io/github/package-json/keywords/ParkingLotGames/Assembly-Definition-Generator?style=plastic)

![Issues](https://img.shields.io/github/issues-raw/ParkingLotGames/Assembly-Definition-Generator?style=plastic) ![Pull requests](https://img.shields.io/github/issues-pr-raw/ParkingLotGames/Assembly-Definition-Generator?style=plastic)

 A makeshift asmdef generator, it iterates through all your folders, those with .cs files but no .asmdef files are marked and asmdef files are created in them, these asmdefs do add the suffixes ".Editor" for Editor folders, taking the Assembly name from the parent, and ".Demo" for folders whose parent contains "Demo, Example or Sample" in its name. It can't make references or set excluded/included platforms as of yet/
 
 ## Usage
Tools/ASMDEF Generator
