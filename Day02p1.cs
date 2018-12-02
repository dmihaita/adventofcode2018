using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class Day02p1 {
    private string inputPath;
    private List<string> inputValues = new List<string>();
    public Day02p1(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public int Solve(){
        int twoChars = 0, threeChars = 0;
        foreach(string s in inputValues){
            var x = s.ToCharArray().GroupBy(t => t).Select(t => new KeyValuePair<char, int>(t.Key, t.Count()));
            twoChars += x.Where(t => t.Value == 2).Count() > 0 ? 1 : 0;
            threeChars += x.Where(t => t.Value == 3).Count() > 0 ? 1 : 0;
        }
        return twoChars * threeChars;
    }
    private void ReadInput()
    {
        string line;  
        // Read the file and display it line by line.  
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){  
            while((line = file.ReadLine()) != null)  
            {  
                inputValues.Add(line);
            }  
            file.Close();  
        }
    }
}