# CLEAN CODE: CAPÍTULO 10 - Clases

## 👉 Organización de clases

La convención de Java nos indica que una clase debe comenzar con el listado de variables, y luego las funciones públicas y si llamamos a otra función, esa función debe ir debajo de la que llama, de esta manera cumplimos también la regla descendente.

### Encapsulación

Es importante que nuestras variables y funciones sean privadas, pero no imprescindible, podemos hacerlas protected para que sean accesibles desde una prueba, por ejemplo.

## 👉 Las clases deben ser de tamaño reducido

En las funciones nos fijamos en el número de líneas, en las clases la medida serán las responsabilidades.
En el libro nos muestra un ejemplo, una clase con 70 métodos, aunque se redujera a 5 métodos seguirán siendo demasiados, esto es porque la clase tiene demasiadas responsabilidades.

Ejemplo, esto es una clase con muchas responsabilidades:

```java
public class SuperDashboard extends JFrame implements MetaDataUser {
  public String getCustomizerLanguagePath()
  public void setSystemConfigPath(String systemConfigPath)
  public String getSystemConfigDocument()
  public void setSystemConfigDocument(String systemConfigDocument)
  public boolean getGuruState()
  public boolean getNoviceState()
  public boolean getOpenSourceState()
  ... // 70 methods
}
```

El nombre de la clase debe describir las responsabilidades que desempeña, de hecho el nombre es a primera forma de determinar el tamaño de una clase.

### **Principio de Responsabilidad Única**

(SRP - Single Responsability Principle) indica que una clase o módulo debe tener un único motivo para cambiar. Si una clase tiene varios motivos para cambiar, es que tiene demasiadas dependencias. Una forma de solucionarlo es dividirla en clases mas pequeñas. Tener clases pequeñas puede dificultar la comprensión del código, pero ayudar a tener una mejor estructura del código y a no conocer aspectos innecesarios de mi aplicación.

Ejemplo: del conjunto de metodos del código anterior se extrajo los métodos que tienen que ver con el versionamiento de la aplicación y se creo la clase `Version`.

```java
public class Version {
  public int getMajorVersionNumber()
  public int getMinorVersionNumber()
  public int getBuildNumber()
}
```

Entonces la regla es que los sistemas deben estar formados por muchas clases reducidas, no por algunas de gran tamaño; pues cada clase reducida encapsula una única responsabilidad, tiene un solo motivo para cambiar y colabora con algunas otras para obtener los comportamientos deseados del sistema.

### **Cohesión**

Las clases deben tener un número reducido de variables de instancia; los métodos de la clase deben manipular dichas variables, en general cuantas mas variables manipule un método, más cohesión tendrá una clase. Una clase en la que cada variable se usa en cada método tiene una cohesión máxima.

En general logar ese tipo de clases, casi no es posible, pero si lo logramos, significa que los metodos y variables de clase dependen unos de otros y actúan como un todo lógico.

Ejemplo: la clase `Stack` es un clase muy consistente, solo `size()` no usa ambas variables.

```java
public class Stack {
  private int topOfStack = 0;
  List<Integer> elements = new LinkedList<Integer>();

  public int size() {
    return topOfStack;
  }

  public void push(int element) {
    topOfStack++;
    elements.add(element);
  }

  public int pop() throws PoppedWhenEmpty {
    if (topOfStack == 0)
      throw new PoppedWhenEmpty();
    int element = elements.get(--topOfStack);
    elements.remove(topOfStack);
    return element;
  }
}
```

Cuando dividimos funciones en una clase y vemos que necesitamos extraer variables para poder usarlas, quizás haya que extraer eso en una clase más pequeña, cuando nuestras clases pierden cohesión hay que crear clases más pequeñas.

### **Mantenimiento de los resultados de cohesión en muchas clases pequeñas**

Imagine una clase que tiene una función grande con varias variables declaradas, para separar la función, lo primero que se hace es extraer código en una nueva función y pasar las variables como argumentos; entonces esas variables se ascienden a variables de instancia de la clase.

La clase pierde cohesión ya que estaría acumulando más y más variables de instancia que solo existen para que otras funciones las compartan. Ese es momento para dividir en clases, es decir cuando pierden cohesión.

Dividir una gran función en otras más reducidas también nos permite dividir varias clases más reducidas.

