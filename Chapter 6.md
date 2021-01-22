# CLEAN CODE: CAPÍTULO 6 - OBJETOS Y ESTRUCTURA DE DATOS

Hay una razón para que las variables sean privadas; que nadie más dependa de ellas. pero ¿Por qué muchos añaden automáticamente métodos de establecimiento (set) y recuperación (get) que muestran sus variables privadas como públicas?

## 👉 Abstracción de datos:

Los ejemplos 6.1 y 6.2 representan los datos de un punto cartesiano.

- Ejemplo 6.1 (Concrete Point): Muestra la implementación de los datos.
  Se implementa claramente en coordenadas rectagulares y obliga a manipularlas de forma independiente, muestra su implementación aunque fueran privadas y tuvieran sus metodos set y get.

```
public class Point {
  public double x;
  public double y;
}
```

- Ejemplo 6.2 (Abstract Point): NO muestra la implementación de los datos.
  No se sabe si la implementacion esta en cordenadas rectagulares o polares o ninguna, pero representa una estructura de datos.

```
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

```
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

Esta es la solución orientada a objetos. Si añado un nuevo "shape", ninguna e las funciones existentes se ven afectadas; pero si añado otra función, habrá que cambiar todas las "shape".

```
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
