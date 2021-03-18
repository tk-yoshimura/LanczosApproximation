from matrix import coef, solve_linear

for n in range(1, 1024 + 1):

    c, u = coef(n)
    r = solve_linear(c, u)

    with open(("../rtable/lanczos_{}.txt").format(n), 'w') as f:
        for i in range(n):
            f.write(str(r[i]) + '\n')

    print(n)
