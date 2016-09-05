using System;

namespace DpSplines {

    /// <summary>
    /// Дискретный периодический сплайн с векторными коэфициентами.
    /// Теоретические материалы: 
    /// http://www.math.spbu.ru/ru/mmeh/AspDok/pub/2010/Chashnikov.pdf
    /// http://dha.spb.ru/PDF/discreteSplines.pdf
    /// </summary>
    public class DpSpline {

        /// <summary>
        /// Вычисляет узловые точки дискретного N-периодического сплайна с векторными коэфициентами.
        /// </summary>
        /// <param name="aPoints">Полюса сплайна (исходные точки). Должно быть не менее 2-х полюсов.</param>
        /// <param name="r">Порядок сплайна.</param>
        /// <param name="n">Число узлов между полюсами сплайна.</param>
        /// <param name="aIsIncludeOriginalPoints">True - сплайн будет проходить через полюса, false - сплайн не будет проходить через полюса.</param>
        /// <returns></returns>
        public static Vector2[] Calculate(Vector2[] aPoints, int r, int n = 5, bool aIsIncludeOriginalPoints = true) {
            if (aPoints == null) {
                throw new ArgumentNullException("aPoints");
            }

            if (aPoints.Length <= 2) {
                throw new ArgumentException("Число полюсов должно быть > 2.");
            }

            if (r <= 0) {
                throw new ArgumentException("Порядок сплайна должен быть > 0.");
            }

            if (n < 1) {
                throw new ArgumentException("Число узлов между полюсами сплайна должно быть >= 1.");
            }

            var m = aPoints.Length;
            var N = n * m;

            Vector2[] vectors;
            if (aIsIncludeOriginalPoints) {
                vectors = RecalculateVectors(aPoints, r, n, m);
            } else {
                vectors = new Vector2[m];
                aPoints.CopyTo(vectors, 0);
            }

            var qSpline = CalculateQSpline(n, m);
            var resultPoints = CalculateSSpline(vectors, qSpline, r, n, m);

            return resultPoints;
        }

        /// <summary>
        /// Вычисляет вектора дискретного периодического сплайна с векторными коэфициентами, согласно
        /// формулам http://www.math.spbu.ru/ru/mmeh/AspDok/pub/2010/Chashnikov.pdf (страница 7).
        /// </summary>
        /// <param name="vectors"></param>
        /// <param name="qSpline"></param>
        /// <param name="r"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private static Vector2[] CalculateSSpline(Vector2[] aVectors, double[] aQSpline, int r, int n, int m) {            
            var N = n * m;
            var sSpline = new Vector2[r + 1][];
            for (var i = 1; i <= r; ++i) {
                sSpline[i] = new Vector2[N];
            }

            for (var j = 0; j < N; ++j) {
                sSpline[1][j] = new Vector2(0, 0);
                for (var p = 0; p < m; ++p) {
                    sSpline[1][j] += aVectors[p] * aQSpline[GetPositiveIndex(j - p * n, N)];
                }
            }

            for (var v = 2; v <= r; ++v) {
                for (var j = 0; j < N; ++j) {
                    sSpline[v][j] = new Vector2(0, 0);
                    for (var k = 0; k < N; ++k) {
                        sSpline[v][j] += aQSpline[k] * sSpline[v - 1][GetPositiveIndex(j - k, N)];
                    }
                    sSpline[v][j] /= n;
                }
            }

            return sSpline[r];
        }        

        /// <summary>
        /// Вычисляет коэфициенты дискретного периодического Q-сплайна 1-ого порядка, согдасно 
        /// формулам http://www.math.spbu.ru/ru/mmeh/AspDok/pub/2010/Chashnikov.pdf (страница 6).
        /// </summary>
        /// <param name="n">Число узлов между полюсами.</param>
        /// <param name="m">Число полюсов.</param>
        /// <returns>Коэфициенты дискретного периодического Q-сплайна 1-ого порядка.</returns>
        private static double[] CalculateQSpline(int n, int m) {
            var N = n * m;
            var qSpline = new double[N];

            for (var j = 0; j < N; ++j) {
                if (j >= 0 && j <= n - 1) {
                    qSpline[j] = (1.0 * n - j) / n;
                }
                if (j >= n && j <= N - n) {
                    qSpline[j] = 0;
                }
                if (j >= N - n + 1 && j <= N - 1) {
                    qSpline[j] = (1.0 * j - N + n) / n;
                }
            }

            return qSpline;
        }

        /// <summary>
        /// Пересчитывает коэфициенты сплайна для того, чтобы результирующий сплайн проходил через полюса.
        /// http://dha.spb.ru/PDF/discreteSplines.pdf (страница 6 и 7). 
        /// </summary>
        /// <param name="aPoints">Исходные точки.</param>
        /// <param name="r">Порядок сплайна.</param>
        /// <param name="n">Количество узлов между полюсами сплайна.</param>
        /// <param name="m">Количество полюсов.</param>
        /// <returns></returns>
        private static Vector2[] RecalculateVectors(Vector2[] aPoints, int r, int n, int m) {
            var N = n * m;

            // Вычисляем знаменатель.
            var tr = new double[m];
            tr[0] = 1;            
            for (var k = 1; k < m; ++k) {
                for (var q = 0; q < n; ++q) {
                    tr[k] += Math.Pow(2 * n * Math.Sin((Math.PI * (q * m + k)) / N), -2 * r);
                }
                tr[k] *= Math.Pow(2 * Math.Sin((Math.PI * k) / m), 2 * r);
            }

            // Вычисляем числитель.
            var zre = new Vector2[m];
            var zim = new Vector2[m];
            for (var j = 0; j < m; ++j) {
                zre[j] = new Vector2(0, 0);
                zim[j] = new Vector2(0, 0);
                for (var k = 0; k < m; ++k) {
                    zre[j] += aPoints[k] * Math.Cos((-2 * Math.PI * j * k) / m);
                    zim[j] += aPoints[k] * Math.Sin((-2 * Math.PI * j * k) / m);
                }
            }

            // Считаем результат.
            var result = new Vector2[m];
            for (var p = 0; p < m; ++p) {
                result[p] = new Vector2(0, 0);
                for (var k = 0; k < m; ++k) {
                    var d = (zre[k] * Math.Cos((2 * Math.PI * k * p) / m)) - (zim[k] * Math.Sin((2 * Math.PI * k * p) / m));
                    d *= 1.0 / tr[k];
                    result[p] += d;
                }
                result[p] /= m;
            }

            return result;
        }

        /// <summary>
        /// Обеспечивает периодичность для заданного множества.
        /// </summary>
        /// <param name="j">Индекс элемента.</param>
        /// <param name="N">Количество элементов.</param>
        /// <returns>Периодический индекс элемента.</returns>
        private static int GetPositiveIndex(int j, int N) {
            if (j >= 0) {
                return j % N;
            }

            return N - 1 + ((j + 1) % N);
        }
    }
}
