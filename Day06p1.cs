
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

public class Day06p1 {
    private string inputPath;
    private List<Area> areas = new List<Area>();
    public Day06p1(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public int Solve(){
        int minX = areas.Min(t => t.Core.X);
        int maxX = areas.Max(t => t.Core.X);
        int minY = areas.Min(t => t.Core.Y);
        int maxY = areas.Max(t => t.Core.Y);
        for(int x = minX; x <= maxX; x++){
            for(int y = minY; y <= maxY; y++){
                Dictionary<int, int> areaDist = new Dictionary<int, int>();
                foreach(var area in areas){
                    var dist = new Point(x, y).Distance(area.Core);
                    areaDist[area.Id] = dist;
                }
                int minDist = areaDist.Values.Min();
                if (areaDist.Where(t => t.Value == minDist).Count() == 1){
                    int areaId = areaDist.Where(t => t.Value == minDist).First().Key;
                    areas.First(t => t.Id == areaId).Surface.Add(new Point(x, y));
                }
            }
        }

        var closedAreas = areas.Where(t => 
            !t.Surface.Any(p => p.X == minX) && 
            !t.Surface.Any(p => p.X == maxX) && 
            !t.Surface.Any(p => p.Y == minY) && 
            !t.Surface.Any(p => p.Y == maxY)).
            Select(t => new {
                Id = t.Id,
                Size = t.SurfaceSize 
            });
        return closedAreas.Max(t => t.Size);
    }
    private void ReadInput()
    {
        string line;
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){
            int areaId = 0;
            while((line = file.ReadLine()) != null)
            {
                int x = int.Parse(line.Substring(0, line.IndexOf(',')));
                int y = int.Parse(line.Substring(line.IndexOf(',') + 1).Trim());
                areas.Add(new Area(areaId++){ Core = new Point(x,y)});
            }
            file.Close();
        }
    }
}

public class Point{
    public Point(int x, int y){
        X = x;
        Y = y;
    }

    public int X {get; internal set;}
    public int Y {get; internal set;}
    public int Distance(Point other){
        return Math.Abs(this.X - other.X) + Math.Abs(this.Y - other.Y);
    }
}

public class Area{
    public Area(int id){
        Id = id;
        Surface = new List<Point>();
    }
    public int Id {get; internal set;}
    public Point Core {get;set;}
    public List<Point> Surface {get;set;}

    public int SurfaceSize {get{return Surface.Count;}}
}