using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Modified notification center that uses event names
/// Created By: NeilDG
/// </summary>
public class EventBroadcaster {

    /// <summary>
    /// Static singleton instance
    /// </summary>
	private static EventBroadcaster sharedInstance;

    /// <summary>
    /// Dictionary of observers and their associated notification code.
    /// </summary>
	private Dictionary<string, ObserverList> eventObservers;

    /// <summary>
    /// Property used to access the singleton.
    /// </summary>
	public static EventBroadcaster Instance {
		get {
			if(sharedInstance == null) {
				sharedInstance = new EventBroadcaster();
			}

			return sharedInstance;
		}
	}

    /// <summary>
    /// Constructor
    /// </summary>
	private EventBroadcaster() {
		this.eventObservers = new Dictionary<string, ObserverList>();
	}

    /// <summary>
    /// Gives a summary of all the observers and how many there are registered in its dictionary.
    /// </summary>
	public void PrintObservers() {

		int totalEvents = 0;

		foreach(ObserverList observer in this.eventObservers.Values) {
			totalEvents += observer.GetListenerLength();
		}

		Debug.LogWarning("TOTAL OBSERVER LENGTH: " +totalEvents);

		foreach(KeyValuePair<string, ObserverList> keyValue in this.eventObservers) {
			Debug.LogWarning(keyValue.Key + " length: " + keyValue.Value.GetListenerLength());
		}
	}

	/// <summary>
	/// Adds an observer to listen to specified by notification name. Action has parameters.
	/// </summary>
	public void AddObserver(string notificationName, System.Action<Parameters> action) {

		//if there is already an existing key, add the listener to the observer list
		if(this.eventObservers.ContainsKey(notificationName)) {
			ObserverList eventObserver = this.eventObservers[notificationName];
			eventObserver.AddObserver(action);
		}
		//create a new instance of an observer list
		else {
			ObserverList eventObserver = new ObserverList();
			eventObserver.AddObserver(action);
			this.eventObservers.Add(notificationName,eventObserver);
		}
	}

    /// <summary>
	/// Adds an observer to listen to specified by notification name. Action has no parameters.
	/// </summary>
	public void AddObserver(string notificationName, System.Action action) {

		//if there is already an existing key, add the listener to the observer list
		if(this.eventObservers.ContainsKey(notificationName)) {
			ObserverList eventObserver = this.eventObservers[notificationName];
			eventObserver.AddObserver(action);
		}
		//create a new instance of an observer list
		else {
			ObserverList eventObserver = new ObserverList();
			eventObserver.AddObserver(action);
			this.eventObservers.Add(notificationName,eventObserver);
		}
	}

	/// <summary>
	/// Removes all observers under the specified notification name
	/// </summary>
	/// <param name="notificationName">Notification name.</param>
	public void RemoveObserver(string notificationName) {
		if(this.eventObservers.ContainsKey(notificationName)) {
			ObserverList eventObserver = this.eventObservers[notificationName];
			eventObserver.RemoveAllObservers();
			this.eventObservers.Remove(notificationName);
		}
	}

	/// <summary>
	/// Removes the action at observer specified by notification name
	/// </summary>
	/// <param name="notificationName">Notification name.</param>
	/// <param name="action">Action.</param>
	public void RemoveActionAtObserver(string notificationName, System.Action action) {
		if(this.eventObservers.ContainsKey(notificationName)) {
			ObserverList eventObserver = this.eventObservers[notificationName];
			eventObserver.RemoveObserver(action);
		}
	}

	/// <summary>
	/// Removes the action at observer specified by notification name
	/// </summary>
	/// <param name="notificationName">Notification name.</param>
	/// <param name="action">Action.</param>
	public void RemoveActionAtObserver(string notificationName, System.Action<Parameters> action) {
		if(this.eventObservers.ContainsKey(notificationName)) {
			ObserverList eventObserver = this.eventObservers[notificationName];
			eventObserver.RemoveObserver(action);
		}
	}


	/// <summary>
	/// Removes all observers.
	/// </summary>
	public void RemoveAllObservers() {
		foreach(ObserverList eventObserver in this.eventObservers.Values) {
			eventObserver.RemoveAllObservers();
		}

		this.eventObservers.Clear();
	}

	/// <summary>
	/// Posts an event specified by name that does not require any parameters. 
	/// Observers associated with this event will be called.
	/// </summary>
	public void PostEvent(string notificationName) {
		if(this.eventObservers.ContainsKey(notificationName)) {
			ObserverList eventObserver = this.eventObservers[notificationName];
			eventObserver.NotifyObservers();
		}
	}

	/// <summary>
	/// Posts an event specified by name that requires parameters. Observers associated with this event will be called.
	/// Requires the parameters class to be passed.
	/// </summary>
	public void PostEvent(string notificationName, Parameters parameters) {
		if(this.eventObservers.ContainsKey(notificationName)) {
			ObserverList eventObserver = this.eventObservers[notificationName];
			eventObserver.NotifyObservers(parameters);
		}

	}


}
