

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc2018.Day14p2 {
    public class Day14p2 {
        private string inputPath;    
        private int recipesCount;
        public Day14p2(string inputPath){
            this.inputPath = inputPath;
            ReadInput();
        }

        public string Solve(){
            string key = "607331";
            RecipeSolver recipeSolver = new RecipeSolver(key);
            try{
            while (!recipeSolver.IsSolved)
            {
                recipeSolver.Iterate();
            }
            }catch(Exception ex){
                Debug.Write(ex.Message);
            }
            var recipes = recipeSolver.Recipes;
            Debug.Write(recipes - key.Length);
            return $"{recipes - key.Length}";
        }

        private void ReadInput()
        {
            this.recipesCount = 607331;
        }
    }

    public class RecipeSolver{
        List<int> recipes;
        List<int> keyDigits;
        int keyDigitsIndex; 
        string key;
        public RecipeSolver(string key){
            recipes = new List<int>(){3,7};
            this.key = key;
            keyDigits = this.key.ToCharArray().Select(t => int.Parse($"{t}")).ToList();
            keyDigitsIndex = 0; 
        }

        public bool IsSolved{get;internal set;}


        public int Recipes{
            get{return recipes.Count;}
        }

        int elf0Index = 0, elf1Index = 1;
        int currentIteration = 0;
        public void Iterate(){
            int recipe1 = recipes[elf0Index];
            int recipe2 = recipes[elf1Index];
            int recipeSum =  recipe1 + recipe2;
            if (recipeSum > 9){
                AddRecipes(1, recipeSum - 10);
            }
            else
            {
                AddRecipes(recipeSum);
            }
            int forwardelf0 = 1 + recipe1;
            int forwardelf1 = 1 + recipe2;
            elf0Index = (elf0Index + forwardelf0) % recipes.Count;
            elf1Index = (elf1Index + forwardelf1) % recipes.Count;
            currentIteration++;
            if (currentIteration % 10000 == 0){
                Debug.Write(DateTime.Now);
                Debug.Write(" : ");
                Debug.WriteLine(currentIteration);
            }
        }

        private void AddRecipes(params int[] newRecipes)
        {
            for(int i = 0; i < newRecipes.Length; i++){
                recipes.Add(newRecipes[i]);
                if (!IsSolved){
                    if (newRecipes[i] == keyDigits[keyDigitsIndex]){
                        keyDigitsIndex++;
                        if (keyDigitsIndex == keyDigits.Count)
                        {
                            IsSolved = true;
                        }                    
                    }
                    else{
                        keyDigitsIndex = 0;
                    }
                }
            }
        }

        public override string ToString(){
            return $"{currentIteration} : {recipes}";
        }

        internal void SaveToFile(string path)
        {
            using (var logFile = System.IO.File.Create(path)){
                using (var logWriter = new System.IO.StreamWriter(logFile)){
                    for(int i = 0; i < this.recipes.Count; i++){
                        logWriter.Write($"{recipes[i]}");
                    }                    
                }
            }
        }
    }
}