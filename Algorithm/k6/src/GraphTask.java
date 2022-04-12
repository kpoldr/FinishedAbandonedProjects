import java.util.*;

/**
 * Container class to different classes, that makes the whole
 * set of classes one class formally.
 */
public class GraphTask {

    public static void main(String[] args) {
        GraphTask a = new GraphTask();
        a.run();
    }

    /**
     * Actual main method to run examples and everything.
     */
    public void run() {
        Graph g = new Graph("G");

        g.createRandomSimpleGraph(5, 5);
        System.out.println(g);

    }

    /**
     * Vertex represents one point in a graph. It has Arcs that point to other vertices.
     */
    class Vertex {

        private String id;
        private Vertex next;
        private Arc first;
        private int info = 0;

        Vertex(String s, Vertex v, Arc e) {
            id = s;
            next = v;
            first = e;
        }

        Vertex(String s) {
            this(s, null, null);
        }


        public Arc getArc() {
            return first;
        }

        @Override
        public String toString() {
            return id;
        }

    }


    /**
     * Arc represents one arrow in the graph. Two-directional edges are
     * represented by two Arc objects (for both directions).
     */
    class Arc {

        private String id;
        private Vertex target;
        private Arc next;

        Arc(String s, Vertex v, Arc a) {
            id = s;
            target = v;
            next = a;
        }

        Arc(String s) {
            this(s, null, null);
        }

        @Override
        public String toString() {
            return id;
        }

    }

    /**
     * A graph is made out of vertices and arcs.
     * It contains all of the main functions for
     * finding the smallest cycle and creating a graph.
     */
    class Graph {

        private String id;
        private Vertex first;
        private int info = 0;
        private Vertex startingPoint;

        Graph(String s, Vertex v) {
            id = s;
            first = v;
            startingPoint = null;
        }

        Graph(String s) {
            this(s, null);
        }

        @Override
        public String toString() {
            String nl = System.getProperty("line.separator");
            StringBuffer sb = new StringBuffer(nl);
            sb.append(id);
            sb.append(nl);
            Vertex v = first;
            while (v != null) {
                sb.append(v.toString());
                sb.append(" -->");
                Arc a = v.first;
                while (a != null) {
                    sb.append(" ");
                    sb.append(a.toString());
                    sb.append(" (");
                    sb.append(v.toString());
                    sb.append("->");
                    sb.append(a.target.toString());
                    sb.append(")");
                    a = a.next;
                }
                sb.append(nl);
                v = v.next;
            }
            return sb.toString();
        }

        public Vertex createVertex(String vid) {
            Vertex res = new Vertex(vid);
            res.next = first;
            first = res;
            return res;
        }

        public Arc createArc(String aid, Vertex from, Vertex to) {
            Arc res = new Arc(aid);
            res.next = from.first;
            from.first = res;
            res.target = to;
            return res;
        }

        /**
         * Create a connected undirected random tree with n vertices.
         * Each new vertex is connected to some random existing vertex.
         *
         * @param n number of vertices added to this graph
         */
        public void createRandomTree(int n) {
            if (n <= 0)
                return;
            Vertex[] varray = new Vertex[n];
            for (int i = 0; i < n; i++) {
                varray[i] = createVertex("v" + String.valueOf(n - i));
                if (i > 0) {
                    int vnr = (int) (Math.random() * i);
                    createArc("a" + varray[vnr].toString() + "_"
                            + varray[i].toString(), varray[vnr], varray[i]);
                    createArc("a" + varray[i].toString() + "_"
                            + varray[vnr].toString(), varray[i], varray[vnr]);
                } else {
                }
            }
        }

        /**
         * Create an adjacency matrix of this graph.
         * Side effect: corrupts info fields in the graph
         *
         * @return adjacency matrix
         */
        public int[][] createAdjMatrix() {
            info = 0;
            Vertex v = first;
            while (v != null) {
                v.info = info++;
                v = v.next;
            }
            int[][] res = new int[info][info];
            v = first;
            while (v != null) {
                int i = v.info;
                Arc a = v.first;
                while (a != null) {
                    int j = a.target.info;
                    res[i][j]++;
                    a = a.next;
                }
                v = v.next;
            }
            return res;
        }

