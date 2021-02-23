# Unity - Connect via WebSocket server to JavaScript client

The purpose of this project is to provide a simple, minimalistic workflow / template to illustrate communication and data transfer between a **Unity3D** client application and a **JavaScript** web client application via **WebSocket** connection implemented as a **Node.js** server.

### Table of contents
* [Features](#Features)
* [Dependencies](#Dependencies)
* [How to use](#How-to-use)
* [Further development](#Further-development)
* [License](#License)

## Features

* Unity3D and JavaScript clients: connect to a WebSocket server, send / receive text data (`string`) formatted as JSON, stringify / parse JSON objects, access JSON fields
* Node.js server: setup WebSocket Secure server, retrieve and forward / pass-through of text data (`string`) messages formatted as JSON to all connected clients (excl. original sender)
* Unity3D: helper class to conveniently handle and further process received messages via WebSocket connection, allowing for any desired (and Unity GameObject-related) action
* comprehensive documentation across all source files

## Dependencies

This project has been built using the following specifications:

* [Unity3D](https://unity3d.com) 2019.2.17f1 Personal
* [websocket-sharp](https://github.com/sta/websocket-sharp) (accessed and compiled on 2020-10-26)
* [JavaScript WebSocket WebAPI](https://developer.mozilla.org/en-US/docs/Web/API/WebSocket) (accessed on 2020-10-26)
* [Node.js](https://nodejs.org/en/) v4.2.6
* [npm](https://www.npmjs.com) 3.5.2
* [ws](https://github.com/websockets/ws) 4.1.0; [npm link](https://www.npmjs.com/package/ws)

### Additional Resources

* [JSON Serialization in Unity3D](https://docs.unity3d.com/2019.2/Documentation/Manual/JSONSerialization.html)
* [JSON Serialization in JavaScript](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON)
* [The WebSocket API (WebSockets)](https://developer.mozilla.org/en-US/docs/Web/API/WebSockets_API/)

## How to use

### WebSocket server

All source code can be found within the `nodejs_src` directory of this repository. Carefully investigate the `app.js` file to follow the implementation.

**Quick start guide**
1. Copy the contents of the `nodejs_src` directory to your server.
2. Run `npm install` in order to locally install all required dependencies as listed in `package.json`.
3. Edit `hostname`, `port`, and (if needed) the paths to your key and certificate files (for TLS support) in the `app.js` file.
4. Run `node app.js` to start the server. Note: Potentially, you need to start the server using `sudo node app.js` for permission to access the key and certificate files on your server.

### Unity3D client

All source code can be found within the `unity_src` directory of this repository. Carefully investigate the `WebSocketServerConnector.cs` file to follow the implementation.

**Quick start guide**
1. Copy the contents of the `unity_src/Assets/` directory into your Unity3D project.
2. Access, clone, build, and import [websocket-sharp](https://github.com/sta/websocket-sharp) library to your Unity3D project (see [websocket-sharp Unity3D import](#websocket-sharp-Unity3D-import)).
3. Open the `DEMO_WebSocketServerConnector.unity` scene in Unity3D.
4. Edit `WEBSOCKET_SERVER_URL` in the `WebSocketServerConnector.cs` script.
5. Run the application, and follow the example keyboard interaction: (1) Press `C` to open connection to the server; (2) Press `S` to send a default example message.
6. *Optional*: For further handling of the default example message, refer to the implementation and documentation in the `WebSocketReceivedMessageHandler.cs` class.

#### websocket-sharp Unity3D import

1. Navigate to the [websocket-sharp](https://github.com/sta/websocket-sharp) GitHub repository, and clone / download the repository.
2. Open `websocket-sharp.sln` in the repository using *Visual Studio*.
3. Remove `Example`, `Example1`, `Example2`, and `Example3` from the solution.
4. Change build option from `Debug` to `Release`.
5. Select `Build websocket-sharp` from the `Build` menu in *Visual Studio*.
6. The build should be `successful`.
7. Navigate to `websocket-sharp/bin/Release` and copy the `websocket-sharp.dll` file into the `Assets` directory in your Unity3D project. 

### JavaScript client

All source code can be found within the `js_src` directory of this repository. Carefully investigate the `index.html` file to follow the implementation.

**Quick start guide**
1. Open the `index.html` in the `js_src` directory to run the JavaScript client locally in your *web browser*. Alternatively: Copy the contents of the `js_src` directory to your server, and navigate to it accordingly.
2. In your *web browser*, preferably open the *browser's developer console* to follow implemented `console.log()` messages.
3. Press the button `Send message.` to send a default example message.

## Further development

Following, a list of known issues and some thoughts for further development:

1. Unity GameObject-related manipulations are not possible directly from within the `WebSocket.OnMessage` event in the `WebSocketServerConnector.cs` class, resulting in a `WebSocket.OnError` event. For this purpose, the `WebSocketReceivedMessageHandler.cs` class has been implemented to provide a work-around accordingly.
2. Extend the communication interface by implementing further event handlers.

## Changelog
### 2021-02-23

 * Added `WebSocketReceivedMessageHandler.cs` helper class in order to further handle received messages via WebSocketServerConnector's WebSocket.OnMessage event, allowing for any desired (and Unity GameObject-related) actions in the application.

## License
MIT License, see [LICENSE.md](LICENSE.md)
