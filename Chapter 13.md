# CLEAN CODE: CAPÍTULO 13 - CONCURRENCIA

## 👉 ¿Porqué Concurrencia?

La concurrencia es una estrategia de desvinculación, para aplicaciones multiproceso. Permite desacoplar lo que se hace de dónde se hace, y mejorar tanto el rendimiento como la estructura de una aplicación.

En aplicaiones de un solo proceso el qué y el cuándo, estan firmemente vinculados. Un programador que depure este tipo de sistemas puede definir un punto de interrupción (o varios) y saber el estado de la aplicaici´ón en función del punto al que se llegue.

Desde el punto de vista estructural una aplicación concurrente parece un conjunto de computadoras colaborando entre sí, haciendo el sistema más facil de entender y permitiendo separar responsabilidades. En cambio una no concurrente parace a un gran bucle principal.

La concurrencia permite mejorar los tiempos de respuesta y la eficiencia de una aplicación.

**Mitos e Impresiones**

- La concurrencia siempre mejora el rendimiento:
  En ocasiones lo hace, pero solo cuando se puede compartir tiempo entre varios procesos o procesadores.

- El diseño no cambia al crear programas concurrentes:
  De hecho, el diseño de un algoritmo concurrente puede ser muy distinto al de un sistema de un solo proceso.

- No es importante entender los problemas de concurrencia al trabajar con un contenedor Web o EJB: En realidad, debe saber lo que hace su contenedor y protegerlo de problemas de actualizaciones concurrentes y bloqueo

- Otros aspectos relacionados con la concurrencia:
  - Genera cierta sobrecarga
  - Es compleja, incluso para problemas sencillos
  - Los errores no se suelen repetir, de modo que se ignoran.
  - Suele acarrear un cambio fundamental de la estrategia de diseño.

## 👉 Desafíos

¿Qué hace que la programación concurrente sea tan dificil? por ejemplo:

```java
public class X {
  private int lastIdUsed;

  public int getNextId() {
    return ++lastIdUsed;
  }
}
```

"Supongamos" xD... que se crea una instancia `X`, establecemos el campo `lastIdUsed` en 42 y después compartimos la instancia entre dos procesos. Esos procesos invocan al método `getNextId()`, hay tres resultado posibles:

- El primer proceso obtiene el valor 43, el segundo, 44 y `lasIdUsed` es 44
- El primer proceso obtiene el valor 44, el segundo, 43 y `lasIdUsed` es 44
- El primer proceso obtiene el valor 43, el segundo, 43 y `lasIdUsed` es 43

Como vemos, el tercer resultado es sorprendente, y esto se debe a que se pueden adoptar varias rutas posibles en una línea de código y algunas generan resultados incorrectos. Existen aproximadamente unas 12.870 rutas diferentes para el ejemplo que acabamos de mencionar, si fuera `long` en vez de `int` ascendería a 2.704.156. Muchas generan resultados válidos, pero el problema es que algunas no lo hacen.

## 👉 Principios de defensa de la concurrencia

**El principio de responsabilidad única (SRP)**, Establece que un método, clase o componente debe tener un motivo para cambiar. El diseño de concurrencia, es lo suficientemente complejo, como para ser un motivo de cambio con derecho propio, la recomendación es que se separe el código de concurrencia del resto del código.

**Limitar el ámbito de datos**, ya que dos procesos que modifican el mismo campo pueden interferir entre ellos. Una solución consiste en usar la palabra clave `synchronized` para proteger una sección del código, aunque conviene limitar el número de estas secciones. Recomendación, encapsular los datos y limitar el acceso a los datos compartidos.

**Usar copias de datos**, Una forma de evitar datos compartidos es no compartirlos. En algunos casos se pueden copiar objetos y procesarlos como solo lectura, o copiar, recopilar los resultados y combinarlos en un resultado en mismo proceso. Si existe una forma sencilla de evitar los objetos compartidos, el código resultante tendrá menos problemas.

**Los procesos deben ser independientes**, intentar dividir los datos en subconjuntos independendientes que se puedan procesar en procesos independientes, posiblemente en distintos procesadores.

## 👉 Conocer las librerias

En Java; Doug Lea, desarrolló varias colecciones compatibles con procesos que pasaron a formar parte del JDK en el paquete `java.util.concurrent`. Por ejemplo la implementación ConcurrentHashMap, tiene mejor rendimiento que HashMap en la mayoría de los casos. Como recomendación, revisar las clases de las que disponga. En el caso de Java, debe familiarizarse con concurrent, `concurrent.atomic` y `concurrent.lock `

## 👉 Conocer los Modelos de Ejecución

Antes de ver los modelos de ejecución empleados en la programación concurrente, tenemos estas definiciones Básicas:

- Recuros Vinculados: recursos de tamaño o número fijo usados en un entorno concurrente, como por ejemplo conexiones a base de datos, búfer de lectura/escritura.
- Exclusión Mutua: Solo un proceso puede acceder a datos o a un recurso compartido por vez.
- Inanición: Se impide que un proceso o grupo de procesos continuen demasiado tiempo o indifinidamente.
- Bloqueo: Dos o mas procesos esperan a que ambos terminen.
- Bloqueo Activo: Procesos bloqueados, intentando realizar su labor pero estorbandose unos a otros.

**Productor-Consumidor (Producer-Consumer):**
Uno o varios procesos productores crean trabajo y lo añaden a un búfer o a una cola. Uno o varios procesos consumidores adquieren dicho trabajo de la cola y lo completan. La cola sería un recurso vinculado, la coordinación entre productores y consumidores a través de la cola hace que unos emitan señales a otros, cuando uno u otro a completado su trabajo.. Ambos esperan notificaciones para poder continuar.

