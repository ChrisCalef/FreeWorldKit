using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class TestAction : RAINAction
{
    public TestAction()
    {
        actionName = "TestAction";
    }

    public override void Start(AI ai)
    {
        base.Start(ai);
		Debug.Log ("TestAction starting!!!");
    }

    public override ActionResult Execute(AI ai)
    {
        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}