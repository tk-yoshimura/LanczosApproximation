using MultiPrecision;
using System.Collections.Generic;

namespace LanczosApproximation {
    class LanczosApprox<N> where N : struct, IConstant {
        private readonly static MultiPrecision<N> p5;
        private readonly MultiPrecision<N>[] table;

        public MultiPrecision<N> G { private set; get; }
        public int Length => table.Length;

        public IReadOnlyList<MultiPrecision<N>> Table => table;

        static LanczosApprox() {
            p5 = MultiPrecision<N>.Ldexp(1, -1);
        }

        public LanczosApprox(int n, int g2) {
            table = Lanczos<N>.ATable(n, g2);
            G = MultiPrecision<N>.Ldexp(g2, -1);
        }

        public MultiPrecision<N> Gamma(MultiPrecision<N> z) {
            if (z < p5) {
                MultiPrecision<N> y = MultiPrecision<N>.PI / (MultiPrecision<N>.SinPI(z) * Gamma(1 - z));

                return y;
            }
            else {
                z -= 1;

                MultiPrecision<Double<N>> x_ex = MultiPrecisionUtil.Convert<Double<N>, N>(Table[0]);
                MultiPrecision<Double<N>> z_ex = MultiPrecisionUtil.Convert<Double<N>, N>(z);

                for (int i = 1; i < Length; i++) {
                    MultiPrecision<Double<N>> w = MultiPrecisionUtil.Convert<Double<N>, N>(Table[i]);

                    x_ex += w / (z_ex + i);
                }

                MultiPrecision<N> x = MultiPrecisionUtil.Convert<N, Double<N>>(x_ex);

                MultiPrecision<N> s = z + p5, t = (s + G) / MultiPrecision<N>.E;

                MultiPrecision<N> y = MultiPrecision<N>.Pow(t, s) * x;

                return y;
            }
        }
    }
}
