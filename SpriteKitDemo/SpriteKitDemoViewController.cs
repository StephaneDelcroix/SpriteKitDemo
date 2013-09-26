//
// SpriteKitDemoViewController.cs
//
// Author:
//       Stephane Delcroix <stephane@mi8.be>
//
// Copyright (c) 2013 Mobile Inception
//

using System;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.SpriteKit;

namespace SpriteKitDemo
{
	public partial class SpriteKitDemoViewController : UIViewController
	{
		public SpriteKitDemoViewController () : base ("SpriteKitDemoViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		SKView skview;
		SKScene backgroundScene;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			skview = new SKView () { 
				Frame = View.Bounds, 
#if DEBUG
				ShowsFPS = true, 
				ShowsNodeCount = true, 
				ShowsDrawCount = true 
#endif
			};

			Add (skview);
			backgroundScene = new GameScene (skview.Frame.Size) { ScaleMode = SKSceneScaleMode.AspectFit };
			skview.BackgroundColor = UIColor.Clear;
			skview.PresentScene (backgroundScene);

		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}



}

