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
        private Texture2D _studioLogo;
        private SpriteFont _timerFont;
        private float _timeRemaining = 0f;
        private int _score = 0;
        private string _mainMenu = "Title Screen \n Press space to play \n Press C for credits";
        private string _gameOver = "Game Over \n Your score:";
        enum Screen { FlashScreen, TitleScreen, CreditsScreen, GameScreen, PauseScreen, GameOverScreen };
        private Screen _screen;
        private SpriteFont _mainMenuFont;
        private SpriteFont _gameOverFont;

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
            _timerFont = Content.Load<SpriteFont>("Timer");
            Vector2 timerSize = _timerFont.MeasureString(_timeRemaining.ToString());

            Vector2 timerPosition = new Vector2((_graphics.GraphicsDevice.Viewport.Width - _timerFont.MeasureString(_timeRemaining.ToString("0.0")).X) / 2, (_graphics.GraphicsDevice.Viewport.Height - _timerFont.MeasureString(_timeRemaining.ToString("0.0")).Y) / 2);

            _studioLogo = Content.Load<Texture2D>("SquareLogo_128px");
            int rectangleWidth = _studioLogo.Width;
            int rectangleHeight = _studioLogo.Height;
            int x = (GraphicsDevice.Viewport.Width - rectangleWidth) / 2;
            int y = (GraphicsDevice.Viewport.Height - rectangleHeight) / 2;
            _rectangle = new Rectangle(x, y, rectangleWidth, rectangleHeight);
            _whitePixelTexture = new Texture2D(GraphicsDevice, 1, 1);
            _whitePixelTexture.SetData(new Color[] { Color.Red });
            _mainMenuFont = Content.Load<SpriteFont>("MainMenuFont");
            _gameOverFont = Content.Load<SpriteFont>("GameOverFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (_screen)
            {
                case Screen.CreditsScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.C))
                    {
                        _screen = Screen.TitleScreen;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        // start new run
                        _timeRemaining = 0f;
                        _score = 0;
                        _screen = Screen.GameScreen;
                    }
                    break;

                
                case Screen.FlashScreen:
                    _timeRemaining += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    MouseState mouse = Mouse.GetState();
                    if (_rectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                    {
                        //_screen = Screen.CreditsScreen;
                        GoToGameOver();
                        break;
                    }
                    if (_timeRemaining > 15f)
                    {
                        GoToGameOver();
                    }
                    break;

                case Screen.TitleScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.T))
                    {
                        _screen = Screen.CreditsScreen;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        _screen = Screen.GameScreen;
                    }
                    break;

                case Screen.GameScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.P))
                    {
                        _screen = Screen.FlashScreen;
                    }
                    else if (_rectangle.Contains(Mouse.GetState().Position) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        // calculates score from time and goes to game over method
                        GoToGameOver();
                    }
                    break;

                case Screen.PauseScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.C))
                    {
                        _screen = Screen.CreditsScreen;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        _screen = Screen.GameScreen;
                    }
                    break;

                case Screen.GameOverScreen:
                    if (Keyboard.GetState().IsKeyDown(Keys.C))
                    {
                        _screen = Screen.CreditsScreen;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        // start another run
                        _timeRemaining = 0f;
                        _score = 0;
                        _screen = Screen.GameScreen;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        private void GoToGameOver()
        {
            const float timeLimit = 10f;   // full score at 10s
            const int maxScore = 1000;

            if (_timeRemaining <= timeLimit)
            {
                // Linear scale: 0s -> 0, 10s -> 1000
                float ratio = _timeRemaining / timeLimit;
                _score = (int)System.MathF.Round(ratio * maxScore);
            }
            else
            {
                // Exceeded 10s -> score is 0
                _score = 0;
            }

            _screen = Screen.GameOverScreen;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            Vector2 timerSize = _timerFont.MeasureString(_timeRemaining.ToString());
            switch (_screen)
            {
                case Screen.FlashScreen:

                    _spriteBatch.Draw(_studioLogo, _rectangle, Color.White);

                    Vector2 timerPosition = new Vector2(
                        (_graphics.GraphicsDevice.Viewport.Width - _timerFont.MeasureString(_timeRemaining.ToString("0.0")).X) / 2, (_graphics.GraphicsDevice.Viewport.Height - _timerFont.MeasureString(_timeRemaining.ToString("0.0")).Y) / 2);

                    _spriteBatch.DrawString(_timerFont, _timeRemaining.ToString("0.0"), timerPosition + new Vector2(2, 2), new Color(242f / 255, 70f / 255, 80f / 255, 1f));
                    _spriteBatch.DrawString(_timerFont, _timeRemaining.ToString("0.0"), timerPosition, new Color(252f / 255, 234f / 255, 51f / 255, 1f));
                    break;

                case Screen.TitleScreen:
                    Vector2 mainMenuPosition = new Vector2((_graphics.GraphicsDevice.Viewport.Width - _mainMenuFont.MeasureString(_mainMenu).X) / 2, _graphics.GraphicsDevice.Viewport.Height / 3);
                    _spriteBatch.DrawString(_mainMenuFont, _mainMenu, mainMenuPosition, Color.White);
                    break;

                case Screen.CreditsScreen:
                    break;

                case Screen.GameScreen:
                    break;

                case Screen.PauseScreen:
                    break;

                case Screen.GameOverScreen:
                    string gameOverText = $"{_gameOver} {_score}";
                    Vector2 textSize = _gameOverFont.MeasureString(gameOverText);
                    Vector2 gameOverPosition = new Vector2(
                        (_graphics.GraphicsDevice.Viewport.Width - textSize.X) / 2f,
                        (_graphics.GraphicsDevice.Viewport.Height - textSize.Y) / 2f
                    );
                    // text shadow
                    _spriteBatch.DrawString(_gameOverFont, gameOverText, gameOverPosition + new Vector2(2, 2), new Color(242f / 255, 70f / 255, 80f / 255, 1f));
                    _spriteBatch.DrawString(_gameOverFont, gameOverText, gameOverPosition, new Color(252f / 255, 234f / 255, 51f / 255, 1f));
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