        /**
         * Create a connected simple (undirected, no loops, no multiple
         * arcs) random graph with n vertices and m edges.
         *
         * @param n number of vertices
         * @param m number of edges
         */
        public void createRandomSimpleGraph(int n, int m) {
            if (n <= 0)
                return;
            if (n > 2500)
                throw new IllegalArgumentException("Too many vertices: " + n);
            if (m < n - 1 || m > n * (n - 1) / 2)
                throw new IllegalArgumentException
                        ("Impossible number of edges: " + m);
            first = null;
            createRandomTree(n);       // n-1 edges created here
            Vertex[] vert = new Vertex[n];
            Vertex v = first;
            int c = 0;
            while (v != null) {
                vert[c++] = v;
                v = v.next;
            }
            int[][] connected = createAdjMatrix();
            int edgeCount = m - n + 1;  // remaining edges
            while (edgeCount > 0) {
                int i = (int) (Math.random() * n);  // random source
                int j = (int) (Math.random() * n);  // random target
                if (i == j)
                    continue;  // no loops
                if (connected[i][j] != 0 || connected[j][i] != 0)
                    continue;  // no multiple edges
                Vertex vi = vert[i];
                Vertex vj = vert[j];
                createArc("a" + vi.toString() + "_" + vj.toString(), vi, vj);
                connected[i][j] = 1;
                createArc("a" + vj.toString() + "_" + vi.toString(), vj, vi);
                connected[j][i] = 1;
                edgeCount--;  // a new edge happily created
            }
        }

        /**
         * Main function of the exercise
         * Check if the given vertex could have a cycle then starts searching using other functions.
         *
         * @return List of vertices that are used in the final cycle
         */
        public List<Arc> solveShortestCycle(String vertexID) {
            this.startingPoint = strVertexToVertex(vertexID);

            int numberOfArcs = 0;
            List<Vertex> surroundingVertices = new ArrayList<>();

            //Check how many arc the vertex has. If it finds less 2 arcs, then it can't be in a cycle.
            Arc tempArc = startingPoint.getArc();
            while (tempArc != null) {
                surroundingVertices.add(tempArc.target);
                numberOfArcs++;
                tempArc = tempArc.next;
            }
            if (numberOfArcs < 2) {
                System.out.println(String.format("Vertex: '%s' is not in a cycle", startingPoint.id));
                return Collections.emptyList();
            }

            //Using given combinations we can create a list of all the possible paths.
            List<int[]> allCombinations = generateCombinations(surroundingVertices);

            //A Set so there are no repeats
            Set<List<Vertex>> allPathsSet = new HashSet<>();
            int[] vertexCombo;

            //Generate all the combinations
            for (int i = 0; i < allCombinations.size(); i++) {
                vertexCombo = allCombinations.get(i);
                allPathsSet.addAll(findAllPaths(surroundingVertices.get(vertexCombo[0]),
                        surroundingVertices.get(vertexCombo[1])));

            }

            ArrayList<List<Vertex>> allPathsList = new ArrayList<>(allPathsSet);
            return findShortestCycle(allPathsList);

        }

        /**
         * Create combinations based on the vertexes that are adjacent to the main vertex
         *
         * @return list of combinations
         */
        public List<int[]> generateCombinations(List<Vertex> vertexList) {
            int n = vertexList.size();
            List<int[]> combinations = new ArrayList<>();
            int[] combination = new int[2];

            for (int i = 0; i < 2; i++) {
                combination[i] = i;
            }

            while (combination[2 - 1] < n) {
                combinations.add(combination.clone());

                int t = 2 - 1;
                while (t != 0 && combination[t] == n - 2 + t) {
                    t--;
                }

                combination[t]++;
                for (int i = t + 1; i < 2; i++) {
                    combination[i] = combination[i - 1] + 1;
                }
            }
            //returns the id combinations of the given vertices.
            return combinations;
        }
        //https://www.baeldung.com/java-combinations-algorithm

