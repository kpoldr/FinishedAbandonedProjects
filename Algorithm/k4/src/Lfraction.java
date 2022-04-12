import java.util.Objects;

/**
 * This class represents fractions of form n/d where n and d are long integer
 * numbers. Basic operations and arithmetics for fractions are provided.
 */
public class Lfraction implements Comparable<Lfraction> {

    long[] fraction;

    /**
     * Main method. Different tests.
     */
    public static void main(String[] param) {
        System.out.println(valueOf("2/-4"));
//        System.out.println(valueOf("2/-4"));
//        System.out.println(valueOf("2/0"));
//        System.out.println(valueOf("a/3"));
//        System.out.println(valueOf("2/b"));
//        System.out.println(valueOf("22"));
//        System.out.println(valueOf("22/2/2"));
//        System.out.println(valueOf("/2"));
//        System.out.println(valueOf("2/"));
    }

    /**
     * Constructor.
     *
     * @param a numerator
     * @param b denominator > 0
     */
    public Lfraction(long a, long b) {
        if (b == 0) throw new DivisionWithZero();
        long greatestCommonDivisor = Math.abs(simplify(a, b));
        fraction = new long[]{a / greatestCommonDivisor, b / greatestCommonDivisor};

    }

    /**
     * Public method to access the numerator field.
     *
     * @return numerator
     */
    public long getNumerator() {
        return fraction[0];
    }

    public Lfraction getFraction() {
        return new Lfraction(getNumerator(), getDenominator());
    }

    /**
     * Public method to access the denominator field.
     *
     * @return denominator
     */
    public long getDenominator() {
        return fraction[1];
    }

    /**
     * Conversion to string.
     *
     * @return string representation of the fraction
     */
    @Override
    public String toString() {
        return String.format("%d/%d", fraction[0], fraction[1]);
    }

    /**
     * Equality test.
     *
     * @param m second fraction
     * @return true if fractions this and m are equal
     */
    @Override
    public boolean equals(Object m) {
        Lfraction toCompare = (Lfraction) m;

        Lfraction tempFraction1 = new Lfraction(getNumerator(), getDenominator());
        Lfraction tempFraction2 = new Lfraction(toCompare.getNumerator(), toCompare.getDenominator());

        if (tempFraction1.getDenominator() != tempFraction2.getDenominator()) {
            return false;
        } else {
            return tempFraction1.getNumerator() == tempFraction2.getNumerator();
        }
    }

    /**
     * Hashcode has to be equal for equal fractions.
     *
     * @return hashcode
     */
    @Override
    public int hashCode() {
        return Objects.hash(getNumerator(), getDenominator());

    }

    /**
     * Sum of fractions.
     *
     * @param m second addend
     * @return this+m
     */
    public Lfraction plus(Lfraction m) {
        if (getDenominator() == m.getDenominator()) {
            return (new Lfraction(getNumerator() + m.getNumerator(), getDenominator()));

        } else {
            long[] mutualDenominators = findMutualDenominators(getFraction(), m);
            return new Lfraction((mutualDenominators[0] + mutualDenominators[1]), mutualDenominators[2]);

        }
    }

    /**
     * Multiplication of fractions.
     *
     * @param m second factor
     * @return this*m
     */
    public Lfraction times(Lfraction m) {
        return new Lfraction(getNumerator() * m.getNumerator(), getDenominator() * m.getDenominator());
    }

    /**
     * Inverse of the fraction. n/d becomes d/n.
     *
     * @return inverse of this fraction: 1/this
     */
    public Lfraction inverse() {
        if (getNumerator() == 0) throw new DivisionWithZero();
        long tempDenominator = getDenominator();
        long tempNumerator = getNumerator();

        if (tempNumerator < 0) {
            tempDenominator *= -1;
            tempNumerator *= -1;
        }

        return new Lfraction(tempDenominator, tempNumerator);
    }

    /**
     * Opposite of the fraction. n/d becomes -n/d.
     *
     * @return opposite of this fraction: -this
     */
    public Lfraction opposite() {
        return new Lfraction(getNumerator() * -1, getDenominator());
    }


    /**
     * Difference of fractions.
     *
     * @param m subtrahend
     * @return this-m
     */
    public Lfraction minus(Lfraction m) {

        return (plus(m.opposite()));
    }

    /**
     * Quotient of fractions.
     *
     * @param m divisor
     * @return this/m
     */
    public Lfraction divideBy(Lfraction m) {
        if (m.getNumerator() == 0) throw new DivisionWithZero();
        return times(m.inverse());
    }

