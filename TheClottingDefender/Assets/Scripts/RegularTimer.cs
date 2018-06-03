using UnityEngine;

public class RegularTimer
{
	public float Interval;

	private float nextFire;

	public RegularTimer(float interval)
	{
		Interval = interval;
		Rewind();
	}

	public bool Elapsed
	{
		get { return nextFire <= Time.time; }
	}

	public void Rewind(float? newInterval = null)
	{
		Interval = newInterval ?? Interval;
		nextFire = Time.time + Interval;
	}

	public bool CheckElapsedAndAutoRewind()
	{
		bool elapsed = Elapsed;
		if (elapsed) Rewind();
		return elapsed;
	}
}