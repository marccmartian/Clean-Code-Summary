# CLEAN CODE: CAPÍTULO 3 - FUNCIONES

## 👉 Intro:

En el código la función es extensa, contiene código duplicado, muchas cadenas, tipos de datos extraños, invocación de funciones mezcladas, instrucciones if doblemente anidadas, todo esto es muy complicado comprenderlo en unos minutos.

Ejemplo 3.1

```
public static String testableHtml(
  PageData pageData,
  boolean includeSuiteSetup
) throws Exception {
    WikiPage wikiPage = pageData.getWikiPage();
    StringBuffer buffer = new StringBuffer();
      if (pageData.hasAttribute("Test")) {
        if (includeSuiteSetup) {
          WikiPage suiteSetup =
          PageCrawlerImpl.getInheritedPage(
            SuiteResponder.SUITE_SETUP_NAME, wikiPage
          );
        if (suiteSetup != null) {
          WikiPagePath pagePath =
            suiteSetup.getPageCrawler().getFullPath(suiteSetup);
            String pagePathName = PathParser.render(pagePath);
            buffer.append("!include -setup .")
              .append(pagePathName)
              .append("\n");
        }
      }
      WikiPage setup =
        PageCrawlerImpl.getInheritedPage("SetUp", wikiPage);
        if (setup != null) {
          WikiPagePath setupPath =
          wikiPage.getPageCrawler().getFullPath(setup);
          String setupPathName = PathParser.render(setupPath);
          buffer.append("!include -setup .")
            .append(setupPathName)
            .append("\n");
        }
    }
    buffer.append(pageData.getContent());
      if (pageData.hasAttribute("Test")) {
        WikiPage teardown =
        PageCrawlerImpl.getInheritedPage("TearDown", wikiPage);
        if (teardown != null) {
          WikiPagePath tearDownPath =
          wikiPage.getPageCrawler().getFullPath(teardown);
          String tearDownPathName = PathParser.render(tearDownPath);
          buffer.append("\n")
            .append("!include -teardown .")
            .append(tearDownPathName)
            .append("\n");
      }
      if (includeSuiteSetup) {
        WikiPage suiteTeardown =
          PageCrawlerImpl.getInheritedPage(
            SuiteResponder.SUITE_TEARDOWN_NAME,
            wikiPage
          );
        if (suiteTeardown != null) {
          WikiPagePath pagePath =
            suiteTeardown.getPageCrawler().getFullPath (suiteTeardown);
            String pagePathName = PathParser.render(pagePath);
              buffer.append("!include -teardown .")
                .append(pagePathName)
                .append("\n");
          }
        }
      }
    pageData.setContent(buffer.toString());
    return pageData.getHtml();
  }
```

Con extracciones de código, algun cambio de nombre y cierta reestructuración, se puede capturar la función en pocas lineas de código:

Ejemplo 3.2

```
public static String renderPageWithSetupsAndTeardowns(
  PageData pageData, boolean isSuite
) throws Exception {
    boolean isTestPage = pageData.hasAttribute("Test");
      if (isTestPage) {
        WikiPage testPage = pageData.getWikiPage();
        StringBuffer newPageContent = new StringBuffer();
        includeSetupPages(testPage, newPageContent, isSuite);
        newPageContent.append(pageData.getContent());
        includeTeardownPages(testPage, newPageContent, isSuite);
        pageData.setContent(newPageContent.toString());
    }
    return pageData.getHtml();
  }
```

Esta segunda función es mucho mas entendible que la primera. Para lograr ello el autor hace una serie de sugerencias:

## 👉 Tamaño Reducido:

La primera regla de las funciones es que deben de ser de tamaño reducido y la siguiente regla es que deben ser aún más reducidas, quiere decir que se debe refactorizar un metodo todo lo que se pueda, siempre y cuando se pueda leer y entender.

Por lo tanto, el codigo del ejemplo 3.2 se puede reducir aún más:

Ejemplo 3.3

```
public static String renderPageWithSetupsAndTeardowns(
  PageData pageData, boolean isSuite) throws Exception {
    if (isTestPage(pageData))
      includeSetupAndTeardownPages(pageData, isSuite);
    return pageData.getHtml();
  }
```

## 👉 Bloques y sangrado:

Sugiere que los bloques `if`, `else`, `while` y similares deben tener en lo posible una linea de longitud, con esto se consigue no solo reducir el tamaño de la función sino que le añade valor documental. Esto implica a que las funciones no tengan un nivel excesivo de anidamiento y recomienda que a lo mucho debe tener uno o dos niveles de anidamiento.

## 👉 Hacer una cosa:

