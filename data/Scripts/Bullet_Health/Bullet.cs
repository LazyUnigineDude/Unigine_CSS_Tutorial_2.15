using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "06cd6dfa2c1925111d239b3c0ff3bd6ba8bb33ce")]
public class Bullet : Component
{
	float StartTime;

	[ShowInEditor]
	float LifeTime = 2f;

	[ShowInEditor]
	int DamageAmount = 1;

	Body Rigid;

	void OnEnter(Body Body, int num)
	{
		node.DeleteLater();
		Body Body1 = Body.GetContactBody0(num),
			 Body2 = Body.GetContactBody1(num),
			 CapturedBody = null;

		if (Body1 && Body1 != Rigid) { CapturedBody = Body1; }
		if (Body2 && Body2 != Rigid) { CapturedBody = Body2; }

		if (CapturedBody)
		{
			// WE hit a body
			Log.Message("HIT BODY: {0}\n", CapturedBody.Object.Name);
			HealthBar Health = GetComponent<HealthBar>(CapturedBody.Object);
			if (Health) { Health.DropHealth(DamageAmount); }

		}

		else
		{
			// We hit a collision
			Log.Message("HIT COLLISION: {0}\n", Body.GetContactObject(num).Name);
			HealthBar Health = GetComponent<HealthBar>(Body.GetContactObject(num));
			if (Health) { Health.DropHealth(DamageAmount); }
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