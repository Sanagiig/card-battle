using Godot;
using System;

public static class Arc
{
	public static Vector2 GetQuadraticBezierPoint(Vector2 start, Vector2 end, Vector2 control, float t)
	{
		if (t <= 0.0f) return start;
		if (t >= 1.0f) return end;

		// Clamp t to valid range [0, 1]
		t = Mathf.Clamp(t, 0.0f, 1.0f);

		// Quadratic Bézier formula: (1-t)² * start + 2 * (1-t) * t * control + t² * end
		float oneMinusT = 1.0f - t;
		return oneMinusT * oneMinusT * start +
					 2.0f * oneMinusT * t * control +
					 t * t * end;
	}
}
