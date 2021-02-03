# CLEAN CODE: CAPÍTULO 11 - SISTEMAS

Cómo contruir una ciudad?

Las ciudades son demasiado grandes para ser administradas por una sola persona, para ello tenemos varios equipos que se encargan del transporte, alcantarillado, alumbrado, etc. En el software los equipos también suelen organizarse de esta forma, pero los sistemas no suelen contar con la misma separación de aspectos y niveles de abstracción.

## 👉 Separar la contrucción de un sistema de, su uso

No es lo mismo la construcción que el uso que se le va a dar al sistema, son procesos totalmente diferentes, cuando construyen un hotel tenemos grúas y obreros, cuando éste ha acabado no habrá grúas y los trabajadores serán diferentes. Los sistemas de software deben separar el proceso de inicio, de la lógica de ejecución que toma la posta tras el inicio.

Formas para separar la construcción del uso:

- **Separación de Main:**
  Consiste en trasladar todos los aspectos de la construcción a `main` o módulos invocados por `main`.
  La función `main` crea los objetos necesarios para el sistema, los pasa a la aplicación y ésta los utiliza.

- **Factorías:**
  En ocasiones, la aplicación tendrá que ser responsable de la creación de algún objeto, en este caso la aplicación controla cuando crear el objeto, pero manteniendo los detalles de dicha construcción separados del código de la aplicación.

- **Inyectar Dependencias:**
  Un objeto no debe ser responsable de instanciar dependencias, sino que debe delegar ese mecanismo en otro autorizado, de modo que se invierte el control. Este mecanismo autorizado suele ser `main` o un contenedor de propósito especial.

## 👉 Escalar

En el software no podemos conseguir sistemas perfectos a la primera, eso es un mito, el libro hace una analogía entre la construcción de un sistema y la construcción de una ciudad. Nadie puede saber y anticipar su crecimiento, por eso la regla es que debemos implementar hoy, y refactorizar y ampliar mañana. "Los sistemas de software son únicos si los comparamos con los sistemas físicos. Sus arquitecturas pueden crecer incrementalmente, si mantenemos la correcta separación de los aspectos."

- **Aspectos Transversales**

  En la infraestructura de una aplicación hay aspectos transversales como persistencia, transacciones, seguridad, almacenamiento en caché, recuperación ante fallos, etc.

  Por lo tanto es difícil crecer de sistemas simples a sistemas complejos debido a las dependencias usadas en la arquitectura. Para evitar estos problemas, se deben separar las distintos responsabilidades de un sistema (AOP Aspect oriented programing).

  Para lograr esta separación en JAVA se pueden usar tres mecanismos: Proxies, frameworks AOP Java puros y aspectos AspectJ.

## Proxies Java

Son útiles en casos sencillos, como envolver invocaciones de métodos en objetos o clases concretas.
En en ejemplo muestra un proxy JDK para ofrecer asistencia de persistencia a una aplicación `Bank`, y solo abarca los métodos para estblecer y obtener la lista de cuentas:

```java
// Bank.java (suppressing package names...)
import java.utils.*;

// The abstraction of a bank.
public interface Bank {
  Collection<Account> getAccounts();
  void setAccounts(Collection<Account> accounts);
}

// BankImpl.java
import java.utils.*;

// The “Plain Old Java Object” (POJO) implementing the abstraction.
public class BankImpl implements Bank {
  private List<Account> accounts;
  public Collection<Account> getAccounts() {
    return accounts;
  }
  public void setAccounts(Collection<Account> accounts) {
    this.accounts = new ArrayList<Account>();
    for (Account account: accounts) {
      this.accounts.add(account);
    }
  }
}

// BankProxyHandler.java
import java.lang.reflect.*;
import java.util.*;

// “InvocationHandler” required by the proxy API.
public class BankProxyHandler implements InvocationHandler {
  private Bank bank;

  public BankHandler (Bank bank) {
    this.bank = bank;
  }
  // Method defined in InvocationHandler
  public Object invoke(Object proxy, Method method, Object[] args)
  throws Throwable {
    String methodName = method.getName();
    if (methodName.equals("getAccounts")) {
      bank.setAccounts(getAccountsFromDatabase());
      return bank.getAccounts();
    } else if (methodName.equals("setAccounts")) {
      bank.setAccounts((Collection<Account>) args[0]);
      setAccountsToDatabase(bank.getAccounts());
      return null;
    } else {
      ...
    }
  }

  // Lots of details here:
  protected Collection<Account> getAccountsFromDatabase() { ... }
  protected void setAccountsToDatabase(Collection<Account> accounts) { ... } }

// Somewhere else...

Bank bank = (Bank) Proxy.newProxyInstance(
  Bank.class.getClassLoader(),
  new Class[] { Bank.class },
  new BankProxyHandler(new BankImpl()));
```

