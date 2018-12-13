
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Day11p1 {
    private int gridSerialNumber;
    private int[,] matrix = new int[300,300];
    public Day11p1(string inputPath){
        ReadInput();
    }

    public long Solve(){
        ComputeMatrix();
        Tuple<int, int, int> res = FindMaxPatch();
        return 0;
    }

    private Tuple<int, int, int> FindMaxPatch()
    {
        ConcurrentDictionary<Tuple<int, int, int>, int> result = new ConcurrentDictionary<Tuple<int, int, int>, int>();
        for(int patchDim = 1; patchDim < 300; patchDim++){
            Parallel.For(0, (300 - patchDim + 1) * (300 - patchDim + 1), (i) => {
                int ix = i / (300 - patchDim + 1);
                int iy = i % (300 - patchDim + 1);
                int power = 0;
                for (int kx = 0; kx < patchDim; kx++){
                    for (int ky = 0; ky < patchDim; ky++){
                        power += matrix[ix + kx, iy + ky];
                    }
                }
                result[Tuple.Create(ix, iy, patchDim)] = power;
            });
        }
        return result.OrderByDescending(t => t.Value).First().Key;
    }

    private void ComputeMatrix()
    {
        Parallel.For(0, 300*300, (i) => {
            int ix = i / 300;
            int iy = i % 300;
            matrix[ix, iy] = (((((ix + 10) * iy + gridSerialNumber) * (ix + 10)) / 100) % 10) - 5;
        });
    }

    private void ReadInput()
    {
        gridSerialNumber = 6392;
    }
}
