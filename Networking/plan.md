# TODO:
> Change all instances of *ID to *Id
> ensure message is sent bfr stop in client
> send message to all clients in server bft quitting



# events:
> file received on the server -> 
> chat message received
> analyzer result received on the client
> client joined
> client disconnected
> connect event

# to implement: 
> refactor the code
> remove warnings
> interchange payload and data names in message

# new pub sub model details:

- each module will subscribe with module Name, and a message handler function
- message will contain following fields:
	- source id
	- dest id
	- data
		- will contain event type
		- and payload to be sent
	- module Name -> will be directed to this module 
- each module will have their own event type contract
	- so if packet is directed to networking team then it will contain one of the events defined by networking team only.
- each module will give one message handler object only. Its upto the module to handle different event types within the same function itself.
- by this each module are free to have their own events
- networking team not dependent on any module


# client: 
send(data)-> data will contain the event type to trigger 

# server:
receive the packet=> extrct the event type

run publisher instance.
publisher(event_type, subscriber(method to call to))
all the modules will subscribe to the pub
moduleA
publisher.subscribe("file_recieved",functionA)

1 interface:

# Communication interface:
> send, subscribe, start, stop 

# start(server dest, clientName): -> client
> call the listen 
> start all threads
> connect to server
> receives serverID

# start(server dest=null): -> server
> call the listen 
> start all threads
> returns serverID


# send(object obj, destination/could be null,event type);
> obj=> we have to ser.
> private dm: dest=client addr.
> group msg: dest=null
> same for client and server
> give clientID

# stop() -> client
> stop all threads
> send disconnect req to server

# stop() -> server
> stop all threads

# subscribe(EventClass.getEvent1())

subscribe(EventClass event,callback)
> called by the module

fle_rec


# EVENTS:

#connect event:
> when client initiates tcp connection with the server
> will be used in start() method
> after tcp connection establised
> create clientID
> will be used in start() method

# client connected event:
> after client has been registered
> server broadcasts newly joined client to all the clients
> sends clientID

# client disconnected event:
> when clients wants to exit the session
> will be used in stop function
> server broadcasts clientID disconnected to all clients


# event class:

class Event{

public static string event1(){
return "event1"
}
}
Event.event1()


# below is event handler implementatoin for dashboard

class EventHandlerImpl: IEventHandler{

Dashboard object = new D.....
public void ChatEvent(object)
{
// do something in chat event

}

}


receiving queue:
packet 1 -> does it contains info for the destination module?


subscribe(EventHandlerImpl, moduleName)

send(object, event, dest)

map<moduleName, IEventHandler>  m

receiving queue:
packet1:
packet1.eventtype -> message received
iterate through m
//call the respective function for ith module
m[i].messageReceived(data)