
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
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
        return 'A' - node2 + 1;
    }
}

public class SleighGraph{
    HashSet<Tuple<char,int>> nodes;
    HashSet<Tuple<char, char>> edges;
    public SleighGraph(){
        nodes = new HashSet<Tuple<char, int>>();
        edges = new HashSet<Tuple<char, char>>();
    }
    public HashSet<Tuple<char, int>> Nodes {get{return nodes;}}
    public HashSet<Tuple<char,char>> Edges {get{return edges;}}

    public string GetTopologicalSort(){
        var res = TopologicalSort<char>(nodes, edges);
        return new string(res.ToArray());
    }
    internal long GetWorkingSort(int workers)
    {
        var res = WorkingSort<char>(nodes, edges, workers);
        return res;
    }
    static long WorkingSort<T>(HashSet<Tuple<T, int>> nodes, HashSet<Tuple<T, T>> edges,
         int workersCount) 
                where T : IEquatable<T> 
     {
        // Empty list that will contain the sorted elements
        long duration = -1;
        // Set of all nodes with no incoming edges
        var S = new HashSet<T>(
                nodes.Where(n => edges.All(e => e.Item2.Equals(n.Item1) == false)).
            Select(t => t.Item1)).OrderBy(t => t).ToList();
        // while S is non-empty do
        Workers<T> workers = new Workers<T>(workersCount);
        while(true){
            duration++;
            foreach(var worker in workers.Items){
                if (worker.Finished(duration)){
                    foreach (var e in edges.Where(e => e.Item1.Equals(worker.Node)).ToList()) {
                        var m = e.Item2;
                        // remove edge e from the graph
                        edges.Remove(e);
                        // if m has no other incoming edges then
                        if (edges.All(me => me.Item2.Equals(m) == false)) {
                            // insert m into S
                            S.Add(m);
                        }
                    }
                    worker.Free();
                }
            }
            if (S.Any()) {
                while(S.Any() && workers.GetFreeWorker() != null){
                    var n = S.First();
                    workers.GetFreeWorker().Start(n, 
                        nodes.First(t => t.Item1.Equals(n)).Item2 ,
                        duration);
                    S.Remove(n);
                }
            }
            else
            {
                if (workers.FreeCount == workersCount)
                    break;
            }
            Debug.WriteLine($"{duration} - {workers.ToString()}");
        }

        // if graph has edges then
        if (edges.Any()) {
            // return error (graph has at least one cycle)
            return -1;
        } else {
            // return L (a topologically sorted order)
            return duration;
        }
    }

    private static int GetAvailableWorker(int[] workersQueue)
    {
        for(int i = 0; i < workersQueue.Length; i++)
        {
            if (workersQueue[i] == 0)
                return i;
        }
        return -1;
    }

    static List<T> TopologicalSort<T>(HashSet<Tuple<T, int>> nodes, HashSet<Tuple<T, T>> edges) 
                where T : IEquatable<T> 
     {
        // Empty list that will contain the sorted elements
        var L = new List<T>();
        // Set of all nodes with no incoming edges
        var S = new HashSet<T>(nodes.Where(n => edges.All(e => e.Item2.Equals(n.Item1) == false)).Select(t => t.Item1));
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

public class Workers<T>{

    Worker<T>[] workers;
    public Workers(int count){
        workers = new Worker<T>[count];
        for(int i = 0; i < count; i++)
            workers[i] = new Worker<T>(); 
    }

    public int FreeCount{
        get{return workers.Count(t => t.IsFree);}
    }

    public Worker<T>[] Items{
        get{return this.workers;}
    }

    public Worker<T> GetFreeWorker(){
        return workers.FirstOrDefault(t => t.IsFree);
    }

    public override string ToString(){
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < workers.Length; i++){
            sb.Append(workers[i].IsFree ? "." : Convert.ToString(workers[i].Node));
            sb.Append("   ");
        }
        return sb.ToString();
    }
}

public class Worker<T>{
    T key;
    long start, end;
    public Worker(){

    }

    bool started = false; 
    public void Start(T key, int weigth, long timestamp){
        this.key = key;
        this.start = timestamp;
        this.end = this.start + weigth;
        started = true;
    }

    public T Node{
        get{return key;}
    }

    public void Free(){
        this.key = default(T);
        this.start = this.end = 0;
        started = false;
    }

    public bool IsFree{
        get {return !started;}
    }

    public bool Finished(long timestamp){
        return this.started && this.end <= timestamp;
    }

}