using System.Collections.Generic;
using UnityEngine;

public class FSMSystem
{
	List<FSMState> _states = new List<FSMState> ();

	public StateID CurrentID { get; private set; }
	public FSMState CurrentState { get; private set; }

	public FSMSystem ()
	{

	}

	public FSMSystem (params FSMState[] states)
	{
		foreach (FSMState s in states)
			AddState (s);
	}

	public void AddState (FSMState state)
	{
		if (state == null)
		{
			Debug.LogWarning ("(FSMSystem) Null reference isn't allowed.");
			return;
		}

		if (_states.Count == 0)
		{
			_states.Add (state);
			CurrentState = state;
			CurrentID = state.ID;
			return;
		}

		foreach (FSMState s in _states)
		{
			if (state.ID != s.ID)
				continue;

			Debug.LogWarning ("(FSMSystem) Impossible to add state " + s.ID.ToString () + " because state has already been added.");
			return;
		}

		_states.Add (state);
	}

	public void DeleteState (StateID id)
	{
		if (id == StateID.NA)
		{
			Debug.LogWarning ("(FSMSystem) NullStateID isn't allowed for a real state");
			return;
		}

		foreach (FSMState state in _states)
		{
			if (state.ID != id)
				continue;

			_states.Remove (state);
			return;
		}

		Debug.LogWarning ("(FSMSystem) Impossible to delete state " + id.ToString () + ". It wasn't in the list of states.");
	}

	public void PerformTransition (Transition transition)
	{
		if (transition == Transition.NA)
		{
			Debug.LogWarning ("(FSMSystem) NullTransition isn't allowed for a real transition.");
			return;
		}

		StateID id = CurrentState.GetOutputState (transition);
		if (id == StateID.NA)
		{
			Debug.LogWarning ("(FSMSystem) State " + CurrentID.ToString () + " doesn't have a target state for transition " + transition.ToString ());
			return;
		}

		CurrentID = id;
		foreach (FSMState s in _states)
		{
			if (s.ID != CurrentID)
				continue;

			CurrentState.DoBeforeLeaving ();
			CurrentState = s;
			CurrentState.DoBeforeEntering ();
			break;
		}
	}
}