EJEMPLO: del programa "PrintPrimes" de Knuth, escrito como una sola función es un desastre; sangrado excesivo, demasiadas variables extrañas. La función deberia dividirse en otras mas pequeñas.

```java
package literatePrimes;

public class PrintPrimes {
  public static void main(String[] args) {
    final int M = 1000;
    final int RR = 50;
    final int CC = 4;
    final int WW = 10;
    final int ORDMAX = 30;
    int P[] = new int[M + 1];
    int PAGENUMBER;
    int PAGEOFFSET;
    int ROWOFFSET;
    int C;
    int J;
    int K;
    boolean JPRIME;
    int ORD;
    int SQUARE;
    int N;
    int MULT[] = new int[ORDMAX + 1];

    J = 1;
    K = 1;
    P[1] = 2;
    ORD = 2;
    SQUARE = 9;

    while (K < M) {
      do {
        J = J + 2;
        if (J == SQUARE) {
          ORD = ORD + 1;
          SQUARE = P[ORD] * P[ORD];
          MULT[ORD - 1] = J;
        }
        N = 2;
        JPRIME = true;
        while (N < ORD && JPRIME) {
          while (MULT[N] < J)
            MULT[N] = MULT[N] + P[N] + P[N];
            if (MULT[N] == J)
            JPRIME = false;
            N = N + 1;
        }
      } while (!JPRIME);
      K = K + 1;
      P[K] = J;
    }
    {
      PAGENUMBER = 1;
      PAGEOFFSET = 1;
      while (PAGEOFFSET <= M) {
        System.out.println("The First " + M +
                          " Prime Numbers --- Page " + PAGENUMBER);
        System.out.println("");
        for (ROWOFFSET = PAGEOFFSET; ROWOFFSET < PAGEOFFSET + RR; ROWOFFSET++){
          for (C = 0; C < CC;C++)
            if (ROWOFFSET + C * RR <= M)
              System.out.format("%10d", P[ROWOFFSET + C * RR]);
          System.out.println("");
        }
        System.out.println("\f");
        PAGENUMBER = PAGENUMBER + 1;
        PAGEOFFSET = PAGEOFFSET + RR * CC;
      }
    }
  }
}
```

Dividimos el código anterior en clases y funciones de menor tamaño:

`PrimePrinter Class`:

```java
package literatePrimes;

public class PrimePrinter {
  public static void main(String[] args) {
    final int NUMBER_OF_PRIMES = 1000;
    int[] primes = PrimeGenerator.generate(NUMBER_OF_PRIMES);

    final int ROWS_PER_PAGE = 50;
    final int COLUMNS_PER_PAGE = 4;
    RowColumnPagePrinter tablePrinter =
      new RowColumnPagePrinter(ROWS_PER_PAGE, COLUMNS_PER_PAGE,
                              "The First " + NUMBER_OF_PRIMES +
                              " Prime Numbers");
    tablePrinter.print(primes);
  }
}
```

`RowColumnPagePrinter Class`:

```java
package literatePrimes;

import java.io.PrintStream;

public class RowColumnPagePrinter {
  private int rowsPerPage;
  private int columnsPerPage;
  private int numbersPerPage;
  private String pageHeader;
  private PrintStream printStream;

  public RowColumnPagePrinter(int rowsPerPage,
                              int columnsPerPage,
                              String pageHeader) {
    this.rowsPerPage = rowsPerPage;
    this.columnsPerPage = columnsPerPage;
    this.pageHeader = pageHeader;
    numbersPerPage = rowsPerPage * columnsPerPage;
    printStream = System.out;
  }

  public void print(int data[]) {
    int pageNumber = 1;
    for (int firstIndexOnPage = 0;
        firstIndexOnPage < data.length;
        firstIndexOnPage += numbersPerPage) {
      int lastIndexOnPage =
        Math.min(firstIndexOnPage + numbersPerPage - 1,
                data.length - 1);
      printPageHeader(pageHeader, pageNumber);
      printPage(firstIndexOnPage, lastIndexOnPage, data);
      printStream.println("\f");
      pageNumber++;
    }
  }

  private void printPage(int firstIndexOnPage,
                         int lastIndexOnPage,
                         int[] data) {
    int firstIndexOfLastRowOnPage =
      firstIndexOnPage + rowsPerPage - 1;
    for (int firstIndexInRow = firstIndexOnPage;
        firstIndexInRow <= firstIndexOfLastRowOnPage;
        firstIndexInRow++) {
      printRow(firstIndexInRow, lastIndexOnPage, data);
      printStream.println("");
    }
  }

  private void printRow(int firstIndexInRow,
                        int lastIndexOnPage,
                        int[] data) {
    for (int column = 0; column < columnsPerPage; column++) {
      int index = firstIndexInRow + column * rowsPerPage;
      if (index <= lastIndexOnPage)
        printStream.format("%10d", data[index]);
    }
  }

  private void printPageHeader(String pageHeader,
                              int pageNumber) {
    printStream.println(pageHeader + " --- Page " + pageNumber);
    printStream.println("");
  }

  public void setOutput(PrintStream printStream) {
    this.printStream = printStream;
  }
}
```

