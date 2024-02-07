using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Holds the associated actions associated with the event name
/// Created By: NeilDG
/// </summary>
public class ObserverList {
    /// <summary>
    /// By default, event listeners with params
    /// </summary>
	private List<System.Action<Parameters>> eventListeners;
    /// <summary>
    /// Event listeners that does not have params;
    /// </summary>
	private List<System.Action> eventListenersNoParams;

    /// <summary>
    /// Constructor
    /// </summary>
	public ObserverList() {
		this.eventListeners = new List<System.Action<Parameters>>();
		this.eventListenersNoParams = new List<System.Action>();
	}

    /// <summary>
    /// Registers an action which accepts parameters.
    /// </summary>
    /// <param name="action"></param>
	public void AddObserver(System.Action<Parameters> action) {
		this.eventListeners.Add(action);
	}

    /// <summary>
    /// Registers an action.
    /// </summary>
    /// <param name="action"></param>
	public void AddObserver(System.Action action) {
		this.eventListenersNoParams.Add(action);
	}
    
    /// <summary>
    /// Removes action which accepts parameters.
    /// </summary>
    /// <param name="action"></param>
	public void RemoveObserver(System.Action<Parameters> action) {
		if(this.eventListeners.Contains(action)) {
			this.eventListeners.Remove(action);
		}
	}

    /// <summary>
    /// Removes an observer.
    /// </summary>
    /// <param name="action"></param>
	public void RemoveObserver(System.Action action) {
		if(this.eventListenersNoParams.Contains(action)) {
			this.eventListenersNoParams.Remove(action);
		}
	}

    /// <summary>
    /// Removes all observers.
    /// </summary>
	public void RemoveAllObservers() {
		this.eventListeners.Clear();
		this.eventListenersNoParams.Clear();
	}

    /// <summary>
    /// Notifies all observers with actions which accept parameters registered to this observerable.
    /// </summary>
    /// <param name="parameters"></param>
	public void NotifyObservers(Parameters parameters) {
		for(int i = 0; i < this.eventListeners.Count; i++) {
			System.Action<Parameters> action = this.eventListeners[i];

			action(parameters);
		}
	}

    /// <summary>
    /// Notifies all observers registered.
    /// </summary>
	public void NotifyObservers() {
		for(int i = 0; i < this.eventListenersNoParams.Count; i++) {
			System.Action action = this.eventListenersNoParams[i];

			action();
		}
	}

    /// <summary>
    /// Returns the number of listeners registered.
    /// </summary>
    /// <returns>Number of listeners</returns>
	public int GetListenerLength() {
		return (this.eventListeners.Count + this.eventListenersNoParams.Count);
	}
}