El codigo del ejemplo 3.1, hace muchas tareas: crea bubffers, obtiene páginas, busca páginas heredadas, añade cadenas y genera HTML. En cambio el codigo del ejemplo 3.3 solo hace una cosa: incluir configuraciones y detalles en páginas de prueba.

Aparentemente la función hace tres cosas: determinar si es una pagina de prueba, de ser afirmativo incluye configuraciones y representa la pagina en HTML.
Estos pasos de la función se encuentran en un nivel de abstraccion por debajo del NOMBRE DE LA FUNCION, cuando esto es así, se dice que la funcion hace una sola cosa.

En resumen el consejo es: LAS FUNCIONES SOLO DEBEN HACER UNA COSA, DEBEN HACERLO BIEN Y DEBE SER LO UNICO QUE HAGAN.

Una forma de saber si una función hace mas de una cosa es, extraer una función de la misma con una nombre apropiado que no sea un resumen de su implementación.

## 👉 Un nivel de abstracción por función

Asegurar que las intruccinones de una función tengan y se encuentren en el mismo nivel de abstracción.
En el ejemplo 3.1 `getHtml();` esta en un alto nivel, `StringPagePathName = PathParser.render(pagePath);` esta en un nivel intermedio y `.append("\n")` en un nivel bajo.

## 👉 Leer Código de arriba hacia abajo

El objetivo es que el código se lea como un texto cualquiera, de arriba hacia abajo. es decir que tras cada función aparezcan las del siguiente nivel de abstracción para poder leer el programa de manera descendente.

## 👉 Instrucciones Switch

Por su naturaleza una instrucción Switch hace varias cosas y es complicado tener de tamaño reducido esta instrucción, lo que se recomienda es incluirla en una clase y no repetirlas.
Pero es tolerable si aparece solo una vez en el código.

ejemplo 3.4:
Esta función es de gran tamaño y cuando se añaden otros tipos de 'empleados' aumenta más y hace más de una cosa

```
public Money calculatePay(Employee e)
throws InvalidEmployeeType {
  switch (e.type) {
    case COMMISSIONED:
      return calculateCommissionedPay(e);
    case HOURLY:
      return calculateHourlyPay(e);
    case SALARIED:
      return calculateSalariedPay(e);
    default:
      throw new InvalidEmployeeType(e.type);     }
  }
```

La solución es ocultar la instrucción switch en una 'factoria' abstracta e impedir que nadie la vea.
La factoria usa la instruccion switch para crear las instancias de los derivados de Empleado y las disintas funciones (`calculatePay`, `isPayDay`, `deliverPay`) y se pasan de forma polimórfica a través de la interfaz `Employee`

Ejemplo 3.5

```
public abstract class Employee {
  public abstract boolean isPayday();
  public abstract Money calculatePay();
  public abstract void deliverPay(Money pay);
}
-----------------
public interface EmployeeFactory {
  public Employee makeEmployee(EmployeeRecord r) throws InvalidEmployeeType;
}
-----------------
public class EmployeeFactoryImpl implements EmployeeFactory {
  public Employee makeEmployee(EmployeeRecord r) throws InvalidEmployeeType {
    switch (r.type) {
      case COMMISSIONED:
        return new CommissionedEmployee(r) ;
      case HOURLY:
        return new HourlyEmployee(r);
      case SALARIED:
        return new SalariedEmploye(r);
      default:
        throw new InvalidEmployeeType(r.type);
    }
  }
}
```

## 👉 Usar nombres descriptivos

Un nombre descriptivo extenso es mejor que uno breve pero enigmático, tomarse el tiempo necesario para elegir un buen nombre, probar varios nombres y leer el código con todos ellos es una buena practica, por ejemplo `includeSetupTeardownPages`.

## 👉 Argumentos de funciones

Recomienda que el número ideal de argumentos de una función debe ser cero, depués uno (mon´ádico), a lo mucho usar hasta dos argumentos y tratar de evitar la presencia de tres.
La razón es porque muchos argumentos dificultan las pruebas, un argumento suele obligar a realizar una comprobación doble.

## 👉 Formas monádicas (un argumento) habituales

Esta regla se utiliza para realizar una pregunta sobre el argumento, por ejemplo; `boolean fileExists("MyFile")`, o para que procese el argumento, es decir lo transforme y lo devuelva, por ejemplo; `InputStream fileOpen(MyFile)`.
En estos casos debe elegir nombres que realicen la distinción con claridad.

## 👉 Funciones diádicas (dos argumentos)

Una función con dos argumentos es dificil de leer, por ejemplo; `writeField(name)` es más entendible que `writeField(outStream, name)`. Pero dependiendo del caso en ocasiones se necesitan los dos argumentos, por ejemplo; `p = new Point(0, 0)`, aqui es totalmente aceptable pues los puntos cartesianos suelen adoptar dos valores cartesianos.
Cuando surgan este tipo de funciones diádicas, recomienda usar todos los mecanismos disponibles en tranformar la función en unitaria.

