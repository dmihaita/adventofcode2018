

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc2018.Day14p1 {
    public class Day14p1 {
        private string inputPath;    
        private int recipesCount;
        public Day14p1(string inputPath){
            this.inputPath = inputPath;
            ReadInput();
        }

        public string Solve(){
            RecipeSolver recipeSolver = new RecipeSolver();
            while (recipeSolver.Recipes < (recipesCount + 11))
            {
                recipeSolver.Iterate();
                // Debug.WriteLine(recipeSolver.ToString());
            }
            var key = recipeSolver.Key(recipesCount);
            Debug.Write(key);
            return key;
        }

        private void ReadInput()
        {
            this.recipesCount = 607331;
        }
    }

    public class RecipeSolver{
        List<byte> recipes; 
        public RecipeSolver(){
            recipes = new List<byte>(){3,7};
        }

        public int Recipes{
            get{return recipes.Count;}
        }

        public string Key(int index){
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < 10; i++){
                sb.Append(recipes[index + i]);
            }
            return sb.ToString();
        }
        int elf0Index = 0, elf1Index = 1;
        int currentIteration = 0;
        public void Iterate(){
            byte recipeSum = (byte)(recipes[elf0Index] + recipes[elf1Index]);
            if (recipeSum > 9){
                recipes.Add((byte)(recipeSum / 10));
                recipes.Add((byte)(recipeSum % 10));
            }
            else
            {
                recipes.Add(recipeSum);
            }
            int forwardelf0 = 1 + recipes[elf0Index];
            int forwardelf1 = 1 + recipes[elf1Index];
            elf0Index = (elf0Index + forwardelf0) % recipes.Count;
            elf1Index = (elf1Index + forwardelf1) % recipes.Count;
            currentIteration++;
        }

        public override string ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append($"{currentIteration} : ");
            for(int i = 0 ; i < recipes.Count; i++){
                sb.Append(
                    i == elf0Index ? 
                        $"({recipes[i]})" :
                        i == elf1Index ?
                            $"[{recipes[i]}]" : $"{recipes[i]}"
                );
                sb.Append(" ");
            }
            return sb.ToString();
        }
    }
}