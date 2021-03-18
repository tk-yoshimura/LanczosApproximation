from fractions import Fraction

def array1d(n):
    return [0] * n

def array2d(m, n):
    return [[0 for j in range(m)] for i in range(n)]

def coef(n):
    m = array1d(n + 1)
    v = array1d(n + 1)
    
    m[0] = 1
    v[0] = 0

    for i in range(1, n + 1):
        for j in reversed(range(0, i - 1)):
            m[j + 1] = m[j] + m[j + 1] * i
            v[j + 2] = v[j + 1] - v[j + 2] * (i - 1)

        m[0] *= i
        m[i] = 1

        v[1] *= -(i - 1)
        v[i] = 1

    c = array2d(n, n)

    for i in range(n):
        r = m[n]
                
        c[i][n - 1] = 1

        for j in reversed(range(0, n - 1)):
            r = m[j + 1] - r * (i + 1)
            c[i][j] = r

    u = [None] * n

    for i in range(n):
        u[i] = v[i] - m[i]

    return c, u

def inverse(mat, n):
    v = array2d(n, n)

    for i in range(n): 
        for j in range(n): 
            v[i][j] = Fraction(1 if (i == j) else 0)
    
    for i in range(n): 
        inv_mii = Fraction(1, mat[i][i])
        mat[i][i] = 1;
        for j in range(i + 1, n):
            mat[i][j] *= inv_mii

        for j in range(n):
            v[i][j] *= inv_mii;
        
        for j in range(i + 1, n):
            mul = mat[j][i];
            mat[j][i] = 0;
            for k in range(i + 1, n):
                mat[j][k] -= mat[i][k] * mul

            for k in range(n):
                v[j][k] -= v[i][k] * mul;

    for i in reversed(range(n)): 
        for j in reversed(range(i)): 
            mul = mat[j][i]

            for k in range(i, n):
                mat[j][k] = 0

            for k in range(n):
                v[j][k] -= v[i][k] * mul

    return v