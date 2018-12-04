
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Day04p2 {
    private string inputPath;
    private Dictionary<int, Dictionary<DateTime, List<SleepInterval>>> inputValues = 
        new Dictionary<int, Dictionary<DateTime, List<SleepInterval>>>();
    public Day04p2(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public int Solve(){
        var someProblem = Enumerable.Range(0, 59).Select(
            t => new {
                minute = t ,
                guardsCount = inputValues.Keys.Select(u => new{
                    guard = u,
                    count = inputValues[u].Values.Sum(y => y.Count(x => x.IsInInterval(t)))
                })
            }
        );
        var sp2 = someProblem.OrderByDescending(t => t.guardsCount.Max(y => y.count));
        var sp3 = sp2.FirstOrDefault();
        return sp3.minute * sp3.guardsCount.
            OrderByDescending(t => t.count).FirstOrDefault().guard;
    }

    private void ReadInput()
    {
        string line;
        // Read the file and display it line by line.
        List<Tuple<DateTime, string>> eventsList = new List<Tuple<DateTime, string>>();
        using (System.IO.StreamReader file = new System.IO.StreamReader(inputPath)){
            while((line = file.ReadLine()) != null)
            {
                string dateTimeAsString = line.Substring(1,16);
                string eventAsString = line.Substring(19);
                eventsList.Add(Tuple.Create(DateTime.Parse(dateTimeAsString), eventAsString));
            }
            file.Close();
        }
        var orderedEvents = eventsList.OrderBy(t => t.Item1);

        int currentGuard = -1;
        SleepInterval interval = null;
        foreach(var guardEvent in orderedEvents){
            var body = guardEvent.Item2;
            switch(body){
                case "falls asleep" :
                    interval = new SleepInterval(guardEvent.Item1.Minute);
                    break;
                case "wakes up" :
                    if (interval != null){
                        interval.End = guardEvent.Item1.Minute;
                        if (!inputValues.ContainsKey(currentGuard))
                            inputValues[currentGuard] = new Dictionary<DateTime, List<SleepInterval>>();
                        if (!inputValues[currentGuard].ContainsKey(guardEvent.Item1.Date))
                            inputValues[currentGuard][guardEvent.Item1.Date] = new  List<SleepInterval>();
                        inputValues[currentGuard][guardEvent.Item1.Date].Add(interval);
                    }
                    break;
                default:
                    currentGuard = Int32.Parse(Regex.Match(guardEvent.Item2, "([0-9])\\w+").Groups[0].Value);
                    break;
            }
        }
    }
}
