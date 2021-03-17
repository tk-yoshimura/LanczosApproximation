using MultiPrecision;

namespace LanczosApproximation {

    struct Expand25<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value * 5 / 4);
    }

    struct Expand50<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value * 3 / 2);
    }

    struct Double<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value * 2);
    }
}
