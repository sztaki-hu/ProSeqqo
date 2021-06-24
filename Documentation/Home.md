## About
Generic task sequencer that captures typical sequencing problems encountered in robot applications.
* Easily integrated into complex solution workflows
* Standalone exe with file interface, DLL function calls, Docker contained REST
* Provides close-to-optimal (but not necessarily optimal) solutions quickly
* Compact solver based on an open-source VRP solver engine - Google-OR-Tools
* Arbitrary dimension - 2D, 3D or robotic joint space planning
* Build in cost functions - Euclidean, Max, Manhattan, Trapezoid Time, Matrix
* General description language to describe sequencing problems
* Order constraints
* Resource handling

Visual Studio 2019 Solution:
*  SequencePlanner - .NET 5.0 Class Library
*  SequenceConsole - .NET 5.0 Console Application
*  SequencerTest   - .NET 5.0 MSTest Framework Application
*  LineAnimation   - .NET 5.0 WPF Application
*  Example
*  Example/Skeletons
*  ~~SequencePlannerService - ASP.NET 5.0 REST Web Service with Docker support~~

## Task
Configurations defined in task space or robot configuration space, in arbitrary dimensions.
These configurations filled into a hierarchy, every configuration take place in a `Motion`, `Task`, `Alternative` and `Process`.
The distance between the configurations can be defined by a matrix or calculated automatically with the selected function.
The result of the execution is a list of Positions corresponds to the following: 
- **Each** of the n **Processes** has to be executed
- by selecting **one** of the given **Alternatives**…
- And executing **every Task** of the alternative…
- By visiting every **Configuration** of **one** possible **Motion** of the given task.

The given task translated to a general travelling salesman (GTSP) graph as an input of the Google-OR-Tools VRP solver.

#### Side constraints:
-  Precedence constraints between Motions
-  Precedence constraints between Processes
-  Only one motion used in a task (Disjunctive constraint, generated automatically)
-  Only one alternative used in a process (Disjunctive constraint, generated automatically)


## Install:
- Standalone executable
- Dynamic Link Library (DLL)
- Visual Studio 2019 Solution
- <del>Docker Container - REST API</del>

[More detailed instructions about installation.](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/Installation)

## Hello World:



## Documentation:



## Contributing:
If you find any bugs, please report them! I am also happy to accept pull requests from anyone.<br>
You can use the issue tracker to report bugs, ask questions, suggest new features or personally:<br>
Kovács András - kovacs.andras@sztaki.hu<br>
Zahorán László - zahoran.laszlo@sztaki.hu

## License:
>    Copyright 2021 SZTAKI EMI
> 
>    Licensed under the Apache License, Version 2.0 (the "License");
>    you may not use this file except in compliance with the License.
>    You may obtain a copy of the License at
> 
>      http://www.apache.org/licenses/LICENSE-2.0
> 
>    Unless required by applicable law or agreed to in writing, software
>    distributed under the License is distributed on an "AS IS" BASIS,
>    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
>    See the License for the specific language governing permissions and
>    limitations under the License.