El código es abundante y complejo para algo sencillo, esto es el incoveniente de los proxies, dificultan el uso de un códgio limpio y va en contra de la AOP.

## Frameworks AOP Java puros

Son libreria de java que implementan AOP como Spring y JBoos, que crean la logica empresarial en forma de POJO, son mas sencillos y mas facile de probar y garantiza que se implemente correctamente las historias y su mantenimiento y evolución de código en historias futuras.

Todos los aspectos tranversales vienen incorporados por medio de archivos de configuración.

Ejemplo: Aqui se muestra un fragmento de un archvo de configuración de Spring

```xml
<beans>
  ...
  <bean
    id="appDataSource"
    class="org.apache.commons.dbcp.BasicDataSource"
    destroy-method="close"
    p:driverClassName="com.mysql.jdbc.Driver"
    p:url="jdbc:mysql://localhost:3306/mydb"
    p:username="me"
  />

  <bean
    id="bankDataAccessObject"
    class="com.example.banking.persistence.BankDataAccessObject"
    p:dataSource-ref="appDataSource"
  />

  <bean
    id="bank"
    class="com.example.banking.model.Bank"
    p:dataAccessObject-ref="bankDataAccessObject"
  />
  ...
</beans>
```

El codigo resultante es mas limpio que en EJB2

```java
package com.example.banking.model;
import javax.persistence.*;
import java.util.ArrayList;
import java.util.Collection;

@Entity
@Table(name = "BANKS")
public class Bank implements java.io.Serializable {
  @Id @GeneratedValue(strategy=GenerationType.AUTO)
  private int id;

  @Embeddable // An object “inlined” in Bank’s DB row
  public class Address {
    protected String streetAddr1;
    protected String streetAddr2;
    protected String city;
    protected String state;
    protected String zipCode;
    }

  @Embedded
  private Address address;

  @OneToMany(cascade = CascadeType.ALL, fetch = FetchType.EAGER,
            mappedBy="bank")
  private Collection<Account> accounts = new ArrayList<Account>();

  public int getId() {
    return id;
  }

  public void setId(int id) {
    this.id = id;
  }

  public void addAccount(Account account) {
    account.setBank(this);
    accounts.add(account);
  }

  public Collection<Account> getAccounts() {
    return accounts;
  }
```

## AspectJ

Es la herramienta mas completa para implementar AOP, es una extension que Java ofrece, el inconveniente es la necesidad de adoptar nuevas herramientas y aprender nuevas construcciones del lenguaje. No se toca esta herramienta poque el analisis completo de Aspect supera los objetivos del libro.

## 👉 Control de Pruebas en la Aqruitectura del sistema

La separación a través de enfoques similares a aspectos no se puede menospreciar. Si puede crear la lógica de dominios de su aplicación mediante POJO, sin conexión con los aspectos arquitectónicos a nivel del código, entonces se podrá probar realmente la arquitectura. De esta forma se podrá evolucionar y no habrá que crear un buen diseño por adelantado (BDUF Big Design Up Front). Aunque el software se rige por una física propia, es económicamente factible realizar cambios radicales si la estructura del software separa sus aspectos de forma eficaz. Para recapitular:

"Una arquitectura de sistema óptima se compone de dominios de aspectos modularizados cada uno implementado con POJO. Los distintos dominios se integran mediante aspectos o herramientas similares mínimamente invasivas. Al igual que en el código, en esta arquitectura se pueden realizar pruebas."

## 👉 Optimizar la toma de Desiciones

La modularidad y separación de aspectos permite la descentralización de la administración y la toma de decisiones. En un sistema suficientemente amplio, no debe existir una sola persona que tome todas las decisiones.

## 👉 Usar Standares cuando añadan un valor demostrable

Otro aspecto a tener en cuenta, es que debe usar estándares solo cuando añadan valor demostrable. Muchos equipos usaron la arquitectura EJB2 por ser un estándar, y se olvidaron de implementar el valor para sus clientes.

## 👉 Los Sistemas Necesitan lenguajes específicos del Dominio.

También debe usar lenguajes específicos del dominio. Los lenguajes específicos del dominio (DSL Domain Specific Languages) permiten expresar como POJO todos los niveles de abstracción y todos los dominios de la aplicación, desde directivas de nivel superior a los detalles más mínimos.

## Conclusión

Los sistemas también deben ser limpios, en todos los niveles de abstracción, los objetivos deben ser claros. Esto solo sucede si crea POJO y usa mecanismos similares a aspectos para incorporar otros aspectos de implementación de forma no invasiva. Independientemente de que diseñe sistemas o módulos individuales, no olvide usar los elementos más sencillos que funcionen.
