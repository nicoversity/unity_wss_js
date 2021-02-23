/*
 * WebSocketReceivedMessageHandler.cs: [as part of Unity3D (websocket-sharp)]
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
 * Conceptual communication workflow: see WebSocketServerConnector.cs
 * 
 * This class is a helper class in order to further process received messages via WebSocketServerConnector's WebSocket.OnMessage event.
 * In particular, from within the Websocket.OnMessage event, it is possible to directly change and manipulate data structures, however the manipulation
 * of Unity GameObjects is not possible (due to how the thread handling is implemented). If one where to attempt the manipulation of a GameObject
 * from within the WebSocket.OnMessage event, it would result in a WebSocket.OnError event. Therefore, this helper class illustrates a work-around
 * for handling any messages received via WebSocketServerConnector's WebSocket.OnMessage event by following an overall conceptual approach as follows:
 * 
 * 1. The WebSocketReceivedMessageHandler.cs script is attached as a component to an active GameObject in the scene, thus running independent of the WebSocketServerConnector.cs.
 * 2. The WebSocketReceivedMessageHandler features a List of type in MessageJSONAPI.DefaultMessage in accordance to its class definition in the WebSocketServerConnector (this can be easily extended for any user purposes in the future). This List functions conceptually as a "queue" for all received and to be processed messages.
 * 3. Once a message is received via WebSocket.OnMessage event, an object is created and added to the message List in the WebSocketReceivedMessageHandler.
 * 4. Within each run of the WebSocketReceivedMessageHandler's Update() method, it checks whether or not there are messages in the List. If so, it takes the first message, allows the user to do any desired (and Unity GameObject-related) action, and deletes the message from the List (potentially allowing for the next message to be processed during the next time the Update() method gets executed).
 *
 * For further information, please refer to the README.md file of this repository.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class allowing for further processing of any received messages via WebSocket.OnMessage event implemented in the WebSocketServerConnector.cs class.
/// </summary>
public class WebSocketReceivedMessageHandler : MonoBehaviour
{
    #region PROPERTIES

    // collection of received (and to be further processed) messages
    private List<WebSocketServerConnector.MessageJSONAPI.DefaultMessage> receivedDefaultMessages;

    #endregion


    #region UNITY_EVENT_FUNCTIONS

    /// <summary>
    /// Instantiation and dynamic reference set up.
    /// </summary>
    public void Awake()
    {
        // set up properties
        receivedDefaultMessages = new List<WebSocketServerConnector.MessageJSONAPI.DefaultMessage>();
    }

    /// <summary>
    /// General update routine.
    /// </summary>
    public void Update()
    {
        // determine whether or not there are messages in the queue that need processing
        if (receivedDefaultMessages.Count > 0)
        {
            // ... and if so: perform and delete the next message in the queue
            performNextDefaultMessage();
        }
    }

    #endregion


    #region WSS_RECEIVED_MESSAGE_HANDLING

    /// <summary>
    /// Method to add a new message to the message collection list (functioning conceptually as a queue).
    /// </summary>
    /// <param name="receivedMessage">Instance of type WebSocketServerConnector.MessageJSONAPI.DefaultMessage, representing a received message that needs to be further processed within this class.</param>
    public void queueDefaultMessage(WebSocketServerConnector.MessageJSONAPI.DefaultMessage receivedMessage)
    {
        receivedDefaultMessages.Add(receivedMessage);
    }

    /// <summary>
    /// Method to act upon and process a received message of type WebSocketServerConnector.MessageJSONAPI.DefaultMessage, allowing for any desired (and Unity GameObject-related) action in the application.
    /// </summary>
    private void performNextDefaultMessage()
    {
        // retrieve first message in the list
        WebSocketServerConnector.MessageJSONAPI.DefaultMessage message = receivedDefaultMessages[0];

        // delete retrieved (first) message from the list (= queue)
        receivedDefaultMessages.RemoveAt(0);

        // LOGIC IMPLEMENTATION AND FURTHER PROCESSING BASED ON THE MESSAGE

        /*
        // access properties of WebSocketServerConnector.MessageJSONAPI.DefaultMessage
        Debug.Log(message);
        Debug.Log(message.sender);
        Debug.Log(message.receiver);
        Debug.Log(message.api);
        Debug.Log(message.valueString);
        Debug.Log(message.valueInt);
        Debug.Log(message.valueFloat);
        Debug.Log(message.valueBool);
        */

        // do something
    }

    #endregion
}
