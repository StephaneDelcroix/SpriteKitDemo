//
// BackgroundScene.cs
//
// Author:
//       Stephane Delcroix <stephane@mi8.be>
//
// Copyright (c) 2013 Mobile Inception
//

using System;
using System.Linq;
using System.Drawing;
using MonoTouch.SpriteKit;

namespace SpriteKitDemo
{
	public class BackgroundScene : SKScene
	{
		const int numClouds = 12;
		readonly Random random = new Random ();

		public BackgroundScene (SizeF size) : base (size)
		{
			InitBackground ();
			
			InitClouds ();
		}

		void InitBackground ()
		{
			var background = SKSpriteNode.FromImageNamed ("background");
			background.Position = new PointF (Size.Width/2, Size.Height/2);
			AddChild (background);
		}

		void InitClouds () 
		{
			for (var i=0; i<numClouds; i++)
				AddChild (new Cloud ());
			ResetClouds ();
		}

		protected void ResetClouds ()
		{
			foreach (var cloud in Children.OfType<Cloud>()) {
				ResetCloud (cloud);
			}
		}

		protected void ResetCloud (Cloud cloud)
		{
			var distance = random.Next () % 20 + 5;
			var scale = 5f / distance;
			cloud.Scale = scale;
			if (random.Next (2) == 1)
				cloud.XScale = -cloud.XScale;

			var x = random.Next ((int)(Size.Width));
			var y = random.Next ((int)(Size.Height));
			cloud.Position = new PointF (x, y);
		}

		public override void Update (double currentTime)
		{
			base.Update (currentTime);
			foreach (var cloud in Children.OfType<Cloud> ()) {
				var pos = cloud.Position;
				var size = cloud.Size;
				pos.X += .5f * cloud.YScale;
				if (pos.X > 320 + size.Width/2)
					pos.X = -size.Width / 2;
				cloud.Position = pos;
			}
		}
	}
}