## 👉 Objeto argumento

Cuando una función parace necesitar dos o mas argumentos, intentar incluir alguno de ellos en una clase propia; por ejemplo:

`Circle makeCircle(double x, double y, double radius);`

`Circle makeCircle(Point center, double radius);`

En la segunda sentencia se redujo el número de argumentos mediante la creación del objeto 'Point'.

## 👉 Lista de Argumentos

Hay ocasiones en que se necesitan pasar una lista variable de argumentos, por ejemplo;

`String.format("%s worked %.2f hours.", name, hours);`

Aquí los argumentos son varios y se procesan de la misma forma, entonces seria equivalente a un único argumento de tipo `List`.

`public String format(String format, Object... args);`

## 👉 Verbos y palabras clave

El nombre correcto de una función mejora la explicación de su propósito, así como el orden y el propósito de sus argumentos, por ejemplo: `write(name)` se puede apreciar el formato verbo y sustantivo, pero un nombre mas acertado sería `writeField(name)`.

Por ejemplo `assertEquals`, se puede escribir como `assertExpectedEqualsActual(expected, actual)`, aquí el nombre de la función da más sentido a sus argumentos.

## 👉 Sin efectos secundarios

Sucede cuando la función promete hacer una cosa, pero tambien hace otras cosas ocultas.

Ejemplo 3.6

Esta función compara el `username` con `password` devuelve `true` si coinciden y `false` si hay problema, pero tiene un efecto secundario.

```
public class UserValidator {
  private Cryptographer cryptographer;

  public boolean checkPassword(String userName, String password) {
    User user = UserGateway.findByName(userName);
    if (user != User.NULL) {
      String codedPhrase = user.getPhraseEncodedByPassword();
      String phrase = cryptographer.decrypt(codedPhrase, password);
      if ("Valid Password".equals(phrase)) {
        Session.initialize();
        return true;
      }
    }
    return false;
  }
}
```

El efecto secundario es la invocación de `Session.initialize()`. Pues la funcion `checkPassword` su nombre no implica que inicialice la sesión. El efecto de esto seria que solo se puede invocar la función `checkPassword` solo cuando se pueda iniciar sesion y no en otros contextos.

Pero en el caso de que se necesite esta función tal cual, se debería cambiar su nombre por ejemplo: `checkPasswordAndInitializeSession`, describe mejor el proposito de la función pero incumple la norma de hacer solo una cosa.

## 👉 Separación de consultas de comando

La función debe cambiar el estado de un objeto o devolver información sobre el mismo, si hace ambas cosas cusa confusión. Por ejemplo:

`if(set("username, "unclebob"))...`

Es complicado entender si la función `set` es un verbo o un adjetivo, o sea; El autor quiere que `set` sea un verbo, pero la instruccion `if` lo vuelve un adjetivo. es decir esa linea se lee "si el atributo username se ha establecido previamente como unclebob", no como "establecer el atributo username en unclebob".

La solución es separar el comando de la consulta para evitar ambigüedad:

```
if(attributeExists("username")){
  setAtribute("username", "unclebob")
}
```

## 👉 Mejor excepciones que devolver códigos de error

Devolver códigos de error es incumplir la regla anterior (separación de comandos de consulta). Por ejemplo:

`if(deletePage(page) == E_OK)`

Aquí no padece de la confusión de verbo, adjetivo, el problema es cuando se quiere devolver un código de error, pues debes procesar ese error de forma inmediata y eso genera estructuras anidadas:

```
if (deletePage(page) == E_OK) {
  if (registry.deleteReference(page.name) == E_OK) {
    if (configKeys.deleteKey(page.name.makeKey()) == E_OK){
      logger.log("page deleted");
    } else {
      logger.log("configKey not deleted");
    }
  } else {
    logger.log("deleteReference from registry failed");
  }
} else {
  logger.log("delete failed"); return E_ERROR;
}
```

Pero si se usan excepciones en vez de códigos de error, el código de procesamiento se puede separar del código de error y se simplifica.

```
try {
  deletePage(page);
  registry.deleteReference(page.name);
  configKeys.deleteKey(page.name.makeKey());
}
catch (Exception e) {
  logger.log(e.getMessage());
}
```

## 👉 Extraer bloques Try/Catch

Se recomienda extraer estos bloques en funciones individuales, porque estos bloques Try/Catch confunden la estructura del código y mezcla el procesamiento de errores con el procesamiento normal. Por ejemplo:

