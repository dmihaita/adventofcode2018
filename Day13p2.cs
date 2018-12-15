

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc2018.Day13p2 {
    public class Day13p2 {
        private string inputPath;    
        private CityMap city;
        public Day13p2(string inputPath){
            this.inputPath = inputPath;
            ReadInput();
        }

        public int Solve(){
            while(city.CarsCount > 1){
                city.Move();
            }
            return city.Cars.First().X;
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
        List<Car> cars;
        public CityMap(char[,] maze){
            this.maze = maze;
            cars = new List<Car>();
            CollectCars();
        }

        public int CarsCount{
            get {return cars.Count;}
        }

        public IEnumerable<Car> Cars{
            get{return cars;}
        }

        public Tuple<int,int> Move(){
            foreach(var car in cars.
                OrderBy(t => t.X).
                ThenBy(t => t.Y))
            {               
                char nextChar = GetNextChar(car);
                car.Advance(nextChar);
                Tuple<int,int> collisionPoint;
                if ((collisionPoint = IsCollision()) != null){
                    MarkCarsAsCollided(collisionPoint);
                }
            }
            cars = cars.Where(t => !t.Collided).ToList();
            return null;
        }

        private void MarkCarsAsCollided(Tuple<int, int> collisionPoint)
        {
            foreach (var car in cars.Where(t => t.X == collisionPoint.Item1 &&
                t.Y == collisionPoint.Item2)){
                    car.Collided = true;
                }
        }

        private Tuple<int, int> IsCollision()
        {
            return cars.GroupBy(t => Tuple.Create(t.X, t.Y)).
                Where(g => g.Count() > 1).
                Select(t => t.Key).FirstOrDefault();
        }

        private char GetNextChar(Car car)
        {
            switch(car.Direction){
                case Direction.Left : return maze[car.X, car.Y - 1];
                case Direction.Right : return maze[car.X, car.Y + 1];
                case Direction.Top : return maze[car.X - 1, car.Y];
                case Direction.Bottom : return maze[car.X + 1, car.Y];
                default:
                throw new ApplicationException("Error moving car.");
            }
        }

        private void CollectCars()
        {
            for (int i = 0; i < maze.GetLength(0); i++){
                for (int j = 0; j < maze.GetLength(1); j++){
                    switch (maze[i,j]){
                        case '^' :
                            cars.Add(new Car(i,j, Direction.Top));
                            maze[i,j] = '|';
                            break;
                        case '<' :
                            cars.Add(new Car(i,j, Direction.Left));
                            maze[i,j] = '-';
                            break;
                        case '>' :
                            cars.Add(new Car(i,j, Direction.Right));
                            maze[i,j] = '-';
                            break;
                        case 'v' :
                            cars.Add(new Car(i,j, Direction.Bottom));
                            maze[i,j] = '|';
                            break;
                    }
                }
            }
        }

        public override string ToString(){
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < maze.GetLength(0); i++){
                for(int j = 0; j < maze.GetLength(1); j++)
                {
                    if (cars.Any(t => t.X == i && t.Y == j)){
                        sb.Append(cars.First(t => t.X == i && t.Y == j).GetChar());
                    }
                    else
                    {
                        sb.Append(maze[i,j]);
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    public class Car{
        public Car(int x, int y, Direction direction){
            X = x; 
            Y = y;
            Direction = direction;
            NextTurn = NextTurn.Left;
        }

        public int X{get; internal set;}
        public int Y{get; internal set;}
        public Direction Direction{get; internal set;}
        public NextTurn NextTurn {get; internal set;}

        public bool Collided {get;set;}

        public void Advance(char nextTile){
            int nextX = Direction == Direction.Top ? X - 1 :
                            Direction == Direction.Bottom ? X + 1 : X;  
            int nextY = Direction == Direction.Left ? Y - 1 :
                            Direction == Direction.Right ? Y + 1 : Y;
            Direction nextDirection = Direction;          
            switch(nextTile){
                case '+' :
                    nextDirection = 
                        NextTurn == NextTurn.Left ? 
                            (
                                Direction == Direction.Top ? Direction.Left :
                                Direction == Direction.Bottom ? Direction.Right :
                                Direction == Direction.Right ? Direction.Top :
                                /*Direction == Direction.Left ?*/ Direction.Bottom
                            ) :
                        NextTurn == NextTurn.Right ?
                            (
                                Direction == Direction.Top ? Direction.Right :
                                Direction == Direction.Bottom ? Direction.Left :
                                Direction == Direction.Right ? Direction.Bottom :
                                /*Direction == Direction.Left ?*/ Direction.Top
                            ) :
                        /*NextTurn == NextTurn.Forward */ Direction;
                    NextTurn = 
                        NextTurn == NextTurn.Left ? NextTurn.Forward :
                            NextTurn == NextTurn.Forward ? NextTurn.Right :
                                NextTurn.Left; 
                    break;
                case '\\':
                nextDirection =
                        Direction == Direction.Top ? Direction.Left :
                        Direction == Direction.Bottom ? Direction.Right :
                        Direction == Direction.Right ? Direction.Bottom :
                        /*Direction == Direction.Left ?*/ Direction.Top;
                    break;
                case '/' :
                    nextDirection = 
                        Direction == Direction.Top ? Direction.Right :
                        Direction == Direction.Bottom ? Direction.Left :
                        Direction == Direction.Right ? Direction.Top :
                        /*Direction == Direction.Left ?*/ Direction.Bottom;
                    break;
            }
            X = nextX;
            Y = nextY;
            Direction = nextDirection;
        }

        internal char GetChar()
        {
            switch(Direction){
                case Direction.Top:
                    return '^';
                case Direction.Bottom:
                    return 'v';
                case Direction.Left:
                    return '<';
                default :
                    return '>';
            }
        }
    }

    public enum Direction{
        Left, Top, Right, Bottom
    }
    public enum NextTurn{
        Left, Forward, Right
    }
}