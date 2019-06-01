# ArcadeShooter-Unity
A 2D arcade shooter game created in Unity engine with a one-week deadline as a tech exam.

**WebGL game link:** https://regaslzr.itch.io/arcade-shooter (free-to-play, come test it out)

# PROJECT ARCHITECTURE:
**Model**
- Classes that implement interfaces specific for base logical operations needed by other classes where direct coupling is discouraged. (I.e. Components in instantiated prefabs that need to have a reference to the current countdown timer.)
- Interface implementations of the Model classes are then extracted from the class instances and then registered into Zenject for dependency injection purposes.

**rView**
- Classes that observe data from injected models and reactively manipulate its own member variables (like base elements) for display.
- No game logic embodied in these kind of classes. They just observe or "get" values from models, maybe format or process them too, and relay them to their member variables.

**rPresenter**
- Classes that both observe and manipulate values from models injected into them. They have the responsibility to guide the models and use the "setter" methods to cause changes to the values, which in turn are reactively acted upon by rViews. 
	
NOTE: the architecture was inspired by MVrP pattern from `neuecc` detailed here: https://github.com/neuecc/UniRx

## Plugins Used:
	- Zenject (for dependency injection)
	- UniRx (for reactive extensions)
	
## Packages Imported:
	- TextMeshPro
	- Cartoon Particle 2D FX

## Other Assets Imported:
	- Free 2D art from various artists and websites. You can check for their links in the project Assets/Art/<Any folder except "ReGaSLZR"> and read the note.txt file for more information.

# Nice-to-have's:
	- [DONE] FX model (for Killed FX, etc)
	- [DONE] Instructions / Tutorial panel
	- Audio: SFX, BGM, Volume Control
	- ScriptableObjects for some classes (e.g. ShopItems)
	- GamePlay UI confirmation dialog (for when player wants to Quit to Main Menu while in GamePlay)
	- Better Loading panel
	- Int values formatting when stringified = 9999 => 9,999 
	- Finish game after Round 15?
	- Have a new set of enemies (tougher, of course) after a particular round.
	- More items to collect, aside from coins. Maybe:
		-5s time
		+1 Rocket ammunition
		Auto-recharge Shield item
		Summon helper
