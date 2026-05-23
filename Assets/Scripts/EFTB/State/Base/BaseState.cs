using System;
using System.Collections.Generic;

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
	}

	public virtual void UpdateLogic(float deltaTime)
	{

	}
	public virtual void OnExitState()
	{
		IsStateActive = false;
	}
}
