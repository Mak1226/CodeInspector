# events:
> file received on the server -> 
> chat message received
> analyzer result received on the client
> client joined
> disconnect also
> login? 
> connect event

# to implement: 
> threading : one for listen, one for send? , one for processing?
> ser./des.
> priority q

# doubt:
> how to implement pub/sub   

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

# clientJoined event:
> client joins a server-> server receives clientName and creates a clientID and broadcasts
clientName , clientID to all the machines.