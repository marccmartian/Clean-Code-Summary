# CLEAN CODE: CAPÍTULO 4 - COMENTARIOS

Los comentarios son justificados solo cuando somos incapaces de expresarnos en el código, la razón es que el código cambia y evoluciona, los comentarios no, esto puede crear información falsa separandose mucho del código que describen.
Solo el código debe contar lo que se hace, es la única fuente de información precisa.

## 👉 Explicarse con el codigo:

En muchos casos basta con crear una función que diga lo mismo que el comentario que se pensaba escribir

## 👉 Comentarios de calidad:

- Comentarios legales: De derechos de autor, licencia, copyright.
- Comentarios informativos: A vece es util informa sobre lo que devuelve una función; pero se puede eliminar, si en el nombre de la función especificamos lo que se devuelve.

- Explicar La intención: es muy util cuando se porporciona la intención de una desición, por ejemplo:

```
public int compareTo(Object o) {
  if(o instanceof WikiPagePath) {
    WikiPagePath p = (WikiPagePath) o;
    String compressedName = StringUtil.join(names, "");
    String compressedArgumentName = StringUtil.join(p.names, "");
    return compressedName.compareTo(compressedArgumentName);
  }
  return 1; // Es verdad, porque somos el tipo correcto
}
```

- Comentarios de Clarificación: En ocasiones se utiliza librerías de terceros, en los cuales no se pueden modificar los nombres de funciones, alli los comentarios pueden ser muy utiles.

- Comentarios TODO: En ocasiones tambien conviene usar este tipo de comentarios, que explica que una función tiene una implementación incorrecta y cuál debe ser su futuro. (depende de otras funciones y por el momento se usan valores por defecto, hasta que se termine su implementación)

## 👉 Comentarios incorrectos:

Estos comentarios suelen ser justificaciones de desiciones insuficientes, algo así como si el programador se hablara a sí mismo.

- Comentarios redundantes, son comentarios que explican lo que el código bien escrito hace.
- Comentarios confusos, cuando se realizan afirmaciones que nos son del todo precisas.
- Comentarios obligatorios, estos comentarios establecen que todas las funciones deben tener un javadoc o que todas las variables tienen que tener un comentario.
- Comentarios Periódicos, son aquellos que se suelen usar al inicio de un modulo como un registro del historial de cambios realizados. Ahora ya hay sistemas de control de versiones que hacen ese trabajo.
- Marcadores de posición, en ocasiones se usan los comentarios para marcar cierta posición del codigo y ubicarlo facilmente.
- Comentarios de llave de cierre, A veces se incluyen comentarios en las llaves de cierre, aunque puede tener sentido en funciones extensas con estructuras anidadas, pero una función extensa anidada ya es señal que no es código limpio.
- Codigo Comentado.
- Comentarios HTML.
- Comentario s que contienen demasiada información.
