<img src="https://github.com/cosparks/le_ballon/blob/62ad85d65ad4fa41c3070c73c4a0109aab303526/docs/LeBallonStart.png" width="800">

Imaginative and whimsical physics-based game where you play as a balloon trying to navigate a series of mazes, which become more and more challenging as the game progresses.

![gameplay](https://github.com/cosparks/le_ballon/blob/b5d7b681ef03d24f770b511dcf9f2ddafee50923/docs/Le_Ballon_Gamepay.gif)

### About

All levels are procedurally generated using cellular automata to create the rooms and pathways of the maze.  Player start and end locations are chosen using a search algorithm which locates the center of the largest rooms in a particular quadrant (which a level designer can choose).  One can also procedurally instantiate from 0-4 enemies (flying tacks) and equally many moving obstacles to different sectors of a level (sectors are split into a 3x3 grid of the level, but this can easily be modified for larger/smaller levels).

<img src="https://github.com/cosparks/le_ballon/blob/43e9df346e549bee1085ef6edda86f557a9e341b/docs/InGame1.png" width="800">

###### Citations:
Map Generation algorithm taken and modified from:
[Sebastian Lague: Procedural Cave Generation](https://github.com/SebLague/Procedural-Cave-Generation)
