
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Day05p1 {
    private string inputPath;
    private string inputValue;
    public Day05p1(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public int Solve(){
        int currentIndex = 0;
        while(currentIndex < inputValue.Length - 1)
        {
            if (Reacts(inputValue[currentIndex], inputValue[currentIndex + 1])){
                inputValue = inputValue.Remove(currentIndex, 2);
                currentIndex = currentIndex > 0 ? currentIndex - 1 : 0;
            }
            else
                currentIndex++;
        }
        return inputValue.Length;
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
