

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc2018.Day15p1{
    public class Day15p1 {
        private string inputPath;
        private GoblinMap map;
        public Day15p1(string inputPath){
            this.inputPath = inputPath;
            ReadInput();
        }

        public int Solve(){
            Debug.Write(map.Print());
            return 0;
        }

        private void ReadInput()
        {
            string line;
            char[,] maze = null;
            using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){
                while((line = file.ReadLine()) != null)
                {
                    if (maze == null){
                        maze = new char[1, line.Length];
                    }
                    else{
                        char[,] newMaze = new char[maze.GetLength(0) + 1, maze.GetLength(1)];
                        Buffer.BlockCopy(maze, 0, newMaze, 0, maze.GetLength(0) * maze.GetLength(1) * sizeof(char));
                        maze = newMaze;
                    }
                    Buffer.BlockCopy(line.ToCharArray(), 0, maze,(maze.GetLength(0) - 1) * maze.GetLength(1) * sizeof(char), line.Length * sizeof(char));
                }
                file.Close();
            }
            map = new GoblinMap(maze);
        }
    }

    class GoblinMap{
        private const int INITIAL_POWER = 200;
        char[,] _map;
        int currentIteration;
        Dictionary<ValueTuple<int, int, char>, int> attackingUnits;
        public GoblinMap(char[,] map){
            _map = map;
            currentIteration = 0;
            attackingUnits = new Dictionary<ValueTuple<int, int, char>, int>();
        }

        public void IterateRound(){
            IEnumerable<Tuple<char, int, int>> units = CollectUnits();
            foreach(var unit in units){
                if (attackingUnits.ContainsKey(ValueTuple.Create(unit.Item2, unit.Item3, unit.Item1)))
                {
                    Tuple<char, int, int> adjacentUnit = GetUnitAdjacentEnemyUnit(unit);
                    UnitAttackEnemy(unit, adjacentUnit);
                }
                else{
                    var nextPosition = ComputeNextUnitPosition(unit);
                    if (nextPosition != null){
                        MoveUnit(unit, nextPosition);
                        if (UnitHasAdjacentEnemies(unit)){
                            attackingUnits.Add(ValueTuple.Create(nextPosition.Item1, nextPosition.Item2, unit.Item1), INITIAL_POWER);
                        }
                    }
                }
            }
            currentIteration++;
        }

        private void MoveUnit(Tuple<char, int, int> unit, Tuple<int,int> nextPosition)
        {
            _map[unit.Item2, unit.Item3] = '.';
            _map[nextPosition.Item1, nextPosition.Item2] = unit.Item1;
        }

        private Tuple<int, int> ComputeNextUnitPosition(Tuple<char, int, int> unit)
        {
            var path = ComputeShortestPathToEnemy(unit);
            if (path != null && path.Count() > 0){
                return path.First();
            }
            else
                return null;
        }

        IEnumerable<Tuple<int, int>> ComputeShortestPathToEnemy(Tuple<char, int, int> unit){
            var graph = MapToGraph();
            return null;// graph.shortest_path().Select(t => (t.Item2, t.Item3)).FirstOrDefault();
        }

        private Graph<ValueTuple<char, int, int>> MapToGraph()
        {
            Graph<ValueTuple<char, int, int>> graph = new Graph<(char, int, int)>();
            return graph;
            
        }

        private void UnitAttackEnemy(Tuple<char, int, int> unit, Tuple<char, int, int> adjacentUnit)
        {
            
        }

        private Tuple<char, int, int> GetUnitAdjacentEnemyUnit(Tuple<char, int, int> unit)
        {
            return null;            
        }

        private bool UnitHasAdjacentEnemies(Tuple<char, int, int> unit)
        {
            int targetUnit = unit.Item1 == 'G' ? 'E' : 'G';
            return (
                (unit.Item2 > 0 && _map[unit.Item2 - 1, unit.Item3] == targetUnit) ||
                (unit.Item2 < _map.GetLength(0) && _map[unit.Item2 + 1, unit.Item3] == targetUnit) ||
                (unit.Item3 > 0 && _map[unit.Item2, unit.Item3 - 1] == targetUnit) ||
                (unit.Item3 < _map.GetLength(1) && _map[unit.Item2, unit.Item3 + 1] == targetUnit)
            );
        }

        private IEnumerable<Tuple<char, int, int>> CollectUnits()
        {
            List<Tuple<char,int, int>> res = new List<Tuple<char, int, int>>();
            for(int i = 0; i< _map.GetLength(0); i++){
                for(int j = 0; j< _map.GetLength(1); j++){
                    if (_map[i,j] == 'G' || _map[i,j] == 'E'){
                        res.Add(Tuple.Create(_map[i,j], i, j));
                    }
                }
            }
            return res.OrderBy(t => t.Item2).ThenBy(t => t.Item3);
        }

        internal string Print()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i< _map.GetLength(0); i++){
                for(int j = 0; j< _map.GetLength(1); j++){
                    sb.Append(_map[i,j]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    class Graph<T> where T: IComparable
    {
        Dictionary<T, Dictionary<T, int>> vertices = new Dictionary<T, Dictionary<T, int>>();

        public void add_vertex(T name, Dictionary<T, int> edges)
        {
            vertices[name] = edges;
        }

        public List<T> shortest_path(T start, T finish)
        {
            var previous = new Dictionary<T, T>();
            var distances = new Dictionary<T, int>();
            var nodes = new List<T>();

            List<T> path = null;

            foreach (var vertex in vertices)
            {
                if (vertex.Key.CompareTo(start) == 0)
                {
                    distances[vertex.Key] = 0;
                }
                else
                {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add(vertex.Key);
            }

            while (nodes.Count != 0)
            {
                nodes.Sort((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove(smallest);

                if (smallest.CompareTo(finish) == 0)
                {
                    path = new List<T>();
                    while (previous.ContainsKey(smallest))
                    {
                        path.Add(smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue)
                {
                    break;
                }

                foreach (var neighbor in vertices[smallest])
                {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path;
        }
    }    
}