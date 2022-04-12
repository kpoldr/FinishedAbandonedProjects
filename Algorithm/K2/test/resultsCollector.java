import java.io.FileWriter;
import java.io.IOException;
import java.io.Writer;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Random;

public class resultsCollector{

    private static ArrayList<Long> insertionSortTimes = new ArrayList<>();
    private static ArrayList<Long> binaryInsertionSortTimes = new ArrayList<>();
    private static ArrayList<Long> quickSortTimes = new ArrayList<>();
    private static ArrayList<Long> mergeSortTimes = new ArrayList<>();
    private static ArrayList<Long> APIsortTimes = new ArrayList<>();
    private static ArrayList<Integer> lengths = new ArrayList<>();

    /** maximal array length */
    static final int MAX_SIZE = 128000/ 2;

    /** number of competition rounds */
    static final int NUMBER_OF_ROUNDS = 5;

    /**
     * Main method.
     *
     * @param args
     *           command line parameters
     */
    public static void main(String[] args) {

        final double[] origArray = new double[MAX_SIZE];
        Random generator = new Random();
        for (int i = 0; i < MAX_SIZE; i++) {
            origArray[i] = generator.nextDouble()*1000.;
        }
        int rightLimit = MAX_SIZE / (int) Math.pow(2., NUMBER_OF_ROUNDS);

        // Start a competition
        for (int round = 0; round < NUMBER_OF_ROUNDS; round++) {
            double[] acopy;
            long stime, ftime, diff;
            rightLimit = 2 * rightLimit;
            System.out.println();
            lengths.add(rightLimit);
            System.out.println("Length: " + rightLimit);

            acopy = Arrays.copyOf(origArray, rightLimit);
            stime = System.nanoTime();
            DoubleSorting.insertionSort(acopy);
            ftime = System.nanoTime();
            diff = ftime - stime;
            insertionSortTimes.add(diff / 1000000);
            System.out.printf("%34s%11d%n", "Insertion sort: time (ms): ", diff / 1000000);
            DoubleSorting.checkOrder(acopy);

            acopy = Arrays.copyOf(origArray, rightLimit);
            stime = System.nanoTime();
            DoubleSorting.binaryInsertionSort(acopy);
            ftime = System.nanoTime();
            diff = ftime - stime;
            binaryInsertionSortTimes.add(diff / 1000000);
            System.out.printf("%34s%11d%n", "Binary insertion sort: time (ms): ", diff / 1000000);
            DoubleSorting.checkOrder(acopy);

            acopy = Arrays.copyOf(origArray, rightLimit);
            stime = System.nanoTime();
            DoubleSorting.mergeSort(acopy, 0, acopy.length);
            ftime = System.nanoTime();
            diff = ftime - stime;
            mergeSortTimes.add(diff / 1000000);
            System.out.printf("%34s%11d%n", "Merge sort: time (ms): ", diff / 1000000);
            DoubleSorting.checkOrder(acopy);

            acopy = Arrays.copyOf(origArray, rightLimit);
            stime = System.nanoTime();
            DoubleSorting.quickSort(acopy, 0, acopy.length);
            ftime = System.nanoTime();
            diff = ftime - stime;
            quickSortTimes.add(diff / 1000000);
            System.out.printf("%34s%11d%n", "Quicksort: time (ms): ", diff / 1000000);
            DoubleSorting.checkOrder(acopy);

            acopy = Arrays.copyOf(origArray, rightLimit);
            stime = System.nanoTime();
            Arrays.sort(acopy);
            ftime = System.nanoTime();
            diff = ftime - stime;
            APIsortTimes.add(diff / 1000000);
            System.out.printf("%34s%11d%n", "Java API  Arrays.sort: time (ms): ", diff / 1000000);
            DoubleSorting.checkOrder(acopy);
            writeToFile();
        }
    }

    public static void writeToFile(){
        try {
            Writer writer = new FileWriter("file.csv");
            writer.write("Insertion Test Times:,");
            writer.write("\nTEST SIZES");
            for (Integer size : lengths) {
                writer.write(size + ",");
            }
            for (Long time : insertionSortTimes) {
                writer.write(time + ",");
            }
            writer.write("\nBinary Insertion Test Times:,");
            for (Long time : binaryInsertionSortTimes) {
                writer.write(time + ",");
            }
            writer.write("\nMerge Sort Test Times:,");
            for (Long time : mergeSortTimes) {
                writer.write(time + ",");
            }
            writer.write("\nQuick Sort Test Times:,");
            for (Long time : quickSortTimes) {
                writer.write(time + ",");
            }
            writer.write("\nAPI Sort Test Times:,");
            for (Long time : APIsortTimes) {
                writer.write(time + ",");
            }
            writer.close();
        } catch (IOException e) {
            e.printStackTrace();
        }

    }


}