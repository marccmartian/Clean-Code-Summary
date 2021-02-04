# CLEAN CODE: CAPÍTULO 12 - EMERGENCIA

## Obener Codigo Limpio a través de Diseños Emergentes

Estas son las cuatro reglas de Kent Beck para crear un software bien diseñado (diseño sencillo - simple design). Siguiéndolas accede a la estructura y al diseño del código y facilita la aplicación de los principios SRP y DIP.

1.  Ejecuta todas las pruebas.
2.  No contiene duplicados.
3.  Expresa la intención del programador.
4.  Minimiza el número de clases y métodos.

## 👉 Simple Desgign Rule 1: **Ejecutar todas las pruebas**

Un diseño debe generar un sistema que actúe de la forma prevista. Un sistema puede tener un buen diseño, pero si no existe una forma sencilla de probarlo, el esfuerzo es cuestionable, y un sistema que no se puede verificar no debe implementarse.

Crear sistemas testables hace que diseñemos clases de tamaño reducido y un solo cometido. Cuantas más pruebas diseñemos más nos acercaremos a elementos más fáciles de probar, del mismo modo ocurre con la inyección de dependencias. Por tanto, la creación de pruebas conduce a obtener mejores diseños.

## 👉 Simple Design Rule 2 - 4: Refactoring

Una vez tenemos las pruebas, debemos mantener limpio el código. La presencia de pruebas hace que perdamos el miedo a refactorizar y que resulte el código dañado. En esta fase podemos aumentar la cohesión, reducir las conexiones, modularizar aspectos del sistema. Aquí también aplicamos las tres últimas reglas del diseño correcto: eliminar duplicados, garantizar la capacidad de expresión y minimizar el número de clases y métodos

- ### **No Contiene Duplicados**

  Los duplicados son los mayores enemigos de un sistema bien diseñado. Suponen un esfuerzo adicional, riesgos añadidos y una complejidad a mayores innecesaria.

  Ejemplo 1: en una colección podríamos tener 2 métodos, `size()` y `isEmpty()`, size podría tener un contador y isEmpty un booleano, pero en vez de tener implementaciones distintas podríamos vincularlos:

  ```java
  boolean isEmpty() {
    return 0 == size();
  }
  ```

  Ejemplo 2:

  ```java
  public void scaleToOneDimension(float desiredDimension, float imageDimension) {
    if (Math.abs(desiredDimension - imageDimension) < errorThreshold)
      return;
    float scalingFactor = desiredDimension / imageDimension;
    scalingFactor = (float)(Math.floor(scalingFactor * 100) * 0.01f);

    RenderedOp newImage = ImageUtilities.getScaledImage(
      image, scalingFactor, scalingFactor);
    image.dispose();
    System.gc();
    image = newImage;
  }
  public synchronized void rotate(int degrees) {
    RenderedOp newImage = ImageUtilities.getRotatedImage(
      image, degrees);
    image.dispose();
    System.gc();
    image = newImage;
  }
  ```

  Eliminando los duplicados (RenderedOp) entre los métodos `scaleToOneDimension` y `rotate`

  ```java
  public void scaleToOneDimension(float desiredDimension, float imageDimension) {
    if (Math.abs(desiredDimension - imageDimension) < errorThreshold)
      return;
    float scalingFactor = desiredDimension / imageDimension;
    scalingFactor = (float)(Math.floor(scalingFactor * 100) * 0.01f);
    replaceImage(ImageUtilities.getScaledImage(
      image, scalingFactor, scalingFactor));
  }

  public synchronized void rotate(int degrees) {
    replaceImage(ImageUtilities.getRotatedImage(image, degrees));
  }

  private void replaceImage(RenderedOp newImage) {
    image.dispose();
    System.gc();
    image = newImage;
  }
  ```

  Ejemplo 3: Aqui aplicamos el patrom Metodo de plantilla (Template Method), algo muy utilizado para eliminar los duplicados de nivel superior.

  El código en ambas clases es prácticamente idéntico, a excepción de calculos mínimos legales; esa parte del algoritmo cambia de acuerdo al tipo de empleado.

  ```java
  public class VacationPolicy {
    public void accrueUSDivisionVacation() {
      // code to calculate vacation based on hours worked to date
      // ...
      // code to ensure vacation meets US minimums
      // ...
      // code to apply vaction to payroll record
      // ...
    }

    public void accrueEUDivisionVacation() {
      // code to calculate vacation based on hours worked to date
      // ...
      // code to ensure vacation meets EU minimums
      // ...
      // code to apply vaction to payroll record
      // ...
    }
  }
  ```

  Las subclases ocupan vacío generado en el algoritmo `accrueVacation` y solo proporcionan los datos que no son duplicados.

  ```java
  abstract public class VacationPolicy {
    public void accrueVacation() {
      calculateBaseVacationHours();
      alterForLegalMinimums();
      applyToPayroll();
    }

    private void calculateBaseVacationHours() { /* ... */ };
    abstract protected void alterForLegalMinimums();
    private void applyToPayroll() { /* ... */ };
  }

  public class USVacationPolicy extends VacationPolicy {
    @Override protected void alterForLegalMinimums() {
      // US specific logic
    }
  }

  public class EUVacationPolicy extends VacationPolicy {
    @Override protected void alterForLegalMinimums() {
      // EU specific logic
    }
  }
  ```

- ### **Expresividad**

  En cuanto a la expresividad, es fácil crear código que entendamos, pero los encargados de mantener el código no lo comprenderán de la misma forma. El principal coste de un proyecto de software es su mantenimiento a largo plazo, para minimizar los costes es fundamental que comprendamos el funcionamiento del sistema. Por tanto el código debe expresar con claridad la intención de su autor.

  Puede expresarse bien si reduce el tamaño de funciones y clases, si mejora los nombres, usar nomenclatura estándar, patrones de diseño. Las pruebas bien escritas también son expresivas, uno de los principales objetivos de una prueba es servir de documentación mediante ejemplos.

  Hay que dedicar tiempo a las funciones y clases, seleccionar mejores nombres, dividir funciones extensas en otras mas reducidas; ser cuidadoso es un recurso precioso.

- ### **Clases y Métodos Mínimos**

  La eliminación de código duplicado, la expresividad y el SRP pueden exagerarse, en un esfuerzo por reducir el tamaño de las clases y métodos, es decir podemos crear demasiadas clases y métodos reducidos. Por ello, ésta la última regla sugiere minimizar la cantidad de funciones y clases.

  El objetivo principal es reducir el tamaño general del sistema, pero recuerde que esta regla es la de menor prioridad de las cuatro. Por ello aunque sea importante reducir la cantidad de clases y funciones, es más importante contar con pruebas, eliminar duplicados y expresarse correctamente.

## Conclusión

Estas prácticas no reemplazan a la experiencia, pero la práctica del diseño correcto anima y permite a los programadores adoptar principios y patrones que en caso contrario tardarían años en aprender.
