# On Purpose

## Introduction  
“On Purpose” was developed in Global Game Jam 2023 (theme: Roots), Shanghai Site. We make the function of √, the root sign in math, into a puzzle-solving platformer. 

<div align=center><img src="https://user-images.githubusercontent.com/61057370/219538954-00adff6e-ae4f-41c9-ba3a-0c2db2e20ee1.png" width="500" height="270" alt="OnPurpose"/></div>

## Guide
Unity Editor version: 2021.3.9f1c1  
If you want to examine the scripts of the game, you can find programs under Assets/Scripts.
<div align=center><img src="https://user-images.githubusercontent.com/61057370/219539718-933884f9-a810-45ad-a8aa-442a980cb153.png" width="320" height="450" alt="Directory"/></div>

## Desciption
In this project, I wrote all scripts in the Control and Manager Folders. Among all these scripts, 
PlayerControl is the core program that drives the root to move, rotate, and interact with blocks. 
In the process of implementing this game, one difficulty is that the root sign is not symmetrical, 
so it is easy to cause bugs when changing the state of the root sign, such as the position of the 
collision body and the ray will become confused or get stuck in the wall. To avoid this, I made two 
different game objects for the root sign: horizontal and vertical. When the player is spinning, it 
switches between these two game objects. This way, the collider and the ray that detects the wall 
are not misaligned. In addition, when the root sign is rotated while facing left, a horizontal position
offset will be performed to ensure that the position of the sprite before and after the rotation is consistent. 
The figure below shows the Rotate function in the program.
<div align=center><img src="https://user-images.githubusercontent.com/61057370/219542642-438f1f80-3c59-419e-8729-f92633cfcd0b.png" alt="Code"/></div>



If you want to play this game, please follow this address https://mark-zf.itch.io/gamejam-2023