`PrimeGenerator class`:

```java
package literatePrimes;
import java.util.ArrayList;

public class PrimeGenerator {
  private static int[] primes;
  private static ArrayList<Integer> multiplesOfPrimeFactors;

  protected static int[] generate(int n) {
    primes = new int[n];
    multiplesOfPrimeFactors = new ArrayList<Integer>();
    set2AsFirstPrime();
    checkOddNumbersForSubsequentPrimes();
    return primes;
  }

  private static void set2AsFirstPrime() {
    primes[0] = 2;
    multiplesOfPrimeFactors.add(2);
  }

  private static void checkOddNumbersForSubsequentPrimes() {
    int primeIndex = 1;
    for (int candidate = 3;
         primeIndex < primes.length;
         candidate += 2) {
      if (isPrime(candidate))
        primes[primeIndex++] = candidate;
    }
  }

  private static boolean isPrime(int candidate) {
    if (isLeastRelevantMultipleOfNextLargerPrimeFactor(candidate)) {
      multiplesOfPrimeFactors.add(candidate);
      return false;
    }
    return isNotMultipleOfAnyPreviousPrimeFactor(candidate);
  }

  private static boolean
  isLeastRelevantMultipleOfNextLargerPrimeFactor(int candidate) {
    int nextLargerPrimeFactor = primes[multiplesOfPrimeFactors.size()];
    int leastRelevantMultiple = nextLargerPrimeFactor * nextLargerPrimeFactor;
    return candidate == leastRelevantMultiple;
  }

  private static boolean
  isNotMultipleOfAnyPreviousPrimeFactor(int candidate) {
    for (int n = 1; n < multiplesOfPrimeFactors.size(); n++) {
      if (isMultipleOfNthPrimeFactor(candidate, n))
        return false;
    }
    return true;
  }

  private static boolean
  isMultipleOfNthPrimeFactor(int candidate, int n) {
    return
      candidate == smallestOddNthMultipleNotLessThanCandidate(candidate, n);
  }

  private static int
  smallestOddNthMultipleNotLessThanCandidate(int candidate, int n) {
    int multiple = multiplesOfPrimeFactors.get(n);
    while (multiple < candidate)
    multiple += 2 * primes[n];
    multiplesOfPrimeFactors.set(n, multiple);
    return multiple;
  }
}
```

El tamaño del programa a aumentado pero es mas legible. El programa tiene ahora tres responsabilidades principales:

- `PrimePrinter` es la parte prinipal que controla el entorno de ejecución.
- `RowColumnPagePrinter` aplica formato a una lista de números con una determinada cantidad de filas y columnas.
- `PrimeGenerator` genera una lista de números primos

## 👉 Organizar por cambios

En los sistemas, el cambio es contínuo, cada cambio es un riesgo de que el sistema no funcione de forma esperada. En un sistema 'limpio' se organiza las clases para reducir los riesgos de los cambios.

Ejemplo: La clase `SQL` se usa para generar cadenas SQL, no admite instrucciones como `update`. Cuando la clase requiera admitir esta instrucción, se tiene que abrir para realizar modificaciones, esta modificación puede afectar a otro código de la clase.

```java
public class Sql {
   public Sql(String table, Column[] columns)
   public String create()
   public String insert(Object[] fields)
   public String selectAll()
   public String findByKey(String keyColumn, String keyValue)
   public String select(Column column, String pattern)
   public String select(Criteria criteria)
   public String preparedInsert()
   private String columnList(Column[] columns)
   private String valuesList(Object[] fields, final Column[] columns)
   private String selectWithCriteria(String criteria)
   private String placeholderList(Column[] columns)
}
```

