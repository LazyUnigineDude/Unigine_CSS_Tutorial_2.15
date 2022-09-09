using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "1fa4e0e57fc69d003112ec5f59d9ce32b78857e9")]
public class ShooterAI : Component
{
	public Node MainCharacter;
	private float ViewDistance, Weight, DistanceRatio;

	BoundFrustum BF;
	bool isInsideFrustum = false;

	enum AISTATE { IDLE, ALERT, SEARCH, AGGRESSIVE}
	AISTATE STATE;

	private void Init()
	{
		// write here code to be called on component initialization

		ViewDistance = 15;
		Weight = 0;
		STATE = AISTATE.IDLE;
	}
	
	private void Update()
	{
		// write here code to be called before updating each render frame
		
		mat4 Frustum = MathLib.Perspective(40, 1.5f, 0.05f, ViewDistance);
			quat Rotation = node.GetWorldRotation() * new quat(90, 0, 0);
			mat4 View = new(Rotation, node.GetChild(0).WorldPosition);
		
		Visualizer.RenderFrustum(Frustum, View, vec4.BLACK);

		BF = new(Frustum, MathLib.Inverse(View));

		if (BF.Inside(MainCharacter.WorldPosition))
		{
			isInsideFrustum = true;
			float distance = MathLib.Distance(node.WorldPosition, MainCharacter.WorldPosition);
			DistanceRatio = distance / ViewDistance;
		}
		else isInsideFrustum = false;

		AiSTATE();
	}

	void AiSTATE() {

		switch (STATE)
		{
			case AISTATE.IDLE:
				Log.Message("IDLE\n");
				Weight = MathLib.Clamp(Weight -= Game.IFps, 0f, 1f);
				if (isInsideFrustum) STATE = AISTATE.ALERT;
				break;
			case AISTATE.ALERT:
				Log.Message("ALRT\n");
				Weight = MathLib.Clamp(Weight += Game.IFps / DistanceRatio, 0f, 1f);
				if (!isInsideFrustum) STATE = AISTATE.IDLE;
				if (Weight == 1f) STATE = AISTATE.AGGRESSIVE;
				break;
			case AISTATE.SEARCH:
				Log.Message("SRCH\n");
				Weight = MathLib.Clamp(Weight -= Game.IFps / 5, 0f, 1f);
				if (Weight == 0f) STATE = AISTATE.IDLE;
				if (isInsideFrustum) { STATE = AISTATE.AGGRESSIVE; Weight = 1; }
				break;
            case AISTATE.AGGRESSIVE:
				Log.Message("AGRO\n");
				if (!isInsideFrustum) STATE = AISTATE.SEARCH;
                break;
            default:
                break;
        }
    }
}