    /**
     * Comparision of fractions.
     *
     * @param m second fraction
     * @return -1 if this < m; 0 if this==m; 1 if this > m
     */
    @Override
    public int compareTo(Lfraction m) {
        long[] numeratorsWithMutualDenominators = findMutualDenominators(getFraction(), m);
        long firstNumerator = numeratorsWithMutualDenominators[0];
        long secondNumerator = numeratorsWithMutualDenominators[1];

        if (firstNumerator < secondNumerator) {
            return -1;
        } else if (firstNumerator > secondNumerator) {
            return 1;
        } else {
            return 0;
        }

    }

    /**
     * Clone of the fraction.
     *
     * @return new fraction equal to this
     */
    @Override
    public Object clone() throws CloneNotSupportedException {
        return new Lfraction(getNumerator(), getDenominator());

    }

    /**
     * Integer part of the (improper) fraction.
     *
     * @return integer part of this fraction
     */
    public long integerPart() {
        return (getNumerator() / getDenominator());
    }

    /**
     * Extract fraction part of the (improper) fraction
     * (a proper fraction without the integer part).
     *
     * @return fraction part of this fraction
     */
    public Lfraction fractionPart() {
        return new Lfraction(getNumerator() % getDenominator(), getDenominator());
    }

    /**
     * Approximate value of the fraction.
     *
     * @return numeric value of this fraction
     */
    public double toDouble() {
        return Double.valueOf(getNumerator()) / Double.valueOf(getDenominator());
    }

    /**
     * Double value f presented as a fraction with denominator d > 0.
     *
     * @param f real number
     * @param d positive denominator for the result
     * @return f as an approximate fraction of form n/d
     */
    public static Lfraction toLfraction(double f, long d) {
        return new Lfraction(Math.round(f * d), d);

    }

    /**
     * Conversion from string to the fraction. Accepts strings of form
     * that is defined by the toString method.
     *
     * @param s string form (as produced by toString) of the fraction
     * @return fraction represented by s
     */
    public static Lfraction valueOf(String s) {
        String[] stringFraction = s.split("/");

        if (stringFraction.length != 2 || stringFraction[0].equals("") || stringFraction[1].equals("")) {
            throw new StringNotAFraction(String.format("String '%s' is not a fraction", s));
        }

        long tempNumerator;
        long tempDenominator;
        String tempStringHolder = stringFraction[0];

        try {
            tempNumerator = Long.parseLong(stringFraction[0]);
            tempStringHolder = stringFraction[1];
            tempDenominator = Long.parseLong(stringFraction[1]);

        } catch (RuntimeException e) {
            throw new ElementIsNotALong(String.format("'%s' | '%s' is not a long", s, tempStringHolder));
        }

        if (tempStringHolder.equals("0")) {
            throw new DivisionWithZero(String.format("'%s' | denominator cannot be zero", s));
        }

        if (tempDenominator < 0) {
            tempDenominator *= -1;
            tempNumerator *= -1;
        }
        return new Lfraction(tempNumerator, tempDenominator);
    }

    public static long simplify(long numerator, long deniminator) {

        long temp;

        while (deniminator != 0) {
            temp = deniminator;
            deniminator = numerator % deniminator;
            numerator = temp;
        }

        return numerator;

        //https://en.m.wikipedia.org/wiki/Euclidean_algorithm
    }

    public static long[] findMutualDenominators(Lfraction firstFraction, Lfraction secondFraction) {

        long firstNumerator = firstFraction.getNumerator();
        long firstDenominator = firstFraction.getDenominator();
        long secondNumerator = secondFraction.getNumerator();
        long secondDenominator = secondFraction.getDenominator();
        long mutualDenominator;

        if (firstDenominator % secondDenominator == 0) {
            secondNumerator = (firstDenominator / secondDenominator) * secondNumerator;
            mutualDenominator = firstDenominator;
        } else if (secondDenominator % firstDenominator == 0) {
            firstNumerator = (secondDenominator / firstDenominator) * firstNumerator;
            mutualDenominator = secondDenominator;
        } else {
            mutualDenominator = secondDenominator * firstDenominator;
            firstNumerator = firstNumerator * secondDenominator;
            secondNumerator = secondNumerator * firstDenominator;
        }
        return new long[]{firstNumerator, secondNumerator, mutualDenominator};
    }
}

class DivisionWithZero extends RuntimeException {
    public DivisionWithZero() {
    }

    public DivisionWithZero(String message) {
        super(message);
    }
}

class ElementIsNotALong extends RuntimeException {

    ElementIsNotALong(String message) {
        super(message);
    }
}

class StringNotAFraction extends RuntimeException {

    StringNotAFraction(String message) {
        super(message);
    }
}