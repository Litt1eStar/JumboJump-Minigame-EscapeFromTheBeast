using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseState
{
	public Dictionary<Type, Func<bool>> StateTransitionMap;
	public bool IsStateActive { get; private set; }
	protected BaseStateController StateController;


	public BaseState(BaseStateController stateController)
	{
		StateController = stateController;
		StateTransitionMap = new Dictionary<Type, Func<bool>>();
	}

	public virtual void Initialize()
    {
		
    }

	public virtual void Dispose()
	{

	}

	public virtual void OnEnterState()
	{
		IsStateActive = true;
		Debug.Log($"Entered state: {GetType().Name}");
	}

	public virtual void UpdateLogic(float deltaTime)
	{
		Debug.Log($"Update Logic: {GetType().Name}");
	}
	public virtual void OnExitState()
	{
		IsStateActive = false;
		Debug.Log($"Exited state: {GetType().Name}");
	}
}
