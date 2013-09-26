//
// Platform.cs
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

	public class Platform : SKSpriteNode
	{
		static readonly Random random = new Random ();
		static readonly string[] platforms = { "platform0", "platform1" };

		public Platform () : base (platforms[random.Next()%platforms.Count()])
		{
			if (random.Next (2) == 1)
				XScale = -1f;
		}
	}
}