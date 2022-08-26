using Unigine;

[Component(PropertyGuid = "06cd6dfa2c1925111d239b3c0ff3bd6ba8bb33ce")]
public class Bullet : Component
{
	float StartTime;

	public float LifeTime = 2f;
	public int DamageAmount { get; set; }

	Body Rigid;

	void OnEnter(Body Body, int num)
	{
		node.DeleteLater();
		Body Body1 = Body.GetContactBody0(num),
			 Body2 = Body.GetContactBody1(num),
			 CapturedBody = null;

		if (Body1 && Body1 != Rigid) { CapturedBody = Body1; }
		else if (Body2 && Body2 != Rigid) { CapturedBody = Body2; }

		if (CapturedBody)
		{
			// WE hit a body
			HealthBar Health = GetComponent<HealthBar>(CapturedBody.Object);
			if (Health) { Health.DropHealth(DamageAmount); Log.Message("{0}\n",Health.ShowHealth()); }

		}

		else
		{
			// We hit a collision
			HealthBar Health = GetComponent<HealthBar>(Body.GetContactObject(num));
			if (Health) { Health.DropHealth(DamageAmount); Log.Message("{0}\n", Health.ShowHealth()); }
		}
	}

	private void Init()
	{
		StartTime = Game.Time;
		Rigid = node.ObjectBodyRigid;
		Rigid.AddContactEnterCallback(OnEnter);
	}

	private void Update()
	{
		// write here code to be called before updating each render frame
		if (Game.Time - StartTime > LifeTime)
		{
			node.DeleteLater();
		}
	}
}