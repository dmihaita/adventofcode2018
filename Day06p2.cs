
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

public class Day06p2 {
    int threshold = 10000;
    private string inputPath;
    private List<Area> areas = new List<Area>();
    public Day06p2(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public int Solve(){
        List<Point> res = new List<Point>();
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
                if (areaDist.Values.Sum() < threshold){
                    res.Add(new Point(x,y));
                }
            }
        }
        return res.Count;
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
