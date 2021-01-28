# CLEAN CODE: CAPÍTULO 8 - PRUEBAS DE UNIDAD

Antiguamente las pruebas era codigo desechable, pero ahora ha evolucionado mucho el desarrollo guiado por pruebas, los moviemientos Agile y TDD son los propulsores de las pruebas de unidad automatizadas.

## 👉 Las tres leyes del TDD (Test-driven development)

1. Primera: No crear código de producción hasta que haya creado una prueba de unidad que falle.
2. Segunda: No debe crear mas de una prueba de unidad que falle y no compilar se considera un fallo.
3. Tercera: No debe crear mas código de producción del necesario para superar la prueba de fallo actual.

Esto nos permitirá tener cientos de pruebas tras varios días y semanas de desarrollo, aunque el tamaño de estas pruebas también puede suponer un problema.

## 👉 Mantener las Pruebas Limpias

Las pruebas tienen que mantener el mismo estandar de Clean Code, es decir nombre de variables adecuados, las funciones de prueba tienen que ser breves y descriptivas, etc. Cuando el codigo evoluciona, las pruebas también, y cuanto menos limpias sean es un gran problema, pues es más dificil cambiarlas.

El efecto de esto puede ser en desechar completamente las pruebas, resultando un código de producción enmarañado y defectuoso. Clientes insatisfechos y perdida de tiempo. La moraleja; El código de pruebas es tan importante como el de producción.

- ### Las pruebas proporcionan posibilidades:

  Sin pruebas el código de producción pierde flexibilidad; es decir ya no se puede mantener y reutilizar. En cambio si tienes pruebas no habra ningún temor de hacer cambios en el código. Con pruebas ese miedo desaparece. Cuando mayor dea el alcance de las pruebas, menos temor habrá.

## 👉 Pruebas Limpias

¿Que hace que una prueba se limpia? tres cosas: legilibilidad, legibilidad y legibilidad. Es decir que la legibilidad es mas importante en las pruebas que en el código de producción en sí.
Legibilidad: claridad, simplicidad y densidad de expresión. una prueba debe decir mucho con el menro numero de expresiones posible.

Las pruebas deden de cumplir con el patron `Build-Operate-Check` (AAA - Arrange, act, assert), primero crear datos de prueba, segundo operar en dichos datos y la tercera comprueba que la operación devuelva los resultados esperados.

- ### Una Afirmación (Assert) por test

  Está permitido hacer más de un assert en una prueba, pero sí se debe cumplir que el número de asserts sea mínimo. Lo que sí se debe cumplir siempre es que solo se prueba una cosa en cada test. Es decir es importante que cada prueba solo compruebe un solo concepto.

  Por ejemplo: esta prueba comprueba que la alarma de baja temperatura, el calentador y el fuelle esten activados cuando la temperatura es demasiado fría.

```java
@Test
public void turnOnLoTempAlarmAtThreashold() throws Exception {
  hw.setTemp(WAY_TOO_COLD);
  controller.tic();
  assertTrue(hw.heaterState());
  assertTrue(hw.blowerState());
  assertFalse(hw.coolerState());
  assertFalse(hw.hiTempAlarm());
  assertTrue(hw.loTempAlarm());
}
```

Ahora las pruebas anteriores son mas fáciles de entender de las siguiente manera:

```java
@Test
public void turnOnCoolerAndBlowerIfTooHot() throws Exception {
  tooHot();
  assertEquals("hBChl", hw.getState());
}

@Test
public void turnOnHeaterAndBlowerIfTooCold() throws Exception {
  tooCold();
  assertEquals("HBchl", hw.getState());
}

@Test
public void turnOnHiTempAlarmAtThreshold() throws Exception {
  wayTooHot();
  assertEquals("hBCHl", hw.getState());
}

@Test
public void turnOnLoTempAlarmAtThreshold() throws Exception {
  wayTooCold();
  assertEquals("HBchL", hw.getState());
}
```

Entonces como regla óptima sea minimizar el número de activos por concepto y probar un solo concepto por funcion de prueba.

- ### F.I.R.S.T

  Las pruebas limpias siguen otras 5 reglas cuyas iniciales forman las siglas FIRST en ingles.

  **Fast** (Rapidez): Las pruebas deben ser rápidas y ejecutarse de forma rápida. Si son lentas terminará por no ejecutarlas.

  **Independent** (Independencia): Las pruebas no deben depender entre ellas, una prueba no debe establecer condiciones para la siguiente.

  **Repeatable** (Repetición): Las pruebas deben poder repetirse en cualquier entorno.

  **Self-Validating** (Validación automática): Las pruebas deben tener un resultado booleano; o aciertan o fallan.

  **Timely** (Puntualidad): Las pruebas deben crearse en el momento preciso: antes del código de producción.

## Conclusión

Las pruebas son tan importantes que el código de producción, incluso más; porque conservan y mejoran la flexibilidad, la capacidad de mantenimiento y reutilización del código de producción. Por ello es muy importante mantener las pruebas "limpias".
