
import java.util.Arrays;
        import java.util.LinkedList;

public class DoubleStack {

    private LinkedList<Double> magazine;
    private static String[] operations = new String[]{"+", "-", "*", "/", "ROT", "SWAP"};

    public static void main(String[] argum) {

//        DoubleStack testStack = new DoubleStack();
//        System.out.println(interpret("-"));
//        System.out.println(interpret("+"));
//        System.out.println(interpret("*"));
//        System.out.println(interpret("1 3 *"));
//        System.out.println(interpret("2 3 SWAP -"));
//        System.out.println(interpret("2 5SWAP -"));
//        System.out.println(interpret("2 5 SWAP -"));
//        System.out.println(interpret("ROT - +"));
//        System.out.println(interpret("2 5 9 ROT + SWAP -"));


    }

    DoubleStack() {
        magazine = new LinkedList<>();
    }

    @Override
    public Object clone() throws CloneNotSupportedException {
        DoubleStack tempStack = new DoubleStack();

        for ( int i = size() - 1; i >= 0; i-- ) {
            tempStack.push(get(i));
        }

        return tempStack;
    }

    public boolean stEmpty() {
        return magazine.isEmpty();
    }


    public int size() {

        return magazine.size();
    }

    public double get(int index) {

        return magazine.get(index);
    }

    public void push(double a) {

        magazine.push(a);
    }

    public double pop() {

        if (stEmpty()) {
            throw new StackUnderflowException("");
        }

        return magazine.pop();
    }

    public void op(String s) {

        if (!Arrays.asList(operations).contains(s)) {
            throw new IllegalArgumentException();
        }

        if (s.equals("ROT") && magazine.size() < 3 || magazine.size() < 2) {
            throw new StackUnderflowException();
        }


        double firstElement = pop();
        double secondElement = pop();


        if (s.equals("+")) {
            push(firstElement + secondElement);
        } else if (s.equals("-")) {
            push(secondElement - firstElement);
        } else if (s.equals("*")) {
            push(firstElement * secondElement);
        } else if (s.equals("/")) {
            push(secondElement / firstElement);
        } else if (s.equals("SWAP")) {
            push(firstElement);
            push(secondElement);
        } else if (s.equals("ROT")) {

            double thirdElement = pop();

            push(secondElement);
            push(firstElement);
            push(thirdElement);

        }

    }


    public double tos() {
        if (stEmpty()) {
            throw new StackUnderflowException("Stack is empty");
        }

        return magazine.getFirst();
    }

    @Override
    public boolean equals(Object o) {
        DoubleStack toCompare = (DoubleStack) o;

        if (size() != toCompare.size()) {
            return false;
        }

        for ( int i = 0; i < toCompare.size(); i++ ) {
            if (get(i) != toCompare.get(i)) {
                return false;
            }
        }
        return true;

    }

    @Override
    public String toString() {

        StringBuffer stackToString = new StringBuffer();

        for ( int i = 0; i < magazine.size(); i++ ) {
            stackToString.insert(0, get(i));
        }
        return String.valueOf(stackToString);
    }

    public static double interpret(String pol) {

        String[] stringStack = pol.split("\\s+");
        String currentString;

        DoubleStack interStack = new DoubleStack();

        for ( int i = 0; i < stringStack.length; i++ ) {

            currentString = stringStack[i];

            if (currentString.equals("")) {
                continue;
            }

            if (currentString.matches("-?\\d+\\.?")) {
                interStack.push(Double.parseDouble(currentString));

            } else {

                try {
                    interStack.op(currentString);
                } catch (IllegalArgumentException e) {
                    throw new IllegalArgumentException(String.format("Interpreter ['%s']: Stack[%d] element is wrong. %s is not an operator", pol, i, currentString));

                } catch (StackUnderflowException e) {
                    if (currentString.equals("ROT")) {
                        throw new StackUnderflowException(String.format("Interpreter ['%s']; index[%d]; Operator ROT needs at least 3 argument, found %d", pol, i, interStack.size()));
                    } else {
                        throw new StackUnderflowException(String.format("Interpreter ['%s']; index[%d]; Operator '%s' needs at least 2 argument, found %d", pol, i, currentString, interStack.size()));
                    }
                }


            }
        }

        double lastElement = interStack.pop();

        if (interStack.stEmpty()) {
            return lastElement;
        } else {
            throw new StackNotEmpty("Stack has more than one element left");
        }

    }
}

class StackUnderflowException extends RuntimeException {

    StackUnderflowException(String message) {
        super(message);
    }

    public StackUnderflowException() {
    }
}

class StackNotEmpty extends RuntimeException {
    public StackNotEmpty(String message) {
        super(message);
    }
}
