# PathfindingVisualiser

This project is an A* pathfinding visualiser made in Unity.

It features a hexagonal grid with start and end tiles that can be moved around and wall that can be drawn on. When the "Visualise!" button is pressed, an A* pathfinding algorithm finds the shortest path from the start tile to the end tile. The start and end tiles can be moved around, and theres a reset button and a colour key to tell you what each coloured tile represents (there's also a handy tutorial to show off the controls)




## Why Hexagons?
I chose to use a hexagonal grid over a square grid mainly due to the fact that travelling in any direction, each hexagon moved equates to one unit of distance, whereas on square grids, travelling diagonally means moving a distance of √2. A way developers get around this issue is by not allowing any diagonal movement on a square grid, but for video games, it would look unnatural if the character could not move diagonally, so instead, most game developers multiply distance values by 10, so moving diagonally would now be 14 units, and horizontal and vertical movement would be 10. However, while good enough for most cases, over a long distance the error will increase to the point that the path calculate by the A* algorythm, may not be the shortest, using hexagons eliminates these problems.
