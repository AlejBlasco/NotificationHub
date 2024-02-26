# NotificationHub
## Prototipo de sistema de envio de notificaciones
[![.NET](https://github.com/AlejBlasco/NotificationHub/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/AlejBlasco/NotificationHub/actions/workflows/dotnet.yml)

**NotificationHub** es una propuesta para generar un nuevo sistema de envío de notificaciones de una o n aplicaciones.

En este prototipo se ha abordado la idea de que este tipo de notificaciones sean Email (con STMP o autorización O365) y SMS, no obstante; ha sido pensado con la idea de poder gestionar más servicios de envío en un futuro.

## Funcionamiento básico
NotificationHub se trata de una API con dos controladores:

- **NotificationController**. Se encarga de llamar al cliente INotificationHub y dada la configuración y el tipo de ***Sender** indicado, envía el mensaje al destinatario.
- **QueueController**. Se encarga de gestionar las colas de mensajes de la aplicación. En este prototipo se ha optado por un AzureServiceBus aunque, al igual que con las notificaciones; se ha preparado para que en un futuro esto sea extensible a otros sistemas de colas (Azure Storege, RabbitMQ, etc.).

**NOTA**: Sender es el nombre que recibe cada clase encargada del envío de notificaciones con un sistema o proveedor determinado. Todos los sender deben de implementar la interface ISender y han de ser devueltos en la clase factoría para su correcto funcionamiento.

Una vez expuesta está API, se podría llegar a consumir de varias maneras posibles.

**Servicios**. La idea original que desencadenó el prototipo. Consiste en tener servicios que se distribuyan la responsabilidad, por ejemplo:
- **Service Sender**. Este servicio gestionaría las peticiones de envío de notificaciones y encolaría mensajes indicando el contenido de la notificación a enviar, su destinatario y el tipo de sender con el que se ha de hacer.
- **Service Listener**. Este servicio estaría escuchando la cola de mensajes y los procesaría llamando a la API > NotificationController con los datos obtenidos.

Otras maneras posibles de consumo del proyecto podrían ser.
- Una **implementación simple** de una aplicación que llama directamente al controlador de API > NotificationController (sin hacer uso de la cola).
- Un **usuario / dispositivo** que hace una llamada a una Azure Function que implementa la API.

![](https://raw.githubusercontent.com/AlejBlasco/NotificationHub/master/doc/Screenshots/Overview.png)

## Gestión de colas
Este prototipo esta pensado para funcionar con una cola AzureServiceBus, por lo que una vez creada; deberemos guardarnos en formato jSON la configuración de esta y añadirla a nuestros ***AppSettings**.

**NOTA**: Esto es para no hacer uso de una BBDD en el prototipo y evitar complejidad.

```json
{
  "ConnectionString": "ConnectionString",
  "QueueName": "QueueName"
}
```

Con la cola configurada la lógica es sencilla, el QueueController expone los métodos y estos mediante Mediatr y patron CQRS llaman al IQueueHandler, que; en función de su configuración usara un hanlder determinado para realizar las operaciones.

![](https://raw.githubusercontent.com/AlejBlasco/NotificationHub/master/doc/Screenshots/QueueHandler.png)

## NotificationHub
Consta de un cliente que mediante Mediatr y patrón CQRS, envía la notificación indicada, al destinatario indicado con el ***Sender** indicado.

**NOTA**: Los sender han de estar configurados y almacenada su configuración en los AppSettings al igual que la de AzureServiceBus, ya que; si bien no usamos BBDD para evitar complejidad, la aplicación valida que los senders han de tener una configuración valida para su uso (ver testing coveragge).

**Office365Configuration**
```json
{
  "AuthClientId": "AuthClientId",
  "AuthTenantId": "AuthTenantId",
  "AuthClientSecret": "AuthClientSecret",
  "AuthorityUrlBase": "AuthorityUrlBase",
  "GraphClientSecret": "GraphClientSecret",
  "GraphClientSecret": "GraphClientSecret",
  "EmailFrom": "EmailFrom"
}
```
**SmtpConfiguration**
```json
{
  "EmailFrom": "EmailFrom",
  "User": "User",
  "Password": "Password",
  "Server": "Server",
  "ServerPort": "ServerPort",
  "EnableSSL": false,
  "EnableHtmlBody": "EnableHtmlBody"
}
```
**Office365Configuration**
```json
{
  "CommunicationServiceConnectionString": "CommunicationServiceConnectionString",
  "PhoneFrom": "PhoneFrom"
}
```

![](https://raw.githubusercontent.com/AlejBlasco/NotificationHub/master/doc/Screenshots/NotificationHub.png)

**AppSettings***
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Configurations": {
    "Smtp": "{json}",
    "O365": "{json}",
    "AzureComm": "{json}",
    "AzureServiceBus": "{json}"
  }
}
```
