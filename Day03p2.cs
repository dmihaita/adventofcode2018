
using System;
using System.Collections.Generic;
using System.Linq;

public class Day03p2 {
    private string inputPath;
    private Dictionary<int, SantaPatch> inputValues = new Dictionary<int, SantaPatch>();
    public Day03p2(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public int Solve(){
        HashSet<int> dirtyPatches = new HashSet<int>();
        var patches = inputValues.Keys.ToList().OrderBy(t => t).ToArray(); 
        for(int i = 0; i < patches.Length - 1; i++){
            for(int j = i + 1; j < patches.Length; j++){
                int key1 = patches[i];
                int key2 = patches[j];
                if (inputValues[key1].IntersectWith(inputValues[key2])){
                    dirtyPatches.Add(key1);
                    dirtyPatches.Add(key2);
                }
            }
        }
        return patches.Except(dirtyPatches).FirstOrDefault();
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