
import java.util.*;

public class Tnode {

    private static Tnode node;
    private String name;
    private Tnode firstChild;
    private Tnode nextSibling;
    private static String lastOperator = null;
    private static int maxTwoNumbers = 0;
    private static String staticRpn;

    Tnode(String nodeName, Tnode nodeChild, Tnode nodeSibling) {
        name = nodeName;
        firstChild = nodeChild;
        nextSibling = nodeSibling;

    }

    @Override
    public String toString() {
        StringBuffer leftString = recursiveStringBuilder(node);
        return leftString.toString();
    }

    //
    public StringBuffer recursiveStringBuilder(Tnode treeNode) {
        StringBuffer temp = new StringBuffer();

        if (node.name != null) {
            if (treeNode.name.matches("[-+*/]")) {
                temp.append(treeNode.name);
                temp.append("(");

                if (treeNode.firstChild.nextSibling != null) {
                    if (treeNode.firstChild.nextSibling.name.matches("[-+*/]")) {
                        temp.append(recursiveStringBuilder(treeNode.firstChild.nextSibling));
                        temp.append(",");
                    } else if (treeNode.firstChild.name.matches("[-+*/]")) {
                        temp.append(treeNode.firstChild.nextSibling.name);
                        temp.append(",");
                    }

                }
                temp.append(recursiveStringBuilder(treeNode.firstChild));


                temp.append(")");
            } else if (treeNode.name.matches("-?\\d+")) {
                if (treeNode.nextSibling != null && treeNode.nextSibling.name.matches("-?\\d+")) {
                    temp.append(treeNode.nextSibling.name);
                    temp.append(",");
                }

                temp.append(treeNode.name);
            }
        }
        return temp;
    }

    public static Tnode buildFromRPN(String pol) {

        String[] treeAsArray = pol.split("\\s+");
        LinkedList<String> rpnStack = new LinkedList<>();

        for (int i = 0; i < treeAsArray.length; i++) {
            if (treeAsArray[i].matches("-?\\d+|[-+*/]|SWAP|ROT")) {
                rpnStack.push(treeAsArray[i]);
            } else {
                throw new IllegalSymbolException(String.format(" '%s' | %s is not an acceptable element", pol, treeAsArray[i]));
            }
        }
        staticRpn = pol;
        errorChecking(treeAsArray, pol);
        node = newbufferBuild(rpnStack);

        return node;
    }

    public static void errorChecking(String[] list, String rpn) {
        int swapRot = 0;
        int operators = 0;
        if (list.length < 1) {
            throw new TreeException(String.format("'%s' | Tree needs at least 1 element", rpn));
        }
        for (int i = list.length - 1; i >= 0; i--) {

            String tempElement = list[i];

            if (list.length > 1 && i == list.length - 1 && tempElement.matches("[^-+*/]")) {
                throw new TreeException(String.format("['%s'] | Root node must be an operator if the size of the" +
                        " tree is more than 1. '%s' is not an operator  ", rpn, tempElement));



            } else if (tempElement.matches("[-+*/]|SWAP|ROT")) {

                if (tempElement.matches("[-+*/]")) {
                    operators++;
                } else {
                    swapRot++;
                }

                if (i - 1 < 0) {
                    throw new OperatorException(String.format("['%s'] | operator %s needs 2 elements found 0",
                            rpn, tempElement));
                }

                if (i - 2 < 0 ) {
                    throw new OperatorException(String.format("['%s'] | operator %s needs 2 elements found 1",
                            rpn, tempElement));
                }

            }
        }

        if (list.length - swapRot - operators > operators + 1) {
            throw new OperatorException(String.format("['%s'] | Tree has too many numbers", rpn));
        }

        if (list.length - swapRot - operators != operators + 1) {
            throw new TreeException(String.format("['%s'] | operator %s needs 2 elements found 1",
                    rpn, list[list.length - 1]));
        }

    }

    public static Tnode bufferBuild(LinkedList rpnStack) {

//        System.out.println(rpnStack);

        if (rpnStack.size() < 1) {
            return null;
        }

        String node = String.valueOf(rpnStack.pop());
        String nextNode = String.valueOf(rpnStack.peekFirst());
        String nextAfterNode = (rpnStack.size() > 1) ? String.valueOf(rpnStack.get(1)) : "null";




        if (node.matches("-?\\d+")) {
            if (nextNode.matches("-?\\d+")) {
                return new Tnode(node, null, bufferBuild(rpnStack));
            } else {
                return new Tnode(node, null, null);
            }
        } else {
            return new Tnode(node, bufferBuild(rpnStack), bufferBuild(rpnStack));
        }
    }

