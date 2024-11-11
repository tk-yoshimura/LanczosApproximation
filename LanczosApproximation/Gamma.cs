using MultiPrecision;
using System;
using System.Collections.Generic;

namespace LanczosApproximation {
    static class Gamma<N> where N : struct, IConstant {
        private static readonly MultiPrecision<N> sqrt_pi;
        private static readonly Dictionary<int, MultiPrecision<N>> table;

        static Gamma() {
            sqrt_pi = MultiPrecision<N>.Sqrt(MultiPrecision<N>.Pi);

            table = new Dictionary<int, MultiPrecision<N>> {
                { 1, 1 },
                { 2, 1 }
            };
        }

        public static MultiPrecision<N> Value(int z2) {
            if (z2 < 1) {
                throw new ArgumentException(null, nameof(z2));
            }

            static MultiPrecision<N> gamma(int i) {
                if (table.TryGetValue(i, out MultiPrecision<N> value)) {
                    return value;
                }

                MultiPrecision<N> y = MultiPrecision<N>.Ldexp(i - 2, -1) * gamma(i - 2);

                table.Add(i, y);
                return y;
            }

            return (z2 % 2 == 0) ? gamma(z2) : (gamma(z2) * sqrt_pi);
        }
    }
}
