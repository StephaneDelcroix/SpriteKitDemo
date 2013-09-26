//
// ScoreLabel.cs
//
// Author:
//       Stephane Delcroix <stephane@mi8.be>
//
// Copyright (c) 2013 Mobile Inception
//

using System;
using System.Linq;
using MonoTouch.SpriteKit;
using MonoTouch.UIKit;

namespace SpriteKitDemo
{

	public class ScoreLabel : SKLabelNode
	{
		public ScoreLabel ()
		{
			FontSize = 48;
			FontColor = UIColor.Blue;
			Text = "--";
		}
	}
}