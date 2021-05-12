# Le Ballon #

Imaginative and whimsical physics-based game where you play as a balloon trying to navigate a series of mazes, which become more and more challenging as the game progresses.

![gameplay](https://github.com/cosparks/le_ballon/blob/b5d7b681ef03d24f770b511dcf9f2ddafee50923/docs/Le_Ballon_Gamepay.gif)

### About

All levels are procedurally generated using cellular automata to create the rooms and pathways of the maze, and marching squares to create the wall mesh.  Player start and end locations are chosen using a search algorithm which locates the center of the largest rooms in a particular quadrant (which a level designer can choose).  One can also procedurally instantiate from 0-4 enemies (flying tacks) and equally many moving obstacles to different sectors of a level.

![firstlevel](https://github.com/cosparks/le_ballon/blob/43e9df346e549bee1085ef6edda86f557a9e341b/docs/InGame1.png)
