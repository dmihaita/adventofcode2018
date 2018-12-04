
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Day04p1 {
    private string inputPath;
    private Dictionary<int, Dictionary<DateTime, List<SleepInterval>>> inputValues = 
        new Dictionary<int, Dictionary<DateTime, List<SleepInterval>>>();
    public Day04p1(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public int Solve(){
        var sleepingBeauty = inputValues.Select(t => new {
            id = t.Key, 
            sleep = t.Value.Sum(x => x.Value.Sum( y => y.Duration))}).
            OrderByDescending(t => t.sleep).
            FirstOrDefault().id;
        var intervals = inputValues[sleepingBeauty];
        int minuteBeauty = Enumerable.Range(0, 59).Select(
            t => new {
                minute = t , 
                count = intervals.Values.Sum(y => y.Count(x => x.IsInInterval(t)))
            }
        ).
        OrderByDescending(t => t.count).
        FirstOrDefault().minute;
        return sleepingBeauty * minuteBeauty;
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

public class SleepInterval{
    public SleepInterval(int start){
        Start = start;
    }
    public int Start{get; internal set;}
    public int End{get;set;}

    public int Duration {
        get{
            return End - Start;
        }
    }

    public bool IsInInterval(int t){
        return Start <= t && t <= End;
    }

    public bool Contains(int value){
        return Start <= value && value <= End;
    }
}