namespace DpSplines {

    /// <summary>
    /// Реализует методы и операции для работы с 2-д вектором.
    /// </summary>
    public class Vector2 {

        public double X;
        public double Y;

        /// <summary>
        /// Конструктор по-умолчанию.
        /// </summary>
        public Vector2() {
            this.X = 0;
            this.Y = 0;
        }

        /// <summary>
        /// Конструктор. Принимает координаты.
        /// </summary>
        /// <param name="x">Координата Х.</param>
        /// <param name="y">Координата Y.</param>
        public Vector2(double x, double y) {
            this.X = x;
            this.Y = y;
        }        

        /// <summary>
        ///  Конструктор. Принимает Другое вектор.
        /// </summary>
        /// <param name="v">Исходный вектор.</param>
        public Vector2(Vector2 v) {
            X = v.X;
            Y = v.Y;
        }

        /// <summary>
        /// Реализует сложение векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        /// <returns>Результат сложения.</returns>
        public static Vector2 operator +(Vector2 a, Vector2 b) {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Реализует вычитание векторов.
        /// </summary>
        /// <param name="a">Первый вектор.</param>
        /// <param name="b">Второй вектор.</param>
        /// <returns>Результат вычитания.</returns>
        public static Vector2 operator -(Vector2 a, Vector2 b) {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Реализует унарный минус.
        /// </summary>
        /// <param name="a">Исходный вектор.</param>
        /// <returns>Результат применения унарного минуса.</returns>
        public static Vector2 operator -(Vector2 a) {
            return new Vector2(-a.X, -a.Y);
        }

        /// <summary>
        /// Реализует умножение вектора на число.
        /// </summary>
        /// <param name="a">Исходный вектор.</param>
        /// <param name="d">Число.</param>
        /// <returns>Рузельтат умножения вектора на число.</returns>
        public static Vector2 operator *(Vector2 a, double d) {
            return new Vector2(a.X * d, a.Y * d);
        }

        /// <summary>
        /// Реализует умножение числа на вектор.
        /// </summary>
        /// <param name="d">Число.</param>
        /// <param name="a">Исходный вектор.</param>
        /// <returns>Результат умножения.</returns>
        public static Vector2 operator *(double d, Vector2 a) {
            return a * d;
        }

        /// <summary>
        /// Реализует умножение вектора на число.
        /// </summary>
        /// <param name="a">Исходный вектор.</param>
        /// <param name="f">Число.</param>
        /// <returns>Рузельтат умножения вектора на число.</returns>
        public static Vector2 operator *(Vector2 a, float f) {
            return a * (double)f;
        }

        /// <summary>
        /// Реализует умножение числа на вектор.
        /// </summary>
        /// <param name="f">Число.</param>
        /// <param name="a">Исходный вектор.</param>
        /// <returns>Результат умножения.</returns>
        public static Vector2 operator *(float f, Vector2 a) {
            return a * (double)f;
        }

        /// <summary>
        /// Реализует деление вектора на число.
        /// </summary>
        /// <param name="a">Исходный вектор.</param>
        /// <param name="d">Число.</param>
        /// <returns>Результат деления вектора на число.</returns>
        public static Vector2 operator /(Vector2 a, double d) {
            return new Vector2(a.X / d, a.Y / d);
        }

        /// <summary>
        /// Реализует деление вектора на число.
        /// </summary>
        /// <param name="a">Исходный вектор.</param>
        /// <param name="f">Число.</param>
        /// <returns>Результат деления вектора на число.</returns>
        public static Vector2 operator /(Vector2 a, float f) {
            return a / (double)f;
        }
    }
}
