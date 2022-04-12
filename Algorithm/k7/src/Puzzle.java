import java.util.*;

public class Puzzle {

    static List<Letter> firstWord;
    static List<Letter> secondWord;
    static List<Letter> answer;
    static List<String> stringAnswers;
    static int solutions;

    public static void main(String[] args) {
        new Puzzle(args);
    }

    public Puzzle(String[] args) {
        answer = new ArrayList<>();
        secondWord = new ArrayList<>();
        firstWord = new ArrayList<>();
        stringAnswers = new ArrayList<>();
        solutions = 0;

        errorChecking(args);
        solvePuzzle(args);
    }

    /**
     * Checks different possible errors.
     */
    public static void errorChecking(String[] words) {

        if (words.length != 3) {
            throw new WordException(String.format("%s is not three words long", Arrays.toString(words)));
        }

        for (int i = 0; i < words.length; i++) {
            String word = words[i];

            if (word.length() > 18) {
                throw new WordException(String.format("%s | '%s' is longer than 18 characters.",
                        Arrays.toString(words), word));
            }

        }
    }

    /**
     * Creates a list of the characters that are used in the equation.
     * Also checks if the given characters are correct.
     */
    public static Set<Letter> createCharacterList(String[] words) {

        Set<Letter> setLetters = new HashSet<>();

        for (int i = 0; i < words.length; i++) {
            String word = words[i];

            for (int j = 0; j < word.length(); j++) {
                if ("ABCDEFGHIJKLMNOPQRSTUVWXY".indexOf(word.charAt(j)) != -1) {

                    boolean isFirst = j == 0;

                    Letter toAdd = new Letter(word.charAt(j), isFirst, -1);
                    setLetters.add(toAdd);
                } else {
                    throw new WordException(String.format("%s | '%s' features a bad character: '%s' ",
                            Arrays.toString(words), word, word.charAt(j)));
                }
            }
        }

        if (setLetters.size() > 10) {
            throw new WordException(String.format("%s | words feature " +
                            "too many different characters, maximum of 10 allowed"
                    , Arrays.toString(words)));
        }
        return setLetters;
    }

    /**
     * Solves the puzzle.
     * returns -1 if there are no answers for the equation
     */
    public static int solvePuzzle(String[] words) {

        if (words[0].equals(words[2]) || words[1].equals(words[2])) {
            return -1;
        }

        Set<Letter> setLetters = createCharacterList(words);
        Letter[] lettersArray = new Letter[setLetters.size()];
        lettersArray = setLetters.toArray(lettersArray);
        letterWordMatcher(words, lettersArray);


        List<int[]> combinations = generateCombinations(lettersArray.length);
        for (int i = 0; i < combinations.size(); i++) {
            generatePermutations(lettersArray.length, combinations.get(i), lettersArray);
        }

        System.out.println(generateAnswer(words));

        if (solutions == 0) {
            return -1;
        }

        return (solutions);
    }

    /**
     * Sorts letters to three different lists to make calculations faster.
     * Super inefficient way to sort them, but it's relatively small of a task compared to the actual exercise.
     */
    public static void letterWordMatcher(String[] words, Letter[] letters) {
        for (int i = 0; i < words.length; i++) {
            for (int j = 0; j < words[i].length(); j++) {
                for (int k = 0; k < letters.length; k++) {
                    if (words[i].charAt(j) == letters[k].getChar()) {
                        if (i == 0) {
                            firstWord.add(letters[k]);
                        } else if (i == 1) {
                            secondWord.add(letters[k]);
                        } else {
                            answer.add(letters[k]);
                        }
                    }
                }
            }
        }
    }

    /**
     * Generates the final answer for the puzzle.
     */
    public static String generateAnswer(String[] words) {
        if (solutions == 1) {
            return String.format("[%s + %s = %s] | There's 1 solution for the equation: %n%n%s%n", words[0], words[1], words[2], stringAnswers.get(0));
        } else if (solutions == 2) {
            return String.format("[%s + %s = %s] | There are 2 solution for the equation: %n%n%s%n%n%s%n", words[0],
                    words[1], words[2], stringAnswers.get(0), stringAnswers.get(1));
        } else if (solutions >= 3) {
            return String.format("[%s + %s = %s] | There are %d solution for the equation. " +
                            "Here are some examples: %n%n%s%n%n%s%n%n%s%n", words[0], words[1], words[2], solutions,
                    stringAnswers.get(0), stringAnswers.get(1), stringAnswers.get(3));
        } else {
            return String.format("[%s + %s = %s] | There are no solutions for the equation", words[0], words[1], words[2]);
        }
    }

