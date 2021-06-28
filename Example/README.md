# ![ProSeqqo Logo](../../Documentation/Images/ProSeqqoLogo.png) ProSeqqo 
[1. Camera-based robotic pick-and-place](#Camera-based robotic pick-and-place)  
[2. Robotic Cartoon Drawings](#Robotic Cartoon Drawings)  
[3. Robotic Laser Engraving](#Robotic Laser Engraving)  
[4. Robotic Building Blocks](#Robotic Building Blocks)  
[5. Robotic Grinding and Polishing of Furniture Parts](#Robotic Grinding and Polishing of Furniture Parts)  

## 1. Camera-based robotic pick-and-place
[![Camera based pick and place cell demo](http://img.youtube.com/vi/9novNg8slN4/1.jpg)](http://www.youtube.com/watch?v=9novNg8slN4)  

> A robot must grasp the parts lying in random poses basedon appropriate sensory information. A potential solution to this challenge hasbeen presented in [1], where various parts are first loaded onto a vibrating light-ing table, their precise poses are determined by a camera system, from where they are taken one-by-one by a robot to a part holder of a press machine.

The example available in the CameraPickAndPlace.seq.

<img src="../../Documentation/Images/CameraPickAndPlaceCell.png" alt="Camera based pick and place cell" width="500"/>

<!-- ![Camera based pick and place cell](../../Documentation/Images/CameraPickAndPlaceCell.png) -->

It is a General, cyclic task started and finished in the camera configuration (1) with validation this time.

```
Task: General
Validate: True
Cyclic: True
StartDepot: 1
```

The distance between the configurations computed by the library, trapezoid time needs maximal speed and acceleration of joints. It is a 6 element vector, since the 6D robotic joint space. The pick and place configurations are point-like so idle penalty, bidirectional motions and add motion length to cost options are not relevant.
```
DistanceFunction: TrapezoidTime
TrapezoidSpeed: [2.269; 2.269; 2.269; 3.316; 3.316;3.316]
TrapezoidAcceleration: [10.472;10.472;10.472;10.472;10.472;10.472]
IdlePenalty: 0
BidirectionMotionDefault: False
AddMotionLengthToCost: False
```

This time resources not used.  
`ResourceChangeover: None`


As solver settings, Guided local search given with 1 sec time limit and presolver alternative shortcut not used because there are no complex precedences or long task sequences.
```
LocalSearchStrategy: GuidedLocalSearch
TimeLimit: 1000 #ms
UseMIPprecedenceSolver: False
UseShortcutInAlternatives: False
```

Configurations defind for camera pose, as initial depot also. Further configurations for placing the parts (Inter) and multiple configurations for one part (Grasp 1_X, Grasp 2_X).
```
ConfigList:
1;	[-1.868548683;	-1.444176579;	 1,69801667;	-1.991634754;	-1.51983071;	-0.29348593];	Camera
2;	[ 1.228282756;	 4.672848412;	-2.048656112;	-0.400847764;	 1.72721314;	-0.28657041];	Inter1
3;	[ 1.228282756;	 4.429631026;	-1.453428189;	 2.388734353;	-1.72721314;	 2.85502224];	Inter2
...
6;	[-1.226079223;	-2.287934176;	 4.720231373;	-0.879203455;	 0.69826385;	 1.59390633];	Grasp 1_1
7;	[ 1.593316887;	-0.791212376;	 1.553326730;	 3.604246193;	-2.38933547;	 1.11213546];	Grasp 1_2
...
11;	[-1.113591758;	-2.531088585;	 5.195653737;	 2.052016645;	-0.69813971;	-1.57626960];	Grasp 2_1
12;	[-1.113591758;	-2.175895840;	 4.497722352;	-0.746837367;	 0.69813971;	 1.56532304];	Grasp 2_2
13;	[ 1.670452891;	-0.889025708;	 1.772236277;	 3.430690037;	-2.36978343;	 1.03963887];	Grasp 2_3
...
```

The process hierarchy not contains the camera position since it is the starting configuration as depot (on Fig. Process 0). The pick and place of one part in the same process (100), and this is the only alternative (50). The pick and place point-like motions as configurations (e.g. 6;[6], 2;[2]) are in different tasks (30, 31) that strictly follow each other in ID order. This time there are no process and motion precedences, or ovveride costs.

```
ProcessHierarchy:
#Camera
#	  103;	54;	36;	1; 	[1]  #Not needed because StartDepot
#Grasp1  	      	
	  100;	50;	30; 6; 	[6]
	  100;	50;	30;	7; 	[7]
	  100;	50;	30;	8; 	[8]
	  100;	50;	30;	9; 	[9]
	  100;	50;	30;	10;	[10]
#InterSet	        
	  100;	50;	31;	2;	[2]
	  100;	50;	31;	3;	[3]
	  100;	50;	31;	4;	[4]
	  100;	50;	31;	5;	[5]
```

Process 0 contains start depot configuration, wrapped automatically. Process 1/2/3 pick and place operations of three different item that need to be ordered. There are only one alternative, two (pick and place) tasks, and motions with only one configurations.

<img src="../../Documentation/Images/PickAndPlaceStruct.png" alt="Pick and Place struct" width="700"/>
<!-- ![Pick and Place struct](../../Documentation/Images/PickAndPlaceStruct.png) -->

## 2. Robotic Cartoon Drawings
[![Robotic Cartoon Drawings Demo](http://img.youtube.com/vi/8ULIP_5nEJ0/2.jpg)](http://www.youtube.com/watch?v=8ULIP_5nEJ0)  

> While the robotic cartoon drawing application was originally built as a pop-ular science demonstration, the involved process planning and task sequencingproblem illustrates various real industrial problems from the domains of cutting,welding, and painting. In this application, a visitor’s picture is taken, and afterappropriate image processing, it is drawn on a white board by a UR5 robot using a marker pen, see Fig. and the video demonstrations (using a previousversion of the sequence planner) above. Since force feedback is applied when push-ing the pen against the board, lifting up and then re-positioning the pen takesconsiderable time. A special challenge is that the sequencing problem must besolved online, with as little computational time as possible.

The example available in the RoboticDrawing.seq.

<img src="../../Documentation/Images/CartoonDrawing.jpg" alt="Robotic drawing cell" width="700"/>

In this case other validated general task, presented for robotic drawing. The robot able to start and finish anywhere so depots are not given and the problem is acyclic.

```
Task: General
Validate: True
Cyclic: False
#StartDepot:  #Start anywhere
#FinishDepot: #Finish anywhere
```

This application solved in 2D with Euclidian distances. Further idle penalty used to minimize draw interrupts. One line and line section can be drawn in any direction, so the reverse motions generated automatically by BidirectionMotionDefault: True option.

```
DistanceFunction: Euclidian
IdlePenalty: 10
BidirectionMotionDefault: True
ResourceChangeover: Nonde
```

The soler settings are the following, GLS with 0.5 sec limit, without precedence solver and insde motion cost use (this could be turned on to manage line length in optimisation).
```
LocalSearchStrategy: GuidedLocalSearch
TimeLimit: 500 #ms
UseMIPprecedenceSolver: False
AddMotionLengthToCost: False
```

The configurations are the 2D points used in the picture.
```
ConfigList:
1; [14.2087;16.6332]	;Point_1
2; [12.7673;16.5828]	;Point_2
3; [11.4878;16.5325]	;Point_3
4; [ 9.9423;16.6011]    ;Point_4
...
```

The struct of processes build by the presented points, as lines (eg. 1 - [1;2] - Line_1[P1,P2]) Every line is a precess that needs to be order, with single alternative, task and line. The reverse motion generated automatically (e.g. -1 - [2;1] - Line_1[P2, P1]) since the BidirectionMotionDefault option turned on.

```
ProcessHierarchy:
1;1;1;1;[1;2];;Line_1[P1,P2]
2;2;2;2;[2;3];;Line_2[P2,P3]
3;3;3;3;[3;4];;Line_3[P3,P4]
...
```

The sturct of three lines, Process S and F are the virtual start and finish depots, these motions can be reach from any potential other motion on free cost. The line processes, alternatives, tasks contains the original and reverse motions that descibe the line.

<img src="../../Documentation/Images/DrawAndLaserStruct.png" alt="Robotic drawing and laser engraving struct" width="700"/>

After running the example, as 2D task, visualisation available by LineAnimation, The serialized result in .json format can be open.  
<img src="../../Documentation/Images/LineVizDraw.png" alt="Robotic drawing result" width="250"/>


## 3. Robotic Laser Engraving

> Somewhat similarly to the previous application, the goal is to create a 2Dimage from lines on different objects by laser engraving, such as the Celticknot drawing. While the problem model is identical to that of theprevious application, the special challenge is the handling of the large numberof lines (e.g., up to 4000) in the raw input. The problem is relevant both forone-of-a-type products, with as low computation times as possible, and for massproduction, where large computation times can also be allowed.

This example contains 500 lines, CelticLaser.seq, and most of the parameters and structure common with RoboticDrawing.seq.  
<img src="../../Documentation/Images/LineVizLaser.png" alt="Line visualisation of celtic laser problem" width="250"/>

## 4. Robotic Building Blocks
The example available in the CubeCastleBuilding.seq.

The example available in the CubeCastleTwoBuilding.seq.


## 5. Robotic Grinding and Polishing of Furniture Parts
The example available in the FurnitureParts.seq.


[1]: B. Tipary, A. Kovács, G. Erdős, Planning and optimization of robotic pick-and-place operations in highly constrained industrial environments, Assem-bly Automation submitted manuscript (2021).

The quote from: ProSeqqo:  A Generic Solver for Process Planning andSequencing in Industrial Robotics (2021).