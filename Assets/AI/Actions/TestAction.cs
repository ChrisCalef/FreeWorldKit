using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class TestAction : RAINAction
{
	Animator animator=null;
	AIRig airig=null;
	int lastTarg=1;

    public TestAction()
    {
        actionName = "TestAction";
    }

    public override void Start(AI ai)
    {
        base.Start(ai);
		GameObject targetObj = null;
		int newTarg=0;
		animator = ai.Body.GetComponent<Animator>();
		airig = ai.Body.GetComponent<AIRig>();
		RAIN.Memory.RAINMemory memory = airig.AI.WorkingMemory;

		int currTarg = memory.GetItem<int>("currentTarget");

		animator.SetFloat ("Speed",airig.AI.Motor.Speed);
		
//		GameObject mPlayer = GameObject.FindGameObjectWithTag("Player");
//		Vector3 playerDiff = ai.Body.transform.position - mPlayer.transform.position;
//		Debug.Log ("player diff: " + playerDiff.magnitude + " currTarg " + currTarg +
//		           " lastTarg " + lastTarg);
//		if (playerDiff.magnitude < 10.0)
//		{
//			lastTarg = currTarg;
//			memory.SetItem<Vector3>("targetPosition", mPlayer.transform.position);
//			memory.SetItem<int>("currentTarget",99);
//			//Debug.Log ("targeting player! currTarg " + currTarg);
//			return;
//		}
			
		if (airig)
		{

			if (currTarg==99)
			{
				//if ((lastTarg==99)&&(playerDiff.magnitude>=10.0f))
				//	lastTarg=1;

				//Debug.Log ("We got 99 targets and none of them are you.");
//				if (lastTarg==1)
//					targetObj = GameObject.Find("Sphere1");
//				else if (lastTarg==2)
//					targetObj = GameObject.Find("Sphere2");
//				else if (lastTarg==3)
//					targetObj = GameObject.Find("Sphere3");
//				else if (lastTarg==4)
//					targetObj = GameObject.Find("Sphere4");

				targetObj = GameObject.Find("Sphere1");
				if (targetObj)
				{
					Debug.Log ("setting up a new target: " + lastTarg );
					memory.SetItem<Vector3>("targetPosition", targetObj.transform.position);
					memory.SetItem<int>("currentTarget",lastTarg);
				}
			}

			if(airig.AI.Motor.IsAtMoveTarget)
			{
				//animator.SetFloat ("Speed",0.0f);

				//Debug.Log ("we are at our target!! " + memory.GetItem<int>("currentTarget"));
				if (currTarg==1)
				{
					targetObj = GameObject.Find("Sphere2");
					newTarg = 2;
				} 
				else if (currTarg==2)
				{
					targetObj = GameObject.Find("Sphere3");
					newTarg = 3;
				} 
				else if (currTarg==3)
				{
					targetObj = GameObject.Find("Sphere4");
					newTarg = 4;
				} 
				else if (currTarg==4)
				{
					targetObj = GameObject.Find("Sphere1");
					newTarg = 1;
				}
				if ((targetObj)&&(newTarg>0))
				{
					memory.SetItem<Vector3>("targetPosition", targetObj.transform.position);
					memory.SetItem<int>("currentTarget",newTarg);
				}
			}
			//else
			//{
				//animator.SetFloat ("Speed",4.0f);
			//}
			//animator.SetFloat ("Speed",ai.Body.transform.);
			//Vector3 moveTarg = new Vector3(-9.0f,0.0f,20.0f);
			//airig.AI.Motor.MoveTo(moveTarg);
			//airig.AI.Motor.UpdateMotionTransforms();
			//animator.SetFloat ("Speed",airig.AI.Kinematic.Velocity.magnitude);

			//Debug.Log ("TestAction starting, got animator!!! speed=" + animator.GetFloat("Speed") +
			//           "  AIRig move target: " + airig.AI.Motor.moveTarget.Position);
		}
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