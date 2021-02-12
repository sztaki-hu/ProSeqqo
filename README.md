[![pipeline status](https://git.sztaki.hu/zahoranl/sequenceplanner/badges/release_0.3/pipeline.svg)](https://git.sztaki.hu/zahoranl/sequenceplanner/-/commits/release_0.3)
[![coverage report](https://git.sztaki.hu/zahoranl/sequenceplanner/badges/release_0.3/coverage.svg)](https://git.sztaki.hu/zahoranl/sequenceplanner/-/commits/release_0.3)

:earth_americas: [Wiki](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/home)<br>
:clipboard: [Snippets](https://git.sztaki.hu/zahoranl/sequenceplanner/snippets)<br>
:file_folder: [Use from file](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/Run-from-file)<br>
:dart: [Pick and Place example](https://git.sztaki.hu/zahoranl/sequenceplanner/snippets/18)  <br>
<!--:paperclip: [Project background]()<br>-->
<!--:computer: [Use from code](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/Run-from-code) <br>-->

## About
>  Under heavy development
¯\＿(ツ)＿/¯

Generic task sequencer that captures typical sequencing problems encountered in robot applications.
* Easily integrated into complex solution workflows
* Standalone exe with file interface or DLL function calls
* Provides close-to-optimal (but not necessarily optimal) solutions quickly, e.g., in <1 sec for typical cases
* Compact solver based on an open-source VRP solver engine - Google-OR-Tools
* Visualize created graph with GraphViz


Two dedicated task type for point-to-point workflow (point-like) and line-like optimization.

## Point-like task
List of `Positions` given with `ID`, `Point` and `Name`. Points defined in task space or robot configuration space, in arbitrary dimensions. These positions filled into a hierarchy, every position take place in a `Task`, `Alternative` and `Process` (Position with the same `Point` used in multiple places have to be duplicated manually). The distance between the point of positions can be defined by a matrix or calculated automatically with the selected function. The result of the execution is a list of Positions corresponds to the following: 
- **Each** of the n **Processes** has to be executed
- by selecting **one** of the given **Alternatives**…
- And executing **every Task** of the alternative…
- By visiting **one** possible **Position** of the given task.

#### Side constraints:
-  Precedence constraints between Positions
-  Precedence constraints between Processes
-  Only one position used in a task (Disjunctive constraint, generated automatically)
-  Only one alternative used in a process (Disjunctive constraint, generated automatically)

#### Abstraction:
The given task translated to a general travelling salesman (GTSP) graph as an input of the Google-OR-Tools.

**Nodes** → Positions  
**GTSP Classes** → Set of all positions of a task, union overall alternatives of a process\
**Edges** → From every position of a task to every position of the next task of the same alternative. From every position of the last task of an alternative to every position of the first task of all other processes \
**Edge weights** → Implemented common distance functions (e.g., max, Euclidean, trapezoid speed, etc.). Distance matrix (in case of complex paths between positions).

![rawgraph2](/uploads/636d217563250509f8eff13a35f6c8d5/rawgraph2.png)

## Line-like task
List of `Positions` given with `ID`, `Point` (n-dimensional vector) and `Name`. Points defined in task space or robot configuration space, in arbitrary dimensions. These positions filled into a hierarchy,  a `Line` is a position and belongs to a `Countour` (You can use positions in multiple times without duplication). The distance between the start and end position of lines can be defined by a matrix or calculated automatically with the selected function. The result of the execution is a list of Lines corresponds to the following: 

#### Side constraints:
-  Precedence constraints between Lines
-  Precedence constraints between Contours

#### Abstraction:
The given task translated to a general travelling salesman (GTSP) graph as an input of the Google-OR-Tools.

**Nodes** → Lines (If the bidirectional visit of lines allowed, lines are duplicated.)  
**Edges** → From every end of a line to every start of others.  
**Edge weights** → Implemented common distance functions (e.g., max, Euclidean, trapezoid speed, etc.). Distance matrix (in case of complex paths between lines).

Installation:
------


- **Standalone executable:**
Download the latest [release](https://git.sztaki.hu/zahoranl/sequenceplanner/-/releases) of the project, unzip and run in PowerShell/cmd on created tasks. Find details of task descriptor file [here](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/Run-from-file).

- **Reuse as .dll (Dynamic Link Library):**
Download the latest [release](https://git.sztaki.hu/zahoranl/sequenceplanner/-/releases) and add Sequencer.dll to your project as a dependency.

- **Use source:**
Clone the `master` branch of the project, it always contains the source code of the latest stable version as Visual Studio Solution.

- Other platforms: The project and the given artefacts are supported on Windows x64/x86. In specific case .NET Standard part able to build on Linux and OSX.

[More detailed instructions about installation.](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/Installation)


Contributing:
------
If you find any bugs, please report them! I am also happy to accept pull requests from anyone.<br>
You can use the [GitLab issue tracker](https://git.sztaki.hu/zahoranl/pathplanner/issues) to report bugs, ask questions, suggest new features or personally:<br>
Kovács András - kovacs.andras@sztaki.hu<br>
Zahorán László - zahoran.laszlo@sztaki.hu
