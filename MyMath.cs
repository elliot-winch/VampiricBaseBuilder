using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath  {

	public static float Distance(float x1, float x2, float y1, float y2){
		return Mathf.Sqrt (((x1-x2) * (x1-x2)) + ((y1-y2) * (y1-y2)));
	}

	public static float SqrDistance(float x1, float x2, float y1, float y2){
		return ((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2));
	}


	public static bool EqualsRoughly2D(Vector3 a, Vector3 b){
		if (a.x <= b.x + Mathf.Epsilon && a.x >= b.x - Mathf.Epsilon
		   && a.y <= b.y + Mathf.Epsilon && a.y >= b.y - Mathf.Epsilon) {
			return true;
		}

		return false;
	}
}
