using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_Introduction
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Rectangle _rectangle;
        private Texture2D _whitePixelTexture;
        private Texture2D _monogameLogoTexture;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _monogameLogoTexture = Content.Load<Texture2D>("SquareLogo_128px");
            int rectangleWidth = _monogameLogoTexture.Width;
            int rectangleHeight = _monogameLogoTexture.Height;
            int x = (GraphicsDevice.Viewport.Width - rectangleWidth) / 2;
            int y = (GraphicsDevice.Viewport.Height - rectangleHeight) / 2;
            _rectangle = new Rectangle(x, y, rectangleWidth, rectangleHeight);
            _whitePixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            _whitePixelTexture.SetData(new Color[] { Color.Red });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_monogameLogoTexture, _rectangle, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
