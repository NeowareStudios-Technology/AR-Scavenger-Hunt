# Scavenger Hunt


<h2>Frameworks</h2>


<h3>Photon Pun v2</h3>


<ul>

<li>Handles multiplayer functionality 
<ul>
 
<li>Loads players to a lobby
 
<li>Handles streaming variables  
<ul>
  
<li>"isReady" boolean is set to true when players are ready to start the game
</li>  
</ul>
</li>  
</ul>
</li>  
</ul>
<h3>Vuforia</h3>


<ul>

<li>Handles Image Target recognition 
<ul>
 
<li>Database is imported from Vuforia's Target Manager website  
<ul>
  
<li><a href="https://developer.vuforia.com/target-manager">https://developer.vuforia.com/target-manager</a>
</li>  
</ul>
 
<li>The ARObjects GameObject holds ImageTargets with references to each Image Target from the database.  
<ul>
  
<li>Each Image Target (GameObject) must have a "Image Target Behavior" script with the correct "database" and "Image Target" set
  
<li>Each Image Target must have a child GameObject which contains the following components   
<ul>
   
<li>Mesh Filter
   
<li>Mesh Renderer
   
<li>Collider
</li>   
</ul>
  
<li>The child GameObject's Collider will be setActive when the target is found
</li>  
</ul>
</li>  
</ul>
</li>  
</ul>
<h2>Prefabs</h2>


<h3>Player Prefab</h3>


<ul>

<li>Each player prefab has a Photon View and a Player Manager

<li>Stored in the Resources Folder to be instantiated at runtime
</li>
</ul>
<h2>Scripts</h2>


<h3>Launcher.cs </h3>


<ul>

<li>Handles connection of players.

<li>Holds the max players per room variable. 

<li>Automatically connects players to a random room if one is open and room is not at max players 
<ul>
 
<li>Otherwise creates a room with the player as the host.
</li> 
</ul>

<li>Loads next level (ArScavengerHunt) once all players are "ready"
</li>
</ul>
<h3>Game.cs</h3>


<ul>

<li>Holds players information in arrays 
<ul>
 
<li>Players
 
<li>Playerinfo
 
<li>Playernamesingame
 
<li>playerNames
</li> 
</ul>

<li>Checks for new players 
<ul>
 
<li>Sets new player to the proper index of the Players array
 
<li>Renames the player object to "player1" "player2".... 
</li> 
</ul>

<li>Loads and Instantiates the Player prefab
</li>
</ul>
<ul>

<li>Determines winner at end of game
</li>
</ul>
<ul>

<li>Handles the Timer and setting it to the UI during gameplay loop and on summary screen
</li>
</ul>
<ul>

<li>Sets the UI "All players are ready" and "Not all players are ready"
</li>
</ul>
<ul>

<li>Increments score

<li>Calls "IncrementScore" on PlayerManager gameobjects
</li>
</ul>
<h3>UIReferences.cs </h3>


<p>
Holds references to UI
</p>
<h3>AudioReferences.cs</h3>


<p>
Holds references to audio GameObjects
</p>
<h3>PlayerManager.cs </h3>


<ul>

<li>The "isReady" variable of each PlayerManager is streamed (viewable) to all other Playermanagers

<li>Sets up the UI when the player joins

<li>Turns off the gameplay theme if it is on
</li>
</ul>
