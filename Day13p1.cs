

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Day13p1 {
    private string inputPath;    
    private CityMap city;
    public Day13p1(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public int Solve(){
        city.Init();
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
        city = new CityMap(maze);
    }
}

public class CityMap{
    char[,] maze;
    public CityMap(char[,] maze){
        this.maze = maze;
    }

    public override string ToString(){
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < maze.GetLength(0); i++){
            for(int j = 0; j < maze.GetLength(1); j++)
            {
                sb.Append(maze[i,j]);
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}

public class Car{
    public Car(int x, int y, Direction direction){

    }

    public int X{get; internal set;}
    public int Y{get; internal set;}
    public Direction Direction{get; internal set;}
}

public enum Direction{
    Left, Top, Right, Bottom
}
