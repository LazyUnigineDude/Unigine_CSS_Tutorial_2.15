using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "d3be6ccb7c465bae8886ef354ac53710fcf31f2f")]
public class HealthBar : Component
{
	[ShowInEditor]
	private int Health;

	public int ShowHealth() { return Health; }
	public void DropHealth(int amount) { Health -= amount; Check(); }
	private void Check() { if (Health <= 0) node.DeleteLater(); }
}