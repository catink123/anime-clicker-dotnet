using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;

namespace Clicker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int coins = Properties.Settings.Default.coins;
        public int hp = Properties.Settings.Default.hp;
        public int atk = Properties.Settings.Default.atk;
        public int crystals = Properties.Settings.Default.crystals;
        public int level = Properties.Settings.Default.level;
        public int stage = Properties.Settings.Default.stage;
        public int nextPic;
        public int minEnemyHp = 10;
        public int maxEnemyHp = 20;

        Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            coinsLabel.Content = coins;
            hpBar.Value = hp;
            hpLabel.Content = hp;
            crystalsLabel.Content = crystals;
            atkLabel.Content = atk;
            enemyHpBar.Value = Properties.Settings.Default.enemyHp;
            System.Threading.Thread.Sleep(10);
            changelevel();
        }

        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            enemyHpBar.Value = enemyHpBar.Value - atk;
            if (enemyHpBar.Value <= 0)
            {
                changelevel();
            }
        }

        public void changelevel()
        {
            levelInfinity();
            Properties.Settings.Default.hp = hp;
            Properties.Settings.Default.level = level;
            Properties.Settings.Default.stage = stage;
            Properties.Settings.Default.coins = coins;
            Properties.Settings.Default.crystals = crystals;
        }

        public void changeEnemyHp(int value1, int value2)
        {
            enemyHpBar.Maximum = random.Next(value1, value2);
            enemyHpBar.Value = enemyHpBar.Maximum;
        }

        public void changeCoins(int value)
        {
            coins = coins + random.Next(1, value);
            coinsLabel.Content = Convert.ToString(coins);
        }

        public void changeAtk(int value)
        {
            atk = atk + value;
            atkLabel.Content = Convert.ToString(atk);
        }

        public void changePic()
        {
            nextPic = random.Next(1, 5);
            switch(nextPic)
            {
                case 1:
                    enemyImage.Source = new BitmapImage(new Uri("Images/enemyImages/enemy1.png", UriKind.Relative));
                    enemyName.Content = "Орк";
                    break;
                case 2:
                    enemyImage.Source = new BitmapImage(new Uri("Images/enemyImages/enemy2.png", UriKind.Relative));
                    enemyName.Content = "Слизень";
                    break;
                case 3:
                    enemyImage.Source = new BitmapImage(new Uri("Images/enemyImages/enemy3.png", UriKind.Relative));
                    enemyName.Content = "Warder";
                    break;
                case 4:
                    enemyImage.Source = new BitmapImage(new Uri("Images/enemyImages/enemy4.png", UriKind.Relative));
                    enemyName.Content = "Archer";
                    break;
                case 5:
                    enemyImage.Source = new BitmapImage(new Uri("Images/enemyImages/enemy5.png", UriKind.Relative));
                    enemyName.Content = "Black";
                    break;
            }
        }

        public void level1()
        {
            changeEnemyHp(10, 20);
            changePic();
        }

        public void levelInfinity()
        {
            minEnemyHp = minEnemyHp + 3;
            maxEnemyHp = maxEnemyHp + 3;
            changeEnemyHp(minEnemyHp, maxEnemyHp);
            changeCoins(100);
            System.Threading.Thread.Sleep(50);
            changePic();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            changeAtk(12);
        }
    }
}
