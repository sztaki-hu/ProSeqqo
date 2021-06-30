# ![ProSeqqo Logo](../Documentation/Images/ProSeqqoLogo.png) [ProSeqqo](../Documentation/Readme.md) 
# Result format
The result of the tasks are also available in different formats, .json, .xml, .seq, .txt. The paramters of task definition are available [here](../Documentation/ResultDefinition.md).
## Parameters

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


## Use
### Command Line Interface
In the case of ProSeqqoConsole.exe [CLI command](../Example/HelloWorld) -out/-o parameter write down the access path and format of the result. By default, the output format and the path is the same with `_out` postfix.  
Example:  
`>ProSeqqoConsole.exe -i C:/task.txt` -->  Creates C:/task_out.txt with SEQ format.   
`>ProSeqqoConsole.exe -i C:/task.json` -->  Creates C:/task_out.json.   
`>ProSeqqoConsole.exe -i C:/task.seq -o X:/result.xml` -->  Creates X:/result.xml. 
### DLL and Code
In the case of DLL/VS solution reuse SequencePlanner.GTSPTask.Serialization.Result namespace contains `LineLikeResultSerializer` and `PointLikeResultSerializer`. Both class contains `ExportSEQ`, `ExportJSON`, `ExportXML `methods with PointTaskResult/LineTaskResult and sting path parameter. On the other hand `ImportSEQ`, `ImportJSON`, `ImportXML `with string path parameter returs PointTaskResult/LineTaskResult.
### Docker-REST API

