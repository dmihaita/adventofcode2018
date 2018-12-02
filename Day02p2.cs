using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

public class Day02p2 {
    private string inputPath;
    private List<string> inputValues = new List<string>();
    public Day02p2(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public string Solve(){
        string result = string.Empty;
        for(int i = 0; i < inputValues.Count - 1; i++){
            string s1 = inputValues[i];
            for (int j = i + 1; j < inputValues.Count; j++){
                string s2 = inputValues[j];
                if (Diff(s1, s2) == 1){
                    return Common(s1, s2);
                }
            }
        }
        return string.Empty;
    }

    private string Common(string s1, string s2)
    {
        if (s1.Length != s2.Length)
            return string.Empty;
        StringBuilder sb = new StringBuilder();;
        for(int i = 0; i < s1.Length; i++){
            sb.Append(s1[i] == s2[i] ? s1[i] : ' ');
        }
        return sb.ToString().Replace(" ", "");
    }

    private int Diff(string s1, string s2)
    {
        if (s1.Length != s2.Length)
            return -1;
        int diff = 0;
        for(int i = 0; i < s1.Length; i++)
            diff += (s1[i] != s2[i]) ? 1 : 0;
        return diff;
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