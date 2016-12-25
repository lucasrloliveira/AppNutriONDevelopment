----------------------------------------------------------------------------------------------------------------
-------------------------------------------Installation & Setup Instructions------------------------------------
----------------------------------------------------------------------------------------------------------------

1. Create Your own UI Panels (from Unity 4.6 UI).
2. Download LeanTween plugin: https://www.assetstore.unity3d.com/en/#!/content/3595
3. Download Common Animated Menu 2D: https://www.assetstore.unity3d.com/#!/content/30025
4. Copy to Your scene 'CAMenu2D' prefab from 'Prefabs' folder.
5. Select 'CAMenu2D' object.
6. Drag and drop references to Your UI Panels and left/right buttons. 
7. Run project! 

----------------------------------------------------------------------------------------------------------------
-------------------------------------------Options to change ---------------------------------------------------
----------------------------------------------------------------------------------------------------------------
You could change many things in Your 'CAMenu2D' object.
Select this object on the scene. Below there are options with explanations what they do.

--> Items
-> Size
Set the size of the number of menu items. After that remember to click 'Enter' on Your keyboard. Then You will notice that number of Elements below are changed. Should be greater than 0.
-> Elements
This is place for references to Your menu items. 

--> Distance Between Items
This is distance between Your menu items. Here You could simply manipulate menu looks. Here you can manipulate the appearance of your menus. 0 means the direct location next to each other.

--> Animation Time
This parameter represents time between transitions. The animation begins when you click the arrow, or when you drag your finger on the phone. Should be greater or equal 0.

--> Transition Delay
This parameter represents delay between You click on the button and time when animation starts. Should be greater or equal 0.

--> Start Item Index
Number of starting item. When You run project You will see that menu is exactly centered on that item. Starts from 0 to Size-1.

--> Transition Type
Here are all LeanTween transitions. Choose the most suitable to Your needed.

--> Left Button Object
Reference to left button object if You have one.

--> Right Button Object
Reference to right button object if You have one.

--> Min Acceptance Touch Distance
This value represents minimum length of touch to move menu. You can change this value to one that you feel more intuitive.

--> Is Touch Detection Active
Touch detection is enabled only on device. The default is set to true. Sometimes you will need to disable touching. 
For example in a situation where the game show pop-up with a dark background behind it. Then you will not want to react to the movements of the menu with your finger, 
because only pop-up will be on top.
