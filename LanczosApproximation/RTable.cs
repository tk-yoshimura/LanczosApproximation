using MultiPrecision;
using System;
using System.Collections.Generic;
using System.IO;

namespace LanczosApproximation {
    public static class RTable<N> where N : struct, IConstant {
        static readonly Dictionary<(int k, int l), MultiPrecision<N>> table = new();

        public static MultiPrecision<N> Value(int k, int l) {
            if (k < l || k < 1 || l < 1) {
                throw new ArgumentOutOfRangeException($"{nameof(k)},{nameof(l)}");
            }
            
            if (table.ContainsKey((k, l))) {
                return table[(k, l)];
            }

            using (StreamReader sr = new StreamReader($"../../../../rtable/lanczos_{k}.txt")) {
                for (int i = 1; i <= k; i++) {
                    MultiPrecision<N> d = sr.ReadLine();
                    table.Add((k, i), d);
                }
            }

            return table[(k, l)];
        }
    }
}
