using System;
using System.IO;
using MultiPrecision;

namespace LanczosApproximation {    
    class Program {
        static void Main() {
            const int margin_bits = 16;

            ParamSearch<Expand25<Pow2.N4>>(4 * 32 + margin_bits);
            ParamSearch<Expand50<Pow2.N4>>(4 * 32 + margin_bits);

            ParamSearch<Expand25<Pow2.N8>>(8 * 32 + margin_bits);
            ParamSearch<Expand50<Pow2.N8>>(8 * 32 + margin_bits);

            ParamSearch<Expand25<Pow2.N16>>(16 * 32 + margin_bits);
            ParamSearch<Expand50<Pow2.N16>>(16 * 32 + margin_bits);

            ParamSearch<Expand25<Pow2.N32>>(32 * 32 + margin_bits);
            ParamSearch<Expand50<Pow2.N32>>(32 * 32 + margin_bits);

            ParamSearch<Expand25<Pow2.N64>>(64 * 32 + margin_bits);
            ParamSearch<Expand50<Pow2.N64>>(64 * 32 + margin_bits);

            ParamSearch<Expand25<Pow2.N128>>(128 * 32 + margin_bits);
            ParamSearch<Expand50<Pow2.N128>>(128 * 32 + margin_bits);

            ParamSearch<Pow2.N4>(4 * 32 + margin_bits);
            ParamSearch<Pow2.N8>(8 * 32 + margin_bits);
            ParamSearch<Pow2.N16>(16 * 32 + margin_bits);
            ParamSearch<Pow2.N32>(32 * 32 + margin_bits);
            ParamSearch<Pow2.N64>(64 * 32 + margin_bits);
            ParamSearch<Pow2.N128>(128 * 32 + margin_bits);

            Console.WriteLine("END");
            Console.Read();
        }

        private static void ParamSearch<N>(int censoring_accuracy) where N : struct, IConstant {
            Console.WriteLine($"bits={MultiPrecision<N>.Bits}");

            using (StreamWriter summary_all = new StreamWriter($"../../../../results/lanczos_bits{MultiPrecision<N>.Bits}.csv")) {
                summary_all.WriteLine("N,g,accuracy(bits)");

                int n_init = MultiPrecision<N>.Bits / 10;
                int max_max_accuracy = 0;

                LanczosCriticalZoneEstimater zone_estimater = new LanczosCriticalZoneEstimater();

                for (int n = n_init; n <= 675; n++) {

                    int max_accuracy = 0, maxacc_g2 = -1;
                    LanczosApprox<N> maxacc_lanczos = null;

                    foreach (int g2 in zone_estimater.Estimate(n)) {
                        Console.WriteLine($"{n},{g2 * 0.5:F1}");

                        LanczosApprox<N> lanczos = new LanczosApprox<N>(n, g2);

                        int accuracy = Accuracy(lanczos);

                        summary_all.WriteLine($"{n},{g2 * 0.5:F1},{accuracy}");
                        summary_all.Flush();

                        if (max_accuracy < accuracy) {
                            max_accuracy = accuracy;
                            maxacc_g2 = g2;
                            maxacc_lanczos = lanczos;
                        }
                        else if (accuracy < max_accuracy - 4 || accuracy <= 0) {
                            break;
                        }
                    }

                    if (maxacc_lanczos != null) {
                        zone_estimater.Sample(n, maxacc_g2);

                        Console.WriteLine($"N = {n}, g = {maxacc_g2 * 0.5:F1}, max_accuracy = {max_accuracy}");

                        string filepath = $"../../../../results/lanczos" +
                            $"_bits{MultiPrecision<N>.Bits}_n{n}_g{maxacc_g2 / 2}{((maxacc_g2 % 2 == 0) ? "" : ".5")}_acc{max_accuracy}.txt";

                        using StreamWriter summary = new StreamWriter(filepath);

                        Summary(maxacc_lanczos, summary);
                    }
                    else {
                        break;
                    }

                    if (max_accuracy >= censoring_accuracy) {
                        break;
                    }
                    if (max_max_accuracy < max_accuracy) {
                        max_max_accuracy = max_accuracy;
                    }
                    else if (max_accuracy < max_max_accuracy - 2) {
                        break;
                    }
                }
            }
        }

        private static int Accuracy<N>(LanczosApprox<N> lanczos) where N : struct, IConstant {
            int min_matchbits = MultiPrecision<N>.Bits;

            for (int z2 = 1; z2 <= 100; z2++) {
                MultiPrecision<N> z = MultiPrecision<N>.Ldexp(z2, -1);

                MultiPrecision<N> y_expected = Gamma<N>.Value(z2);
                MultiPrecision<N> y_actual = lanczos.Gamma(z);

                for (int keepbits = MultiPrecision<N>.Bits; keepbits >= 0; keepbits--) {
                    if (keepbits == 0) {
                        min_matchbits = 0;
                    }
                    else if (MultiPrecision<N>.RoundMantissa(y_expected, keepbits) == MultiPrecision<N>.RoundMantissa(y_actual, keepbits)) {
                        if (keepbits < min_matchbits) {
                            min_matchbits = keepbits;
                        }

                        break;
                    }
                }
            }

            return min_matchbits;
        }

        private static void Summary<N>(LanczosApprox<N> lanczos, StreamWriter sw) where N : struct, IConstant {
            sw.WriteLine("state");
            sw.WriteLine($"  N={lanczos.Length - 1}");
            sw.WriteLine($"  g={lanczos.G}");
            sw.WriteLine($"  bits={MultiPrecision<N>.Bits}");

            sw.WriteLine("approx coef");
            foreach (MultiPrecision<N> c in lanczos.Table) {
                sw.WriteLine($"  {c}");
                sw.WriteLine($"  {c.ToHexcode()}");
            }

            sw.WriteLine("approx result");
            for (int z2 = 1; z2 <= 100; z2++) {
                MultiPrecision<N> z = MultiPrecision<N>.Ldexp(z2, -1);

                MultiPrecision<N> y_expected = Gamma<N>.Value(z2);
                MultiPrecision<N> y_actual = lanczos.Gamma(z);

                MultiPrecision<N> err = MultiPrecision<N>.Abs(y_expected - y_actual);

                sw.WriteLine($"  gamma({z2 * 0.5:F1})");
                sw.WriteLine($"    true   : {y_expected}");
                sw.WriteLine($"    approx : {y_actual}");
                sw.WriteLine($"    err    : {err}");

                for (int keepbits = MultiPrecision<N>.Bits; keepbits > 0; keepbits--) {
                    if (MultiPrecision<N>.RoundMantissa(y_expected, keepbits) == MultiPrecision<N>.RoundMantissa(y_actual, keepbits)) {
                        sw.WriteLine($"    matchbits : {keepbits}");

                        break;
                    }
                }
            }
        }
    }
}
