# Twitter-Clone
使用F#模拟Twitter的简单功能

The goal of this project is to implement a Twitter Clone and a client tester/simulator. The problem statement is to implement an engine that can be paired up with WebSockets to provide full functionality. The client part (send/receive tweets) and the engine (distribute tweets) were simulated in separate OS processes.

Authors

Yu, Mingjun (UFID 6170 7843)

Sun, Hui (UFID 6654 2614)

-------------------------------------------------------
 COP5615 : DISTRIBUTED SYSTEMS - TWITTER CLONE 
-------------------------------------------------------
The goal of this project is to:
 - Implement a Twitter like engine with the following functionality:
	* Register account
	* Send tweet. Tweets can have hashtags (e.g. #COP5615isgreat) and mentions (@bestuser)
	* Subscribe to user's tweets
	* Re-tweets (so that your subscribers get an interesting tweet you got by other means)
	* Allow querying tweets subscribed to, tweets with specific hashtags, tweets in which the user is mentioned (my mentions)
	* If the user is connected, deliver the above types of tweets live (without querying)
 - Implement a tester/simulator to test the above
	* Simulate as many users as possible
	* Simulate periods of live connection and disconnection for users
	* Simulate a Zipf distribution on the number of subscribers. For accounts with a lot of subscribers, increase the number of tweets. Make some of these messages re-tweets
 - Measure various aspects of the simulator and report performance 


INSTRUCTIONS FOR EXECUTION 
------------------
 * Introduction
 * Pre-requisites
 * Client Simulator Program inputs
 * Running project4.tgz


INTRODUCTION
------------
The project folder contains:
 - Project4Client(Client.fs,Message.fs,Program.fs)
 - Project4Server(Server.fs,Message.fs,Program.fs)

PRE-REQUISITES
--------------
The following need to be installed to run the project:
 - F#
 - Akka
 - Visual Studio 2019
Two Visual Studio windows are required to execute Twitter server and clients simulator separately.

CLIENTS SIMULATOR PROGRAM INPUTS
--------------------------------
 - numClients: 		the number of clients to simulate
 - maxSubscribers: 	the maximum number of subscribers a Twitter account can have in the simulation (must be < numClients)
 - disconnectClients: 	the percentage of clients to disconnect to simulate periods of live connection and disconnection
