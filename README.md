# Forest Supply Run

Forest Supply Run is a small 3D survival game checkpoint
The player starts at a forest camp and has to find missing supplies
This is the first playable version for checkpoint 1

Game concept

The player explores a forest area and collects supplies for the camp
Right now the goal is to collect all 5 supplies and return to camp
Later this can become more of a survival game with more resources hazards and objectives

Controls

WASD move
Arrow keys move
Click and drag mouse move camera
E collect items or finish at camp

Current objective

Collect the water food wood rope and map
After all supplies are collected return to camp and press E

What works in checkpoint 1

Playable 3D forest camp world
Controllable player
Third person camera follow
Five collectible supply items
Camp interaction after collecting everything
UI text for supplies and objective
Interaction prompt when near supplies or camp
Forest ambience and pickup sound
Lighting fog trees rocks path lake and camp area

Files changed

PlayerController.cs moves the player
ThirdPersonCamera.cs follows the player
CheckpointGameManager.cs tracks supplies objective UI and sounds
CollectibleSupply.cs handles item collection
CampGoal.cs handles finishing at camp
ForestAmbience.cs plays background ambience

Assets and resources

No external asset packs used yet
Unity primitives and generated placeholder audio are used for checkpoint 1
