using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace M1_towerdefense
{
    public class OrbitCamera : GameComponent
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
        public Vector3 UpVector { get; set; }
        public float Distance { get; set; }
        public Vector2 Rotation;

        public OrbitCamera(Game game)
            : base(game)
        {
            this.Position = Vector3.Zero;
            this.Target = Vector3.Zero;
            this.UpVector = Vector3.Zero;
            this.Rotation = Vector2.Zero;
            this.Distance = 15.0f;
        }

        public override void Update(GameTime gameTime)
        {
            float MoveIncrement = 1.0f;
            float ZoomIncrement = 0.1f;
            //Récupération des données
            KeyboardState KbState = Keyboard.GetState();
            if (KbState.IsKeyDown(Keys.Left))
                Rotation.Y += MoveIncrement;
            if (KbState.IsKeyDown(Keys.Right))
                Rotation.Y -= MoveIncrement;
            if (KbState.IsKeyDown(Keys.Up))
                Rotation.X += MoveIncrement;
            if (KbState.IsKeyDown(Keys.Down))
                Rotation.X -= MoveIncrement;
            if (KbState.IsKeyDown(Keys.PageUp))
                this.Distance += ZoomIncrement;
            if (KbState.IsKeyDown(Keys.PageDown))
                this.Distance -= ZoomIncrement;

            GamePadState gpstate = GamePad.GetState(PlayerIndex.One);

            this.Rotation += gpstate.ThumbSticks.Left;

            Rotation.X %= 360;
            Rotation.Y %= 360;
            if (Rotation.X < 0) Rotation.X = 359;
            if (Rotation.Y < 0) Rotation.Y = 359;

            //Calcul de modification des valeurs
            Quaternion q1 = Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(Rotation.X));
            Quaternion q2 = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(Rotation.Y));
            Quaternion q = q1 * q2;
            Matrix rotationMatrix = Matrix.CreateFromQuaternion(q);
            this.Position = this.Target - (rotationMatrix.Forward * this.Distance);
            this.UpVector = rotationMatrix.Up;

            base.Update(gameTime);
        }
    }
}
