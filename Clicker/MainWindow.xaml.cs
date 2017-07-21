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
using System.Collections.Specialized;
using System.Windows.Threading;

namespace Clicker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int coins = Properties.Settings.Default.coins;
        public int xp = Properties.Settings.Default.xp;
        public int atk = Properties.Settings.Default.atk;
        public int crystals = Properties.Settings.Default.crystals;
        public int level = Properties.Settings.Default.level;
        public int stage = Properties.Settings.Default.stage;
        public int spinCost = Properties.Settings.Default.spinCost;
        public int nextPic;
        public int minEnemyHp = 10;
        public int maxEnemyHp = 20;
        public int enemyHp;
        public int randResult;
        public bool isInventoryOpen;
        public int levelXp = Properties.Settings.Default.levelXp;
        public int levelUpXp = Properties.Settings.Default.levelUpXp;
        public int additionXp;
        public bool isBoss = Properties.Settings.Default.isBoss;
        public int dps = Properties.Settings.Default.dps;

        DispatcherTimer dpsTimer = new DispatcherTimer();
        List<string> charList = new List<string>();
        Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            coinsLabel.Content = coins;
            xpBar.Value = xp;
            xpLabel.Content = xp;
            crystalsLabel.Content = crystals;
            atkLabel.Content = atk;
            System.Threading.Thread.Sleep(10);
            changePic();
            charList = Properties.Settings.Default.charList;
            Inventory.ItemsSource = charList;
            Inventory.Items.Refresh();
            xpBar.Maximum = levelUpXp;
            xpBar.Value = xp;
            if (isBoss)
            {
                boss("Images/bossImages/boss1.png", "Kuroro Lucifer");
            }
            enemyHpBar.Value = Properties.Settings.Default.enemyHp;
            spinCostLabel.Content = spinCost;
            levelXpLabel.Content = levelXp;
            dpsLabel.Content = dps;
            dpsTimer.Tick += new EventHandler(dpsOnTimer);
            dpsTimer.Interval = new TimeSpan(0, 0, 1);
            dpsTimer.Start();
        }

        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            enemyHpBar.Value = enemyHpBar.Value - atk;
            enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
            if (enemyHpBar.Value <= 0)
            {
                changelevel();
                if (isBoss)
                {
                    addCrystals(1);
                    isBoss = false;
                }
            }
        }

        public void changelevel()
        {
            level++;
            xp = xp + (enemyHp / 3);
            xpBar.Value = xp;
            xpLabel.Content = Convert.ToString(xp);
            if (xp == levelUpXp)
            {
                xp = 0;
                levelUpXp += random.Next(1, 20);
                levelXp++;
                xpLabel.Content = Convert.ToString(xp);
                xpBar.Maximum = levelUpXp;
                levelXpLabel.Content = Convert.ToString(levelXp);
                changeAtk(1);
            }
            else if (xp > levelUpXp)
            {
                additionXp = xp - levelUpXp;
                xp = additionXp;
                levelUpXp += random.Next(1, 20);
                levelXp++;
                xpLabel.Content = Convert.ToString(xp);
                xpBar.Maximum = levelUpXp;
                xpBar.Value = xp;
                levelXpLabel.Content = Convert.ToString(levelXp);
            }
            if (level == 10)
            {
                boss("Images/bossImages/boss1.png", "Kuroro Lucifer");
                enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
            }
            else if (level == 9)
            {
                levelInfinity();
                enemyHp = Convert.ToInt32(enemyHpBar.Value);
                enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
            }
            else
            {
                levelInfinity();
                enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
            }
            Properties.Settings.Default.xp = xp;
            Properties.Settings.Default.level = level;
            Properties.Settings.Default.stage = stage;
            Properties.Settings.Default.coins = coins;
            Properties.Settings.Default.crystals = crystals;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.levelUpXp = levelUpXp;
            Properties.Settings.Default.levelXp = levelXp;
            enemyHp = Convert.ToInt32(enemyHpBar.Value);
        }

        private void InventoryToggle_Click(object sender, RoutedEventArgs e)
        {
            isInventoryOpen = !isInventoryOpen;
            if (isInventoryOpen)
            {
                Inventory.Visibility = Visibility.Visible;
            }
            else
            {
                Inventory.Visibility = Visibility.Hidden;
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
            addCoins(100);
            System.Threading.Thread.Sleep(50);
            changePic();
        }

        private void spin_Click(object sender, RoutedEventArgs e)
        {
            if (coins > spinCost)
            {
                coins -= spinCost;
                coinsLabel.Content = Convert.ToString(coins);
                randResult = random.Next(1, 20);
                switch (randResult)
                {
                    case 1:
                        charColor.Fill = new SolidColorBrush(Colors.LimeGreen);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero1.png", UriKind.Relative));
                        charName.Content = "Леорио";
                        charList.Add("Леорио");
                        dps += 1;
                        break;
                    case 2:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero2.png", UriKind.Relative));
                        charName.Content = "Хисока";
                        charList.Add("Хисока");
                        dps += 8;
                        break;
                    case 3:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero3.png", UriKind.Relative));
                        charName.Content = "Киллуа";
                        charList.Add("Киллуа");
                        dps += 4;
                        break;
                    case 4:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero4.png", UriKind.Relative));
                        charName.Content = "Гон";
                        charList.Add("Гон");
                        dps += 4;
                        break;
                    case 5:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero5.png", UriKind.Relative));
                        charName.Content = "Курапика";
                        charList.Add("Курапика");
                        dps += 2;
                        break;
                    case 6:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero6.png", UriKind.Relative));
                        charName.Content = "Какаши";
                        charList.Add("Какаши");
                        dps += 4;
                        break;
                    case 7:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero7.png", UriKind.Relative));
                        charName.Content = "Наруто";
                        charList.Add("Наруто");
                        dps += 8;
                        break;
                    case 8:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero8.png", UriKind.Relative));
                        charName.Content = "Обито";
                        charList.Add("Обито");
                        dps += 8;
                        break;
                    case 9:
                        charColor.Fill = new SolidColorBrush(Colors.LimeGreen);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero9.png", UriKind.Relative));
                        charName.Content = "Сакура";
                        charList.Add("Сакура");
                        dps += 1;
                        break;
                    case 10:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero10.png", UriKind.Relative));
                        charName.Content = "Саске";
                        charList.Add("Саске");
                        dps += 8;
                        break;
                    case 11:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero11.png", UriKind.Relative));
                        charName.Content = "Санджи";
                        charList.Add("Санджи");
                        dps += 4;
                        break;
                    case 12:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero12.png", UriKind.Relative));
                        charName.Content = "Зоро";
                        charList.Add("Зоро");
                        dps += 4;
                        break;
                    case 13:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero13.png", UriKind.Relative));
                        charName.Content = "Нами";
                        charList.Add("Нами");
                        dps += 8;
                        break;
                    case 14:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero14.png", UriKind.Relative));
                        charName.Content = "Усопп";
                        charList.Add("Усопп");
                        dps += 2;
                        break;
                    case 15:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero15.png", UriKind.Relative));
                        charName.Content = "Monkey D. Luffy";
                        charList.Add("Monkey D. Luffy");
                        dps += 8;
                        break;
                    case 16:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero16.png", UriKind.Relative));
                        charName.Content = "Kurosaki Ichigo";
                        charList.Add("Kurosaki Ichigo");
                        dps += 8;
                        break;
                    case 17:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero17.png", UriKind.Relative));
                        charName.Content = "Zaraki Kenpachi";
                        charList.Add("Zaraki Kenpachi");
                        dps += 4;
                        break;
                    case 18:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero18.png", UriKind.Relative));
                        charName.Content = "Byakuya Kuchiki";
                        charList.Add("Byakuya Kuchiki");
                        dps += 4;
                        break;
                    case 19:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero19.png", UriKind.Relative));
                        charName.Content = "Toshiro Hitsugaya";
                        charList.Add("Toshiro Hitsugaya");
                        dps += 4;
                        break;
                    case 20:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero20.png", UriKind.Relative));
                        charName.Content = "Inoue Orihime";
                        charList.Add("Inoue Orihime");
                        dps += 2;
                        break;
                    default:
                        break;
                }
                spinCost += 10;
                dpsLabel.Content = dps;
                spinCostLabel.Content = Convert.ToString(spinCost);
                Properties.Settings.Default.spinCost = spinCost;
                Properties.Settings.Default.charList = charList;
                Properties.Settings.Default.dps = dps;
                Properties.Settings.Default.Save();
                Inventory.Items.Refresh();
            }
            else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        public void addCrystals(int value)
        {
            crystals++;
            crystalsLabel.Content = Convert.ToString(crystals);
        }

        public void changeAtk(int value)
        {
            atk = atk + value;
            atkLabel.Content = Convert.ToString(atk);
        }

        public void addCoins(int value)
        {
            coins = coins + random.Next(1, value);
            coinsLabel.Content = Convert.ToString(coins);
        }

        public void changeEnemyHp(int value1, int value2)
        {
            enemyHpBar.Maximum = random.Next(value1, value2);
            enemyHpBar.Value = enemyHpBar.Maximum;
        }

        private void boss(string imagePath, string bossName)
        {
            stage++;
            level = 0;
            enemyImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            enemyName.Content = bossName;
            enemyHpBar.Maximum = enemyHp * 2;
            enemyHpBar.Value = enemyHpBar.Maximum;
            isBoss = true;
        }

        private void changePic()
        {
            nextPic = random.Next(1, 5);
            switch (nextPic)
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

        public void dpsOnTimer(object sender, EventArgs e)
        {
            enemyHpBar.Value = enemyHpBar.Value - dps;
            enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
            if (enemyHpBar.Value <= 0)
            {
                changelevel();
                if (isBoss)
                {
                    addCrystals(1);
                    isBoss = false;
                }

            }

            void changelevel()
            {
                level++;
                xp = xp + (enemyHp / 3);
                xpBar.Value = xp;
                xpLabel.Content = Convert.ToString(xp);
                if (xp == levelUpXp)
                {
                    xp = 0;
                    levelUpXp += random.Next(1, 20);
                    levelXp++;
                    xpLabel.Content = Convert.ToString(xp);
                    xpBar.Maximum = levelUpXp;
                    levelXpLabel.Content = Convert.ToString(levelXp);
                    changeAtk(1);
                }
                else if (xp > levelUpXp)
                {
                    additionXp = xp - levelUpXp;
                    xp = additionXp;
                    levelUpXp += random.Next(1, 20);
                    levelXp++;
                    xpLabel.Content = Convert.ToString(xp);
                    xpBar.Maximum = levelUpXp;
                    xpBar.Value = xp;
                    levelXpLabel.Content = Convert.ToString(levelXp);
                }
                if (level == 10)
                {
                    boss("Images/bossImages/boss1.png", "Kuroro Lucifer");
                    enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
                }
                else if (level == 9)
                {
                    levelInfinity();
                    enemyHp = Convert.ToInt32(enemyHpBar.Value);
                    enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
                }
                else
                {
                    levelInfinity();
                    enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
                }
                Properties.Settings.Default.xp = xp;
                Properties.Settings.Default.level = level;
                Properties.Settings.Default.stage = stage;
                Properties.Settings.Default.coins = coins;
                Properties.Settings.Default.crystals = crystals;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.levelUpXp = levelUpXp;
                Properties.Settings.Default.levelXp = levelXp;
                enemyHp = Convert.ToInt32(enemyHpBar.Value);
            }
        }
    }
}
