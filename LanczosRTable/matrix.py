import numpy as np
import math
from fractions import Fraction

from tqdm import tqdm as progressbar

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
                
        c[n - 1, i] = 1

        for j in reversed(range(0, n - 1)):
            r = m[j + 1] - r * (i + 1)
            c[j, i] = r

    for i in range(n):
        u[i] = v[i] - m[i]

    return c, u

def abs(x):
    return x if x > 0 else -x

def gcd(a, b):
    a = abs(a)
    b = abs(b)
    return math.gcd(a, b)

def solve_linear(m, x, with_progressbar=False):
    n = x.shape[0]

    if m.shape != (n, n) or x.ndim != 1:
        raise ValueError('shape m, n')

    m, x = np.flip(m, axis = 0).astype(np.object), np.flip(x, axis = 0).astype(np.object)

    if with_progressbar:
        progress = progressbar(total = n * n + n, desc='solve', smoothing=0.01, mininterval=1)
    
    for i in range(n):
        mii = m[i, i]
        
        for j in range(i + 1, n):
            g = gcd(mii, m[j, i])
            mi, mj = m[j, i] // g, mii // g

            m[j, i] = 0            
            for k in range(i + 1, n):
                m[j, k] = m[j, k] * mj - m[i, k] * mi

            x[j] = x[j] * mj - x[i] * mi
            
            if with_progressbar:
                progress.update(1)

    for i in reversed(range(n)): 
        g = gcd(m[i, i], x[i])
        m[i, i], x[i] = m[i, i] // g, x[i] // g

        mii = m[i, i]

        for j in reversed(range(i)): 
            g = gcd(mii, m[j, i])
            mi, mj = m[j, i] // g, mii // g

            for k in range(i):
                m[j, k] *= mj
            
            for k in range(i, n):
                m[j, k] = 0

            x[j] = x[j] * mj - x[i] * mi
            
        if with_progressbar:
            progress.update(1)
            
    if with_progressbar:
        progress.close()

    for i in reversed(range(n)):
        x[i] = Fraction(x[i], m[i, i])

    return x