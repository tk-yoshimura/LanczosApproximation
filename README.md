# LanczosApproximation

Lanczos approximation is a method for computing the gamma function numerically, published by Cornelius Lanczos in 1964. 
It is a practical alternative to the more popular Stirling's approximation for calculating the gamma function with fixed precision.

The Lanczos approximation consists of the formula:

![Gamma Lanczos Approximation](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/figures/fig1.svg)

for the gamma function, with

![Ag term](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/figures/fig2.svg)

The p coefficients are given by:

![p coef](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/figures/fig8.svg)

where C represents the (n, m)th element of the matrix of coefficients for the Chebyshev polynomials.

as equal to:

![gamma2](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/figures/fig9.svg)
![Ag term2](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/figures/fig10.svg)
![p coef2](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/figures/fig11.svg)

If a fixed g is chosen, the coefficients can be calculated in advance and the sum is recast into the following form:

![Ag term expand](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/figures/fig3.svg)

Refer to [Ag table](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/results "Ag table").

The multiplier of p is calculated from the following formula:

![c series](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/figures/fig4.svg)

N = 1:

![N1](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/figures/fig5.svg)

N = 2:

![N2](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/figures/fig6.svg)

N = 3:

![N3](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/figures/fig7.svg)

N &gt; 3:

Refer to [r table](https://github.com/tk-yoshimura/LanczosApproximation/blob/main/rtable "r table").

(Quote: [wikipedia](https://en.wikipedia.org/wiki/Lanczos_approximation "wikipedia"))
