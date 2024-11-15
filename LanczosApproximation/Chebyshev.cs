﻿using System;
using System.Collections.Generic;
using System.Numerics;

namespace LanczosApproximation {
    public static class Chebyshev {
        private static readonly Dictionary<(int m, int n), BigInteger> table;

        static Chebyshev() {
            table = new Dictionary<(int n, int m), BigInteger> {
                { (1, 1), 1 },
                { (2, 2), 1 }
            };
        }

        public static BigInteger Table(int n, int m) {
            if (n < 1 || m < 1 || n < m) {
                throw new ArgumentOutOfRangeException();
            }

            if ((checked(m + n) & 1) == 1) {
                return 0;
            }
            if (m == 1) {
                return (((n / 2) & 1) == 0) ? 1 : -1;
            }
            if (n == m) {
                return (n >= 2) ? (BigInteger.One << (n - 2)) : 1;
            }

            if (table.ContainsKey((n, m))) {
                return table[(n, m)];
            }

            BigInteger v = Table(n - 1, m - 1) * 2 - Table(n - 2, m);
            table.Add((n, m), v);

            return v;
        }
    }
}