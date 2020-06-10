using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SequencePlanner.GTSP
{
    public static class GraphViz
    {
        public static string CreateGraphViz(GTSPRepresentation g, string file = null, bool virtualNodes = true)
        {
            string viz = "digraph G {\n";
            viz += "node[style = filled];\n";
            viz += "\n\t" + "style = filled;";
            viz += "\n\t" + "color = salmon2;";
            foreach (var item in g.Graph.Edges)
            {
                viz += "\t" + item.NodeA.Name + " -> " + item.NodeB.Name + "[label = " + item.Weight.ToString("0,0.00", new CultureInfo("en-US", false)) + "];\n";
            }

            foreach (var proc in g.Processes)
            {
                string subgraph = "";
                if (virtualNodes)
                {
                    foreach (var pos in g.Positions)
                    {
                        if (proc.ID == pos.Process.ID && InEdges(g.Graph, pos.ID))
                        {
                            subgraph += pos.Name + "; ";
                        }
                    }
                }
                else
                {
                    foreach (var pos in g.Positions)
                    {
                        if (proc.ID == pos.Process.ID)
                        {
                            var find = false;
                            foreach (var alt in g.Alternatives)
                            {
                                if (pos.ID == alt.Start.ID || pos.ID == alt.Finish.ID)
                                {
                                    find = true;
                                }
                            }
                            if (!find && InEdges(g.Graph, pos.ID))
                                subgraph += pos.Name + "; ";
                        }
                    }
                }
                viz += AddSubgraph(subgraph, proc.Name, "", "thistle3");

                if (virtualNodes)
                {
                    foreach (var alt in proc.Alternatives)
                    {
                        string subgraphAlt = "\t";
                        foreach (var pos in g.Positions)
                        {
                            if (proc.ID == pos.Process.ID && pos.Alternative?.ID == alt.ID)
                            {
                                if (InEdges(g.Graph, pos.ID))
                                    subgraphAlt += pos.Name + "; ";
                            }
                        }
                        viz += AddSubgraph(subgraphAlt, alt.Name, "\t\t", "lightblue");
                        viz += AddTasksSubgraph(alt);
                        viz += "\n\t\t}\n";
                    }
                }
                viz += "\n\t}\n";
            }
            viz += "}";
            //Console.WriteLine(viz);
            //if (file!=null)
            System.IO.File.WriteAllText(file, viz);
            return viz;
        }

        private static string AddSubgraph(string content, string label, string prefix = "\t", string color = "white")
        {
            string viz = "";
            viz += "\n" + prefix + "subgraph cluster_" + label + "{";
            viz += "\n\t" + prefix + "style = filled;";
            viz += "\n\t" + prefix + "color = " + color + ";";
            viz += "\n\t" + prefix + "label = " + label + ";\n\t\t";
            viz += content;
            return viz;
        }
        private static string AddTasksSubgraph(Alternative a)
        {
            string tmp = "";
            foreach (var task in a.Tasks)
            {
                tmp += "subgraph cluster_" + task.Name + "{";
                tmp += "\nstyle = filled;";
                tmp += "\ncolor = peachpuff1 ;";
                tmp += "\nlabel = " + task.Name + ";";
                foreach (var pos in task.Positions)
                {
                    tmp += pos.Name + ";";
                }
                tmp += "}\n";
            }
            return tmp;
        }

        private static bool InEdges(GraphRepresentation g, int ID)
        {
            foreach (var edge in g.Edges)
            {
                if (edge.NodeA.ID == ID || edge.NodeB.ID == ID)
                    return true;

            }
            return false;
        }

    }
}
