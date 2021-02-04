# CLEAN CODE: CAPÍTULO 7 - PROCESAR ERRORES (ERROR HANDLING)

## 👉 Usar excepciones en lugar de códigos devueltos:

No usar códigos de error ya que confunden el flujo de ejecución y obligan al invocador a procesarlos inmediatamente. Antiguamente se usaba este enfoque.

Ejemplo 7.1: Device controller without exceptions

```java
public class DeviceController {
  ...
  public void sendShutDown() {
    DeviceHandle handle = getHandle(DEV1);
    // Check the state of the device
    if (handle != DeviceHandle.INVALID) {
      // Save the device status to the record field
      retrieveDeviceRecord(handle);
      // If not suspended, shut down
      if (record.getStatus() != DEVICE_SUSPENDED) {
        pauseDevice(handle);
        clearDeviceWorkQueue(handle);
        closeDevice(handle);
      } else {
        logger.log("Device suspended.  Unable to shut down");
      }
    } else {
      logger.log("Invalid handle for: " + DEV1.toString());
    }
  }
  ... }
```

Ejemplo 7.2: Device controller with exceptions. El código es mucho mas limpio, el algoritmo para apagar el dispositivo y el control de errores se encuentran separados.

```java
public class DeviceController {
  ...
  public void sendShutDown() {
    try {
      tryToShutDown();
    } catch (DeviceShutDownError e) {
      logger.log(e);
    }
  }

  private void tryToShutDown() throws DeviceShutDownError {
    DeviceHandle handle = getHandle(DEV1);
    DeviceRecord record = retrieveDeviceRecord(handle);
    pauseDevice(handle);
    clearDeviceWorkQueue(handle);
    closeDevice(handle);
  }

  private DeviceHandle getHandle(DeviceID id) {
    ...
    throw new DeviceShutDownError("Invalid handle for: " + id.toString());
    ...
  }
  ...
}
```

### Crear primero la instrucción try-catch-finally

De esta manera se define que debe esperar el usuario del código, independientemente de que se produzca un error en el código ejecutado en la clausula `try`

Por ejemplo, este código accede a un archivo y lee los objetos; si primero se escribe la prueba unitaria:

```java
@Test(expected = StorageException.class)
public void retrieveSectionShouldThrowOnInvalidFileName() {
  sectionStore.retrieveSection("invalid - file");
}
```

Ese test nos lleva a escribir el código:

```java
public List<RecordedGrip> retrieveSection(String sectionName) {
  // dummy return until we have a real implementation
  return new ArrayList<RecordedGrip>();
}
```

la prueba falla porque no genera alguna excepción, se cambia la implementación para que intente acceder a un archivo no valido y asi generar la excepción

```java
public List<RecordedGrip> retrieveSection(String sectionName) {
  try {
    FileInputStream stream = new FileInputStream(sectionName)
  } catch (Exception e) {
    throw new StorageException("retrieval error", e);
  }
  return new ArrayList<RecordedGrip>();
}
```

La prueba es correcta ahora porque captura la excepción. ahora se puede personalizar la excepción para que coincida con el tipo generado desde el constructor `FileInputStream`, y prodía ser una excepcion `FileNotFoundException`.

```java
public List<RecordedGrip> retrieveSection(String sectionName) {
  try {
    FileInputStream stream = new FileInputStream(sectionName);
    stream.close();
  } catch (FileNotFoundException e) {
    throw new StorageException("retrieval error", e);
  }
  return new ArrayList<RecordedGrip>();
}
```

Una vez definido el ambito con la estructura `try-catch` se puede usar TDD para diseñar el resto de la lógica, es decir es bueno crear pruebas que fuercen las excepciones.

- ### Usar excepciones no comprobadas

  Esta regla se da ya que si se genera una excepcion comprobada desde un metodo y la clausula `catch` se encuentra tres niveles por debajo, se debe declarar dicha excepción en la firma de todos los métodos compredidos entre su posición y el `catch`

- ### Ofrecer contexto junto a las excepciones

  En los errores incluir información que nos dé contexto de dónde se ha producido el fallo.

- ### Definir clases de Excepción de acuerdo a las necesidades del invocador.
  Al definir clases de excepción en una aplicación debemos hacerlo enfocándonos en como se capturan. No tanto por su origen (si provienen de uno u otro componente) o por su tipo (fallos de dispositivo, de la red, errores de programción) de excepción.

Ejemplo: esta clasificaión de excepciones abarca todas las excepciones que las invocaciones pueden generar

```java
ACMEPort port = new ACMEPort(12);
try {
    port.open();
  } catch (DeviceResponseException e) {
    reportPortError(e);
    logger.log("Device response exception", e);
  } catch (ATM1212UnlockedException e) {
    reportPortError(e);
    logger.log("Unlock exception", e);
  } catch (GMXError e) {
    reportPortError(e);
    logger.log("Device response exception");
  } finally {
    …
}
```

Independientemente de la excepción, se puede simplificar el código y asegurar de que se devuelva un tipo de excepción común.

```java
LocalPort port = new LocalPort(12);
try {
  port.open();
} catch (PortDeviceFailure e) {
  reportError(e);
  logger.log(e.getMessage(), e);
} finally {
  …
}
```

La clase `LocalPort` es un simple envoltorio que contiene las excepciones generadas por la clase `ACMEPort`

```java
public class LocalPort {
  private ACMEPort innerPort;

  public LocalPort(int portNumber) {
    innerPort = new ACMEPort(portNumber);
  }

  public void open() {
    try {
      innerPort.open();
    } catch (DeviceResponseException e) {
      throw new PortDeviceFailure(e);
    } catch (ATM1212UnlockedException e) {
      throw new PortDeviceFailure(e);
    } catch (GMXError e) {
      throw new PortDeviceFailure(e);
    }
  }
  …
}
```

Del ejemplo anterior solo se ha definido un solo tipo de excepción para el fallo de puertos, A menudo una clase de excepciones es suficiente para una zona concreta de código.

### No retornar Null

Al retornar null se generan problemas para los invocadores, solo basta que falle una comprobación de null para que una aplicaión pierda el control.

Por ejemplo: Si `persistenceStore` fuera null se genera una excepción de nivel superior

```java
public void registerItem(Item item) {
  if (item != null) {
    ItemRegistry registry = persistentStore.getItemRegistry();
    if (registry != null) {
      Item existing = registry.getItem(item.getID());
      if (existing.getBillingPeriod().hasRetailOwner()) {
        existing.register(item);
      }
    }
  }
}

```

En lugar de devolver null desde un metodo, generar una excepción o devolver un objeto especial;

por ejemplo:

```java
List<Employee> employees = getEmployees();
if(employees != null){
  for(Employee e : employees){
    totalPay += e.getPay();
  }
}
```

Ahora `getEmployees` puede devolver `null`, pero en lugar de ello hacemos que devuelva una lista vacía:

```java
List<Employee> employees = getEmployees();
for(Employee e : employees){
  totalPay += e.getPay();
}

```

para eso en java se dispone de `Collections.emptyList()` que retorna una lista inmutable predefinida.

```java
public List<Employee> getEmployees() {
 if( .. there are no employees .. )
 return Collections.emptyList();
}

```

### No pasar Null

Tampoco se debe pasar null como parámetro, a no ser que una librería de terceros espere un null. Al no haber una forma racional de controlar null para parámetros, evitarlo por convención es la mejor solución posible.

## Conclusión:

El control de errores se debe enfocar de forma independiente desde nuestra lógica principal. Lograr eso aumenta la capacidad de mantenimiento del codigo.
