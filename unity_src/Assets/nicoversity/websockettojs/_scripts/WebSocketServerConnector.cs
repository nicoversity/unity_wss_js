/*
 * WebSocketServerConnector.cs: [Unity3D (websocket-sharp)]
 *
 * Supported Unity version: 2019.2.17f1 Personal(tested)

 * Third-party assets:
 * - websocket-sharp: https://github.com/sta/websocket-sharp
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
 * Server connection URLs:
 * (Note: wss:// indicates the usage of "WebSocket Secure" as protocol, specifying that all traffic uses secure communication (HTTPS).)
 * Unity3D(websocket-sharp): wss://MY_SERVER_URL:MY_SERVER_PORT/
 *
 * For further information, please refer to the README.md file of this repository.
 * 
 * === VERSION HISTORY | FEATURE CHANGE LOG ===
 * 2020-10-26: Original GitHub release.
 * 2021-02-23: Added WebSocketReceivedMessageHandler helper class in order to further handle received messages via WebSocketServerConnector's WebSocket.OnMessage event, allowing for any desired (and Unity GameObject-related) actions in the application.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

/// <summary>
/// Class illustrating communication to a WebSocket server from within Unity, using the WebSocket-Sharp library.
/// </summary>
public class WebSocketServerConnector : MonoBehaviour {

    #region INTERNAL_CLASSES

    /// <summary>
    /// Internal class to define / collect classes representing individual JSON Message classes for a structured and logical communication with the WebSocket server.
    /// </summary>
    public class MessageJSONAPI
    {
        /// <summary>
        /// Class representing a default message for communication with the server (and other clients).
        /// </summary>
        public class DefaultMessage
        {
            // Properties (== Keys in the parsed JSON).
            public string sender;
            public string receiver;
            public string api;
            public string valueString;
            public int valueInt;
            public float valueFloat;
            public bool valueBool;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="sender">String representing information about the sender.</param>
            /// <param name="receiver">String representing information about the receiver.</param>
            /// <param name="api">String representing the api endpoint (tbd by the user).</param>
            /// <param name="valueString">String value representing some information in string format.</param>
            /// <param name="valueInt">Int value representing some information in int format.</param>
            /// <param name="valueFloat">Float value representing some information in float format.</param>
            /// <param name="valueBool">Bool value representing some information in bool format.</param>
            public DefaultMessage(string sender, string receiver, string api, string valueString, int valueInt, float valueFloat, bool valueBool)
            {
                this.sender = sender;
                this.receiver = receiver;
                this.api = api;
                this.valueString = valueString;
                this.valueInt = valueInt;
                this.valueFloat = valueFloat;
                this.valueBool = valueBool;
            }
        }

        // Further templates of JSON object messages for structured communication with the WebSocket server's logic go here.
    }

    #endregion


    #region PROPERTIES

    // reference to the WebSocket instance, responsible for establishing the WebSocketSecure (WSS) connection to the WebSocket server
    private WebSocket websocket;

    // configuration of the WebSocket server
    private static readonly string WEBSOCKET_SERVER_URL = "wss://MY_SERVER_URL:MY_SERVER_PORT/";

    // reference to the WebSocketReceivedMessageHandler helper instance (see its documentation for further details)
    public WebSocketReceivedMessageHandler webSocketReceivedMessageHandler;     // assigned via Unity Inspector

    #endregion


    #region WEBSOCKET_MESSAGE_API

    // Unity3D (out) -> WebSocket server (inc/out) -> WebClient (inc)
    public const string UNITYsendMessageToJAVASCRIPT = "unity_to_js_defaultmessage";

    // WebClient (out) -> WebSocketServer (inc/out) -> Unity3D (inc)
    public const string JAVASCRIPTsendMessageToUNITY = "js_to_unity_defaultmessage";

    #endregion


    #region UNITY_EVENT_FUNCTIONS

    /// <summary>
    /// Instantiation and dynamic reference set up.
    /// </summary>
    public void Awake()
    {
        // check if helper instance is set up
        if (webSocketReceivedMessageHandler == null) Debug.LogError("[WebSocketServerConnector] WebSocketReceivedMessageHandler is not assigned.");

        // instantiate new WebSocket object using the specified server url
        websocket = new WebSocket(WEBSOCKET_SERVER_URL);

        // configure WebSocket event listeners if instantiation was successful
        if (websocket != null)
        {
            // Event Handler: OnOpen
            websocket.OnOpen += (sender, e) =>
            {
                Debug.Log("[WebSocketServerConnector] Connection established to WSS Server.");
            };

            // Event Handler: OnClose
            websocket.OnClose += (sender, e) =>
            {
                Debug.LogWarning("[WebSocketServerConnector] OnClose event received.");
                websocket.Close();
            };

            // Event Handler: OnClose
            websocket.OnError += (sender, e) =>
            {
                Debug.LogError("[WebSocketServerConnector] OnError event received.");
            };

            // Event Handler: OnMessage
            websocket.OnMessage += (sender, e) =>
            {
                // determine type of message event

                // text (== string) data received
                if (e.IsText)
                {
                    Debug.Log("[WebSocketServerConnector] OnMessage received with TEXT data: " + e.Data);

                    // parse received message formatted in JSON, and create an object accordingly
                    MessageJSONAPI.DefaultMessage receivedJsonMessage = JsonUtility.FromJson<MessageJSONAPI.DefaultMessage>(e.Data);

                    // access / use parsed JSON data
                    Debug.Log(receivedJsonMessage);
                    Debug.Log(receivedJsonMessage.sender);
                    Debug.Log(receivedJsonMessage.receiver);
                    Debug.Log(receivedJsonMessage.api);
                    Debug.Log(receivedJsonMessage.valueString);
                    Debug.Log(receivedJsonMessage.valueInt);
                    Debug.Log(receivedJsonMessage.valueFloat);
                    Debug.Log(receivedJsonMessage.valueBool);

                    // determine what to do next based on determined API field
                    switch (receivedJsonMessage.api)
                    {
                        case JAVASCRIPTsendMessageToUNITY:
                            Debug.Log("Message received from JavaScript client.");
                            // do something

                            // === update from 2021-02-23 ===
                            // Generally, data structure manipulation is possible directly from within this event, but not Unity GameObject-related manipulations.
                            // As a work-around, the WebSocketReceivedMessageHandler class was created (see its documentation for further information).

                            // hand over received message to WebSocketReceivedMessageHandler, responsible for further handling of any desired (and Unity GameObject-related) actions in the application
                            webSocketReceivedMessageHandler.queueDefaultMessage(receivedJsonMessage);
                            break;

                        default:
                            break;
                    }
                }

                // binary data received
                else if (e.IsBinary)
                {
                    Debug.Log("[WebSocketServerConnector] OnMessage received with BINARY raw data: " + e.RawData);
                }

                // ping received
                else if (e.IsPing)
                {
                    Debug.Log("[WebSocketServerConnector] OnMessage ping received from server.");
                }

                // data could not be determined
                else
                {
                    Debug.Log("[WebSocketServerConnector] Message received with UNDETERMINED data: " + e.Data);
                }
            };
        }
    }

    /// <summary>
    /// General update routine.
    /// </summary>
    public void Update()
    {
        // KEYBOARD INTERACTION for demonstration purposes
        //

        // C - connect to WebSocket Server
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("[WebSocketServerConnector] C pressed");

            if (websocket != null)
            {
                Debug.Log("[WebSocketServerConnector] Initiate connection to configured WebSocket server.");
                websocket.Connect();
            }
        }

        // S - send message from Unity to JavaScript via WebSocket Server
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("[WebSocketServerConnector] S pressed");

            if (websocket != null)
            {
                Debug.Log("[WebSocketServerConnector] Send a DefaultMessage to the WebSocket server.");

                // construct an example DefaultMessage
                MessageJSONAPI.DefaultMessage message = new MessageJSONAPI.DefaultMessage("Unity3D client", "JavaScript client", UNITYsendMessageToJAVASCRIPT, "An example string from Unity to JavaScript via Node.js.", 42, 23.7f, true);

                // create JSON string based on message object's properties
                string jsonStringMessage = JsonUtility.ToJson(message);

                // send string to WebSocket server
                websocket.Send(jsonStringMessage);
            }
        }
    }

    #endregion
}