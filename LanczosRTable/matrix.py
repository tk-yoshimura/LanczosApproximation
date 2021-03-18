from tqdm import tqdm as progressbar
import numpy as np
from fractions import Fraction

def array1d(n):
    arr = np.full((n), 0, dtype=np.object)

    return arr

def array2d(m, n):
    arr = np.full((m, n), 0, dtype=np.object)

    return arr

def coef(n):
    m = array1d(n + 1)
    v = array1d(n + 1)
    
    c = array2d(n, n)
    u = array1d(n)
    
    m[0] = 1

    for i in range(1, n + 1):
        for j in reversed(range(0, i - 1)):
            m[j + 1] = m[j] + m[j + 1] * i
            v[j + 2] = v[j + 1] - v[j + 2] * (i - 1)

        m[0] *= i
        m[i] = 1

        v[1] *= -(i - 1)
        v[i] = 1

    for i in range(n):
        r = m[n]
                
        c[n-1, i] = 1

        for j in reversed(range(0, n - 1)):
            r = m[j + 1] - r * (i + 1)
            c[j, i] = r

    for i in range(n):
        u[i] = v[i] - m[i]

    return c, u

def solve_linear(m, x, with_progressbar = False):
    n = x.shape[0]

    if m.shape != (n, n) or x.ndim != 1:
        raise ValueError('shape m, n')

    m, x = m.copy().astype(np.object), x.copy().astype(np.object)

    if with_progressbar:
        progress = progressbar(total = n * n + n, desc='solve', smoothing=0.01, mininterval=1)
    
    for i in range(n): 
        inv_mii = Fraction(1, m[i, i])
        m[i, i] = 1
        for j in range(i + 1, n):
            m[i, j] *= inv_mii
        
        x[i] *= inv_mii
        
        for j in range(i + 1, n):
            mul = m[j, i]
            m[j, i] = 0

            for k in range(i + 1, n):
                m[j, k] -= m[i, k] * mul

            x[j] -= x[i] * mul

            if with_progressbar:
                progress.update(1)

    for i in reversed(range(n)): 
        for j in reversed(range(i)): 
            mul = m[j, i]

            for k in range(i, n):
                m[j, k] = 0

            x[j] -= x[i] * mul

        if with_progressbar:
            progress.update(1)

    if with_progressbar:
        progress.close()

    return x
