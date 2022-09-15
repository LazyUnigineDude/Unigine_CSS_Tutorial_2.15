using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "7085ad171d6ee78a87cbb1619cec9b4ca4bf1e3b")]
public class PathMaker : Component
{
	public Node Obj2Move;
	public int TimeBetweenPoints = 5;
	public List<Node> PathPoints;

	SplineGraph Path;
	float Weight = 0.0f;
	int num = 0;

	private void Init()
	{
		// write here code to be called on component initialization
		Path = new();

		for (int i = 0; i < PathPoints.Count; i++) { Path.AddPoint(PathPoints[i].WorldPosition); }

		Path.AddSegment(0, PathPoints[0].GetWorldDirection(MathLib.AXIS.Y), PathPoints[0].GetWorldDirection(MathLib.AXIS.Z),
						1, PathPoints[1].GetWorldDirection(MathLib.AXIS.NY), PathPoints[1].GetWorldDirection(MathLib.AXIS.Z));
		Path.AddSegment(1, PathPoints[1].GetWorldDirection(MathLib.AXIS.Y), PathPoints[1].GetWorldDirection(MathLib.AXIS.Z),
						2, PathPoints[2].GetWorldDirection(MathLib.AXIS.NY), PathPoints[2].GetWorldDirection(MathLib.AXIS.Z));
		Path.AddSegment(2, PathPoints[2].GetWorldDirection(MathLib.AXIS.Y), PathPoints[2].GetWorldDirection(MathLib.AXIS.Z),
						3, PathPoints[3].GetWorldDirection(MathLib.AXIS.NY), PathPoints[3].GetWorldDirection(MathLib.AXIS.Z));
		Path.AddSegment(3, PathPoints[3].GetWorldDirection(MathLib.AXIS.Y), PathPoints[3].GetWorldDirection(MathLib.AXIS.Z),
						0, PathPoints[0].GetWorldDirection(MathLib.AXIS.NY), PathPoints[0].GetWorldDirection(MathLib.AXIS.Z));

	}
	
	private void Update()
	{
		// write here code to be called before updating each render frame
		Weight = MathLib.Clamp(Weight += (Game.IFps / TimeBetweenPoints), 0.0f, 1.0f);
		if (Weight == 1.0f) { Weight = 0; num++; }
		num %= Path.NumPoints;

		vec3 Point = Path.CalcSegmentPoint(num, Weight),
			 Direc = Path.CalcSegmentTangent(num, Weight),
			 UpVec = Path.CalcSegmentUpVector(num, Weight);

		Obj2Move.WorldPosition = Point;
		Obj2Move.SetWorldDirection(Direc, UpVec, MathLib.AXIS.Y);

		RenderPath();
	}

	void RenderPath()
    {
		int segments = 50;

        for (int i = 0; i < Path.NumPoints; i++)
        {
			Visualizer.RenderPoint3D(Path.GetPoint(i), 0.1f, vec4.BLACK);

			vec3 SPoint = Path.GetSegmentStartPoint(i),
				 STang = Path.GetSegmentStartTangent(i),
				 EPoint = Path.GetSegmentEndPoint(i),
				 ETang = Path.GetSegmentEndTangent(i);

			Visualizer.RenderVector(SPoint, SPoint + STang, vec4.GREEN);
			Visualizer.RenderVector(EPoint, EPoint + ETang, vec4.RED);

            for (int j = 0; j < segments; j++)
            {
				vec3 p0 = Path.CalcSegmentPoint(i, j/segments),
					 p1 = Path.CalcSegmentPoint(i, (j+1)/segments);
				Visualizer.RenderLine3D(p0, p1, vec4.WHITE);
            }
		}
    }
}