# ![ProSeqqo Logo](../../Documentation/Images/ProSeqqoLogo.png) ProSeqqo 
[1. Camera-based robotic pick-and-place](#Camera-based robotic pick-and-place)  
[2. Robotic Cartoon Drawings](#Robotic Cartoon Drawings)  
[3. Robotic Laser Engraving](#Robotic Laser Engraving)  
[4. Robotic Building Blocks](#Robotic Building Blocks)  
[5. Robotic Grinding and Polishing of Furniture Parts](#Robotic Grinding and Polishing of Furniture Parts)  

## 1. Camera-based robotic pick-and-place
[![IMAGE ALT TEXT HERE](http://img.youtube.com/vi/9novNg8slN4/1.jpg)](http://www.youtube.com/watch?v=9novNg8slN4)  
http://www.youtube.com/watch?v=9novNg8slN4

A robot must grasp the parts lying in random poses basedon appropriate sensory information. A potential solution to this challenge hasbeen presented in [1], where various parts are first loaded onto a vibrating light-ing table, their precise poses are determined by a camera system, from where they are taken one-by-one by a robot to a part holder of a press machine.

![Camera based pick and place cell](../../Documentation/Images/CameraPickAndPlaceCell.png)

It is a General, acyclic tast started in camera configuration (1) with validation this time.

```
Task: General
Validate: True
Cyclic: False
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





![Pick and Place struct](../../Documentation/Images/PickAndPlaceStruct.png)

## 2. Robotic Cartoon Drawings
[![IMAGE ALT TEXT HERE](http://img.youtube.com/vi/8ULIP_5nEJ0/2.jpg)](http://www.youtube.com/watch?v=8ULIP_5nEJ0)  
http://www.youtube.com/watch?v=8ULIP_5nEJ0  

## 3. Robotic Laser Engraving

## 4. Robotic Building Blocks

## 5. Robotic Grinding and Polishing of Furniture Parts



[1]: http://slashdot.org B. Tipary, A. Kovács, G. Erdős, Planning and optimization of robotic pick-and-place operations in highly constrained industrial environments, Assem-bly Automation submitted manuscript (2021).