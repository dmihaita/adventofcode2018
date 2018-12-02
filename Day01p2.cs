using System;
using System.Collections.Generic;
using System.Globalization;

public class Day01p2 {
    private string inputPath;
    private List<int> inputValues = new List<int>();
    public Day01p2(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public int Solve(){
        HashSet<int> reachedFrequencies = new HashSet<int>(){ 0 };
        int currentIndex = 0;
        int frequency = 0;
        do{
            int change = inputValues[currentIndex];
            frequency += change;
            if (reachedFrequencies.Contains(frequency))
                break;
            reachedFrequencies.Add(frequency);
            currentIndex = currentIndex < (inputValues.Count - 1) ? currentIndex + 1 : 0;
        }
        while(true);
        return frequency;
    }
    private void ReadInput()
    {
        string line;  
        // Read the file and display it line by line.  
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){  
            while((line = file.ReadLine()) != null)  
            {  
                inputValues.Add(Int32.Parse(line, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture)); 
            }  
            file.Close();  
        }
    }
}