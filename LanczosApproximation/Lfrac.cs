using MultiPrecision;
using System;
using System.Collections.Generic;

namespace LanczosApproximation {
    static class Lfrac<N> where N : struct, IConstant {
        private static readonly MultiPrecision<N> p5, sqrt_pi;
        private static readonly Dictionary<int, MultiPrecision<N>> table;

        static Lfrac() {
            p5 = MultiPrecision<N>.Ldexp(1, -1);
            sqrt_pi = MultiPrecision<N>.Sqrt(MultiPrecision<N>.Pi);

            table = new Dictionary<int, MultiPrecision<N>> {
                { 0, 1 }
            };
        }

        public static MultiPrecision<N> Value(int l) {
            if (l < 0) {
                throw new ArgumentException(null, nameof(l));
            }

            static MultiPrecision<N> lfrac(int i) {
                if (table.ContainsKey(i)) {
                    return table[i];
                }
                else {
                    MultiPrecision<N> y = (i - p5) * lfrac(i - 1);

                    table.Add(i, y);

                    return y;
                }
            }

            return lfrac(l) * sqrt_pi;
        }
    }
}
