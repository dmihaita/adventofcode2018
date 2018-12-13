
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Day10p1 {
    private string inputPath;
    private Sky sky;
    public Day10p1(string inputPath){
        this.inputPath = inputPath;
        sky = new Sky();
        ReadInput();
    }

    public string Solve(){

        for (int i = 0; i < 10036; i++){
            sky.Advance();
        }
        sky.Print();
        Debug.WriteLine($"{sky.Timestamp} - {sky.Area}");
        return "HI";
    }
    private void ReadInput()
    {
        string line;
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){
            while((line = file.ReadLine()) != null)
            {
                string position = line.Substring(
                    line.IndexOf('<') + 1, line.IndexOf('>') - line.IndexOf('<') - 1
                );
                string[] xy = position.Split(',');
                string velocity = line.Substring(
                    line.LastIndexOf('<') + 1, line.LastIndexOf('>') - line.LastIndexOf('<') - 1
                );
                string[] vxvy = velocity.Split(',');
                sky.Stars.Add(new Star(
                    Int32.Parse(xy[0].Trim()),
                    Int32.Parse(xy[1].Trim()),
                    Int32.Parse(vxvy[0].Trim()),
                    Int32.Parse(vxvy[1].Trim())
                ));
            }
            file.Close();
        }
    }
}

public class Sky{
    List<Star> stars;
    private int timestamp;

    public Sky(){
        stars = new List<Star>();
        timestamp = 0;
    }

    public IList<Star> Stars{
        get{return stars;}
    }

    public void Advance(){
        timestamp++;
        // Parallel.ForEach(stars, (t) => {t.Advance();});
        foreach(var star in stars){
            star.Advance();
        }
    }

    internal void Print()
    {
        int minx = stars.Min(t => t.X);
        int maxx = stars.Max(t => t.X);
        int miny = stars.Min(t => t.Y);
        int maxy = stars.Max(t => t.Y);
        for(int j = miny; j <= maxy; j++){
            for(int i = minx; i<= maxx; i++){
                Debug.Write(stars.Any(t => t.X == i && t.Y == j) ? "#" : " ");
            }
            Debug.WriteLine("");
        }
    }

    public int Timestamp{
        get{return timestamp;}
    }

    public long Area{
        get{       
            long minx = stars.Min(t => t.X);
            long maxx = stars.Max(t => t.X);
            long miny = stars.Min(t => t.Y);
            long maxy = stars.Max(t => t.Y);
            return Math.Abs((maxx - minx)) * Math.Abs((maxy - miny));
        }
    }
}

public class Star{
    public Star(int x, int y, int vx, int vy){
        X = x;
        Y = y;
        vX = vx;
        vY = vy;
    }

    public int X { get; internal set;}
    public int Y { get; internal set;}
    public int vX { get; internal set;}
    public int vY { get; internal set;}

    public void Advance(){
        X += vX;
        Y += vY;
    }
}