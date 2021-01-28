# CLEAN CODE: CAPÍTULO 8 - LÍMITES

En algunos casos nos vemos en la necesidad de integrar codigo externo, en otros casos dependemos de de equipos de nuestra propia empresa, se debe integrar todo eso conociendo los límites de nuestro software.

## Utilizar código de terceros

Los paquetes de terceros tienen una capacidad de aplicaión global para trabajar diversos entornos o para atraer mas público. Pero por nuestra parte deseamos algo centrado en una necesidad concreta, esto puede ocasionar probleas en los limites de nuestro sistema.

Por ejemplo: La clase `java.util.map`, tiene numerosas prestaciones, esto puede ser un problema; un metodo de esta clase es `clear()`, esntonces cualquier usuario del mapa puede borrarlo. Es decir solo se deben almacenar ciertos objetos concretos del mapa.

Si nuestra app necesita un Map Sensor, se definiría asi:

```java
Map sensors = new HasMap();
```

Cuando una parte del código necesita acceder al sensor: (esto se podria ver muchas veces a o largo del código, no es código limpio)

```java
Sensor s = (Sensor) sensors.get(sensorId);
```

La legibilidad se puede mejorar con el uso de genéricos:

```java
Map<sensor> sensors = new HasMap<sensor>();
...
Sensor s = sensors.get(sensorId);
```

Pero esto no soluciona el problema de que `Map<sensor>` ofrezca mas prestaciones de lo que se desea. Una forma mas limpia de usar Map sería:

```java
public class Sensors {
  private Map sensors = new HashMap();

  public Sensor getById(String id) {
    return (Sensor) sensors.get(id);
  }

  //snip
}
```

Encapsulamos la interfaz Map en una clase propia para usarla solo con lo que se necesita; En el código anterior la clase `Sensors` se ha ajustado y limitado a las necesidades de la aplicación, aplica las reglas empresariales y de diseño. Entonce, si pasamos una instancia de esta clase ya no se podrá borrar o añadir objetos, solo tenemos la funcionalidad que se necesita.

### Explorar y aprender limites

En ocasiones no es facil usar librerías de terceros, se puede perder tiempo en leer su documentación y decidir como usarla. Aprender el código de terceros e integrarlo al mismo tiempo es difícil. En lugar de hacer eso se recomienda crear pruebas de aprendizaje, donde se invoca la API de terceros como supuesta mente se usaría en nuestra aplicaión, es decir realizar experimentos controlados y asi comprobar si se entiende, centrándose en lo que sequiere obtener de la API.

En java el autor recomienda aprender la libreriá `log4j` que permite a los desarrolladores de software escribir mensajes de registro, cuyo propósito es dejar constancia de una determinada transacción en tiempo de ejecución.

Estas pruebas de aprendizaje no cuestan nada, a parte de ayudar a enteder la librería de terceros, si ésta se actualiza en el futuro se puede ver si sigue funcionando, por lo tanto es rentable.

### Usar código que todavía no existe

En ocasiones hay que desarrollar aplicaiones que se van a relacionar con otros sistemas, de los cuales se desconoce su código o simplemente aún no se han diseñado, el punto es no mirar mas allá de ese límite y trabajar en lo que te concierne, desarrollar tu interfaz, manteniendo el enfoque de lo que debe de hacer.

### Conclusión

Cuando se usa codigo de terceros, que no controlas hay que asegurarse que los cambios futuros no sean demasiado costosos. El código en los limites requiere una separación evidente y pruebas que definan expectativas, se debe evitar que nuestro código conozca detalles de terceros.

Es mas aconsejable depender de algo que controlemos que de algo que no controlemos, se pueden evolver como en el caso de Map o convertir nuestra aplicación en una interfaz que proporcione algo.
