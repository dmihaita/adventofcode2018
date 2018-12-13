

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Day12p1 {
    private string inputPath;
    private string initialState;
    public Dictionary<string, char> transitions;
    public Day12p1(string inputPath){
        this.inputPath = inputPath;
        transitions = new Dictionary<string, char>();
        ReadInput();
    }

    public int Solve(){
        string input = "........................................" + initialState + "........................................";
        Debug.WriteLine($"00: {input}");
        int firstPotIndex = input.IndexOf('#');
        for(int i = 0; i < 100; i ++){
            while (!input.StartsWith(".....")){
                input = input.Insert(0,".");
                firstPotIndex++;
            }
            while (!input.EndsWith(".....")){
                input = input + ".";
            }
            input = Transition(input);
            int sum = 0;
            for(int j = 0; j< input.Length; j++){
                sum += input[j] == '#' ? j - firstPotIndex : 0;
            }
            Debug.WriteLine($"{i + 1,2}: {input} {sum}");
        }
        return 0;
    }

    private string Transition(string input)
    {
        StringBuilder next = new StringBuilder(new String('.', input.Length));
        for (int i = 0 ; i< input.Length - 5; i++){
            string chunk = input.Substring(i, 5);
            if (transitions.ContainsKey(chunk))
                next[i + 2] = transitions[chunk];
        }
        return next.ToString();
    }

    private void ReadInput()
    {
        string line;
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){
            while((line = file.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line)){
                    if (line.StartsWith("initial")){
                        initialState = line.Substring(line.IndexOf(':') + 1).Trim();
                    }
                    else{
                        string pattern = line.Substring(0, 5);
                        string result = line.Substring(9).Trim();
                        transitions[pattern] = result[0];
                    }
                }
            }
            file.Close();
        }
    }
}
