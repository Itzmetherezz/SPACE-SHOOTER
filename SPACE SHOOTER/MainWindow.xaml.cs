using Accessibility;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SPACE_SHOOTER
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
    
        DispatcherTimer timer = new DispatcherTimer();
        bool moveLeft, moveRight;
        List<Rectangle> itemRemover = new List<Rectangle>();
        Random rand = new Random();

        int enemySpriteCount = 0;
        int enemyCount = 100;
        int playerSpeed = 15;
        int limit = 50;
        int score = 0;
        int dmg = 100;
        int eneSpeed = 10;


        Rect PlayerHitBox;


        public MainWindow()
        {
            InitializeComponent();


            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += GameLoop;
            timer.Start();

            MyCanvas.Focus();

            ImageBrush BackGround = new ImageBrush();

            BackGround.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ASSETS/purple.png"));
            BackGround.TileMode = TileMode.Tile;
            BackGround.Viewport = new Rect(0, 0, 0.15, 0.15);
            BackGround.ViewportUnits=BrushMappingMode.RelativeToBoundingBox;
            MyCanvas.Background = BackGround;


            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ASSETS/player.png")); 
            player.Fill = playerImage;



        }











        private void GameLoop(object? sender, EventArgs e)
        {
            PlayerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            enemyCount -= 1;

            scoreText.Content = "Score: " + score;
            damageText.Content = "Health: " + dmg;

            if (enemyCount < 0)
            {
                MakeEnemies();
                enemyCount = limit;

            }
            if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);

            }
            if (moveRight== true && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if(x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x) , x.Width, x.Height);

                    if (Canvas.GetTop(x) < 10)
                    {
                        itemRemover.Add(x);
                    }

                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())

                    {
                        if(y is Rectangle &&  (string)y.Tag == "enemy")
                        {
                            Rect enemyHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bulletHitBox.IntersectsWith(enemyHit))
                            {
                                itemRemover.Add(x);
                                itemRemover.Add(y);
                                score++;


                            }
                        }

                    }

 

                }
                if (x is Rectangle && (string)x.Tag == "enemy")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + eneSpeed);

                    if(Canvas.GetTop(x) > 750)
                    {
                        itemRemover.Add(x);
                        dmg -= 10;
                    }

                    Rect enemyHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if(PlayerHitBox.IntersectsWith(enemyHitBox)) 
                    {
                        itemRemover.Add(x); 
                        dmg -= 5;
                    }
                }









            }

            foreach (Rectangle i in itemRemover)
            {
                MyCanvas.Children.Remove(i);

            }


            if (score > 10)
            {

                limit = 20;
                eneSpeed = 15;

                   
            }
            if (dmg <= 0)
            {
                GameOver();

            }









            
        }



        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;



            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
            
            
            if (e.Key==Key.R)
            {
                RestartGame();

            }




        }

        private void OnlyKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;



            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }

            if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red

                };

                Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);
                Canvas.SetTop(newBullet, Canvas.GetTop(player)- newBullet.Height);

                MyCanvas.Children.Add(newBullet);



            }


        }

        private void MakeEnemies() 
        {
            ImageBrush enemySprite = new ImageBrush();

            enemySpriteCount = rand.Next(1, 5);

            switch (enemySpriteCount)
            {
                case 1:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ASSETS/1.png"));
                    break;
                case 2:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ASSETS/2.png"));
                    break;
                case 3:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ASSETS/3.png"));
                    break;
                case 4:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ASSETS/4.png"));
                    break;
                case 5:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ASSETS/5.png"));
                    break;
            



            }

            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 50,
                Width = 56,
                Fill = enemySprite


            };

            Canvas.SetTop(newEnemy, -100);
            Canvas.SetLeft(newEnemy, rand.Next(30, 430));
            MyCanvas.Children.Add(newEnemy);









        }
        private void GameOver()
        {
            timer.Stop();
            GameOverScreen.Visibility = Visibility.Visible;
            FinalScore.Text = "Ships Destroyed: " + score;
        }

        private void RestartGame()
        {
            
            GameOverScreen.Visibility = Visibility.Collapsed;
            score = 0;
            dmg = 100;
            eneSpeed = 10;
            MyCanvas.Children.Clear();
            MyCanvas.Children.Add(player);
            MyCanvas.Children.Add(scoreText);
            MyCanvas.Children.Add(damageText);
            Canvas.SetLeft(player, 246);
            Canvas.SetTop(player, 498);
            timer.Start();
        }
        private void RestartGame(object sender, RoutedEventArgs e)
        {
            RestartGame();
        }

          

                
    }
}