Ahora on este codigo el riesgo de que una función afecte a otra desaparece, resulta mas sencillo probar la lógica de la solución, y cuando se añada una instracción como por ejemplo `update` este cambio no afecta a tro código del sistema.

```java
abstract public class Sql {
  public Sql(String table, Column[] columns)
  abstract public String generate();
}

public class CreateSql extends Sql {
  public CreateSql(String table, Column[] columns)
  @Override public String generate()
}

public class SelectSql extends Sql {
  public SelectSql(String table, Column[] columns)
  @Override public String generate()
}

public class InsertSql extends Sql {
  public InsertSql(String table, Column[] columns, Object[] fields)
  @Override public String generate()
  private String valuesList(Object[] fields, final Column[] columns)
}

public class SelectWithCriteriaSql extends Sql {
  public SelectWithCriteriaSql(
    String table, Column[] columns, Criteria criteria)
  @Override public String generate()
}

public class SelectWithMatchSql extends Sql {
  public SelectWithMatchSql(
    String table, Column[] columns, Column column, String pattern)
  @Override public String generate()
}

public class FindByKeySql extends Sql {
  public FindByKeySql(
    String table, Column[] columns, String keyColumn, String keyValue)
  @Override public String generate()
}

public class PreparedInsertSql extends Sql {
  public PreparedInsertSql(String table, Column[] columns)
  @Override public String generate() {
  private String placeholderList(Column[] columns)
}

public class Where {
  public Where(String criteria)
  public String generate()
}

public class ColumnList {
  public ColumnList(Column[] columns)
  public String generate()
}
```

La lógica, ahora reestructurada cumple con SRP, y también con otro principio del diseño de clases orientada a objetos; que es:

- **Principio abierto / cerrado**: Las clases deben abrirse para su ampliación para cerrarse para su modificación.
  La nueva clase `SQL` se abre a nuevas funcionalidades mediante la creación de subclases, pero podemos realizar estos cambios y mantener cerradas las demas clases. Entocnes bastaría sólo con agregar la clase `UpdateSql` para añadir la instruccion "update".

  Incorporamos nuevas funcionalidades ampliándolo, no modificando el código existente.

### Aislarnos de los cambios

En POO hay clases concretas que contienen los detalles de implementación (el código) y clases abstractas que representan conceptos. Una clase que depende de detalles concretos, está en peligro si dichos detalles cambian; para aislar el impacto de esos detalles, hay que recurrir a interfaces y clases abstractas.

Por ejemplo: tenemos una clase `Portafolio` que depende de una API externa `TokioStockExchange` para obtener su valor; resultaría muy complicado hacer una prueba ya que se obtendría una respuesta diferente cada cierto tiempo; entonces en lugar de que dependa direactamente de la API, creamos una interfaz `StockExchange` que tiene un único método:

```java
public interface StockExchange {
  Money currentPrice(String symbol);
}
```

`TokioStockExchange` implementa la interfaz, también se asegura de que `Portfolio` adopte como argumento una referencia a `StockExchange`

```java
public Portfolio {
   private StockExchange exchange;
   public Portfolio(StockExchange exchange) {
     this.exchange = exchange;
   }
   // ...
}
```

Ahora la prueba puede crear una implementación de la interfaz.
La clase `Portfolio` en lugar de depender de los detalles de la implementación de la API `TokyoStockExchange`, depende de la interfaz `StockExchange`, que representa el concepto abstracto de solicitar el precio actual de una acción.

Esta abstracción aisla todos los datos concretos de la obtención de dicho precio, incluyendo de donde se obtiene.

```java
public class PortfolioTest {
  private FixedStockExchangeStub exchange;
  private Portfolio portfolio;

  @Before
  protected void setUp() throws Exception {
    exchange = new FixedStockExchangeStub();
    exchange.fix("MSFT", 100);
    portfolio = new Portfolio(exchange);
  }

  @Test
  public void GivenFiveMSFTTotalShouldBe500() throws Exception {
    portfolio.add(5, "MSFT");
    Assert.assertEquals(500, portfolio.value());
  }
}
```

Entonces, otra manera de hacer frente al cambio es la de creación de interfaces, así también se cumple el principio de inversión de dependencias (DIP - Dependency Inversion Principle), que dice que las clases deben depender de abstracciones y no de detalles concretos.
