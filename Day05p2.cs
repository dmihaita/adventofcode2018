
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

public class Day05p2 {
    private string inputPath;
    private string inputValue;
    public Day05p2(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public int Solve(){
        var x = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList().
        Select(t =>
            new KeyValuePair<char, int>(
                t,
                Reduce(inputValue.Replace($"{t}", "", true, CultureInfo.InvariantCulture))
            )
        );    
        return x.Min(t => t.Value);
    }

    private int Reduce(string polymer){
        int currentIndex = 0;
        while(currentIndex < polymer.Length - 1)
        {
            if (Reacts(polymer[currentIndex], polymer[currentIndex + 1])){
                polymer = polymer.Remove(currentIndex, 2);
                currentIndex = currentIndex > 0 ? currentIndex - 1 : 0;
            }
            else
                currentIndex++;
        }
        return polymer.Length;
    }

    private bool Reacts(char a, char b){
        return (a != b && char.ToLower(a) == char.ToLower(b));
    }

    private void ReadInput()
    {
        List<Tuple<DateTime, string>> eventsList = new List<Tuple<DateTime, string>>();
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){
            inputValue = file.ReadToEnd();
            file.Close();
        }
    }
}
