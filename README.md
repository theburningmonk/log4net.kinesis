log4net.Kinesis
===============

An actor-based log4net appender for logging to [Amazon Kinesis](http://aws.amazon.com/kinesis/).

#### Installation

Download and install the library from Nuget [here](https://www.nuget.org/packages/log4net.kinesis/).

<a href="https://www.nuget.org/packages/log4net.kinesis/"><img src="https://raw.github.com/theburningmonk/log4net.kinesis/develop/nuget/banner.png"/></a>


#### Appender Properties

<table>
	<tr>
		<td><strong>StreamName</strong></td>
		<td>The name of the Amazon Kinesis stream to log entries into.</td>
	</tr>
	<tr>
		<td><strong>LevelOfConcurrency</strong></td>
		<td>the max number of concurrent `PutRecord` requests to Amazon Kinesis allowed. <strong>Default is 10</strong>.</td>
	</tr>
</table> 

#### Example log4net.config

```
<log4net>
	<appender name="KinesisAppender" type="log4net.Appender.KinesisAppender, log4net.Kinesis">
  		<streamName value="MyStream" />
  		<levelOfConcurrency value="25" />
	</appender>

	<root>
  		<level value="ALL" />
  		<appender-ref ref="KinesisAppender" />
	</root>
</log4net>
```

In addition, you should ensure that you have provided the necessary credentials for accessing your AWS account, for instance you might specify the credentials in your `app.config` file:
```
<configuration>
  <appSettings>
    <add key="AWSAccessKey" value="YOUR_ACCESS_KEY"/>
    <add key="AWSSecretKey" value="YOUR_SECRET_KEY"/>
    <add key="AWSRegion" value="us-west-2"/>
  </appSettings>
</configuration>
```
Alternatively, if you're running on an `Amazon EC2` instance with an IAM role (that has sufficient permission to access the Kinesis stream) then you won't need the above configuration.

#### Example Record Data

The data sent to `Amazon Kinesis` from the logger is a JSON string of the form:

```
{ 
	"LoggerName":"KinesisLogger",
	"Level":"INFO",
	"Timestamp":"2014-01-06 17:20:24.7571264",
	"ThreadName":"21",
	"CallerInfo":"log4net.Kinesis.ExampleCs.Program+<Loop>d__2.MoveNext(c:\Dev\Trunk\Personal\log4net.kinesis\examples\log4net.Kinesis.ExampleCs\Program.cs:31)",
	"Message":"Hello World!",
	"ExceptionMessage":"",
	"StackTrace":""
}
``` 


#### Remarks

Please report any issues in the **[issues](https://github.com/theburningmonk/log4net.kinesis/issues)** page.


To help you process the records in your Kinesis stream more easily, check out my other project **[ReactoKinesiX](https://github.com/theburningmonk/reactokinesix)**, a Rx-based .Net client library for building a record-consuming client application on top of `Amazon Kinesis`.


[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/theburningmonk/log4net.kinesis/trend.png)](https://bitdeli.com/free "Bitdeli Badge")