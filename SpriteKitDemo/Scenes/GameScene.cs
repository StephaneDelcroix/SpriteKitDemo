//
// GameScene.cs
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
using MonoTouch.CoreMotion;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SpriteKitDemo
{

	public class GameScene : BackgroundScene
	{
		const int numPlatforms = 10;
		const int minPlatformStep = 50;
		const int maxPlatformStep = 300;
		const int platformTopPadding = 10;

		int score;
		bool gameSuspended;
		readonly Random random = new Random();

		public GameScene (SizeF size) : base (size)
		{
			InitPlatforms ();

			InitBird ();

			InitScore ();

			UIAccelerometer.SharedAccelerometer.Acceleration += (sender, e) => {
				if (gameSuspended)return;
				float accel_filter = 0.1f;
				birdVelocity.X = birdVelocity.X * accel_filter + (float)e.Acceleration.X * (1.0f - accel_filter) * 500.0f;
			};

		}


		public override void DidMoveToView (SKView view)
		{
			base.DidMoveToView (view);

			StartGame ();
		}

		public override void WillMoveFromView (SKView view)
		{
			//StopGame ();
		}

		float currentPlatformY;
		float currentMaxPlatformStep = 60f;

		void InitPlatforms ()
		{
			for (var i=0; i<numPlatforms; i++) {
				AddChild (new Platform ());
			}
			ResetPlatforms ();
		}

		void ResetPlatforms ()
		{
			currentPlatformY = -1;

			foreach (var platform in Children.OfType<Platform> ())
				ResetPlatform (platform);
		}

		void ResetPlatform (Platform platform)
		{
			var currentPlatformX = 160f;

			if (currentPlatformY < 0)
				currentPlatformY = 30f;
			else {
				currentPlatformY += random.Next (minPlatformStep, (int)currentMaxPlatformStep);
				currentPlatformX = random.Next ((int)(platform.Size.Width / 2), (int)(Size.Width - platform.Size.Width / 2));
			}
			currentMaxPlatformStep = Math.Min (currentMaxPlatformStep + .5f, maxPlatformStep);

			if (random.Next (2) == 1)
				platform.XScale = -1f;

			platform.Position = new PointF (currentPlatformX, currentPlatformY);
		}
		
		PointF birdPosition;
		PointF birdVelocity;
		PointF birdAcceleration;

		void InitBird ()
		{
			AddChild (new Bird ());
		}

		void ResetBird ()
		{
			var bird = Children.OfType<Bird> ().Single ();
			bird.Position = birdPosition = new PointF (Size.Width / 2, 160f);
			birdVelocity = new PointF (0f, 0f);
			birdAcceleration = new PointF (0, -550f);
			bird.XScale = 1f;
		}

		void InitScore ()
		{
			AddChild (new ScoreLabel {Position = new PointF (Size.Width/2, Size.Height-60)});
		}

		void StartGame ()
		{
			score = 0;

			ResetClouds ();
			ResetPlatforms ();
			ResetBird ();

			gameSuspended = false;
		}

		public override void Update (double currentTime)
		{
			base.Update (currentTime);

			var bird = Children.OfType<Bird> ().Single ();

			var dt = 1 / 30f; //should be currentime - previoustime

			if (gameSuspended)
				return;

			//integrate X position
			birdPosition.X += birdVelocity.X * dt;

			//make the bird look in the right direction
			if (birdVelocity.X < -30f)
				bird.XScale = -1f;
			else if (birdVelocity.X > 30)
				bird.Scale = 1f;

			//make sure the bird is on screen
			birdPosition.X = Math.Min (birdPosition.X, Size.Width - bird.Size.Width / 2);
			birdPosition.X = Math.Max (birdPosition.X, bird.Size.Width / 2);

			//integrate Y position and velocity
			birdVelocity.Y += birdAcceleration.Y * dt;
			birdPosition.Y += birdVelocity.Y * dt;

			if (birdVelocity.Y < 0) { //going down, detect collision with platforms
				foreach (var platform in Children.OfType<Platform>()) {
					if (birdPosition.X > platform.Position.X - platform.Size.Width / 2 - 10 &&
					    birdPosition.X < platform.Position.X + platform.Size.Width / 2 + 10 &&
					    birdPosition.Y > platform.Position.Y &&
					    birdPosition.Y < platform.Position.Y + (platform.Size.Height + bird.Size.Height) / 2 - platformTopPadding)
						birdVelocity.Y = 400f;
				}
				if (birdPosition.Y < -bird.Size.Height/2) //Game Over
					StartGame ();

			} else if (birdPosition.Y > Size.Height/2) { //going up in the top middle of the screen: scroll
				var delta = birdPosition.Y - Size.Height / 2;
				birdPosition.Y = Size.Height / 2;

				currentPlatformY -= delta;

				//scroll the clouds
				foreach (var cloud in Children.OfType<Cloud>()) {
					var cloudPosition = cloud.Position;
					cloudPosition.Y -= delta * cloud.YScale * .8f;
					if (cloudPosition.Y < -cloud.Size.Height / 2) {
						ResetCloud (cloud);
						cloud.Position = new PointF (cloud.Position.X, cloud.Position.Y + Size.Height);
					} else
						cloud.Position = cloudPosition;
				}

				//scroll the platforms
				foreach (var platform in Children.OfType<Platform>()) {
					var platformPosition = new PointF (platform.Position.X, platform.Position.Y - delta);
					if (platformPosition.Y < -platform.Size.Height / 2)
						ResetPlatform (platform);
					else
						platform.Position = platformPosition;
				}

				var scoreLabel = Children.OfType<ScoreLabel> ().Single ();
				score += (int)delta;
				scoreLabel.Text = score.ToString ();

			}


			bird.Position = birdPosition;

		}



		void ShowHighScores ()
		{
		}
	}

}
