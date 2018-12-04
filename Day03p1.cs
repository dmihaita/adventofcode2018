
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class Day03p1 {
    private string inputPath;
    private Dictionary<int, SantaPatch> inputValues = new Dictionary<int, SantaPatch>();
    public Day03p1(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public int Solve(){
        int maxX = GetMaxX(), maxY = GetMaxY();
        int doublePatches = 0;
        int[,] patch = new int[maxX,maxY];
        foreach(int patchId in inputValues.Keys){
            SantaPatch currentPatch = inputValues[patchId];
            for(int i = currentPatch.X; i < currentPatch.X + currentPatch.W; i++){
                for(int j = currentPatch.Y ; j < currentPatch.Y + currentPatch.H; j++){
                    patch[i,j] = patch[i,j] > 1 ? patch[i,j] : patch[i,j] + 1;
                }
            }
        }
        for(int i = 0; i < maxX; i++){
            for(int j = 0; j < maxY; j++){
                doublePatches += patch[i,j] == 2 ? 1 : 0;
            }
        }
        return doublePatches;
    }

    private int GetMaxX()
    {
        return inputValues.Values.Select(t => t.X + t.W).Max();
    }
    private int GetMaxY()
    {
        return inputValues.Values.Select(t => t.Y + t.H).Max();
    }

    private void ReadInput()
    {
        string line;
        // Read the file and display it line by line.
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){
            while((line = file.ReadLine()) != null)
            {
                int patchId = Int32.Parse(line.Substring(1, line.IndexOf('@') - 1).Trim());
                line = line.Substring(line.IndexOf('@') + 1).Trim();
                string[] xy = line.Substring(0, line.IndexOf(':')).Trim().Split(',');
                line = line.Substring(line.IndexOf(':') + 1).Trim();
                string[] wh = line.Split('x');
                inputValues.Add(patchId, new SantaPatch(
                    Int32.Parse(xy[0]), Int32.Parse(xy[1]),
                    Int32.Parse(wh[0]), Int32.Parse(wh[1])
                ));
            }
            file.Close();
        }
    }
}

public class SantaPatch{
    public SantaPatch(int x, int y, int w, int h){
        X = x;
        Y = y;
        W = w;
        H = h;
    }
    public int X{get; internal set;}
    public int Y{get;internal set;}
    public int W{get;internal set;}
    public int H{get;internal set;}
}