```
public void delete(Page page) {
  try {
    deletePageAndAllReferences(page);
  }
  catch (Exception e) {
    logError(e);
  }
}

private void deletePageAndAllReferences(Page page) throws Exception {
  deletePage(page);
  registry.deleteReference(page.name);
  configKeys.deleteKey(page.name.makeKey());
}

private void logError(Exception e) {
  logger.log(e.getMessage());
}
```

Aquí la función delete, es solo para procesamiento de errores, es facil de entender y de ignorar, esta separación facilita la comprensión y la modificación del codigo. Esto afianza la regla de que las funciones deben hacer una sola cosa y el procesamiento de errores es un ejemplo. Por tanto la función que procese errores no debe hacer nada mas que eso.

## 👉 No repetirse

En el ejemplo 3.1, hay una algoritmo que se repite 4 veces, en `SetUp`, `SuiteSetUp`, `TearDown` y `SuiteTearDown`, esta repetición se mezcla con el código por ello no es facil detectar, esto es un gran problema porque aumenta el tamaño del código y requerirá 4 modificaciones si se actualiza el código, eso cuadruplica el riesgo de errores.

Esa repetición se remedia gracias al metodo `include` del ejemplo 3.7, donde el código es mas legible y se reduce la duplicación.

Ejemplo 3.7

```
package fitnesse.html;

import fitnesse.responders.run.SuiteResponder;
import fitnesse.wiki.*;

public class SetupTeardownIncluder {
  private PageData pageData;
  private boolean isSuite;
  private WikiPage testPage;
  private StringBuffer newPageContent;
  private PageCrawler pageCrawler;

  public static String render(PageData pageData) throws Exception {
    return render(pageData, false);
  }

  public static String render(PageData pageData, boolean isSuite)
    throws Exception {
      return new SetupTeardownIncluder(pageData).render(isSuite);
    }

  private SetupTeardownIncluder(PageData pageData) {
    this.pageData = pageData;
    testPage = pageData.getWikiPage();
    pageCrawler = testPage.getPageCrawler();
    newPageContent = new StringBuffer();
  }

  private String render(boolean isSuite) throws Exception {
    this.isSuite = isSuite;
    if (isTestPage())
      includeSetupAndTeardownPages();
      return pageData.getHtml();
  }

  private boolean isTestPage() throws Exception {
    return pageData.hasAttribute("Test");
  }

  private void includeSetupAndTeardownPages() throws Exception {
    includeSetupPages();
    includePageContent();
    includeTeardownPages();
    updatePageContent();
  }

  private void includeSetupPages() throws Exception {
    if (isSuite)
      includeSuiteSetupPage();
      includeSetupPage();
  }

  private void includeSuiteSetupPage() throws Exception {
    include(SuiteResponder.SUITE_SETUP_NAME, "-setup");
  }

  private void includeSetupPage() throws Exception {
    include("SetUp", "-setup");
  }

  private void includePageContent() throws Exception {
    newPageContent.append(pageData.getContent());
  }

  private void includeTeardownPages() throws Exception {
    includeTeardownPage();
    if (isSuite)
      includeSuiteTeardownPage();
  }

  private void includeTeardownPage() throws Exception {
    include("TearDown", "-teardown");
  }

  private void includeSuiteTeardownPage() throws Exception {
    include(SuiteResponder.SUITE_TEARDOWN_NAME, "-teardown");
  }

  private void updatePageContent() throws Exception {
    pageData.setContent(newPageContent.toString());
  }

  private void include(String pageName, String arg) throws Exception {
    WikiPage inheritedPage = findInheritedPage(pageName);
    if (inheritedPage != null) {
      String pagePathName = getPathNameForPage(inheritedPage);
      buildIncludeDirective(pagePathName, arg);
    }
  }

  private WikiPage findInheritedPage(String pageName) throws Exception {
    return PageCrawlerImpl.getInheritedPage(pageName, testPage);
  }

  private String getPathNameForPage(WikiPage page) throws Exception {
    WikiPagePath pagePath = pageCrawler.getFullPath(page);
    return PathParser.render(pagePath);
  }

  private void buildIncludeDirective(String pagePathName, String arg) {
    newPageContent
      .append("\n!include ")
      .append(arg)
      .append(" .")
      .append(pagePathName)
      .append("\n");
  }
}
```

## 👉 Conclusión

Cuando escribes un sistema las funciones son los verbos y las clases los sustantivos, el verdadero objetivo es que las funciones deben contar la historia del sistema. Pero escribir funcinones que cumplan con todas las reglas es difícil que alguien pueda hacerlo, es un proceso posterior, es decir se consigue retocando el codigo, cambiando nombres, eliminando duplicados, reduciendo metodos, ordenarlos y haciendo pruebas.
