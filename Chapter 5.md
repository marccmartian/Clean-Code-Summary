# CLEAN CODE: CAPÍTULO 5 - FORMTATO

El codigo debe de sorprender, estar en orden, causar una buena inpresión, no debe ser como toda una masa amorfa.

## 👉 Formato Vertical:

El tamaño de los ficheros no debería superar las 200 líneas de media, con un límite máximo en 500.

### Metáfora del Periódico:

Un periódico esta estructurado en partes, cada una tiene un titulo, su descripcion; hay articulos grandes y otros pequeños.
De la misma forma debe estar nuestro código, cada modilo del programa es una compilacion de grandes y pequeños articulos. dividido por clases que poco a poco entran en los detalles

- Apertura Vertical entre conceptos:
  Una línea en blanco es una pista visual, por ejemplo para separar importaciones de archivos, con declaraciones de paquetes y funciones.

- Densidad Vertical:
  Las lineas de código que tiene una relacion directa deben aparacer verticalmente densas; por ejemplo, para agrupar las variables de instancia de los metodos de una clase.

- Distancia Vertical:
  Los conceptos relacionados entre si deben mantenerse juntos verticamente, en consecuencia evitar separar estos elementos en dos ficheros distintos.

- Declaraciones de Variables:
  Las variables deben declararse de la forma mas próxima a su uso. Como las funcinoes son breves, las variables locales deben aparecer en la parte superior de cada función.

- Variables de Instancia:
  Estas variables deben declararse en la partee superior de la clase, ya que en la la misma clase se usan en muchos sino en todos sus métodos.

- Funciones dependientes:
  Si una funcion invoca a otra, deben estar verticalmente próximas, y la función de invocaion debe estar por encima de la invocada siempre que sea posible. Es decir una funcion superir invoca a las situadas por debajo que, a su vez, invocan a las siguientes. Esto mejora la legibilidad del modulo completo.

## 👉 Formato Horizontal:

La anchura de las líneas de código deben estar entre los 80 y 120 caracteres, no debe de desplazarce hacia la derecha, ni reducir el tamaño de fuente para leer una sola línea de código.

- Apertura y densidad horizontal:
  Los espacios en blanco horizontal se usan para asociar elementos directamente relacionados y separar otros con una relación mas estrecha. por ejemplo:

  - los operadores de asignación se rodea con espacios en banco para destacarlos.
  - el espacio entre argumentos de una función.
  - el espacio en blanco para acentuar la presedencia de operadores.

- Sangrado:
  En un archivo de codigo hay información, por ejemplo de las clases, sus metodos, los bloques de los metodos, los bloques de los bloques, etc. es decir hay ua jerarquia de código.
  Para que esta jerarquia sea visible y dar mas legibilidad, se sangra las lineas de código; y se recomienda no romper esta regla por mas pequeña que sea la línea de codigo (if, while).

- Reglas de equipo:
  Cualquier equipo de desarrollo debería tener unas reglas convenientemente consensuadas. Todos deben seguir estas reglas (el equipo manda). El estilo y formato debe ser siempre el mismo ya que el código es compartido. Esto garantiza que el software tenga un estilo coherente.
