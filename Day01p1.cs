using System;
using System.Globalization;

public class Day01p1 {
    private string inputPath;
    public Day01p1(string inputPath){
        this.inputPath = inputPath;
    }

    public int Solve(){
        int counter = 0, frequency = 0;  
        string line;  
        // Read the file and display it line by line.  
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){  
            while((line = file.ReadLine()) != null)  
            {  
                counter++;
                frequency += Int32.Parse(line, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture); 
            }  
            file.Close();  
        }
        return frequency;
    }
}