from util import *

for n in range(1, 1024 + 1):

    c, u = coef(n)
    w = inverse(c, n)

    r = array1d(n)

    for i in range(n):
        x = 0

        for j in range(n):
            x += w[j][i] * u[j]

        r[i] = x

    with open(("../rtable/lanczos_{}.txt").format(n), 'w') as f:
        for i in range(n):
            f.write(str(r[i]) + '\n')

    print(n)
