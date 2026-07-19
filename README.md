# Forest Supply Run

Forest Supply Run is a small 3D survival game checkpoint
The player starts at a forest camp and has to find missing supplies
This is the checkpoint 2 version

Game concept

The player explores a forest area and collects supplies for the camp
The player has to collect supplies find the radio battery and return to camp
The forest has poison mushrooms a wolf and a timer so the player can lose
The goal is to call for rescue before time runs out

Controls

WASD move
Arrow keys move
Click and drag mouse move camera
E collect items or finish at camp
Space restart after winning or losing

Current objective

Collect the water food wood rope and map
Find the radio battery
Use the camp crate if you need help
Avoid poison mushrooms and the wolf
Return to camp and press E at the radio

Gameplay systems

Supply collection
Radio battery collection
Medkits for healing
Camp crate gives health and time
Camp radio interaction
Health tracking
Timer tracking
Win and lose screens
Restart with space

Challenges

Timer pressure
Poison mushrooms damage the player
Patrol wolf damages the player
The camp radio is locked until supplies and battery are collected

What works in checkpoint 2

Playable 3D forest camp world
Controllable player
Third person camera follow
Five collectible supply items
Radio battery item
Two medkit items
Camp crate item
Camp radio interaction after collecting everything
UI text for supplies health time battery and objective
Interaction prompt when near supplies or camp
Forest ambience pickup sound and finish sound
Lighting fog trees rocks path lake and camp area
Win condition and lose condition

Files changed

PlayerController.cs moves the player
ThirdPersonCamera.cs follows the player
CheckpointGameManager.cs tracks supplies battery health timer UI sounds win and lose
CollectibleSupply.cs handles item collection
BatteryPickup.cs handles battery collection
MedkitPickup.cs handles healing
CampCrate.cs handles the camp crate
DamageHazard.cs handles poison mushroom damage
EnemyPatrol.cs handles the wolf
CampGoal.cs handles the camp radio
ForestAmbience.cs plays background ambience

Assets and resources

No external asset packs used yet
Unity primitives and generated placeholder audio are used for checkpoint 2

Known issues

The art is still simple placeholder art
The wolf is a simple capsule model for now
The audio is placeholder tones and ambience
