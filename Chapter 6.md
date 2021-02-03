# CLEAN CODE: CAPÍTULO 6 - OBJETOS Y ESTRUCTURA DE DATOS

Hay una razón para que las variables sean privadas; que nadie más dependa de ellas. pero ¿Por qué muchos añaden automáticamente métodos de establecimiento (set) y recuperación (get) que muestran sus variables privadas como públicas?

## 👉 Abstracción de datos:

Los ejemplos 6.1 y 6.2 representan los datos de un punto cartesiano.

- Ejemplo 6.1 (Concrete Point): Muestra la implementación de los datos.
  Se implementa claramente en coordenadas rectangulares y obliga a manipularlas de forma independiente, muestra su implementación aunque fueran privadas y tuvieran sus metodos set y get.

```java
public class Point {
  public double x;
  public double y;
}
```

- Ejemplo 6.2 (Abstract Point): NO muestra la implementación de los datos.
  No se sabe si la implementación esta en coordenadas rectagulares o polares o ninguna, pero representa una estructura de datos.

```java
public interface Point {
  double getX();
  double getY();
  void setCartesian(double x, double y);
  double getR();
  double getTheta();
  void setPolar(double r, double theta);
}
```

## 👉 Antisimetría de Datos y Objetos

Diferencia entre Objetos y Estructura de datos.

- Objetos: Ocultan sus datos tras abstraciones y muestran funciones que operan en dichos datos.
- Estructura de Datos: Muestra sus datos y carece de funciones con significado.

### Ejemplo: Procedural Shape

La clase `Geometry` opera en las tres clases "shape" (`Square`, `Rectangle`, `Circle`) que son estructuras sin comportamiento, todo el comportamiento se encuentra en la clase `Geometry`.

Este es un ejemplo de procedimientos, su añadimos una función `perimeter()` a `Geometry`, las clases "shape" no se verian afectadas y las demas clases que dependieran de ellas tampoco.
Pero si se añade un nuevo "shape" tendria que cambiar todas las funciones de `Geometry`. Ambas condiciones son opuestas.

```java
public class Square {
  public Point topLeft;
  public double side;
}

public class Rectangle {
  public Point topLeft;
  public double height;
  public double width;
}

public class Circle {
  public Point center;
  public double radius;
}

public class Geometry {
  public final double PI = 3.141592653589793;
  public double area(Object shape) throws NoSuchShapeException   {
    if (shape instanceof Square) {
      Square s = (Square)shape;
      return s.side * s.side;
    }
    else if (shape instanceof Rectangle) {
      Rectangle r = (Rectangle)shape;
      return r.height * r.width;
    }
    else if (shape instanceof Circle) {
      Circle c = (Circle)shape;
      return PI * c.radius * c.radius;
    }
    throw new NoSuchShapeException();
  }
}
```

### Ejemplo: Polymorphic Shape

Aqui el método `area()` es polimórfico, no se necesita una clase `Geometry`.

Esta es la solución orientada a objetos. Si añado un nuevo "shape", ninguna de las funciones existentes se ven afectadas; pero si añado otra función, habrá que cambiar todas las "shape".

```java
public class Square implements Shape {
  private Point topLeft;
  private double side;

  public double area() {
    return side*side;
  }
}

public class Rectangle implements Shape {
  private Point topLeft;
  private double height;
  private double width;

  public double area() {
    return height * width;
  }
}

public class Circle implements Shape {
  private Point center;
  private double radius;
  public final double PI = 3.141592653589793;

  public double area() {
    return PI * radius * radius;
  }
}
```

- Dicotomía entre estructura de datos y objetos: "Lo que es difícil para la POO es fácil para los procedimientos y viceversa"

  El código por procedimientos (el que usa estructura de datos), facilita la inclusión de nuevas funciones sin modificar las estructuras de datos existentes.
  El código oientado a objetos, facilita la inclusión de nuevas clases sin cambiar las funciones existentes.

  El código por procedimientos dificulta la inclusión de nuevas estructuras de datos ya que es necesario cambiar todas las funciones. El códfigo orientado a objetos dificulta la inclusión de nuevas funciones ya que es necesario cambiar todas las clases.

