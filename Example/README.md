# ![ProSeqqo Logo](../Documentation/Images/ProSeqqoLogo.png) [ProSeqqo](../Documentation/Readme.md) 
[1. Camera-based robotic pick-and-place](#cc)  
[2. Robotic Cartoon Drawings](#2.-Robotic-Cartoon-Drawings)  
[3. Robotic Laser Engraving](#3.-Robotic-Laser-Engraving)  
[4. Robotic Building Blocks](#4.-Robotic-Building-Blocks)  
[5. Robotic Grinding and Polishing of Furniture Parts](#Robotic-Grinding-and-Polishing-of-Furniture-Parts)  

## 1. Camera-based robotic pick-and-place <a id="cc">
[![Camera based pick and place cell demo](http://img.youtube.com/vi/9novNg8slN4/1.jpg)](http://www.youtube.com/watch?v=9novNg8slN4)  

> A robot must grasp the parts lying in random poses based on appropriate sensory information. A potential solution to this challenge has been presented in [1], where various parts are first loaded onto a vibrating lighting table. Their precise poses are determined by a camera system, from where they are taken one-by-one by a robot to a part holder of a press machine. [2]

The example is available in the CameraPickAndPlace.seq.

<img src="../Documentation/Images/CameraPickAndPlaceCell.png" alt="Camera based pick and place cell" width="300"/>

<!-- ![Camera based pick and place cell](../Documentation/Images/CameraPickAndPlaceCell.png) -->

It is a General, cyclic task started and finished in the camera configuration (1) with validation this time.

```
Task: General
Validate: True
Cyclic: True
StartDepot: 1
```

The distance between the configurations computed by the library, trapezoid time, needs maximal speed and acceleration of joints. It is a 6 element vector since the 6D robotic joint space. The pick and place configurations are point-like, so idle penalty, bidirectional motions and add motion length to cost options are irrelevant.
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

Configurations defined for camera pose as initial depot also. Further configurations for placing the parts (Inter) and multiple configurations for one part (Grasp 1_X, Grasp 2_X).
```
ConfigList:
1;  [-1.868548683;  -1.444176579;    1,69801667;    -1.991634754;   -1.51983071;    -0.29348593];   Camera
2;  [ 1.228282756;   4.672848412;   -2.048656112;   -0.400847764;    1.72721314;    -0.28657041];   Inter1
3;  [ 1.228282756;   4.429631026;   -1.453428189;    2.388734353;   -1.72721314;     2.85502224];   Inter2
...
6;  [-1.226079223;  -2.287934176;    4.720231373;   -0.879203455;    0.69826385;     1.59390633];   Grasp 1_1
7;  [ 1.593316887;  -0.791212376;    1.553326730;    3.604246193;   -2.38933547;     1.11213546];   Grasp 1_2
...
11; [-1.113591758;  -2.531088585;    5.195653737;    2.052016645;   -0.69813971;    -1.57626960];   Grasp 2_1
12; [-1.113591758;  -2.175895840;    4.497722352;   -0.746837367;    0.69813971;     1.56532304];   Grasp 2_2
13; [ 1.670452891;  -0.889025708;    1.772236277;    3.430690037;   -2.36978343;     1.03963887];   Grasp 2_3
...
```

The process hierarchy does not contain the camera position since it is the starting configuration as a depot (Fig. Process 0). The pick and place of one part in the same process (100) is the only alternative (50). The pick and place point-like motions as configurations (e.g. 6;[6], 2;[2]) are in different tasks (30, 31) that strictly follow each other in ID order. This time there are no process and motion precedences or override costs.

```
ProcessHierarchy:
#Camera
#     103;  54; 36; 1;  [1]  #Not needed because StartDepot
#Grasp1             
      100;  50; 30; 6;  [6]
      100;  50; 30; 7;  [7]
      100;  50; 30; 8;  [8]
      100;  50; 30; 9;  [9]
      100;  50; 30; 10; [10]
#InterSet           
      100;  50; 31; 2;  [2]
      100;  50; 31; 3;  [3]
      100;  50; 31; 4;  [4]
      100;  50; 31; 5;  [5]
```

Process 0 contains start depot configuration, wrapped automatically. Process 1/2/3 pick and place operations of three different items that need to be ordered. There are only one alternative, two (pick and place) tasks, and motions with only one configuration.

<img src="../Documentation/Images/PickAndPlaceStruct.png" alt="Pick and Place struct" width="500"/>
<!-- ![Pick and Place struct](../Documentation/Images/PickAndPlaceStruct.png) -->

## 2. Robotic Cartoon Drawings
[![Robotic Cartoon Drawings Demo](http://img.youtube.com/vi/8ULIP_5nEJ0/2.jpg)](http://www.youtube.com/watch?v=8ULIP_5nEJ0)  

> While the robotic cartoon drawing application was initially built as a popular science demonstration, the involved process planning and task sequencing problem illustrates various real industrial problems from the domains of cutting, welding, and painting. A visitor???s picture is taken in this application, and after appropriate image processing, it is drawn on a whiteboard by a UR5 robot using a marker pen, see Fig. and the video demonstrations (using a previous version of the sequence planner) above. Since force feedback is applied when pushing the pen against the board, lifting up and re-positioning the pen takes considerable time. A special challenge is that the sequencing problem must be solved online with as little computational time as possible. [2]

The example is available in the RoboticDrawing.seq.

<img src="../Documentation/Images/CartoonDrawing.jpg" alt="Robotic drawing cell" width="400"/>

In this case, other validated general task, presented for robotic drawing. The robot can start and finish anywhere, so depots are not given, and the problem is acyclic.

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

The soler settings are GLS with a 0.5-sec limit, without precedence solver and inside motion cost use (this could be turned on to manage line length in optimization).
```
LocalSearchStrategy: GuidedLocalSearch
TimeLimit: 500 #ms
UseMIPprecedenceSolver: False
AddMotionLengthToCost: False
```

The configurations are the 2D points used in the picture.
```
ConfigList:
1; [14.2087;16.6332]    ;Point_1
2; [12.7673;16.5828]    ;Point_2
3; [11.4878;16.5325]    ;Point_3
4; [ 9.9423;16.6011]    ;Point_4
...
```

The struct of processes build by the presented points, as lines (e.g. 1 - [1;2] - Line_1[P1,P2]) Every line is a process that needs to order, with a single alternative, task and line. The reverse motion generated automatically (e.g. -1 - [2;1] - Line_1[P2, P1]) since the BidirectionMotionDefault option turned on.

```
ProcessHierarchy:
1;1;1;1;[1;2];;Line_1[P1,P2]
2;2;2;2;[2;3];;Line_2[P2,P3]
3;3;3;3;[3;4];;Line_3[P3,P4]
...
```

The sturct of three lines, Process S and F, are the virtual start and finish depots; these motions can be reached from any potential other motion at free cost. The line processes, alternatives, tasks contain the original and reverse motions that describe the line.

<img src="../Documentation/Images/DrawAndLaserStruct.png" alt="Robotic drawing and laser engraving struct" width="400"/>

After running the example, as a 2D task, visualization available by LineAnimation, The serialized result in .json format can be open.  
<img src="../Documentation/Images/LineVizDraw.png" alt="Robotic drawing result" width="250"/>


## 3. Robotic Laser Engraving

> Somewhat similarly to the previous application, the goal is to create a 2Dimage from lines on different objects by laser engraving, such as the Celtic knot drawing. While the problem model is identical to that of the previous application, the special challenge is handling many lines (e.g., up to 4000) in the raw input. Thus, the problem is relevant for one-of-a-type products, with as low computation times as possible, and mass production, where significant computation times can also be allowed. [2]

This example contains 500 lines, CelticLaser.seq, and most of the parameters and structure standard with RoboticDrawing.seq.  
<img src="../Documentation/Images/LineVizLaser.png" alt="Line visualisation of celtic laser problem" width="250"/>

## 4. Robotic Building Blocks
> The building blocks application is a student project originally focused on identifying objects, their poses, and the potential ways of grasping them using a vision camera. The building blocks must be grasped using a two-finger gripper and taken from their identified source poses to the specified target poses without applying an intermediate buffer.  
The sequencing problem originating from this application has been modelled in the 3D task space, with one process standing for each block, which contains one alternative and two tasks for picking and placing the block. The two grasp-ing modes, NS and EW, are captured by two motions within the task. Note that the current blocks have a 90???rotational symmetry, which implies that the target configuration is realized correctly independently of the chosen grasping mode. [2]

In the first example available in the CubeCastleBuilding.seq, only pick from one position and build the castle wall.

In the second example available in the CubeCastleTwoBuilding.seq, one castle built and wrecked while another is making from the cubes picked up.

The header parts are common and known from the previous examples:
```
Task: General
Validate: True
Cyclic: False
#StartDepot:  #Start anywhere
#FinishDepot: #Finish anywhere

DistanceFunction: Euclidian
IdlePenalty: 0
BidirectionMotionDefault: False
AddMotionLengthToCost: False

LocalSearchStrategy: GreedyDescent
TimeLimit: 0
UseMIPprecedenceSolver: True

ResourceChangeover: None
```

The cubes of the two side (build, wreck) are in pairs previously. Therefore, only the ordering of the movements is the task.
The configurations of a place in the 0,0,0 position, in build building (BB) C side, Layer 0 with X or Y grab.
The configurations of a pick in the 100,10,90 position, in wreck building (BW) A side, Layer 9 with X or Y grab.
```
ConfigList:
#Put down X
0;[0;0;0];      BB_Wall_C0_L0_Putdown_x
...
#Put down Y
100;[0;0;0];    BB_Wall_C0_L0_Putdown_y
...
#Pick up X
200;[100;10;90];BW_Wall_A1_L9_Pickup_X
...
#Pick up Y
300;[100;10;90];BW_Wall_A1_L9_Pickup_Y
```

The pick and place of one cube represented by a process, only one alternative and two following task. Each X, Y orientations are transferable, so these are different point-like movements. [2]
```
ProcessHierarchy:
#Cube: BW_Wall_A1_L9 --> BB_Wall_C0_L0
0 ; 0 ; 0 ; 200 ;   [200]   #BW_Wall_A1_L9_Pickup_X
0 ; 0 ; 0 ; 300 ;   [300]   #BW_Wall_A1_L9_Pickup_Y
0 ; 0 ; 1 ; 0 ;     [0]     #BB_Wall_C0_L0_Putdown_X
0 ; 0 ; 1 ; 100 ;   [100]   #BB_Wall_C0_L0_Putdown_Y
#Cube: BW_Wall_A2_L9 --> BB_Wall_D3_L0
1 ; 0 ; 0 ; 201 ;   [201]   #BW_Wall_A2_L9_Pickup_X
1 ; 0 ; 0 ; 301 ;   [301]   #BW_Wall_A2_L9_Pickup_Y
1 ; 0 ; 1 ; 1 ;     [1]     #BB_Wall_D3_L0_Putdown_X
1 ; 0 ; 1 ; 101 ;   [101]   #BB_Wall_D3_L0_Putdown_Y
```

The challenge of the problem is the order constraints, each cube accessible for (pick and place, too) by Y pose, if Ahead and Back cube moved or not exist. Pose X available, if Left and Right side free. In case of build Down cube have to be placed, and in case of wreck Up cube have to be picked before the selected cube become available. These complex constraints are resolvable only with MIP presolver. [2]
```
MotionPrecedence:
...
100 ;   108 #BB_Wall_C0_L0 Yaxis ---Ahed---> BB_Wall_C1_L0 Yaxis
100 ;   8   #BB_Wall_C0_L0 Yaxis ---Ahed---> BB_Wall_C1_L0 Xaxis
0 ;     111 #BB_Wall_C0_L0 Xaxis ---Right---> BB_Wall_A1_L0 Yaxis
0 ;     11  #BB_Wall_C0_L0 Xaxis ---Right---> BB_Wall_A1_L0 Xaxis
...
200 ;   208 #BW_Wall_A1_L9 Xaxis ---Up---> BW_Wall_A1_L8 Xaxis
200 ;   308 #BW_Wall_A1_L9 Xaxis ---Up---> BW_Wall_A1_L8 Yaxis
300 ;   208 #BW_Wall_A1_L9 Yaxis ---Up---> BW_Wall_A1_L8 Xaxis
300 ;   308 #BW_Wall_A1_L9 Yaxis ---Up---> BW_Wall_A1_L8 Yaxis
...
```


The build (right) and wreck (left) of the build at halftime, solved with more than 1200 order constraints.
As a 3D solution, LineAnimation able to show the created order by the result .json file.  
<img src="../Documentation/Images/LineVizBuild.png" alt="Visualisation of buildings" width="300"/>


## 5. Robotic Grinding and Polishing of Furniture Parts
The example is available in the FurnitureParts.seq.

> In the last application scenario, the robotic belt grinding and polishing of cast aluminium furniture parts are the goal. The surface of the part is decomposed into nine longitudinal stripes, and each stripe must undergo up to three surface finishing tasks: rough grinding, fine grinding, and polishing. Five stripes need all three tasks, whereas four stripes need only polishing, resulting in 19 tasks altogether. For technological reasons, all rough grinding tasks must precede all fine grinding tasks, which in turn must precede all polishing tasks. Each task corresponds to a robot motion specified in the 6D joint configuration space of the robot, which guides the part along with a contact trajectory between the given stripe of the part surface and the tool. The direction of the motion can be reversed. Idle motions between the effective tasks can be rather complicated due to the difficult part geometry and the densely populated work-cell. Hence, all possible 38??38 idle motions between the effective path endpoints were pre-computed using the path planning library. [2]


<img src="../Documentation/Images/Grinding1.JPG" alt="Robotic drawing and laser engraving struct" height="250"/>
<img src="../Documentation/Images/Grinding2.png" alt="Robotic drawing and laser engraving struct" height="250"/>

This case also a general, acyclic task with validation, where the start and the finish can be chosen free.
```
Task: General
Validate: True
Cyclic: False
#StartDepot:  #Start anywhere
#FinishDepot: #Finish anywhere
```

The distance between the motions written by own computed collision-free time values. Idle penalty and in motion length not necessary. But bidirectional, the operations can be done from any direction.
```
DistanceFunction: Matrix
IdlePenalty: 0
BidirectionMotionDefault: True
AddMotionLengthToCost: False
AddInMotionChangeoverToCost: False
```

GuidedLocalSearch with a 30-sec time limit, MIP needed to resolve motion constraints without alternative shortcuts.
```
LocalSearchStrategy: GuidedLocalSearch
TimeLimit: 30000
UseMIPprecedenceSolver: True
UseShortcutInAlternatives: False
ResourceChangeover: None
```

Configuration IDs and costs between them.
```
ConfigMatrix:
0;    1;    2;  ...; 37    #Configuration ID header
0;    2.4;  1.5;...; 4.03  #Costs
...
2.2;  2.3;  0.3;...; 0
```

Each motion needs to be ordered, so each motion has its own process, alternative, task.
```
ProcessHierarchy:
0; 0; 0; 0; [0; 1]; ; P1A
1; 1; 1; 1; [2; 3]; ; P1B
2; 2; 2; 2; [4; 5]; ; P1C
...
17; 17; 17; 17; [34; 35]; ; P8C
18; 18; 18; 18; [36; 37]; ; P9C
```

The three phases of the operation need to be done in A, B, and C for each edge.
```
MotionPrecedence:
#A->B
19; 1
19; 4
...
#B->C
1; 2
1; 5
...
4; 2
4; 5
...
```

[1]: B. Tipary, A. Kov??cs, G. Erd??s, Planning and optimization of robotic pick-and-place operations in highly constrained industrial environments, Assem-bly Automation submitted manuscript (2021).

[2]: L. Zahor??n, A. Kov??cs, ProSeqqo:  A Generic Solver for Process Planning and Sequencing in Industrial Robotics (2021).
