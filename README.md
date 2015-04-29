#damascus

Code named after Damascus, Siria, important trade center in the past. 

Damascus will be your friend, when setting up Voice and SMS workflows. 

 - Say you want to setup and automated voice system for your company. 
 - Say you want to implement complicated marketing logics, thru SMS. 
 - Say you want to go thru a set of steps for a user using callback urls.

Damascus it's designed thinking into Workflows and Steps, each Step having names for which they will be called, and every input from the outside is passed right into the step, for the step to make decissions.

###Workflows:

	[WorkflowName("hello_world")]
    public class HelloWorldWorkflow : Damascus.Core.Workflow
    {
        public HelloWorldWorkflow()
        {
            this.Steps = new Dictionary<string, IStep>()
            {
                {"hello", new FunctionStep(Hello)},
                {"world", new FunctionStep(World)},
            };
        }
        
        private string Hello(IStepInput input)
        {
        	//Do some stuff with the input
        	return "Hello";
        }
        
        private string World(IStepInput input)
        {
        	//Do some stuff with the input
        	return "World";
        }
    }
    
The following urls will make the workflow execute each of the steps:

`http://www.voiceflows.com/workflow/call?type=hello_world&step=hello`


`http://www.voiceflows.com/workflow/call?type=hello_world&step=world`		

The reason why 'call' or 'sms' is specified in the request is because there are ussually different ways to gather the parameters depending on wether the input comes from SMS or Phone.

It's responsability of the workflow to link steps from one to other, Damascus doesn't impose rules on what step should be executed or any error handling whatsoever. 
Steps and Workflows are responsible


### Workflow Data 

It's usual that workflows want to store data and retrieve it later in different steps, one easy example would be a workflow that gathers user information on the first step and tries to calculate in subsequent steps based on that information.

For such situations Damascus implement a mechanism for storing and retrieve data that is standard for every workflow. Every time a workflow executes an step, a key is generated that is unique for that Workflow and some of the outside input, like the phone number. Using that unique key, workflows can obtain data specific for each of the steps, and maintain it over the full workflow execution, for a given user.

In order to store information Workflows are provided from a "Data", dictionary where keys are stored and later accessed. When inside the context of a Workflow, you can access that Dictionary, and store or retrieve data.

	private string OneStepOnly(IStepInput input){
    	if (Data["hello"] == null){
    		return "world";
    	}
    	else{
    		return "world"
    	}
	}


In order to store Data Damascus is extensible using DataSerializers, currently two different DataSerializers have been implemented. RedisSerializer and MemorySerializer. But any inheritor of DataSerializer, can be plugged in and used. MySql, SQLServer, MongoDB they can all be implemented and used.

The following explains the class definition for DataSerializer

	public interface IDataSerializer
    {
        void SerializeData(string key, DataStorage data);

        DataStorage DeserializeData(string key);
    }


###Message Dispatchers

Damascus is implemented using the NServiceBus framework and it tries to implement the CQRS pattern [http://en.wikipedia.org/wiki/Command-query_separation](http://en.wikipedia.org/wiki/Command-query_separation)

On that end, Damascus relied on NServiceBus to distribute messages to subscribers. In this case any communication to the outside, and generally any time consuming operation would be dispatched as an event, and there will be Services in charge of processing those messages. These Services are 

###Examples

Sending an SMS to a phone number

	private string HelloWorldStep(IStepInput input){
    	Bus.Send(new CreateSmsMessage(){
			PhoneNumber = '7865433131',
            Message = "hello world"
            Id = Guid.NewGuid().ToString()
        });
    }


Sending an API Call to a Rest API

	private string HelloWorldStep(IStepInput input){
    	Bus.Send(new ServiceCallMessage(){
            Url ="http://example.com/api/hello",
            Method = "POST",
            Headers = new Dictionary<string, string>(),
            Payload = new Dictionary<string, string>(){
                {"hello", "world"},
            }
        });
    }
    
    
In order for these messages to be processed, NServiceBus will have a configuration of it's own, please refer to: http://particular.net/ for follow ups.

###Dependency Injection

Damascus implement dependency injection thru CastleWindsor in order to manage dependencies, make sure you have all your dependencies filled up, before running the project. Both Damascus.Web and Damascus.Channel have bootstrapp files that manages the injection of objects. 

In both Web.config and app.config a few settings have to be provided too: 
	
	<add key="TwillioAccountSid" value="ACc..........." />
	<add key="TwillioAuthToken" value="23..................." />
	<add key="TwillioSmsOutPhone" value="786-222-2222" />
	<add key="TwillioCallPhone" value="786-222-2222" />
	<add key="TwillioBaseUrl" value="http://example.com/workflow" />



#contact
Any question or specific request, please contact the owners via Github, we are actively working on this project and we're also working into using it as our communications provider for http://imeet.io


