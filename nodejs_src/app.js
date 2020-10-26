/*
 * app.js: [WebSocket server (Node.js, ws)]
 *
 * Supported Node.js version: v4.2.6 (tested)
 * Supported npm version: 3.5.2 (tested)
 * Supported ws version: 4.1.0 (tested; https://www.npmjs.com/package/ws)
 *
 * Author: Nico Reski
 * Web: https://reski.nicoversity.com
 * Twitter: @nicoversity
 * GitHub: https://github.com/nicoversity
 * 
 * ==================================================
 *
 * # Documentation
 *
 * Conceptual communication workflow:
 * [Unity3D (websocket-sharp)] ---- [WebSocket server (Node.js, ws)] ---- [Web client (JavaScript)]
 * 
 * Client connection URLs:
 * (Note: wss:// indicates the usage of "WebSocket Secure" as protocol, specifying that all traffic uses secure communication (HTTPS).)
 * Unity3D (websocket-sharp): 	wss://MY_SERVER_URL:MY_SERVER_PORT/
 * WebClient (JavScript): 		wss://MY_SERVER_URL:MY_SERVER_PORT/
 *
 * For further information, please refer to the README.md file of this repository.
 */

// ===== NODE.JS SERVER SETUP =====
//

// require modules
const fs = require('fs');
const https = require('https');
const WebSocket = require('ws');

// setup server host name and port
const hostname = 'MY_SERVER_URL';
const port = MY_SERVER_PORT;

// setup https server with TLS (Transport Layer Security) options
// e.g., access LetsEncrypt files (see also [ https://nodejs.org/api/https.html#https_https_createserver_options_requestlistener ])
const server = https.createServer({
	key: fs.readFileSync("/path/to/privkey.pem"),
	cert: fs.readFileSync("/path/to/fullchain.pem")
});

// setup server as WebSocket server
const wss = new WebSocket.Server({ server });

// a client connected to the WebSocket server
wss.on('connection', function connection(ws) {
  
	// Event Handler: Open
	ws.on('open', function incoming(message) {
		console.log("A client has connected to the WebSocket server.");
	});

	// Event Handler: Message
	ws.on('message', function incoming(message) {
		
		// // Debug: Parse and inspect message (== string in JSON format)
		// console.log(message); 						// print to console: message as received
		// var jsonMessage = JSON.parse(message);		// message (string) parsed into JSON object
		// console.log(jsonMessage); 					// print to console: JSON object
		// console.log(jsonMessage.sender); 			// print to console: sender field of JSON object

		// // Debug: Example of composing a new JSON object
		// var jsonContent = { 'sender' : 'WebSocker Server', 'receiver' : 'everyone excl. client who triggered this event', 'valueString' : 'Some text from the WebSocker server.'};	// create a new JSON object
		// console.log(jsonContent);								// print to console: JSON object
		// console.log(jsonContent.sender);						// print to console: sender field of JSON object
		// var jsonContentString = JSON.stringify(jsonContent);	// string created from JSON object
		// console.log(jsonContentString);							// print to console: string representing the JSON object

		// // send a string back to the sender
		// ws.send(jsonContentString);

		// default behavior: simply forward / pass-through all received messages to all other clients:
		// broadcast received message to all (other) connected clients (except the sender)
		wss.clients.forEach(function each(client) {
			if (client !== ws && client.readyState === WebSocket.OPEN) {
		 		client.send(message);
			}
		});
	});
});

// start WebSocket server
server.listen(port, hostname);
