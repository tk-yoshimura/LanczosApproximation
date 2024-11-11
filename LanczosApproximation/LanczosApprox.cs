using MultiPrecision;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LanczosApproximation {
    class LanczosApprox<N> where N : struct, IConstant {
        private static readonly MultiPrecision<N> p5;
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

        private LanczosApprox(MultiPrecision<N>[] table, MultiPrecision<N> g) {
            this.table = (MultiPrecision<N>[])table.Clone();
            this.G = g;
        }

        public LanczosApprox<Ndst> Convert<Ndst>() where Ndst : struct, IConstant {
            return new LanczosApprox<Ndst>(
                table.Select((v) => MultiPrecisionUtil.Convert<Ndst, N>(v)).ToArray(),
                MultiPrecisionUtil.Convert<Ndst, N>(G)
            );
        }

        public MultiPrecision<N> Gamma(MultiPrecision<N> z) {
            if (z < p5) {
                MultiPrecision<N> y = MultiPrecision<N>.Pi / (MultiPrecision<N>.SinPi(z) * Gamma(1 - z));

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

        public static void Write(LanczosApprox<N> lanczos, BinaryWriter writer) {
            writer.Write(lanczos.Length);

            writer.Write(lanczos.G);

            foreach (MultiPrecision<N> v in lanczos.Table) {
                writer.Write(v);
            }
        }

        public static LanczosApprox<N> Read(BinaryReader reader) {
            int length = reader.ReadInt32();

            MultiPrecision<N> g = reader.ReadMultiPrecision<N>();

            MultiPrecision<N>[] table = new MultiPrecision<N>[length];
            for (int i = 0; i < table.Length; i++) {
                MultiPrecision<N> v = reader.ReadMultiPrecision<N>();

                table[i] = v;
            }

            return new LanczosApprox<N>(table, g);
        }
    }
}
