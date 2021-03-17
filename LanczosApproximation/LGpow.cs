using MultiPrecision;

namespace LanczosApproximation {
    static class LGpow<N> where N : struct, IConstant {
        private static readonly MultiPrecision<N> p5;

        static LGpow() {
            p5 = MultiPrecision<N>.Ldexp(1, -1);
        }

        public static MultiPrecision<N> Value(MultiPrecision<N> g, int l) {
            MultiPrecision<N> x = MultiPrecision<N>.E / (l + g + p5);
            MultiPrecision<N> y = MultiPrecision<N>.Sqrt(MultiPrecision<N>.Pow(x, 2 * l + 1));

            return y;
        }
    }
}
