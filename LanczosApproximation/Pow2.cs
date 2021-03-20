using MultiPrecision;

namespace LanczosApproximation {

    struct Expand25<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value * 5 / 4);
    }

    struct Expand50<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value * 3 / 2);
    }

    struct Plus1<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 1);
    }

    struct Plus2<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 2);
    }

    struct Plus4<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 4);
    }

    struct Plus8<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 8);
    }

    struct Plus16<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 16);
    }

    struct Plus32<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 32);
    }

    struct Plus64<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value + 64);
    }

    struct Double<N> : IConstant where N : struct, IConstant {
        public int Value => checked(default(N).Value * 2);
    }
}
