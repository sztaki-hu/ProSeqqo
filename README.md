:earth_americas: [Wiki](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/home)<br>
:clipboard: [Snippets](https://git.sztaki.hu/zahoranl/sequenceplanner/snippets)<br>
:computer: [Use from code](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/Run-from-code) <br>
:file_folder: [Use from file](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/Run-from-file)  <br>
:dart: [Pick and Place example](https://git.sztaki.hu/zahoranl/sequenceplanner/snippets/18)  <br>
:paperclip: [Project background]()  <br>


## About
Generic task sequencer that captures typical sequencing problems encountered in robot applications.
* Easily integrated into complex solution workflows
* Standalone exe with file interface or DLL function calls
* Provides close-to-optimal (but not necessarily optimal) solutions quickly, e.g., in <1 sec for typical cases
* Compact solver based on an open-source VRP solver engine - Google-OR-Tools
* Visualize created graph with GraphViz



#### Semantics: 
- **Each** of the n **Processes** has to be executed
- by selecting **one** of the given **Alternatives**…
- And executing **every Task** of the alternative…
- By visiting **one** possible **Position** of the given task.

#### Positions:
- Defined in task space or robot configuration space, in arbitrary dimensions
- Distances (time or space) between them can be calculated using some function

#### Side constraints:
-  Precedence constraints between Positions
-  Precedence constraints between Processes
-  Only one position used in a task
-  Only one alternative used in a process

#### Abstraction:
**Nodes** → Positions \
**Classes** → Set of all positions of a task, union overall alternatives of a process\
**Edges** → From every position of a task to every position of the next task of the same alternative. From every position of the last task of an alternative to every position of the first task of all other processes \
**Edge weights** → Implemented common distance functions (e.g., max, Euclidean, trapezoid speed, etc.). Distance matrix (in case of complex paths between positions).

 
![rawgraph2](/uploads/636d217563250509f8eff13a35f6c8d5/rawgraph2.png)



Installation:
------

>  Under heavy development
¯\＿(ツ)＿/¯

- **Standalone executable:**
Download the latest [release](https://git.sztaki.hu/zahoranl/sequenceplanner/-/releases) of the project, unzip and run in PowerShell/cmd on created tasks. Find details of task descriptor file [here](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/Run-from-file).

- **Reuse as .dll (Dynamic Link Library):**
Download the latest [release](https://git.sztaki.hu/zahoranl/sequenceplanner/-/releases) and add Sequencer.dll to your project as a dependency.

- **Use source:**
Clone the `master` branch of the project, it always contains the source code of the latest stable version as Visual Studio Solution.

- Other platforms: The project and the given artefacts are supported on Windows x64/x86. In specific case .NET Standard part able to build on Linux and OSX.

[More detailed instructions about installation.](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/Install)


Contributing:
------
If you find any bugs, please report them! I am also happy to accept pull requests from anyone.<br>
You can use the [GitLab issue tracker](https://git.sztaki.hu/zahoranl/pathplanner/issues) to report bugs, ask questions, suggest new features or personally:<br>
Kovács András - kovacs.andras@sztaki.hu<br>
Zahorán László - zahoran.laszlo@sztaki.hu