    /**
     * Solves the equation and adds to the solution counter.
     */
    public static void equationMaker(Letter[] letters) {

        if (firstWord.get(0).getValue() == 0 || secondWord.get(0).getValue() == 0 || answer.get(0).getValue() == 0) {
            return;
        }

        long firstWordValue = stringValueFinder(firstWord);
        long secondWordValue = stringValueFinder(secondWord);
        long answerWordValue = stringValueFinder(answer);


        if (firstWordValue + secondWordValue == answerWordValue) {
            solutions++;
            if (stringAnswers.size() <= 3) {
                StringBuffer tempBuffer = new StringBuffer();
                tempBuffer.append(String.format("[%d + %d = %d] \n", firstWordValue, secondWordValue, answerWordValue));
                tempBuffer.append("(");

                for (int i = 0; i < letters.length; i++) {
                    tempBuffer.append(letters[i]);
                    tempBuffer.append(": ");
                    tempBuffer.append(letters[i].getValue());
                    if (i != letters.length - 1) {
                        tempBuffer.append(",");
                        tempBuffer.append(" ");
                    }

                }
                tempBuffer.append(")");
                stringAnswers.add(tempBuffer.toString());
            }
        }
    }


    /**
     * Takes the given word and based on the letter values, turns it into a long value.
     */
    public static long stringValueFinder(List<Letter> word) {
        StringBuilder tempBuffer = new StringBuilder("");
        for (int i = 0; i < word.size(); i++) {
            tempBuffer.append(word.get(i).getValue());
        }

        return Long.parseLong(tempBuffer.toString());
    }

    /**
     * Generates all the different combinations of the given numbers.
     * Returns a list of all the combinations.
     */
    public static List<int[]> generateCombinations(int r) {
        List<int[]> combinations = new ArrayList<>();
        int[] combination = new int[r];

        for (int i = 0; i < r; i++) {
            combination[i] = i;
        }

        while (combination[r - 1] < 10) {
            combinations.add(combination.clone());

            int t = r - 1;
            while (t != 0 && combination[t] == 10 - r + t) {
                t--;
            }
            combination[t]++;
            for (int i = t + 1; i < r; i++) {
                combination[i] = combination[i - 1] + 1;
            }
        }
        return combinations;
    }
    //https://www.baeldung.com/java-combinations-algorithm


    /**
     * Takes the generated combinations and uses them to generate permutations.
     * Then tries them in an equation.
     */
    public static void generatePermutations(int n, int[] elements, Letter[] letters) {
        if (n == 1) {

            for (int i = 0; i < elements.length; i++) {
                letters[i].value = elements[i];
            }
            equationMaker(letters);

        } else {
            for (int i = 0; i < n - 1; i++) {
                generatePermutations(n - 1, elements, letters);
                if (n % 2 == 0) {
                    swap(elements, i, n - 1);
                } else {
                    swap(elements, 0, n - 1);
                }
            }
            generatePermutations(n - 1, elements, letters);
        }
    }
//    https://www.baeldung.com/java-array-permutations

    /**
     * Swaps two given values
     */
    private static void swap(int[] input, int a, int b) {
        int tmp = input[a];
        input[a] = input[b];
        input[b] = tmp;
    }


}


class Letter {
    Character name;
    boolean isFirst;
    int value;

    public Letter(Character name, boolean isFirst, int value) {
        this.name = name;
        this.isFirst = isFirst;
        this.value = value;

    }

    @Override
    public boolean equals(Object obj) {
        if (obj == null) return false;
        if (obj instanceof Character) {
            return this.getChar().equals(obj);
        }
        if (!(obj instanceof Letter))
            return false;

        if (obj == this)
            return true;
        return this.getChar() == ((Letter) obj).getChar();
    }

    @Override
    public int hashCode() {
        return Objects.hash(name);
    }

    @Override
    public String toString() {
        return String.valueOf(this.name);
    }

    public Character getChar() {
        return name;
    }

    public int getValue() {
        return value;
    }


}

class WordException extends RuntimeException {

    WordException(String message) {
        super(message);
    }
}

