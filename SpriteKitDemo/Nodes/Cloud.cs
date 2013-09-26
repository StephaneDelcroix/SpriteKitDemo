//
// Cloud.cs
//
// Author:
//       Stephane Delcroix <stephane@mi8.be>
//
// Copyright (c) 2013 Mobile Inception
//

using System;
using System.Linq;
using MonoTouch.SpriteKit;

namespace SpriteKitDemo
{

	public class Cloud : SKSpriteNode
	{
		static readonly Random random = new Random ();
		static readonly string[] clouds = { "cloud0", "cloud1", "cloud2" };

		public Cloud () : base (clouds[random.Next()%clouds.Count()])
		{
			Alpha = .5f;
		}
	}
}