using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "1fa4e0e57fc69d003112ec5f59d9ce32b78857e9")]
public class ShooterAI : Component
{
	public Node PathMakerNode;
	public Node MainCharacter;
	private float ViewDistance, Weight, DistanceRatio;

	BoundFrustum BF;
	bool isInsideFrustum = false;

	enum AISTATE { IDLE, ALERT, SEARCH, AGGRESSIVE}
	AISTATE STATE;
	quat HorReset = new quat(90, 0, 0);
	PathMaker Path;
	mat4 View;
	private void Init()
	{
		// write here code to be called on component initialization

		ViewDistance = 15;
		Weight = 0;
		STATE = AISTATE.IDLE;
		Path = GetComponent<PathMaker>(PathMakerNode);
		BF = new();
		View = new();
		Path.InitPath();
	}
	
	private void Update()
	{
		// write here code to be called before updating each render frame
		
		mat4 Frustum = MathLib.Perspective(40, 1.5f, 0.05f, ViewDistance);
			quat Rotation = node.GetWorldRotation() * HorReset;
		vec3 Pos = node.GetChild(0).WorldPosition;
		 View.Set(Rotation, Pos);
		
		Visualizer.RenderFrustum(Frustum, View, vec4.BLACK); 
		BF.Set(Frustum, MathLib.Inverse(View));

		if (BF.Inside(MainCharacter.WorldPosition))
		{
			isInsideFrustum = true;
			float distance = MathLib.Distance(node.WorldPosition, MainCharacter.WorldPosition);
			DistanceRatio = distance / ViewDistance;
		}
		else isInsideFrustum = false;
		
		Path.RenderPath();
		AiSTATE();
	}

	void AiSTATE() {

		switch (STATE)
		{
			case AISTATE.IDLE:
				Log.Message("IDLE\n");
				Weight = MathLib.Clamp(Weight -= Game.IFps, 0f, 1f);
				if (isInsideFrustum) STATE = AISTATE.ALERT;
                if (MathLib.Distance(node.WorldPosition, Path.GetCurrentPathPosition()) > 0.1f)
                {
					MoveTowards(Path.GetCurrentPathPosition(), node);
					RotateTowards(Path.GetCurrentPathPosition(), node, 0.05f);
                }
                else
                {
					Path.MoveAlongPath();
					Path.MoveObject(node);
                }
				break;
			case AISTATE.ALERT:
				Log.Message("ALRT\n");
				Weight = MathLib.Clamp(Weight += Game.IFps / DistanceRatio, 0f, 1f);
				if (!isInsideFrustum) STATE = AISTATE.IDLE;
				if (Weight == 1f) STATE = AISTATE.AGGRESSIVE;

					RotateTowards(MainCharacter.WorldPosition, node, 0.005f);
				break;
			case AISTATE.SEARCH:
				Log.Message("SRCH\n");
				Weight = MathLib.Clamp(Weight -= Game.IFps / 5, 0f, 1f);
				if (Weight == 0f) STATE = AISTATE.IDLE;
				if (isInsideFrustum) { STATE = AISTATE.AGGRESSIVE; Weight = 1; }
					RotateTowards(MainCharacter.WorldPosition, node, 0.05f);
				break;
            case AISTATE.AGGRESSIVE:
				Log.Message("AGRO\n");
				if (!isInsideFrustum) STATE = AISTATE.SEARCH;
					MoveTowards(MainCharacter.WorldPosition, node);
					RotateTowards(MainCharacter.WorldPosition, node, 0.05f);
				break;
            default:
                break;
        }
    }

	void RotateTowards(vec3 TowardsObject, Node Obj2Move, float Speed) {

		vec3 Vec1 = Obj2Move.GetWorldDirection(MathLib.AXIS.Y),
			 Vec2 = (TowardsObject - Obj2Move.WorldPosition).Normalized;
		float Angle = MathLib.Angle(Vec1, Vec2, vec3.UP);
		Obj2Move.Rotate(-Obj2Move.GetWorldRotation().x, -Obj2Move.GetWorldRotation().y, Angle * Speed);
	}

	void MoveTowards(vec3 TowardsObject, Node Obj2Move) {

		vec3 Pos = MathLib.Lerp(Obj2Move.WorldPosition,
								TowardsObject,
								Game.IFps / MathLib.Distance(Obj2Move.WorldPosition,
															TowardsObject));
		Obj2Move.WorldPosition = Pos;
	}
}