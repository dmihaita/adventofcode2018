
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

public class Day07p2 {
    private string inputPath;
    private SleighGraph graph;
    public Day07p2(string inputPath){
        this.inputPath = inputPath;
        graph = new SleighGraph();        
        ReadInput();
    }

    public long Solve(){
        long res = graph.GetWorkingSort(5);
        Console.WriteLine(res);
        return res;
    }
    private void ReadInput()
    {
        string line;
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){
            while((line = file.ReadLine()) != null)
            {
                char node1 = line[5];
                char node2 = line[36];
                graph.Edges.Add(Tuple.Create(node1, node2));
                if (!graph.Nodes.Any(t => t.Item1 == node1))
                    graph.Nodes.Add(Tuple.Create(node1, Weight(node1)));
                if (!graph.Nodes.Any(t => t.Item1 == node2))
                    graph.Nodes.Add(Tuple.Create(node2, Weight(node2)));
            }
            file.Close();
        }
    }
    private int Weight(char node2)
    {
        return node2 - 'A' + 61;
    }
}
