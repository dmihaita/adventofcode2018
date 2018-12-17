
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

public class Day09p1 {
    private string inputPath;
    MarbleGame marbleGame;
    public Day09p1(string inputPath){
        this.inputPath = inputPath;
        ReadInput();
    }

    public long Solve(){
        marbleGame.Play();        
        long maxPoints = marbleGame.MaxScore;
        Console.WriteLine(maxPoints);
        return maxPoints;
    }

    private void ReadInput()
    {
        marbleGame = new MarbleGame(438,7162600);
    }
}

public class MarbleGame{
    int players, maxMarble;
    Dictionary<int, long> scores;
    List<int> marbles;

    public MarbleGame(int players, int maxMarble){
        this.players = players;
        this.maxMarble = maxMarble;
        scores = new Dictionary<int, long>(players);
        for(int i = 0; i < players; i++){
            scores[i + 1] = 0;
        }
        marbles = new List<int>(maxMarble);
    }

    public void Play(){
        marbles.Clear();
        marbles.Add(0);
        marbles.Add(1);
        int currentIndex = 1;
        for(int currentMarble = 2; currentMarble <= maxMarble; currentMarble++){
            // insert marble
            int newIndex = (currentIndex + 2) % marbles.Count;
            if (newIndex == 0){
                marbles.Add(currentMarble);
                currentIndex = marbles.Count - 1;
            }
            else{
                marbles.Insert(newIndex, currentMarble);
                currentIndex = newIndex;
            }
            //
            if (currentMarble % 23 == 0){
                int currentPlayer = currentMarble % players + 1;
                scores[currentPlayer] += currentMarble;
                marbles.RemoveAt(currentIndex);
                int additionalIndex = currentIndex - 9;
                if (additionalIndex < 0){
                    additionalIndex = marbles.Count + additionalIndex;
                }
                scores[currentPlayer] += marbles[additionalIndex];
                marbles.RemoveAt(additionalIndex);
                currentIndex = additionalIndex;
            }
            // Debug.WriteLine(string.Join("  ", marbles.Select(t => t.ToString())));
        }
    }

    public long MaxScore{
        get{ return scores.Values.Max(); }
    }
}

