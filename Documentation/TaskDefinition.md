# ![ProSeqqo Logo](../Documentation/Images/ProSeqqoLogo.png) [ProSeqqo](../Documentation/Readme.md) 
# Task definition language
The given parameters able to write down the universal form of sequencing problems. The library able to phrase .xml, .json task definitions and .seq / .txt well-readable text format. The [result of the tasks](../Documentation/ResultDefinition.md) also available in these formats. [Example](../Example) and [Skeletons](../Example/Skeletons) directory of the repository contains the complete list of parameters.


The following sections define the own .seq format parameters; these are also used in the standard formats (JSON, XML) with their own specificity. The input file interpreted line by line, within the lines separated by `:` and `;` characters. Floating-point number separators: `.` and `,`. Full line comment: `#`. The order of parameters is commutable, not sensitive for whitespace and not case sensitive.

Notations: ~ Optional, ! Default, * Needed

# Summary
| Option | Values | Description|
|--------|--------|------------|
|*[`Task`](#task)|`General`| For sequencing configurations and motions. One motion/configuration selected for execution in a task. The tasks have a strict sequence in an alternative. Finally, one alternative selected in a process while the processes are ordered. |
|[`Validate`](#validate)|True / False|If true, the task is validated before execution, it can be time-consuming. |
|*[`Cyclic`](#cyclic)|True / False| The cyclic sequence has the same start and finishes in the solution. In the case of a cyclic task, the `StartDepot` have to be given.|
|[`StartDepot`](#startdepot)|`ConfigID`/`MotionID`|Start depot is the first position of the circles or walks based on `Cyclic`. It is going to ignore all alternatives and task elements of the given element. The StartDepot have to be defined in the `ProcessHierarchy`.|
|[`FinishDepot`](#finishdepot)|`ConfigID`/`MotionID`|Finish depot is the last position of the path if `Cyclic` = False. It is going to ignore all alternatives and task elements of the given position. |
|**Hierarchy** | | |
|[`ConfigList`](#configlist) | `ConfigID` ; `Position` ; ~`Name` ; ~`ResourceID` | Used when `DistanceFunction` is not Matrix. List of records, that defines all possible configurations of the task. These configurations can be used for motion definition (`MotionList`) at any times. Also used in the `ProcessHierarchy` section but only once. Order constraints can be defined in the `MotionPrecedence` section. |
|[`ConfigMatrix`](#configmatrix) |`ConfigID`[ ] <br/> Double[ ][ ] <br/> ~`Name`[ ] <br/>     ~`ResourceID` [ ] | Used in case of `DistanceFunction`: Matrix excludes the use of `ConfigList`. Cost matrix with id header, optional name and resource id footer. The matrix should contain the cost of every pair of positions. These configurations can be used for motion definition (`MotionList`) at any times. Also used in the `ProcessHierarchy` section but only once. Order constraints can be defined in the `MotionPrecedence` section. |
|[`ProcessHierarchy`](#processhierarchy) | `ProcessID` ; `AlternativeID` ; `TaskID` ; `MotionID` ;`ConfigID`;; ~`Bidirectional` ; ~`Name`                                                                                                 | List   of ProcessID ; AlternativeID ; TaskID ; MotionID ; ConfigID; Bidirectional; Name. Every defined configuration (from `ConfigList` / `ConfigMatrix`) can be part of motions, and every motion need to be used and settled in a Task, Alternative and Process. The motions can be used as a single configuration or with multiple ones as a complex movement. The bidirectional and name property refers to the motion. |
|[`ProcessPrecedence`](#processprecedence)|`ProcessID` ; `ProcessID`| List of `ProcessID` ; `ProcessID` records. The positions in the process with the first `ProcessID` must be before the positions in the process with the second `ProcessID` in the solution. The definition must be a directed acyclic graph, and not defined processes are free. Process of `StartDepot` and `FinishDepot` can not be used. |
|[`MotionPrecedence`](#motionprecedence)|`MotionID` ; `MotionID`|The position or motion with the first id must be before the second id of position or motion in the solution if both positions selected. This representation of constraints does not have to be DAG. Configuration or motion of `StartDepot` and `FinishDepot` can not be used.|
|[`OverrideCost`](#overridecost)|`ConfigIDA` ; `ConfigIDB` ; `Cost` ; ~`Bidirectional`|The list of records that directly define the cost of A, B positions, overrides the result of `DistanceFunctions`. Bidirectional is false by default (applied only on A->B), else cost override also used for B->A. Different type of resource methods able to modify this overridden value.|
| **Costs**||| 
|*[`DistanceFunction`](#distancefunction)|| The distance function defines the basic way of cost computation. Assign cost value to configuration pairs, the motions for the VRP matrix. It can be overwritten by `OverrideCost`, `ResourceChangeover` or `IdlePenalty`. |
||`Euclidian`|Simple euclidian distance of configurations. (https://en.wikipedia.org/wiki/Euclidean_distance)|
||`Max`| Greatest absolute difference of configuration elements.|
||`Trapezoid`|[Trapezoid Time](https://www.youtube.com/watch?v=3Kmlpe8kgbk) based on maximum acceleration & speed of dimensions. |
||`TrapezoidTimeWithTieBreaker`|[Trapezoid Time](https://www.youtube.com/watch?v=3Kmlpe8kgbk) based on maximum acceleration & speed of dimensions.|
||`Manhattan`| Also known as [taxicab metric](https://en.wikipedia.org/wiki/Taxicab_geometry).  |
||`Matrix`|Manual distance/cost, given by distance matrix of configurations.|
|[`TrapezoidAcceleration`](#trapzoidacceleration)|Double []|In case we choose `TrapezoidTimeWithTieBreaker` or `TrapezoidTime` need to be define maximal acceleration in n dimensions.|
|[`TrapezoidSpeed`](#trapezoidspeed) | Double [] |In case we choose `TrapezoidTimeWithTieBreaker` or `TrapezoidTime` need to be define maximal speed in n dimensions. |
| [`IdlePenalty`](#idlepenalty) | Double | This value-added to the costs if the following positions/motions are not in contact, and the tool path or motion has to be interrupted. |
| [`BidirectionMotionDefault`](#bidirectionmotiondefault) | True / False | If True, extra motion generated and added to the hierarchy with the _Reverse postfix and swapped end configurations in case of movements. If False, only the given direction added. At the end of `MotionList` records can be overwritten one by one. |
| [`AddMotionLengthToCost`](#addmotionLengthtocost) | True / False | If true: The length of the motion (computed between ConfigA, ConfigB with `DistanceFunction` or `OverrideCost`) need to be used in cost computation. |
|**Resources**| | |
|[`ResourceChangeover`](#resourcechangeover)|! `Off`| Resources not used in the task. |
||`Constant`| Resources are used, and the changeover cost given by a constant (`ChangeoverConstant`). |
||`Matrix` | Resources are used, and the changeover cost given by a matrix (`ChangeoverMatrix`) for every resource pair.|
|[`ResourceChangeoverFunction`](#resourcechangeoverfunction)| !`Add`| Function for handle connection of computed edge weight and resource cost. w = w + r|
||`Max`| Function for handle connection of computed edge weight and resource cost. w = Max(w,r) |
|[`ChangeoverConstant`](#changeoverconstant) | Double | Constant cost of resource change. The resources are defined in the `ConfigList`/`ConfigMatrix` section.|
|[`ChangeoverMatrix`](#changeovermatrix)|| Resource changeover cost, for every pair of resources. Also contains a `ResourceID` header. The matrix should contain the distance/cost of every resource change. The resources are defined in the `ConfigList`/`ConfigMatrix` section. |
|**Solver**| |
|[`LocalSearchStrategy`](#localsearchstrategy)| | Google OR-Tools VRP solver metaheuristics. [Reference](https://developers.google.com/optimization/routing/routing_options#local_search_options) |
||`Automatic`|Default value. Lets the solver select the metaheuristic.|
||`GreedyDescent`|AAccepts improving (cost-reducing) local search neighbours until a local minimum is reached.|
||`GuidedLocalSearch`|Uses guided local search to escape local minima (cf. http://en.wikipedia.org/wiki/Guided_Local_Search); this is generally the most efficient metaheuristic for vehicle routing.|
||`SimulatedAnnealing`|Uses simulated annealing to escape local minima (cf. http://en.wikipedia.org/wiki/Simulated_annealing).|
||`TabuSearch`|Uses tabu search to escape local minima (cf.http://en.wikipedia.org/wiki/Tabu_search).|
||`ObjectiveTabuSearch`|Uses tabu search on the objective value of the solution to escape local minima|
|[`TimeLimit`](#timelimit)|Int|The time limit for the VRP solver in [ms]. It has an effect only for the VRP solver, not the full runtime. Read, phrase, process, initial MIP solver not included. It can stop earlier with a solution or without through timeout.|                                                                                                                                                                                                                                                                                                                                                        ||[`UseMIPprecedenceSolver`](#usemipprecedencesolver)|True / False|Creates an initial solution by CBC-MIP solver, for tasks with complex precedence constraints.|
|[`UseShortcutInAlternatives`](#useshortcutinalternatives)|True / False| In the case of long sequences of alternatives, find the shortest paths and replace the original alternative with a virtual one can be remuneratory. It contains only one task and a virtual single config representation of shortcuts. After the execution of sequencing, the original hierarchy is restored. Reduce complexity, the number of nodes and constraints at build time.|
|[`UseMIPprecedenceSolver`](#usemippresolver)|True / False| |


# Details

#### *Task

For sequencing configurations and motions. One motion/configuration selected for execution in a task. The tasks have a strict sequence in an alternative. Finally, one alternative was chosen in a process while the processes are ordered. 

| Value  | Include | Exclude | Optional|
| ------ | ------  | ------  | ------  |
| `General`| `Cyclic`, `DistanceFunction`, `ProcessHierarchy` |  | `Validate`,`StartDepot`, `FinishDepot`, `LocalSearchStrategy`, `TimeLimit`, `UseShortcutInAlternatives`, `TrapezoidAcceleration`, `TrapezoidSpeed`, `IdlePenalty`, `BidirectionMotionDefault`, `AddMotionLengthToCost`, `ResourceChangeover`, `ResourceChangeoverFunction`, `ChangeoverConstant`, `ChangeoverMatrix`, `ConfigList`, `ConfigMatrix`, `MotionList`, `ProcessPrecedence`, `MotionPrecedence`, `OverrideCost` |
| `Line`   |  |  |  |

<details>
<summary>Usage</summary>

```
//SEQ
Task: General

//JSON
"TaskType": "General"

//XML
<TaskType>General</TaskType>

//C#
GeneralTask t = new GeneralTask();
```
</details>
   

#### Validate

If true, the task is validated before execution; it can be time-consuming.

<details>
<summary>Usage</summary>

```
//SEQ
Validate: True
Validate: False

//JSON
    "Validate": true,
    "Validate": false,

//XML
  <Validate>true</Validate>
  <Validate>false</Validate>

//C#
    GeneralTask t = new GeneralTask();
    t.Validate = true;
    t.Validate = false;
```
</details>

#### *Cyclic

The cyclic sequence has the same start and finishes in the solution. In the case of a cyclic task, the `StartDepot` have to be given.

| Value  | Include      | Exclude      | Optional                    |
| ------ | ------       | ------       | --------                    |
| True   | `StartDepot` | `FinishDepot`|  -                          |
| False  |      -       |      -       | `StartDepot`, `FinishDepot` |

<details>
<summary>Usage</summary>

```
//SEQ
    Cyclic: True
    Cyclic: False

//JSON
    "Cyclic": true,
    "Cyclic": false,

//XML
    <Cyclic>true</Cyclic>
    <Cyclic>false</Cyclic>

//C#
    GeneralTask t = new GeneralTask();
    t.Cyclic = true;
    t.Cyclic = false;
```
</details>

#### StartDepot

Start depot is the first configuration of the circles or walks based on `Cyclic`. The selected configuration wrapped automatically in a virtual motion, alternative, process struct with ID: 9999999. The StartDepot have to be defined in the `ConfigList` or `ConfigMatrix`.

| Param  | Rule   |
| ------ | ------ |
| Int    | Must be part of `ConfigList` or `ConfigMatrix` as `ConfigID`.|

<details>
<summary>Usage</summary>

```
//SEQ
    StartDepot: 0

//JSON
    "StartDepot": 0,

//XML
    <StartDepot>0</StartDepot>

//C#
    GeneralTask t = new GeneralTask();
    t.StartDepotConfig  = new Config() { ID = 0, Name = "Start",  Configuration = new List<double>() {0, 0} }; 
```
</details>

#### FinishDepot
Finish depot is the last position of the path if `Cyclic` = False. The selected configuration wrapped automatically in a virtual motion, alternative, process struct with ID: 888888. The FinishDepot have to be defined in the `ConfigList` or `ConfigMatrix`.

| Param  | Rule   |
| ------ | ------ |
| Int    | Must be part of `ConfigList` or `ConfigMatrix` as `ConfigID`.|

<details>
<summary>Usage</summary>

```
//SEQ
    FinishDepot: 1

//JSON
    "FinishDepot": 1,

//XML
    <FinishDepot>1</FinishDepot>

//C#
    GeneralTask t = new GeneralTask();
    t.FinishDepotConfig = new Config() { ID = 1, Name = "Finish", Configuration = new List<double>() {10, 10} };
```
</details>



## Costs

#### *DistanceFunction

The distance function defines the basic way of cost computation. Assign cost values to motion pairs (between last configuration $x$ of motion one, and first configuration $y$ of motion two), for the GTSP matrix. It can be overwritten by `OverrideCost`, `ResourceChangeover` or `IdlePenalty`.

| Value     | Include      | Exclude      | 
| ------    | ------       | ------       |
| Euclidian | `ConfigList`   |`TrapezoidAcceleration`, `TrapezoidSpeed`, ConfigMatrix| 
| Max       | `ConfigList`   |`TrapezoidAcceleration`, `TrapezoidSpeed`, ConfigMatrix|  
| Trapezoid | `ConfigList`, `TrapezoidAcceleration`, TrapezoidSpeed           |ConfigMatrix|  
| TrapezoidTimeWithTieBreaker| `ConfigList`, `TrapezoidAcceleration`, `TrapezoidSpeed`           |`ConfigMatrix`|
| Manhattan |   `ConfigList |`TrapezoidAcceleration`, `TrapezoidSpeed`, ConfigMatrix|
| Matrix    | `ConfigMatrix |`TrapezoidAcceleration`, `TrapezoidSpeed`, `ConfigList`|

$`x = [x_1, x_2, ..., x_n]`$  
$`y = [y_1, y_2, ..., y_n]`$

|            Function            | Implementation |
|--------------------------------|--------------|
|`Euclidian`            | $`\sqrt{(x_1-y_1)^2+(x_2-y_2)^2+...+(x_n-y_n)^2}`$             |
|`Max`                  | $`max(\vert x_1-y_1 \vert, \vert x_2-y_2 \vert,..., \vert x_n-y_n \vert)`$|
|[`TrapezoidTime`](https://www.linearmotiontips.com/how-to-calculate-velocity/)                | $`max(time( \vert x_1-y_1 \vert), time( \vert x_2-y_2 \vert) ,..., time( \vert x_n-y_n \vert))`$|
|`TrapezoidTimeWithTieBreaker` | $`TrapezoidTime + 0.01 * (sumTime / n)`$             |
|                                | $`sumTime = time(x_1-y_1) + time(x_2-y_2) + time(x_n-y_n) `$             |
|`Manhattan`            |$`\sum_{i=1}^n{\vert a_i-b_i \vert}`$|
|`Matrix`                   | $`G = \begin{pmatrix} 0 & x \rightarrow y \\ y \rightarrow x & 0 \end{pmatrix}`$  |

<details>
<summary>Usage</summary>

```
//SEQ
DistanceFunction: Euclidian
DistanceFunction: Max       
DistanceFunction: Trapezoid 
DistanceFunction: TrapezoidTimeWithTieBreaker
DistanceFunction: Manhattan 
DistanceFunction: Matrix

//JSON
"DistanceFunction": {
    "Function": "Euclidian",
    "DistanceMatrix": null,
    "TrapezoidAcceleration": null,
    "TrapezoidSpeed": null
},

"DistanceFunction": {
    "Function": "Max",
    "DistanceMatrix": null,
    "TrapezoidAcceleration": null,
    "TrapezoidSpeed": null
},

"DistanceFunction": {
    "Function": "Manhattan",
    "DistanceMatrix": null,
    "TrapezoidAcceleration": null,
    "TrapezoidSpeed": null
},

"DistanceFunction": {
    "Function": "Trapezoid",
    "DistanceMatrix": null,
    "TrapezoidAcceleration": [1,1,1],
    "TrapezoidSpeed": [1,1,1]
},

"DistanceFunction": {
    "Function": "TrapezoidTimeWithTieBreaker",
    "DistanceMatrix": null,
    "TrapezoidAcceleration": [1,1,1],
    "TrapezoidSpeed": [1,1,1]
},

"DistanceFunction": {
        "Function": "Matrix",
        "DistanceMatrix": {
            "IDHeader": [0,1,2,3,4,5,6,7,8,9,10],
            "DistanceMatrix": [
                [0.0,1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0],
                [0.0,1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0],
                [0.0,1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0],
                [0.0,1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0],
                [0.0,1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0],
                [0.0,1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0],
                [0.0,1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0],
                [0.0,1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0],
                [0.0,1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0],
                [0.0,1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0]
            ],
            "NameFooter": ["@","S","Z","T","A","K","I","_","E","M","I"],
            "ResourceFooter": [0,0,0,0,0,0,0,0,2,2,2],
            "ConfigList": null
        },
        "TrapezoidAcceleration": null,
        "TrapezoidSpeed": null
    },

//XML
<DistanceFunction>
    <Function>Euclidian</Function>
</DistanceFunction>

<DistanceFunction>
    <Function>Max</Function>
</DistanceFunction>

<DistanceFunction>
    <Function>Trapezoid</Function>
    TODO:
</DistanceFunction>

<DistanceFunction>
    <Function>TrapezoidTimeWithTieBreaker</Function>
    TODO:
</DistanceFunction>

<DistanceFunction>
    <Function>Manhattan</Function>
</DistanceFunction>

<DistanceFunction>
    <Function>Matrix</Function>
    <DistanceMatrix>
      <IDHeader>
        <int>0</int>
        ...
        <int>10</int>
      </IDHeader>
      <DistanceMatrix>
        <ArrayOfDouble>
          <double>0</double>
            ...
          <double>10</double>
        </ArrayOfDouble>
        ...
        <ArrayOfDouble>
          <double>0</double>
          ...
          <double>10</double>
        </ArrayOfDouble>
      </DistanceMatrix>
      <NameFooter>
        <string>@</string>
        <string>S</string>
        <string>Z</string>
        <string>T</string>
        <string>A</string>
        <string>K</string>
        <string>I</string>
        <string>_</string>
        <string>E</string>
        <string>M</string>
        <string>I</string>
      </NameFooter>
      <ResourceFooter>
        <int>0</int>
        <int>0</int>
        <int>0</int>
        <int>0</int>
        <int>0</int>
        <int>0</int>
        <int>0</int>
        <int>0</int>
        <int>2</int>
        <int>2</int>
        <int>2</int>
      </ResourceFooter>
    </DistanceMatrix>
  </DistanceFunction>

//C#
```
    GeneralTask t = new GeneralTask();
    t.CostManager.DistanceFunction = new EuclidianDistanceFunction();
    t.CostManager.DistanceFunction = new MaxDistanceFunction();
    t.CostManager.DistanceFunction = new ManhattanDistanceFunction();
    t.CostManager.DistanceFunction = new TrapezoidTimeDistanceFunction(new double[] { }, new double[] { });
    t.CostManager.DistanceFunction = new TrapezoidTimeWithTimeBreakerDistanceFunction(new double[] { }, new double[] { });
    t.CostManager.DistanceFunction = new MatrixDistanceFunction(new List<List<double>>(), new List<int>());


</details>

#### TrapezoidAcceleration

|   Type    | Rule   |
| --------- | ------ |
| Double[ ] | `DistanceFunction` must be Trapezoid / TrapezoidTimeWithTieBreaker and the dimension must fit the dimension of configuration vectors.|

<details>
<summary>Usage</summary>

```
//SEQ
TrapezoidAcceleration: [9.1;8.7;5.5]

//JSON
//XML
<DistanceFunction>
    <Function>Trapezoid</Function>
    TODO:
</DistanceFunction>

<DistanceFunction>
    <Function>TrapezoidTimeWithTieBreaker</Function>
    TODO:
</DistanceFunction>


//C#
    GeneralTask t = new GeneralTask();
            t.CostManager.DistanceFunction = new TrapezoidTimeDistanceFunction(new double[] { }, new double[] { });
            t.CostManager.DistanceFunction = new TrapezoidTimeWithTimeBreakerDistanceFunction(new double[] { }, new double[] { });

```
</details>

#### TrapezoidSpeed

|   Type    | Rule   |
| --------- | ------ |
| Double[ ] | `DistanceFunction` must be Trapezoid / TrapezoidTimeWithTieBreaker and the dimension must fit the dimension of configuration vectors.  |

<details>
<summary>Usage</summary>

```
//SEQ
TrapezoidSpeed: [9.1;8.7;5.5]

//JSON
    TODO:

//XML
<DistanceFunction>
    <Function>Trapezoid</Function>
    TODO:
</DistanceFunction>

<DistanceFunction>
    <Function>TrapezoidTimeWithTieBreaker</Function>
    TODO:
</DistanceFunction>

//C#
    GeneralTask t = new GeneralTask();
            t.CostManager.DistanceFunction = new TrapezoidTimeDistanceFunction(new double[] { }, new double[] { });
            t.CostManager.DistanceFunction = new TrapezoidTimeWithTimeBreakerDistanceFunction(new double[] { }, new double[] { });
```
</details>

#### IdlePenalty

This value-added to the costs if the following motions are not in contact, and the tool path or motion must be interrupted.

|   Type    | Rule   |
| --------- | ------ |
| Double    |        |

<details>
<summary>Usage</summary>

```
//SEQ
IdlePenalty: 100 

//JSON
    "IdlePenalty": 100.0,

//XML
  <IdlePenalty>100</IdlePenalty>

//C#
    GeneralTask t = new GeneralTask();
    t.CostManager.IdlePenalty = 100;
```
</details>

#### BidirectionMotionDefault

If True, extra motion generated and added to the hierarchy with the _Reverse postfix in name, the new motions's ID is the inverse of the original one, and swapped end configurations in case of movements. If False, only the given direction added. At the end of `MotionList`, records can be overwritten one by one.

|   Value   | Rule   |
| --------- | ------ |
| True      |        |
| False     | Default. |

<details>
<summary>Usage</summary>

```
//SEQ
BidirectionMotionDefault: True      
BidirectionMotionDefault: False

//JSON
    "BidirectionMotionDefault": false,

//XML
    <BidirectionMotionDefault>false</BidirectionMotionDefault>

//C#
    GeneralTask t = new GeneralTask();
    t.Hierarchy.BidirectionalMotionDefault = true;

```
</details>

#### AddMotionLengthToCost

If true: The inner length of the motion (computed between each configuration in the motion) added to the cost of every input edge of the motion.

|   Value   | Rule   |
| --------- | ------ |
| True      |        |
| False     |  Default|

<details>
<summary>Usage</summary>

```
//SEQ
AddMotionLengthToCost: True      
AddMotionLengthToCost: False

//JSON
    "AddMotionLengthToCost": false,

//XML
    <AddMotionLengthToCost>false</AddMotionLengthToCost>

//C#
    GeneralTask t = new GeneralTask();
    t.CostManager.AddMotionLengthToCost = true;
```
</details>

## Sequence task definition

#### *ConfigList

Used when `DistanceFunction` is not matrix. List of records that defines all possible configurations of the task. These configurations can be used for hierarchy definition (`ProcessHierarchy`) at any times.

| Param                    | Type  | Rule   |
| ------------------------ | ----- | ------ |
| `ConfigID`      | Int | Every new value indicates a new config. Every id must be unique globally.|
| `Configuration` | Double[n] | The length of vectors should be equal in the full configuration list. Used in cost computation as `DistanceFunction` defines. |
| ~` Name`       | String | An optional list of config names. Any value acceptable, only used for identification in the solution or debug. |
| ~`ResourceID` | Int | Value is required if ResourceChangeover not `Off`. Must be part of `ResourceChangeover` if that is given. Used in cost computation, if resource change needed, more info at `ResourceChangeover`.|

<details>
<summary>Usage</summary>

```
//SEQ
ConfigList:
#ConfigID;Config;~Name;~ResourceID
1;[8.7;8.9];A;5
2;[5.7;1];B;5
3;[1.7;3];C;8

//JSON
    "ConfigList": [
        {
            "ID": 0,
            "Config": [],
            "Name": "@",
            "ResourceID": 0
        },
        ...        
        {
            "ID": 10,
            "Config": [],
            "Name": "I",
            "ResourceID": 2
        }
    ],

//XML
<ConfigList>
    <ConfigSerializationObject>
      <ID>0</ID>
      <Config />
      <Name>@</Name>
      <ResourceID>0</ResourceID>
    </ConfigSerializationObject>
   ...
    <ConfigSerializationObject>
      <ID>10</ID>
      <Config />
      <Name>I</Name>
      <ResourceID>2</ResourceID>
    </ConfigSerializationObject>
  </ConfigList>


//C#
    //Configuration added with the motions to the hierarchy.
    var CA = new Config() { ID = 1, Configuration = new List<double>() { 5, 5 },     Name = "Config A" };
    var CB = new Config() { ID = 2, Configuration = new List<double>() { 2, 2 },     Name = "Config B" };
    var CC = new Config() { ID = 3, Configuration = new List<double>() { 7.5, 7.5 }, Name = "Config C" };

    var MA = new Motion() { ID = 1, Name = "Motion A", Configs = new List<Config>() { CA } };
    var MB = new Motion() { ID = 2, Name = "Motion B", Configs = new List<Config>() { CB } };
    var MC = new Motion() { ID = 3, Name = "Motion C", Configs = new List<Config>() { CC } };

    t.Hierarchy.HierarchyRecords.Add(new HierarchyRecord()
    {
        Process = PA,
        Alternative = AA,
        Task = TA,
        Motion = MA
    }));

    t.Hierarchy.HierarchyRecords.Add(new HierarchyRecord()
    {
        Process = PB,
        Alternative = AB,
        Task = TB,
        Motion = MB,
    }));

    t.Hierarchy.HierarchyRecords.Add(new HierarchyRecord()
    {
        Process = PC,
        Alternative = AC,
        Task = TC,
        Motion = MC,
    }));
}
```
</details>

#### ConfigMatrix

Used in case of `DistanceFunction`: Matrix, excludes the use of `ConfigList`. Cost matrix with id header, optional name and resource id footer. The matrix should contain the cost of every pair of positions, pointless connections must be 0. These configurations can be used for hierarchy definition (`ProcessHierarchy`) at any times.

| Param              | Type  |Rule   |
| ------------------ | ----- |------ |
| Header (`ConfigID`[ ]) | Int[n]| Every new value indicates a new config. Every id must be unique in the scope of MotionList and PositionMatrix|
| Costs (`Double`[ ][ ])       | Double[n][n]| Dimension: n x n, n is the number of configs, should be equal to the length of the header. Cost of movement between configs. [Resource](#resource) changeover can be added. |
| ~`Name`[ ]           | String[n]| An optional list of config names. Any value acceptable, only used for identification in the solution or debug. |
| ~`ResourceID`[ ]     | Int[n]   | An optional list of resource ids should be equal to the length of the header is given. Must be part of `ResourceChangeover` if that is given. Used in cost computation, if resource change needed, more info at ResourceChangeover.|

<details>
<summary>Usage</summary>

```
//SEQ
ConfigMatrix: 
1;2;3
0;9;8
9;0;8
9;8;0
A;B;C
7;7;6

//JSON
  See Distance Function
//XML
  See Distance Function
//C#
  See Distance Function
```
</details>




#### *ProcessHierarchy

List   of ProcessID ; AlternativeID ; TaskID ; MotionID ; ConfigID; Bidirectional; Name. Every defined configuration (from `ConfigList` / `ConfigMatrix`) can be part of motions, and every motion need to be used and settled in a Task, Alternative and Process. The motions can be used as a single configuration or with multiple ones as a complex movement. The bidirectional and name property refers to the motion. 

| Param                    | Type | Rule   |
| ------------------------ | ---- |------ |
| `ProcessID` | Int | Every new value indicates a new process.|
| `AlternativeID` |  Int | Every new value with the same `ProcessID` indicates a new alternative sequence in the process.|
| `TaskID` | Int | Every new value with the same `AlternativeID` indicates a new task in the alternative. **Tasks have fix order in an alternative order by the given IDs ascendant.** |
| `MotionID` | Int |Must be part of `ConfigList`/`ConfigMatrix` in case of `Config` or `MotionList` in case of motion. **Every config and motion must be used once** (but configs can be used multiple times in motions).|
| `ConfigID` | Int |Must be part of `ConfigList`/`ConfigMatrix` in case of `Config` or `MotionList` in case of motion. **Every config and motion must be used once** (but configs can be used multiple times in motions).|
| `Bidirectionl` | Bool |Must be part of `ConfigList`/`ConfigMatrix` in case of `Config` or `MotionList` in case of motion. **Every config and motion must be used once** (but configs can be used multiple times in motions).|
| `Name` | String |Must be part of `ConfigList`/`ConfigMatrix` in case of `Config` or `MotionList` in case of motion. **Every config and motion must be used once** (but configs can be used multiple times in motions).|

<details>
<summary>Usage</summary>

```
//SEQ
ProcessHierarchy:
0;0;0;0
0;1;0;1

//JSON
"ProcessHierarchy": [
        {
            "ProcessID": 1,
            "AlternativeID": 0,
            "TaskID": 0,
            "MotionID": 1,
            "ConfigIDs": [
                1
            ],
            "Bidirectional": null,
            "Name": null
        },
        ...
        {
            "ProcessID": 10,
            "AlternativeID": 0,
            "TaskID": 0,
            "MotionID": 10,
            "ConfigIDs": [
                10
            ],
            "Bidirectional": null,
            "Name": null
        }
    ],

//XML

<ProcessHierarchy>
    <ProcessHierarchySerializationObject>
      <ProcessID>1</ProcessID>
      <AlternativeID>0</AlternativeID>
      <TaskID>0</TaskID>
      <MotionID>1</MotionID>
      <ConfigIDs>
        <int>1</int>
      </ConfigIDs>
      <Bidirectional xsi:nil="true" />
    </ProcessHierarchySerializationObject>
    ...
    <ProcessHierarchySerializationObject>
      <ProcessID>10</ProcessID>
      <AlternativeID>0</AlternativeID>
      <TaskID>0</TaskID>
      <MotionID>10</MotionID>
      <ConfigIDs>
        <int>10</int>
      </ConfigIDs>
      <Bidirectional xsi:nil="true" />
    </ProcessHierarchySerializationObject>
  </ProcessHierarchy>
    

//C#
    GeneralTask t = new GeneralTask();
    var PA = new Process() { ID = 1, Name = "Process A" };
    var AA  = new Alternative() { ID = 1, Name = "Alternative A" };
    var TA  = new Task() { ID = 1, Name = "Task A" };
    var CA = new Config() { ID = 1, Configuration = new List<double>() { 5, 5 },     Name = "Config A" };
    var MA = new Motion() { ID = 1, Name = "Motion A", Configs = new List<Config>() { CA } };

    t.Hierarchy.HierarchyRecords.Add(new HierarchyRecord()
    {
        Process = PA,
        Alternative = AA,
        Task = TA,
        Motion = MA
    }));
```
</details>

#### ProcessPrecedence

List of `ProcessID` ; `ProcessID` records. The positions in the process with the first `ProcessID` must be before the positions in the process with the second `ProcessID` in the solution. The definition must be a directed acyclic graph, and not defined processes are free. Process of `StartDepot` and `FinishDepot` can not be used.

| Param                    | Type | Rule   |
| ------------------------ | ---- | ------ |
| Predecessor(`ProcessID`) |  Int |Must be part of `ProcessHierarchy` as `ProcessID`. The Predecessor process items need to be before the successor process's items in the solution. The process of `StartDepot`, `FinishDepot` can not be used.|
| Successor  (`ProcessID`) |  Int |Must be part of `ProcessHierarchy` as `ProcessID` The Successor needs to be after the predecessor item in the solution if each selected. The process of `StartDepot, `FinishDepot` can not be used. |

<details>
<summary>Usage</summary>

```
//SEQ
ProcessPrecedence:
1;2
2;3

//JSON
ProcessPrecedence:[
    Precedence: {
         Predecessor: 1,
         Successor: 2
    },
    Precedence: {
         Predecessor: 2,
         Successor: 3
    }
]

//XML
    //TODO
    <ProcessPrecedences />

//C#
            GeneralTask t = new GeneralTask();
            var PA = new Process() { ID = 1, Name = "Process A" };
            var PB = new Process() { ID = 2, Name = "Process B" };
            t.ProcessPrecedences = new List<ProcessPrecedence>();
            t.ProcessPrecedences.Add(new ProcessPrecedence(PA, PB));

```
</details>


#### MotionPrecedence

The position or motion with the first id must be before the second id of position or motion in the solution if both positions selected. This representation of constraints does not have to be DAG. Configuration or motion of `StartDepot` and `FinishDepot` can not be used.

| Param                    | Type | Rule  |
| ------------------------ | ---- |------ |
| Predecessor(`MotionID`) | Int | Must be part of `ProcessHierarchy` as `MotionID`. The Predecessor needs to be before the successor item in the solution if each selected. The StartDepot, FinishDepot can not be used.|
| Successor  (`MotionID`) | Int | Must be part of `ProcessHierarchy` as `MotionID` The Successor needs to be after the predecessor item in the solution if each selected. The StartDepot, FinishDepot can not be used. |

<details>
<summary>Usage</summary>

```
//SEQ
MotionPrecedence:
1;2
2;3

//JSON
    TODO:
    MotionPrecedence:[
        Precedence: {
            Predecessor: 1,
            Successor: 2
        },
        Precedence: {
            Predecessor: 2,
            Successor: 3
        }
]

//XML
    TODO:
    <MotionPrecedence />
//C#
    GeneralTask t = new GeneralTask();
    var CA = new Config() { ID = 1, Configuration = new List<double>() { 5, 5 }, Name = "Config A" };
    var CB = new Config() { ID = 2, Configuration = new List<double>() { 2, 2 }, Name = "Config B" };
    var MA = new Motion() { ID = 1, Name = "Motion A", Configs = new List<Config>() { CA } };
    var MB = new Motion() { ID = 2, Name = "Motion B", Configs = new List<Config>() { CB } };
    t.MotionPrecedences = new List<MotionPrecedence>();
    t.MotionPrecedences.Add(new MotionPrecedence(MA, MB));

```
</details>

#### OverrideCost

The list of records that directly define A, B positions' cost overrides the result of `DistanceFunctions`. Bidirectional is false by default (applied only on A->B), else cost override also used for B->A. Different type of resource methods able to modify this overridden value. 

| Param                    | Type |Rule   |
| ------------------------ | ---- |------ |
| From (`ConfigID`/`MotionID`) | Int  | Must be part of `ProcessHierarchy` as `ConfigID` / `MotionID` |
| To (`ConfigID` / `MotionID`)   | Int | Must be part of `ProcessHierarchy` as `ConfigID` / `MotionID` |
| `Cost`                     | Double | It is override the original, DistanceFunction computed cost. ResourceChangeOver cost can be used additionaly. |
| `Bidirectional`           | Bool | Optional value, by default: false. If false, ovverides the cost of From->To. If true overrides cost of From->To and To->From too.|

<details>
<summary>Usage</summary>

```
//SEQ
OverrideCost:
1;2;99;True
2;3;98;False

//JSON
OverrideCosts: [
    OverrideCost:{
        From: 1,
        To:   2,
        Cost: 99,
        Bidirection: True
    },
    OverrideCost:{
        From: 1,
        To:   2,
        Cost: 99,
        Bidirection: True
    ]
}

//XML
    TODO:

//C#
    GeneralTask t = new GeneralTask();
    var CA = new Config() { ID = 1, Configuration = new List<double>() { 5, 5 }, Name = "Config A" };
    var CB = new Config() { ID = 2, Configuration = new List<double>() { 2, 2 }, Name = "Config B" };
    t.CostManager.OverrideCost.Add(new DetailedConfigCost() { A = CA, B = CB, OverrideCost = 2.0, Bidirectional = true });
```
</details>

## Resources

#### ResourceChangeover

| Value     | Include      | Exclude                                                        | 
| ------    | ------       | ------                                                         |
| !`Off`       |              |`ResourceChangeoverFunction`, `ChangeoverConstant`, `ChangeoverMatrix`| 
| `Constant`| `ResourceChangeoverFunction`, `ChangeoverConstant`   | ChangeoverMatrix             |
| `Matrix`| `ResourceChangeoverFunction`, `ChangeoverMatrix`       |`ChangeoverConstant`            |  

<details>
<summary>Usage</summary>

```
//SEQ
    ResourceChangeover: None
    ResourceChangeover: Constant
    ResourceChangeover: Matrix

//JSON
    "ResourceFunction": {
        "ResourceDistanceFunction": "Add",
        "ResourceSource": "Constant",
        "ResourceCostConstant": 50.0,
        "ResourceCostMatrix": null
    },

//XML
  <ResourceFunction>
    <ResourceSource>Constant</ResourceSource>
    <ResourceDistanceFunction>Add</ResourceDistanceFunction>
    <ResourceCostConstant>50</ResourceCostConstant>
  </ResourceFunction>

//C#
    GeneralTask t = new GeneralTask();
    t.CostManager.ResourceFunction = new NoResourceFunction();
    t.CostManager.ResourceFunction = new ConstantResourceFunction(2, new AddResourceDistanceLinkFunction());
    t.CostManager.ResourceFunction = new ConstantResourceFunction(2, new MaxResourceDistanceLinkFunction());
    t.CostManager.ResourceFunction = new MatrixResourceFunction(resChangeCostMatrix, resourceIDList, new AddResourceDistanceLinkFunction());
    t.CostManager.ResourceFunction = new MatrixResourceFunction(resChangeCostMatrix, resourceIDList, new MaxResourceDistanceLinkFunction());
```
</details>

#### ResourceChangeoverFunction



| Param | Rule   |
| ----- | ------ |
|!`Add`   | Every new value indicates a new config. Every id must be unique in the scope of MotionList and `PositionMatrix`  |
|`Max`    | The vector length should be equal to the value given in dimension. Used in cost computation as `DistanceFunction` defines. |

<details>
<summary>Usage</summary>

```
//SEQ
ResourceChangeoverFunction: Add
ResourceChangeoverFunction: Max

//JSON
    "ResourceFunction": {
        "ResourceDistanceFunction": "Add",
        "ResourceSource": "Constant",
        "ResourceCostConstant": 50.0,
        "ResourceCostMatrix": null
    },

//XML
  <ResourceFunction>
    <ResourceSource>Constant</ResourceSource>
    <ResourceDistanceFunction>Add</ResourceDistanceFunction>
    <ResourceCostConstant>50</ResourceCostConstant>
  </ResourceFunction>

//C#
    GeneralTask t = new GeneralTask();
    t.CostManager.ResourceFunction = new NoResourceFunction();
    t.CostManager.ResourceFunction = new ConstantResourceFunction(2, new AddResourceDistanceLinkFunction());
    t.CostManager.ResourceFunction = new ConstantResourceFunction(2, new MaxResourceDistanceLinkFunction());
    t.CostManager.ResourceFunction = new MatrixResourceFunction(resChangeCostMatrix, resourceIDList, new AddResourceDistanceLinkFunction());
    t.CostManager.ResourceFunction = new MatrixResourceFunction(resChangeCostMatrix, resourceIDList, new MaxResourceDistanceLinkFunction());
```
</details>

#### ChangeoverConstant

Constant cost of resource change. The resources are defined in the `ConfigList`/`ConfigMatrix` section.

| Type  | Rule   |
| ----- | ------ |
| Int   | No default. Every changeover of resources has this cost. |

<details>
<summary>Usage</summary>

```
//SEQ
ChangeoverConstant: 10


//JSON
    "ResourceFunction": {
        "ResourceDistanceFunction": "Add",
        "ResourceSource": "Constant",
        "ResourceCostConstant": 50.0,
        "ResourceCostMatrix": null
    },

//XML
  <ResourceFunction>
    <ResourceSource>Constant</ResourceSource>
    <ResourceDistanceFunction>Add</ResourceDistanceFunction>
    <ResourceCostConstant>50</ResourceCostConstant>
  </ResourceFunction>

//C#
    GeneralTask t = new GeneralTask();
    t.CostManager.ResourceFunction = new NoResourceFunction();
    t.CostManager.ResourceFunction = new ConstantResourceFunction(2, new AddResourceDistanceLinkFunction());
    t.CostManager.ResourceFunction = new ConstantResourceFunction(2, new MaxResourceDistanceLinkFunction());
```
</details>

#### ChangeoverMatrix

Resource changeover cost, for every pair of resources. Also contains a `ResourceID` header. The matrix should contain the distance/cost of every resource change. The resources are defined in the `ConfigList`/`ConfigMatrix` section.

| Param              | Type  |Rule   |
| ------------------ | ----- |------ |
| Header (`ResourceID`[ ]) | Int[n]| Every value indicates a resource. Every id must be unique in the scope. These values are usable at `ConfigList`/`ConfigMatrix`.  |
| Double[ ][ ]       | Double[n][n]| Dimension: n x n, n is the number of resources, should be equal to the length of the header. Cost of changeover between resources. |

<details>
<summary>Usage</summary>

```
//SEQ
ChangeoverMatrix:
5;6;7 #Header
0;9;8 #Matrix
9;0;8
9;8;0

//JSON
    "ResourceFunction": {
        "ResourceDistanceFunction": "Add",
        "ResourceSource": "Matrix",
        "ResourceCostMatrix": null TODO
    },

//XML
  <ResourceFunction>
    <ResourceSource>Matrix</ResourceSource>
    TODO:
  </ResourceFunction>

//C#
    GeneralTask t = new GeneralTask();
    t.CostManager.ResourceFunction = new NoResourceFunction();
    t.CostManager.ResourceFunction = new MatrixResourceFunction(resChangeCostMatrix, resourceIDList, new AddResourceDistanceLinkFunction());
    t.CostManager.ResourceFunction = new MatrixResourceFunction(resChangeCostMatrix, resourceIDList, new MaxResourceDistanceLinkFunction());
```
</details>

## Solver parameters

#### LocalSearchStrategy

Google OR-Tools VRP solver metaheuristics. [Reference](https://developers.google.com/optimization/routing/routing_options#local_search_options)


| Value              | Include      | Exclude      | Optional  |
| ------             | ------       | ------       | --------  |
| !Automatic          |   -          |   -          |  -        |
| GreedyDescent      |   -          |   -          |  -        |
| GuidedLocalSearch  |   -          |   -          |  -        |
| SimulatedAnnealing |   -          |   -          |  -        |
| TabuSearch         |   -          |   -          |  -        |
| ObjectiveTabuSearch|   -          |   -          |  -        |

<details>
<summary>Usage</summary>

```
//SEQ
LocalSearchStrategy: Automatic
LocalSearchStrategy: GreedyDescent
LocalSearchStrategy: GuidedLocalSearch  
LocalSearchStrategy: SimulatedAnnealing 
LocalSearchStrategy: TabuSearch         
LocalSearchStrategy: ObjectiveTabuSearch

//JSON
    "LocalSearchStrategy": "Automatic",
    "LocalSearchStrategy": "GreedyDescent",
    "LocalSearchStrategy": "GuidedLocalSearch",
    "LocalSearchStrategy": "SimulatedAnnealing",
    "LocalSearchStrategy": "TabuSearch",
    "LocalSearchStrategy": "ObjectiveTabuSearch",

//XML
  <LocalSearchStrategy>Automatic</LocalSearchStrategy>
  <LocalSearchStrategy>GreedyDescent</LocalSearchStrategy>
  <LocalSearchStrategy>GuidedLocalSearch</LocalSearchStrategy>
  <LocalSearchStrategy>SimulatedAnnealing</LocalSearchStrategy>
  <LocalSearchStrategy>TabuSearch</LocalSearchStrategy>
  <LocalSearchStrategy>ObjectiveTabuSearch</LocalSearchStrategy>

//C#
    t.SolverSettings.Metaheuristics = Metaheuristics.Automatic;
    t.SolverSettings.Metaheuristics = Metaheuristics.GreedyDescent;
    t.SolverSettings.Metaheuristics = Metaheuristics.GuidedLocalSearch;
    t.SolverSettings.Metaheuristics = Metaheuristics.SimulatedAnnealing;
    t.SolverSettings.Metaheuristics = Metaheuristics.TabuSearch;
    t.SolverSettings.Metaheuristics = Metaheuristics.ObjectiveTabuSearch;
                                                            
```
</details>

#### TimeLimit

The time limit for the VRP solver in [ms]. It has an effect only for the VRP solver, not the full runtime. Read, phrase, process, initial MIP solver not included. It can stop earlier with a solution or without through timeout.

| Type  | Rule   |
| ----- | ------ |
| Int   | Given in milliseconds and must be `TimeLimit` >=0. If TimeLimit = 0 stop based on VRP solver. If TimeLimit > 0, VRP solver stops after the time limit. |

<details>
<summary>Usage</summary>

```
//SEQ
TimeLimit: 0
TimeLimit: 60000

//JSON
    "TimeLimit": 10000

//XML
  <TimeLimit>10000</TimeLimit>

//C#
    GeneralTask t = new GeneralTask();
    t.SolverSettings.TimeLimit = 1000;
```
</details>

#### UseMIPprecedenceSolver

Creates an initial solution by CBC-MIP solver for tasks with complex precedence constraints.

| Param                    | Type  |Rule   |
| ------------------------ | ----- |------ |
| True | Bool | Runs MIP to create an initial solution for VRP. |
| False| Bool | -                                               |

<details>
<summary>Usage</summary>

```
//SEQ
UseMIPprecedenceSolver: True
UseMIPprecedenceSolver: False

//JSON
    "UseMIPprecedenceSolver": false

//XML
    <UseMIPprecedenceSolver>false</UseMIPprecedenceSolver>

//C#
    GeneralTask t = new GeneralTask();
    t.SolverSettings.UseMIPprecedenceSolver = 1000;
```
</details>

#### UseShortcutInAlternatives

In the case of long sequences of alternatives, finding the shortest paths and replacing the original alternative with a virtual one can be remuneratory. It contains only one task and a virtual single config representation of shortcuts. After the execution of sequencing, the original hierarchy is restored. Thus, reduce complexity, the number of nodes and constraints at build time.

| Param                    | Type  |Rule   |
| ------------------------ | ----- |------ |
| True | Bool | Compute shortest paths for every alternative. |
| !False| Bool | -                                               |

<details>
<summary>Usage</summary>

```
//SEQ
UseShortcutInAlternatives: True
UseShortcutInAlternatives: False

//JSON
    "UseShortcutInAlternatives": true,
    
//XML
    <UseShortcutInAlternatives>true</UseShortcutInAlternatives>

//C#
    GeneralTask t = new GeneralTask();
    t.SolverSettings.UseShortcutInAlternatives = 1000;
```
</details>