**Lectores-Escritores (Readers-Writers)**
Cuando un recurso compartido actúa básicamente como fuente de información para lectores pero ocasionalmente se actualiza por parte de escritores, la producción es un problema. La coordinación de los lectores para que no lean algo que un escritor está actualizando y viceversa es complicada, los escritores tienden a bloquear lectores durante tiempo prolongado, lo que genera problemas. El desafío consiste en equilibrar las necesidades de ambos para satisfacer un funcionamiento correcto.

**Cena de Filósofos**
Imagine varios filósofos, sentados en una mesa y una fuente de espaguetis, mientras no tienen hambre piensan, y para comer necesitan 2 tenedores, si el de la derecha o izquierda ya tiene un tenedor tendrá que esperar, ahora cambie los filósofos por procesos y los tenedores por recursos y tendrá un problema habitual en muchas aplicaciones en las que los procesos compiten por recursos, a menos que se diseñen correctamente, estos sistemas sufren problemas de bloqueo.

La mayoría de problemas de concurrencia son una variante de estos 3 que mencionados. Se recomienda, aprender estos algoritmos básicos y comprender sus soluciones.

## 👉 Dependencias entre Métodos Sincronizados

No deben existir dependencias entre métodos sincronizados, pueden generar sutiles errores en el código, si hay más de un método sincronizado en la misma clase compartida puede que su sistema sea incorrecto.

En ocasiones tendrá que usar más de un método en un objeto compartido. En ese caso hay tres formas:

**Bloqueo basado en clientes:** El cliente debe bloquear al servidor antes de invocar el primer método y asegurar de que el alcance incluye la invocación al último método.

**Bloqueo basado en servidores:** Debe crear un método en el servidor que bloquee el servidor, invoque todos sus métodos y después anule el bloqueo.

**Servidor adaptado:** Cree un intermediario que realice el bloque.

Como recomendación hay que evitar usar más de un método en un objeto compartido.

## Reducir el tamaño de las secciones sincronizadas

Es recomendable hacer esto, la palabra clave `synchronized` en Java presenta un bloqueo, los bloqueos son costosos ya que generan retrasos y añaden sobrecarga, por ello reduzca al máximo el tamaño de dichas secciones `synchronized`.

## Crear código de cierre correcto es complicado

Crear un sistema y que se ejecute indefinidamente es distinto a crear algo que funcione de forma temporal y después se cierre. Pueden existir en estos casos bloqueos, con procesos que esperan una señal para continuar que nunca se produce.

Imagine un sistema que crea procesos y finaliza cuando todos han finalizado, si uno de los subprocesos no finaliza, el principal nunca finalizará.

Ahora imagine un sistema similar, que se le indica que finalice y 2 procesos que funcionan como productor/consumidor, y el producto se cierra de pronto. El consumidor puede quedarse esperando indefinidamente por un mensaje que jamás llegará y quedarse bloqueado y no recibir la señal del principal.

Recomendación, planifique con antelación el proceso de cierre y pruébelo hasta que funcione.

## 👉 Probar código con Procesos

A la hora de probar código con procesos, se recomienda crear pruebas que puedan detectar problemas y ejecutarlas periódicamente, con distintas configuraciones de programación y del sistema, y cargas.

Hay muchos factores a tener en cuenta, entre los más importantes:

- **Considerar los fallos como posibles problemas de los procesos.**
  El código con procesos hace que fallen elementos que no deberían fallar. Los problemas del código con procesos pueden mostrar sus fallos una vez cada mil o un millón de ejecuciones. Los intentos por repetir el fallo suelen fallar, lo que suele provocar que los programadores lo consideren como un caso aislado. Recomendación, no ignore los fallos del sistema como algo aislado

- **Conseguir que primero funciones el código sin procesos.**
  Los procesos ejecutan código fuera de sus entornos, no intente identificar fallos de procesos y que no sean de procesos al mismo tiempo. Asegúrese de que su código funciona fuera de los procesos.

- **El código con procesos se debe poder conectar a otros elementos.**
  El código con procesos debe poder conectar a otros elementos y ejecutar en distintas configuraciones.

- **El código con procesos debe ser modificable.**
  La obtención del equilibrio adecuado de procesos suele requerir operaciones de ensayo y error. Permita que se puedan modificar los distintos procesos y también durante la ejecución del sistema.

- **Ejecutar con más procesos que procesadores.**
  Cuando el sistema cambia de tarea se producen reacciones, para ello realice la ejecución con más procesos que procesadores o núcleos, así comprobará si el código carece de sección crítica o se producen bloqueos.

- **Ejecutar en diferentes plataformas.**
  En 2007 diseñamos un curso de programación concurrente para OS X, la clase la hicimos en WIndows XP con máquina virtual, en todos los casos de prueba el código era incorrecto, cada sistema operativo tiene una política de procesos diferentes que afecta a la ejecución del sistema. Ejecute el código con procesos en todas las plataformas de destino con frecuencia y en las fases iniciales.

- **Diseñar el código para probar y forzar fallos.**
  Es habitual que los fallos del código concurrente se oculten. Para ello es conveniente intentar forzar estos fallos, haciendo que el código se ejecute de mil formas diferentes, para ello puede usar métodos como wait(), sleep(), yield() o priority(). Hay dos opciones de instrumentación de código:

  - **Manual**
    Puede añadir invocaciones de los métodos anteriormente mencionados manualmente a su código.
  - **Automática**
    Puede usar herramientas como la estructura orientada a aspectos, CGLIB o ASM para instrumentar su código mediante programación.
