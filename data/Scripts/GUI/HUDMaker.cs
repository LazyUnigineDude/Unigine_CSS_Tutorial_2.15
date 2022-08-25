using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "d1dca4d9e782c864abaef873ee8a63362d43811d")]
public class HUDMaker : Component
{
	public AssetLink _image;
	Gui GUI;
	WidgetCanvas Canvas;
	WidgetSprite Sprite;

	private void Init()
	{
		// write here code to be called on component initialization
		GUI = Gui.Get();

		int Width = App.GetWidth(), Height = App.GetHeight();

		Canvas = new();

		int x = Canvas.AddText(1);
		Canvas.SetTextText(x, "SAMPLE_TEXT");
		Canvas.SetTextColor(x, new vec4(1, 1, 1, 0.5));
		Canvas.SetTextSize(x, 30);
		Canvas.SetTextPosition(x, new vec2((Width / 2) - (Canvas.GetTextWidth(x) / 2), Height / 2 - (Canvas.GetTextHeight(x) / 2)));

		int y = Canvas.AddPolygon(0);
		Canvas.SetPolygonColor(y, new vec4(0, 0, 0, 0.5));
		Canvas.AddPolygonPoint(y, new vec3(0, 0, 0));
		Canvas.AddPolygonPoint(y, new vec3(400, 0, 0));
		Canvas.AddPolygonPoint(y, new vec3(400, 100, 0));
		Canvas.AddPolygonPoint(y, new vec3(0, 100, 0));

		Sprite = new();

		int z = Sprite.AddLayer();
		Image _i = new(); _i.Load(_image.AbsolutePath);
		Sprite.SetImage(_i);
		Sprite.SetPosition((Width / 2) - 25, (Height / 2) - 25);
		Sprite.Width = 50;
		Sprite.Height = 50;

		//GUI.AddChild(Canvas, Gui.ALIGN_EXPAND);
		GUI.AddChild(Sprite, Gui.ALIGN_EXPAND | Gui.ALIGN_OVERLAP);
	}

	private void Update()
	{
		// write here code to be called before updating each render frame

		GUI = Gui.Get();
	}
}