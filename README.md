# ![](https://raw.githubusercontent.com/neutmute/nlog-targets-loggly/master/SolutionItems/NLoggly.png) nlog-targets-loggly #
An NLog Target For [Loggly](http://www.loggly.com). 

[![nlog-targets-loggly MyGet Build Status](https://www.myget.org/BuildSource/Badge/nlog-targets-loggly?identifier=01f46438-a7ab-49c7-ba53-5195726e0ec0)](https://www.myget.org/) [![NuGet](https://img.shields.io/nuget/v/NLog.Targets.Loggly.svg)](https://www.nuget.org/packages/NLog.Targets.Loggly) 

Install via [nuget](https://www.nuget.org/packages/NLog.Targets.Loggly/) with

	Install-Package nlog.targets.loggly

See the Demo project in the solution for a working example. 
Be sure to create your own config file which is not included in the repo. Try something like this:
	
	C:\nlog-targets-loggly> copy .\examples\Demo\example.loggly.user.config .\examples\Demo\loggly.user.config

## Example Config ##
This NLog target project reads the [loggly-csharp configuration](https://github.com/neutmute/loggly-csharp/), so be sure to add the Loggly config section as well as NLog config. 

See below for sample NLog config (loggly config not shown).

```xml
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"  throwConfigExceptions="true">
	<extensions>
		<add assembly="NLog.Targets.Loggly" />
	</extensions>
	<targets async="true">
		<target name="logconsole" xsi:type="ColoredConsole" />
		<target name="Loggly" xsi:type="Loggly" layout="${message}" />
	</targets>
	<rules>
		<logger name="*" minlevel="Info" writeTo="logconsole,Loggly" />
	</rules>
</nlog>
```

### Loggly EndPoint Config
Loggly Config can be configured through the NLog Target properties:

```xml
<target name="Loggly" xsi:type="Loggly" layout="${message}" applicationName="MyApp" customerToken="your token" endpointHostname="logs-01.loggly.com" endpointPort="443" logTransport="https">
</target>
```

The following settings are supported:

- _CustomerToken_ - Required value, unless configured elsewhere
- _LogTransport_ - Loggly EndPoint Protocol (Https, SyslogUdp, SyslogTcp, SyslogSecure). Default = Https
- _EndpointHostname_ - Loggly EndPoint HostName. Default = logs-01.loggly.com
- _EndpointPort_ - Loggly EndPoint Port Number. Default = Value matching LogTransport
- _ApplicationName_ - Application Identifier. Default = AppDomain FriendlyName
- _ForwardedForIp_ - Include HTTP Header X-Forwarded-For. Default = Not set

### Batching Policy
- _batchSize_ - Number of LogEvents to send in a single batch (Default=10)
- _taskDelayMilliseconds_ - Artificial delay before sending to optimize for batching (Default=200 ms)
- _queueLimit_ - Number of pending LogEvents to have in memory queue, that are waiting to be sent (Default=10000)
- _overflowAction_ - Action to take when reaching limit of in memory queue (Default=Discard)

### Tags

Loggly Tags can be added like this:

```xml
<target name="Loggly" xsi:type="Loggly" layout="${message}">
	<tag name="${exception:format=type}" />
</target>
```

### MetaData

Loggly includes NLog LogEvent Properties automatically, but one can also add extra context like this:

```xml
<target name="Loggly" xsi:type="Loggly" layout="${message}">
	<contextproperty name="HostName" layout="${machinename}" />
	<contextproperty name="ThreadId" layout="${threadid}" />
</target>
```

### MappedDiagnosticsLogicalContext (MDLC)

Loggly can also include async context from NLog MDLC (Contains state from NetCore `ILogger.BeginScope()`)

```xml
<target name="Loggly" xsi:type="Loggly" layout="${message}" includeMdlc="true" />
```

### Suppression
Sometimes you might emit something to a flat file log that doesn't make sense in loggly, such as a delimiting line of dashes: ---------

Add a property to your nLog event with the name `syslog-suppress` to filter these out so they don't transmit to loggly.
