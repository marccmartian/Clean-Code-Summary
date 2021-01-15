# CLEAN CODE: CAPITULO 2 - NOMBRES CON SENTIDO

## 👉 Usar nombres que reflejen intensiones:

El nombre debe reflejar el porque existe, que hace y como se usa.

ejemplo 1:

```
int d; // tiempo transcurrido en días (incorrecto)

int elapsedTimeInDays;      (correcto)
int daysSinceCreation;      (correcto)
int daysSinceModification;  (correcto)
int fileAgeInDays;          (correcto)
```

ejemplo 2:

Las variables en este codigo no son entendibles

```
public List<int[]> getThem() {
  List<int[]> list1 = new ArrayList<int[]>();
  for (int[] x : theList)
    if (x[0] == 4)
      list1.add(x);
  return list1;
}
```

Solo cambiando el nombre de las variables se vuelve mas entendible

```
public List<int[]> getFlaggedCells() {
  List<int[]> flaggedCells = new ArrayList<int[]>();
  for (int[] cell : gameBoard)
    if (cell[STATUS_VALUE] == FLAGGED)
      flaggedCells.add(cell);
  return flaggedCells;
}
```

## 👉 Evitar la desinformacion:

El nombre te puede dar a entender una información que no es correcta

Ejemplo:
`accountList` (incorrecto) connota que es una lista cuando en realidad no lo es
`accountGroup` (correcto)

## 👉 Realizar distinciones con sentido

Hay nombres que conotan lo mismo pero para distinguirlos, hay que hacerlo de tal manera que tengan sentido

ejemplo:
`a1` y `a2` son arrays, el codigo funciona pero sus nombres no tienen sentido, la funcion se lee mejor si se usara por ejemplo `source` y `destination` como argumentos

```
public static void copyChars(char a1[], char a2[]) {
  for (int i = 0; i < a1.length; i++) {
    a2[i] = a1[i];
  }
}
```

ejemplo:
`NameString` no tiene sentido indicar el tipo de dato en el nobre de la variable puede confundir al lector de preferencia solo nombrar como `Name`

ejemplo:
Algo a evitar es tratar de hacer distinciones a nombres agregando solo una letra o palabra.

```
getActiveAccount();
getActiveAccounts();
getActiveAccountInfo();
```

El problema que surge aqui, es como saber a que funcion llamar.

## 👉 Usar nombres que se puedan pronunciar

En algunos desarrollos se usan nombres que son la cambinacion de varias palabras, esto hay que evitar

ejemplo:
aqui `genymdhms`; `gen` hace referencia a fecha de generación, `y` al año, `m` al mes, etc.

```
class DtaRcrd102 {
  private Date genymdhms;
  private Date modymdhms;
  private final String pszqint = "102";
};
```

Correcto:

```
class Customer {
  private Date generationTimestamp;
  private Date modificationTimestamp;;
  private final String recordId = "102";
};
```

## 👉 Usar nombres que se puedan buscar

Esta regla nace por la razon de que es muy complicado buscar un nombre que sea una letra o constante numerica en el texto.
si una variable o constante aparece en varias partes del codigo debe tener un nombre que sea facil de buscar.

ejemplo:
En este codigo es imposibe saber que el numero 5 es una constante y es el numero de dias trabajados por semana
que el numero 4 es el peso por dia ideal, y que s es acumulador de la suma.

```
for (int j=0; j<34; j++) {
  s += (t[j]\*4)/5;
}
```

Aqui estasa variables son mas entendibles y se pueden buscar sin ningun problema

```
int realDaysPerIdealDay = 4;
const int WORK_DAYS_PER_WEEK = 5;
int sum = 0;
for (int j=0; j < NUMBER_OF_TASKS; j++) {
  int realTaskDays = taskEstimate[j] \* realDaysPerIdealDay;
  int realTaskWeeks = (realdays / WORK_DAYS_PER_WEEK);
  sum += realTaskWeeks;
}
```

