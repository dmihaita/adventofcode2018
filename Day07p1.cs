
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

public class Day07p1 {
    private string inputPath;
    private SleighGraph graph;
    public Day07p1(string inputPath){
        this.inputPath = inputPath;
        graph = new SleighGraph();        
        ReadInput();
    }

    public string Solve(){
        string res = graph.GetTopologicalSort();
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
                if (!graph.Nodes.Contains(node1))
                    graph.Nodes.Add(node1);
                if (!graph.Nodes.Contains(node2))
                    graph.Nodes.Add(node2);
            }
            file.Close();
        }
    }
}

public class SleighGraph{
    HashSet<char> nodes;
    HashSet<Tuple<char, char>> edges;
    public SleighGraph(){
        nodes = new HashSet<char>();
        edges = new HashSet<Tuple<char, char>>();
    }
    public HashSet<char> Nodes {get{return nodes;}}
    public HashSet<Tuple<char,char>> Edges {get{return edges;}}

    public string GetTopologicalSort(){
        var res = TopologicalSort<char>(nodes, edges);
        return new string(res.ToArray());
    }
     static List<T> TopologicalSort<T>(HashSet<T> nodes, HashSet<Tuple<T, T>> edges) 
                where T : IEquatable<T> 
     {
        // Empty list that will contain the sorted elements
        var L = new List<T>();
        // Set of all nodes with no incoming edges
        var S = new HashSet<T>(nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false)));
        // while S is non-empty do
        while (S.Any()) {
            //  remove a node n from S
            var n = S.OrderBy(t => t).First();
            S.Remove(n);
            // add n to tail of L
            L.Add(n);

            // for each node m with an edge e from n to m do
            foreach (var e in edges.Where(e => e.Item1.Equals(n)).ToList()) {
                var m = e.Item2;
                // remove edge e from the graph
                edges.Remove(e);
                // if m has no other incoming edges then
                if (edges.All(me => me.Item2.Equals(m) == false)) {
                    // insert m into S
                    S.Add(m);
                }
            }
        }

        // if graph has edges then
        if (edges.Any()) {
            // return error (graph has at least one cycle)
            return null;
        } else {
            // return L (a topologically sorted order)
            return L;
        }
    }
}
