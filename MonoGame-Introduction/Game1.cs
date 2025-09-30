using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Collections.Generic;

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
        private List<string> _highScores = new List<string>();
        private int _highestScore;

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
            
            LoadHighScores();
        }
        //method that loads high scores not working at the moment i think it might be that monogame doesn't recogize txt files
        private void LoadHighScores()
        {
            string path = Path.Combine(Content.RootDirectory, "HighScore.txt");
            if (File.Exists(path))
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    _highScores.Add(line);
                    // parses score like name:score
                    var parts = line.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                    {
                        if (score > _highestScore)
                            _highestScore = score;
                    }
                }
            }
            else
            {
                _highScores = new List<string> { "No high scores found." };
            }
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
                    _timeRemaining += (float)gameTime.ElapsedGameTime.TotalSeconds; //have a feeling this just unpauses the timer and repauses again instead of resetting it 
                    MouseState mouseGame = Mouse.GetState();
                    if (_rectangle.Contains(mouseGame.Position) && mouseGame.LeftButton == ButtonState.Pressed)
                    {
                        //_screen = Screen.CreditsScreen;
                        //need to fix this so it resets for real 
                        
                        _score = 0;
                        GoToGameOver();
                        break;
                    }
                    if (_timeRemaining > 15f)
                    {
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
                // Linear scale 
                float ratio = _timeRemaining / timeLimit;
                _score = (int)System.MathF.Round(ratio * maxScore);
                _timeRemaining = 0f;

            }
            else
            {
                _timeRemaining = 0;
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
                    //have a feeling this is why rectangles and textrues load here and not in the gamescreen since it is empty nvm fixed it
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
                    _spriteBatch.Draw(_studioLogo, _rectangle, Color.White);
                     Vector2 timerPositionGame = new Vector2((_graphics.GraphicsDevice.Viewport.Width - _timerFont.MeasureString(_timeRemaining.ToString("0.0")).X) / 2, (_graphics.GraphicsDevice.Viewport.Height - _timerFont.MeasureString(_timeRemaining.ToString("0.0")).Y) / 2);

                    _spriteBatch.DrawString(_timerFont, _timeRemaining.ToString("0.0"), timerPositionGame + new Vector2(2, 2), new Color(242f / 255, 70f / 255, 80f / 255, 1f));
                    _spriteBatch.DrawString(_timerFont, _timeRemaining.ToString("0.0"), timerPositionGame, new Color(252f / 255, 234f / 255, 51f / 255, 1f));
                    break;

                case Screen.PauseScreen:
                    break;

                case Screen.GameOverScreen:
                    //updated high score to include player highest score
                    bool isHighScore = _score >= _highestScore && _score > 0;
                    string highScoreMsg = isHighScore ? "\n\nBravo! You Got a New High Score!" : "";
                    string gameOverText = $"{_gameOver} {_score}\n\nHighest Score: {_highestScore}{highScoreMsg}";
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
