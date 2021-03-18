using System;
using System.Collections.Generic;
using System.Linq;

namespace LanczosApproximation {
    class LanczosCriticalZoneEstimater {

        private readonly List<(int n, int g2)> wellknown_points;
        private readonly int g2_search_neighbors;
        
        public LanczosCriticalZoneEstimater(int g2_search_neighbors = 4) {
            if (g2_search_neighbors < 1) {
                throw new ArgumentException(nameof(g2_search_neighbors));
            }

            this.wellknown_points = new List<(int, int)>();
            this.g2_search_neighbors = g2_search_neighbors;
        }

        public void Sample(int n, int g2) {
            wellknown_points.Add((n, g2));

            if (wellknown_points.Count > 4) {
                wellknown_points.RemoveAt(0);
            }
        }

        public IEnumerable<int> Estimate(int n) {
            if (wellknown_points.Count <= 0) {
                int g2_empi = n * 2;

                return EnumG2(n * 2 - g2_search_neighbors * 8, n * 16 + g2_search_neighbors * 8);
            }
            if (wellknown_points.Count <= 1) {
                int g2_prev = wellknown_points[0].g2;
                return EnumG2(g2_prev - g2_search_neighbors * 4, g2_prev + g2_search_neighbors * 4);
            }

            double[] xs = wellknown_points.Select((point) => (double)point.n).ToArray();
            double[] ys = wellknown_points.Select((point) => (double)point.g2).ToArray();

            double x_avg = xs.Average(), y_avg = ys.Average();
            double sxx = 0, sxy = 0;

            for (int i = 0; i < xs.Length; i++) {
                double dx = xs[i] - x_avg, dy = ys[i] - y_avg;
                
                sxx += dx * dx;
                sxy += dx * dy;
            }

            double slope = sxy / sxx, intercept = y_avg - slope * x_avg;

            int g2_pred = (int)Math.Floor(slope * n + intercept + 0.5);
            
            return EnumG2(g2_pred - g2_search_neighbors, g2_pred + g2_search_neighbors);
        }

        private static IEnumerable<int> EnumG2(int g2_min, int g2_max) {
            for (int i = Math.Max(1, g2_min); i <= g2_max; i++) {
                yield return i;
            }
        }
    }
}