## 👉 Ley de Demeter: Principio de menor conocimiento

"Solo habla con tus amigos y evita hablar con extraños".
Lo que busca esta ley es que un objeto solo se relacione consigo mismo todo lo que sea posible, es decir intentar que cada objeto sepa lo menos posible de otros ajenos. Un método f de una clase C solo debe invocar los métodos de:

- C (metodos de la misma clase),

Por ejemplo, `Ejecutar()` puede invocar a `Validar()` porque pertenece a la misma clase

```c#
public class Servicio
{
  public void Validar()
  {

  }

  public void Ejecutar()
  {
    this.validar();
  }
}
```

- Un objeto creado por f

```c#
public class Servicio
{
  public void Ejecutar()
  {
    ServicioHelper helper = new ServicioHelper();
    helper.RegistrarInicio();
  }
}
```

- Un objeto pasado como un argumento de f (metodos que pertenecen a los parámetros de la funcion )

```c#
public class Servicio
{
  public void Ejecutar(Usuario usuario)
  {
    Datetime nombre = usuario.ObtenerPrimerLogin();
  }
}
```

- Un objeto contenido en una variable de instancia de C

```c#
public class Servicio
{
  private ServiceHelper helper;

  public Servicio()
  {
    helper = new ServicioHelper();
  }

  public void Ejecutar()
  {
    helper.RegistrarInicio();
  }
}
```

Otro ejemplo, aqui incumple esta ley pues una función llama al valor devuelto de otro.

```java
final String outputDir = ctxt.getOptions().getScratchDir().getAbsolutePath();
```

### Choque de trenes:

Es un código que se asemeja a vagones de tren, son cadenas de invocaciones, incumplen con la ley de demeter. En el fonde este código es similar al anterior (se puede escribir en una sola linea), solo que se le asignan variables a los métodos.

```java
Options opts = ctxt.getOptions();
File scratchDir = opts.getScratchDir();
final String outputDir = scratchDir.getAbsolutePath();
```

## 👉 Objetos de transferencia de datos (OTD)

Es una clase con variables públicas y sin funciones, utiles para comunicarse con bases de datos, convierten datos sin procesar en objetos en el código de la aplicación. La forma común es "Bean" - variables privadas manipuladas por métodos de establecimiento y recuperación. por ejemplo:

```java
public class Address {
  private String street;
  private String streetExtra;
  private String city;
  private String state;
  private String zip;

  public Address(String street, String streetExtra, String city, String state, String zip) {
    this.street = street;
    this.streetExtra = streetExtra;
    this.city = city;
    this.state = state;
    this.zip = zip;
  }

  public String getStreet() {
    return street;
  }

  public String getStreetExtra() {
    return streetExtra;
  }

  public String getCity() {
    return city;
  }

  public String getState() {
    return state;
  }

  public String getZip() {
    return zip;
  }
}
```

### Registro Activo (Active record)

Son una forma especial de OTD, son estructuras de datos con variables públicas (o de acceso por "bean"), pero tienen metodos de navegación como `save` y `find`, son traducciones directas de bases de datos.
Muchos procesan las estructuras de datos como si fueran objetos y les añaden metodos de reglas empresariales, esto crea un híbrido entre una estructura de datos y un objeto.

La solución es considerar el registro activo una estructura de datos y crear objetos independientes que contengan las reglas empresariales y que oculten sus datos internos.

## 👉 Conclusión

En ocasiones se necesita la flexibilidad de añadir nuevos tipos de datos, por lo que preferimos objetos para esa parte del sistema; en otros casos se necesitan añadir nuevos comportamientos, para lo que se prefiere tipos de datos y procedimentos. Hay que entender eso y eligir el enfoque mas adecuado para cada tarea.
