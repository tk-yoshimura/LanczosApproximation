using MultiPrecision;
using System.IO;
using System.Linq;
using System.Numerics;

namespace LanczosApproximation {
    static class Lanczos<N> where N : struct, IConstant {
        public static MultiPrecision<Double<N>>[] PTable(int n, int g2) {
            MultiPrecision<Double<N>> g = MultiPrecision<Double<N>>.Ldexp(g2, -1);
            
            MultiPrecision<Double<N>>[] ps = new MultiPrecision<Double<N>>[n + 1];
            MultiPrecision<Double<N>>[] fs = new MultiPrecision<Double<N>>[n + 1];

            for (int l = 0; l <= n; l++) {
                fs[l] = Lfrac<Double<N>>.Value(l) * LGpow<Double<N>>.Value(g, l);
            }

            for (int k = 0; k <= n; k++) {
                MultiPrecision<Double<N>> p = 0;
                
                for (int l = 0; l <= k; l++) {
                    BigInteger chebyshev = Chebyshev.Table(2 * k + 1, 2 * l + 1);

                    MultiPrecision<Double<N>> c = $"{chebyshev}";
                    MultiPrecision<Double<N>> f = fs[l];

                    MultiPrecision<Double<N>> a = c * f;

                    p += a;
                }

                ps[k] = p;
            }

            return ps;
        }

        public static MultiPrecision<N>[] ATable(int n, int g2) {
            MultiPrecision<Double<N>>[] ps = PTable(n, g2);
            MultiPrecision<Double<N>>[] cs = new MultiPrecision<Double<N>>[n + 1];
            MultiPrecision<Double<N>> b = 2 / MultiPrecision<Double<N>>.Sqrt(MultiPrecision<Double<N>>.PI);
            
            cs[0] = ps[0] / 2;

            for (int k = 1; k <= n; k++) {
                MultiPrecision<Double<N>> p = ps[k];

                cs[0] += p;
                cs[k] = 0;

                for (int l = 1; l <= k; l++) {
                    MultiPrecision<Double<N>> d = RTable<Double<N>>.Value(k, l);

                    cs[l] += p * d;
                }
            }

            MultiPrecision<N>[] rcs = cs.Select((c) => MultiPrecisionUtil.Convert<N, Double<N>>(c * b)).ToArray();

            return rcs;
        }
    }
}
