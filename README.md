# VR-Photo-Studio
Implement a VR Photo Studio System. The project should include a simple VR Menu, a save and load feature, and a simple IK controller system (you can implement it yourself or use Unity's Animation Rigging Package). The Input should be implemented using the new Input system with InputActions.

## Additional assets used
- Unity XR Interaction Toolkit version 2.2.0 + some Starter Assets
- Oculus hands assets and animations
- UnityChan Sunny Side Up (URP): [link](https://unity-chan.com/download/releaseNote.php?id=ssu_urp)
- Free dancing animations from Asset Store: [link](https://assetstore.unity.com/packages/3d/animations/dance-animations-free-161313#content)

## Requirements
The system should, as a minimum:
- Implement a Teleporting System Using VR
- An Ingame Photography System
- A Save System for the Photos
- Editable Camera Settings
- A VR Menu To control the Project
- A Timeline System To Control the Character Animations
- A simple IK System For The Character

Details:
- [ ] The camera system should Implement The Following Concepts
	- [x] A Shutter To take a Picture
	- [x] A display To Previzualize the Photo
	- [x] A Zoom Function
	- [ ] A Save Function(photos should be saved on a Persistent file on the user's PC)
	- [ ] A Gallery Function (Photos taken on previous sessions should be loaded upon startup)
	- [x] A limit of Photos Per Session
		- [x] After Exceeding the Limit, there should be a fade out to the main Menu
	- [x] The Camera Settings(Zoom, Photos per session, Saved Data Location, etc.) should be Editable via Scriptable Objects.
  
- [x] The Locomotion system should work in the following manner.
	- [x] You should only have Specific Points on the map where you can move.
	- [x] To move to any of these points, you should Take a Picture of the Position you want to move.
	- [x] After Taking the photo, the player should be teleported to the next position but oriented toward the Idol.
	- [x] The system should be flexible enough for a designer to create new Points where the player could stand.
  
- [ ] The VR Idol should have at least four different Animations(Timeline Controlled) + Waving and looking toward the player.
	- [ ] Through the VR Menu should be able To select 1 out of 3 modes
		- [x] Single Animation Mode
			- [x] Only One animation should be Played
		- [x] All Animations Looped
			- [x] The 4+ animations should be played One after the other and Loop to the first one upon completion.
		- [ ] Waving Mode
			- [ ] The Character should Be in Idle mode while Looking at the player and Waving(Use an Ik Controller for this)
      
- [x] The experience should have a beginning and an end.
