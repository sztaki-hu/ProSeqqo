# ![ProSeqqo Logo](../Documentation/Images/ProSeqqoLogo.png) [ProSeqqo](../Documentation/Readme.md) 
# Result format
The executed tasks ([definition](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/Task-definition-file)) are also available in different formats, JSON, XML, SEQ, TXT. 
## Parameters
| Parameter     | Comment                                                     |
| --------------| ----------------------------------------------------------- |
| Created       | Date and time of generation.                                |
| FullTime      | Time of task execution.                                     |
| SolverTime    | Time of OR-Tools VRP execution.                             |
| PreSolverTime | Time of MIP pre-solver, initial solution creation.          |
| SolutionRaw   | List of position/line UserIDs as the solution of the task.  |
| CostsRaw      | List of double cost between the SolutionRaw positions/lines.|
| CostSum       | Sum of CostsRaw.                                            |
| StatusCode    | Final status code of OR-Tools VRP.                          |
| StatusMessage | Final status message of OR-Tools VRP.                       |
| Log           | Log messages of the solver, detailed as the given log level.|

## Parameters2
| Parameter     | Comment                                                     |
| --------------| ----------------------------------------------------------- |
| Created       | Date and time of generation.                                |
| FullTime      | Time of task execution.                                     |
| SolverTime    | Time of OR-Tools VRP execution.                             |
| PreSolverTime | Time of MIP pre-solver, initial solution creation.          |
| StatusCode    | Final status code of OR-Tools VRP.                          |
| StatusMessage | Final status message of OR-Tools VRP.                       |
| ErrorMessage  | Error message execution.                                    |
| Log           | Log messages of the solver, detailed as the given log level.|

Example [.SEQ](https://git.sztaki.hu/zahoranl/sequenceplanner/-/blob/master/Example/LineLike_Original.txt) and its [JSON](https://git.sztaki.hu/zahoranl/sequenceplanner/-/blob/master/Example/Out/LineLike_Original.json), [XML](https://git.sztaki.hu/zahoranl/sequenceplanner/-/blob/master/Example/Out/LineLike_Original.xml), [SEQ ](https://git.sztaki.hu/zahoranl/sequenceplanner/-/blob/master/Example/Out/LineLike_Original.seq) result. 


## Use
### Command Line Interface
In the case of Sequencer.exe [CLI command](https://git.sztaki.hu/zahoranl/sequenceplanner/-/wikis/Run-from-file) -out/-o parameter write down the access path and format of the result. By default, the output format and the path is the same with `_out` postfix.  
Example:  
`>Sequencer.exe -i C:/task.txt` -->  Creates C:/task_out.txt with SEQ format.   
`>Sequencer.exe -i C:/task.json` -->  Creates C:/task_out.json.   
`>Sequencer.exe -i C:/task.seq -o X:/result.xml` -->  Creates X:/result.xml. 
### DLL and Code
In the case of DLL/VS solution reuse SequencePlanner.GTSPTask.Serialization.Result namespace contains `LineLikeResultSerializer` and `PointLikeResultSerializer`. Both class contains `ExportSEQ`, `ExportJSON`, `ExportXML `methods with PointTaskResult/LineTaskResult and sting path parameter. On the other hand `ImportSEQ`, `ImportJSON`, `ImportXML `with string path parameter returs PointTaskResult/LineTaskResult.
### Docker-REST API

