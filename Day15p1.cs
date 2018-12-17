

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
        char[,] _map;
        int currentIteration;
        public GoblinMap(char[,] map){
            _map = map;
            currentIteration = 0;
        }

        public void IterateRound(){
            IEnumerable<Tuple<char, int, int>> units = CollectUnits();
            foreach(var unit in units){
                if (UnitHasAdjacentEnemies(unit)){
                    Tuple<char, int, int> adjacentUnit = GetUnitAdjacentEnemyUnit(unit);
                    UnitAttackEnemy(unit, adjacentUnit);
                }
                else{
                    var nextPosition = ComputeNextUnitPosition(unit);
                    MoveUnit(unit, nextPosition);
                }
            }
            currentIteration++;
        }

        private void MoveUnit(Tuple<char, int, int> unit, Tuple<int,int> nextPosition)
        {
            
        }

        private Tuple<int, int> ComputeNextUnitPosition(Tuple<char, int, int> unit)
        {
            return null;
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
}