## 👉 Evitar usar prefijos de miembros:

Para denotar que una variable proviene de una clase no es necesario usar prefijos, es una practica obsoleta.

```
public class Part {
  private String m_dsc; //prefijo para indicar que es una variable de clase
  void setName(String name) {
    m_dsc = name;
  }
}
```

correcto:

```
public class Part {
  String description;
  void setDescription(String description) {
    this.description = description;
  }
}
```

## 👉 Evitar asignaciones mentales:

Los lectores no tienen porque traducir mentalmente el nombre de las variables.
por ejemplo si yo usé tres nombres de variable como `a`, `b`, `c` para usar en bucles; es incorrecto porque por convencion en el mundo del desarrollo se usa `i`, `j` y `k`

## 👉 Nombre de clases:

Las clases y los objetos deben tener nombres o frases de nombres; por ejemplo:
`Customer`, `WikiPage`, `Account`, `AddresParser`.
El nombre de una clase no debe ser un verbo

## 👉 Nombre de Metodos:

Deben tener nombre de verbo por ejemplo:
`postPayment`, `deletePage`, o `save`
Los metodos de acceso, de modificación y los predicados deben usar su prefijo `get`, `set`, `is` pues es parte del estandar javabean.

por ejemplo:

```
string name = employee.getName();
customer.setName("mike");
if (paycheck.isPosted())...
```

## 👉 Usar una palabra por concepto:

Es conveniente usar una palabra por concepto abstracto por ejemplo si tengo, `fetch`, `retrieve` y `get` como metodos equivalentes de clases distintas, seria muy complicado recordar a que metodo corresponde cada clase.

## 👉 Añadir contexto con sentido:

La mayoria de los nombres no tienen sentido por si mismos, para ello se debe incluirlos en un contexto (clases, funciones)
El contexto es muy util pues te dice de donde proviene una variable.

ejemplo:
La funcion es extensa y las variables estan por todas partes

```
private void printGuessStatistics(char candidate, int count) {
  String number;
  String verb;
  String pluralModifier;
  if (count == 0) {
    number = "no";
    verb = "are";
    pluralModifier = "s";
  } else if (count == 1) {
    number = "1";
    verb = "is";
    pluralModifier = "";
  } else {
    number = Integer.toString(count);
    verb = "are";
    luralModifier = "s";
  }
  String guessMessage = String.format(
    "There %s %s %s%s", verb, number, candidate, pluralModifier
  );
  print(guessMessage);
}
```

Con una clase se puede dividir este metodo en varias partes mas pequeñas, las tres variables pasarian a ser campos de la clase.
De esa manera el contexto de esas variables se vuelve mas obvio

```
public class GuessStatisticsMessage {
  private String number;
  private String verb;
  private String pluralModifier;

  public String make(char candidate, int count) {
    createPluralDependentMessageParts(count);
    return String.format(
      "There %s %s %s%s",
      verb, number, candidate, pluralModifier
    );
  }

  private void createPluralDependentMessageParts(int count) {
    if (count == 0) {
      thereAreNoLetters();
    } else if (count == 1) {
      thereIsOneLetter();
    } else {
      thereAreManyLetters(count);
    }
  }

  private void thereAreManyLetters(int count) {
    number = Integer.toString(count);
    verb = "are";
    pluralModifier = "s";
  }

  private void thereIsOneLetter() {
    number = "1";
    verb = "is";
    pluralModifier = "";
  }

  private void thereAreNoLetters() {
    number = "no";
    verb = "are";
    pluralModifier = "s";
  }
}
```

## 👉 Conclusion:

Elegir bien nombres es algo complicado es una habiliadad mas decriptiva que tecnica, y la realidad es que, es un problema que tiene mucha gente de nuestro rubro, pero hay que ponerlo en practica pues tiene sus beneficios a corto y largo plazo.