        /**
         * Find all the cycles that pass through the given path.
         *
         * @return List of vertices that are used in the final cycle
         */
        public List<Arc> findShortestCycle(ArrayList<List<Vertex>> paths) {

            //filter out all the paths that include the starting point of the circle.
            List<Vertex> tempVertexList = null;
            ArrayList<List<Vertex>> cleanPathList = new ArrayList<>();

            for (int i = 0; i < paths.size(); i++) {
                tempVertexList = paths.get(i);
                if (!tempVertexList.contains(startingPoint)) {
                    cleanPathList.add(tempVertexList);
                }
            }

            int distance = Integer.MAX_VALUE / 4;
            int buffDistance;

            //Take the filtered list and find the smallest distance
            for (int i = 0; i < cleanPathList.size(); i++) {
                buffDistance = cleanPathList.get(i).size() + 1;

                if (distance > buffDistance) {
                    tempVertexList = cleanPathList.get(i);
                }
                distance = Math.min(distance, cleanPathList.get(i).size() + 1);

            }
            //If distance didn't change from the MAX_VALUE then it means it's not in a cycle.
            if (distance == Integer.MAX_VALUE / 4) {
                System.out.println(String.format("Vertex: '%s' is not in a cycle", startingPoint.id));
                return Collections.emptyList();
            }

            return findArcVariant(tempVertexList, distance);
        }

        /**
         * Prints out the final answer and returns list of vertexes that were used ifn the list.
         *
         * @return List of vertices that are used in the final cycle
         */
        public List<Arc> findArcVariant(List<Vertex> path, int distance) {
            StringBuffer finalAnswer = new StringBuffer();

            finalAnswer.append(startingPoint.id);
            finalAnswer.append("->");
            for (int i = 0; i < path.size(); i++) {
                finalAnswer.append(path.get(i));
                finalAnswer.append("->");
            }
            finalAnswer.append(startingPoint.id);

            path.add(0, startingPoint);
            path.add(startingPoint);
            List<Arc> finalArcList = new ArrayList<>();

            for (int i = 1; i < path.size(); i++) {
                Arc tempArc = path.get(i - 1).first;

                while (tempArc.next != null) {
                    if (tempArc.target == path.get(i)) {
                        finalArcList.add(tempArc);
                    }
                    tempArc = tempArc.next;
                }
            }

            System.out.println((String.format("Length of the shortest cycle that passes through vertex '%s' is <%d>" +
                    " \n An example of such cycle: [%s]", startingPoint.id, distance, finalAnswer.toString())));

            return finalArcList;


        }

        /**
         * Find the shortest distance between two vertices using BFS (Breadth-First Search)
         *
         * @return list of available paths
         */
        public Set<List<Vertex>> findAllPaths(Vertex start, Vertex end) {

            Set<List<Vertex>> paths = new HashSet<>();
            LinkedList<Vertex> queue = new LinkedList<>();
            Set<Vertex> visited = new HashSet<>();
            Map<Vertex, Vertex> parentVertices = new HashMap<>();

            Vertex s = start;
            visited.add(s);
            queue.add(s);

            // starts discovering new elements 1 by 1
            while (!queue.isEmpty()) {
                s = queue.poll();
                Arc i = s.first;

                while (i != null) {
                    Vertex n = i.target;

                    if (n == end) {
                        parentVertices.put(n, s);
                        List<Vertex> shortestPath = new ArrayList<>();
                        Vertex node = end;

                        if (node == parentVertices.get(parentVertices.get(node))) {
                            break;
                        }
                        while (node != null) {
                            shortestPath.add(node);
                            node = parentVertices.get(node);
                        }
                        Collections.reverse(shortestPath);

                        //The length of the path traveled must be at least 1
                        if (shortestPath.size() >= 1) {
                            paths.add(shortestPath);
                        }

                    }

                    //Check if we have already visited the node
                    if (!visited.contains(n)) {
                        parentVertices.put(n, s);
                        visited.add(n);
                        queue.add(n);
                    }
                    i = i.next;
                }
            }
            return paths;
        }
        // https://www.geeksforgeeks.org/breadth-first-search-or-bfs-for-a-graph/


        /**
         * Turns the given vertexID to the corresponding vertex;
         *
         * @return corresponding Vertex
         */
        public Vertex strVertexToVertex(String vertexName) {
            Vertex curVertex = first;
            while (curVertex != null) {
                if (curVertex.id.equals(vertexName)) {
                    return curVertex;
                }
                curVertex = curVertex.next;
            }

            // When a vertex match isn't returned throw an exception
            throw new GraphException(String.format("%s vertex doesn't exist in this graph", vertexName));
        }

    }
}

class GraphException extends RuntimeException {

    GraphException(String message) {
        super(message);
    }
}