    public static Tnode newbufferBuild(LinkedList rpnStack) {


        int rotSwap = 0;

        if (rpnStack.size() < 1) {
            return null;
        }

        String node = String.valueOf(rpnStack.pop());
        if (node.matches("SWAP|ROT")) {
            node = rotSwap(rpnStack, node);
        }

        if (node.matches("-?\\d+") && maxTwoNumbers != 2) {
            maxTwoNumbers++;
            if (lastOperator.matches("-?\\d+")) {
                lastOperator = node;
                return new Tnode(node, null, null);
            } else {
                lastOperator = node;
                return new Tnode(node, null, newbufferBuild(rpnStack));
            }
        } else {
            maxTwoNumbers = 0;
//            System.out.println(node);
            lastOperator = node;
            return new Tnode(node, newbufferBuild(rpnStack), newbufferBuild(rpnStack));
        }

    }

    public static String rotSwap(LinkedList rpnStack, String node) {

        int rotSwap = 0;
        if (node.matches("ROT")) {
            rotSwap = rpnStack.size() - 3;
            if (rpnStack.size() - 3 < 0) {
                throw new OperatorException(String.format("['%s'] | operator %s needs 3 elements found %d",
                        staticRpn, "ROT", 3 + rotSwap));
            }

            String firstTempNode = String.valueOf(rpnStack.pop());
            String secondTempNode = String.valueOf(rpnStack.pop());
            String thirdTempNode = String.valueOf(rpnStack.pop());
            System.out.println("Rot1:" + firstTempNode);
            System.out.println("Rot2:" + secondTempNode);
            System.out.println("Rot3:" + thirdTempNode);
            rpnStack.push(secondTempNode);
            rpnStack.push(firstTempNode);
            rpnStack.push(thirdTempNode);

            node = String.valueOf(rpnStack.pop());

        } else if (node.matches("SWAP")) {
            rotSwap = rpnStack.size() - 2;
            if (rpnStack.size() - 2 < 0) {
                throw new OperatorException(String.format("['%s'] | operator %s needs 2 elements found %d",
                        staticRpn, "SWAP", 2 + rotSwap));
            }

            String firstTempNode = String.valueOf(rpnStack.pollFirst());
            String secondTempNode = String.valueOf(rpnStack.pollFirst());
            rpnStack.push(firstTempNode);
            rpnStack.push(secondTempNode);
            node = String.valueOf(rpnStack.pollFirst());
        }
        if (node.matches("ROT|SWAP")) {
            return rotSwap(rpnStack, node);
        }
        return node;
    }


    public static void main(String[] param) {

        String rpn = "2 5 SWAP -";
        System.out.println("RPN: " + rpn);
        Tnode res = buildFromRPN(rpn);
        System.out.println("Tree: " + res);
        rpn = "2 5 9 ROT - +";
        System.out.println("RPN: " + rpn);
        res = buildFromRPN(rpn);
        System.out.println("Tree: " + res);
        rpn = "2 5 9 ROT + SWAP -";
        System.out.println("RPN: " + rpn);
        res = buildFromRPN(rpn);
        System.out.println("Tree: " + res);
        System.out.println("2 1 - 4 * 6 3 / + ");
        rpn = "2 1 - 4 * 6 3 / + ";
        System.out.println("expected: +(*(-(2,1),4),/(6,3))");
        res = buildFromRPN(rpn);
//        System.out.println(res.name);
//        System.out.println(res.firstChild.name);
////        System.out.println(res.nextSibling.name);
//        System.out.println(res.firstChild.nextSibling.name);
//        System.out.println(res.firstChild.nextSibling.firstChild.name);
//        System.out.println(res.firstChild.nextSibling.firstChild.nextSibling.name);
//        System.out.println(res.firstChild.nextSibling.firstChild.nextSibling.firstChild.name);
//        System.out.println(res.firstChild.nextSibling.firstChild.nextSibling.name);
//        System.out.println(res.firstChild.firstChild.name);
//        System.out.println(res.firstChild.firstChild.nextSibling.name);
//        System.out.println(res.firstChild.firstChild.nextSibling.name);
//        System.out.println(res.firstChild);
//        System.out.println(res.firstChild.nextSibling.name);
//        System.out.println(res.firstChild.name);
        System.out.println("Tree: " + res);

    }
}

class IllegalSymbolException extends RuntimeException {

    IllegalSymbolException(String message) {
        super(message);
    }

}

class TreeException extends RuntimeException {

    TreeException(String message) {
        super(message);
    }

}

class OperatorException extends RuntimeException {

    OperatorException(String message) {
        super(message);
    }
}