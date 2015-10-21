This application is a game called 'Piracy!'. In it you play as a ship, sailing 
the seas and fighting off enemy ships. The game features wave based combat, in 
which the player must fight off randomly spawned enemy ships, until they are 
all defeated, at which point the game spawns another round, with even more 
ships than before. The aim in wave based combat, is not to defeat all enemies, 
as that's impossible as more will just spawn, but to fight off as many as you 
can before you are defeated, racking up as high a score as possible in the 
process. To control the ship, tilt the computer in the direction you want the 
ship to go, and the ship will start acceleration in that direction. To fire 
your cannons, tap the screen on the side of the ship in which you want to fire. 
There are two types of enemy ships that you need to avoid, demolition ships, 
and galleons. Demo ships will try and take you out by ramming into your ship, 
dealing damage and destroying themselves. Galleons will try to come aside and 
will repeatedly fire their weapons at you. Take them down before they take you 
down, but avoid crashing into them as this will hurt you too. Also avoid 
collisions with islands and the edge of the screen, as crashes will damage your 
ship.

The modelling of objects within the code was agreed on based on the modelling 
of objects in the last project, and was a simple object oriented, game 
development style of representing individual game objects with their own class, 
and using superclasses to unify behaviour.

Graphics were handled using SharpDX, and standard draw methods along with 
lighting and shading models. The camera is set to follow the player, at a 
slight angle from vertical.

The external code we used for this assignment is the algorithm for the diamond 
square algorithm, and the framework was based on tutorial/workshop code. The 
diamond square algorithm was used as decribed by the Wikipedia article 
(https://en.wikipedia.org/wiki/Diamond-square_algorithm), and was based on work 
done in project 1.