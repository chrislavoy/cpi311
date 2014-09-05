namespace CPI311.Labs
{
    public struct Fraction
    {
        private int numerator;
        private int denominator;

        /// <summary>
        /// Numerator of this fraction
        /// </summary>
        public int Numerator
        {
            get { return numerator; }
            set { numerator = value; Simplify(); }
        }

        /// <summary>
        /// Denominator of this fraction
        /// </summary>
        public int Denominator
        {
            get { return denominator; }
            set
            {
                if (value == 0)
                    value = 1;
                denominator = value;
                Simplify();
            }
        }

        /// <summary>
        /// Creates a new Fraction object using the parameters
        /// </summary>
        /// <param name="n">Numerator</param>
        /// <param name="d">Denominator</param>
        public Fraction(int n = 0, int d = 1)
        {
            numerator = n;
            // Denominator check - cannot be zero
            if (d == 0)
                d = 1;
            denominator = d;
            Simplify();
        }

        /// <summary>
        /// Simplifies the fraction by dividing the numerator and denominator by their GCD.
        /// Further ensures that the denominator is not negative.
        /// </summary>
        private void Simplify()
        {
            // Reduce the number by dividing both the numerator and
            // denominator by their GCD
            int gcd = GCD(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;

            if (denominator < 0)
            {
                denominator *= -1;
                numerator *= -1;
            }
            // Ensure that Denomainator is positive
            // NOTE: The denominator check is done after reduction to solve
            // the problem of possible negative value from the GCD method
        }

        /// <summary>
        /// Computes a multiplication of two input fractions
        /// </summary>
        /// <param name="lhs">Multiplicand</param>
        /// <param name="rhs">Multiplier</param>
        /// <returns>Product</returns>
        public static Fraction operator *(Fraction lhs, Fraction rhs)
        {
            return new Fraction(lhs.numerator * rhs.numerator,
                                lhs.denominator * rhs.denominator);
        }

        /// <summary>
        /// Divides one fraction with another
        /// </summary>
        /// <param name="lhs">Dividend</param>
        /// <param name="rhs">Divisor</param>
        /// <returns>Quotient (fraction)</returns>
        public static Fraction operator /(Fraction lhs, Fraction rhs)
        {
            return new Fraction(lhs.numerator * rhs.denominator,
                                lhs.denominator * rhs.numerator);
        }

        /// <summary>
        /// Computes an addition of two input fractions
        /// </summary>
        /// <param name="lhs">Augend</param>
        /// <param name="rhs">Addend</param>
        /// <returns>Sum</returns>
        public static Fraction operator +(Fraction lhs, Fraction rhs)
        {
            int lcm = lhs.denominator / GCD(lhs.denominator, rhs.denominator) * rhs.denominator;
            return new Fraction(lcm / lhs.denominator * lhs.numerator + lcm / rhs.denominator * rhs.numerator, lcm);
        }

        /// <summary>
        /// Subtracts one fraction from another
        /// </summary>
        /// <param name="lhs">Minuend</param>
        /// <param name="rhs">Subtrahend</param>
        /// <returns>Difference</returns>
        public static Fraction operator -(Fraction lhs, Fraction rhs)
        {
            rhs.numerator *= -1;
            return lhs + rhs;
        }

        /// <summary>
        /// Computes the greatest common divisor (GCD) of two input integers.
        /// NOTE: The result could be negative if the inputs aren't both positive
        /// </summary>
        /// <param name="a">First Integer</param>
        /// <param name="b">Second Integer</param>
        /// <returns>Greatest Common Divisor</returns>
        public static int GCD(int a, int b)
        {
            // GCD is not defined if either number is 0, but we return larger value
            // Why? So 0/x becomes 0/1 even if we call GCD(d,n) instead of GCD(n,d)
            if (b == 0)
                return a;
            // OK, b is not zero. Store numbers into an array for convenience
            int[] input = { a, b };
            int larger = 0; // Current larger number index
            // Keep finding remainders until the one is zero
            while ((input[larger] %= input[larger = (larger + 1) % 2]) != 0) ;
            /* The above is an example of how code should NOT be written. It is
             * obscure, esoteric, and prone to errors. Just included as example.
             * It works only because the operations are performed left-to-right,
             * but assignment (=) is done right-to-left:
             * Eg: let's say larger = 1 then
             * input[larger] %= input[larger = (larger + 1) % 2]
             * => input[1] %= input[larger = (larger + 1) % 2]
             * => input[1] %= input[larger = (1 + 1) % 2]
             * => input[1] %= input[larger = 0]
             * => input[1] %= input[0] (now, larger = 0)
             * Also notice that the while loop has a null statement.
             * See also: Discussion Board
             */
            // OK, remainder is zero, return the other one
            return input[larger];
        }

        /// <summary>
        /// Converts a fraction to string
        /// </summary>
        /// <returns>string representation of this fraction object</returns>
        public override string ToString()
        {
            return numerator + "/" + denominator;
        }

    }
}
