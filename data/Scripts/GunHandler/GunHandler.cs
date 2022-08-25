using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "8c0197dccf81518b4310796e1064f1f5eae3dad0")]
public class GunHandler : Component
{
	Unigine.Object Gun;

	private void Init()
	{
		// write here code to be called on component initialization
		
	}
	
	private void Update()
	{
		// write here code to be called before updating each render frame
		
	}

	public void GetGun(Unigine.Object Gun) { this.Gun = Gun; }
}