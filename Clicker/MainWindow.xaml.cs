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
using System.Windows.Threading;
using System.Media;
using System.IO;
using Ionic.Zip;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;

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
        public int levelXp = Properties.Settings.Default.levelXp;
        public int levelUpXp = Properties.Settings.Default.levelUpXp;
        public int additionXp;
        public bool isBoss = Properties.Settings.Default.isBoss;
        public int dps = Properties.Settings.Default.dps;
        public int cps = Properties.Settings.Default.cps;

        DispatcherTimer Timer = new DispatcherTimer();
        ImageBrush imgBrush = new ImageBrush();
        SoundPlayer attackSound1 = new SoundPlayer(Properties.Resources.attack1);
        SoundPlayer attackSound2 = new SoundPlayer(Properties.Resources.attack2);
        SoundPlayer attackSound3 = new SoundPlayer(Properties.Resources.attack3);
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
            Timer.Tick += new EventHandler(OnTimer);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
            System.Threading.Thread.Sleep(50);
            Inventory.ItemsSource = Properties.Settings.Default.charList;
            Inventory.Items.Refresh();
        }

        public void HurtAnimation()
        {
            Storyboard storyboard = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1.0, 1.0);
            enemyImage.RenderTransform = scale;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromMilliseconds(100);
            growAnimation.From = 1;
            growAnimation.To = 0.9;
            growAnimation.AutoReverse = true;
            storyboard.Children.Add(growAnimation);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, enemyImage);

            storyboard.Begin();
        }

        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            randResult = random.Next(1, 4);
            switch (randResult)
            {
                case 1:
                    attackSound1.Play();
                    break;
                case 2:
                    attackSound2.Play();
                    break;
                case 3:
                    attackSound3.Play();
                    break;
            }
            HurtAnimation();
            enemyHpBar.Value = enemyHpBar.Value - atk;
            enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
            if (enemyHpBar.Value <= 0)
            {
                changelevel();
                if (isBoss)
                {
                    addCrystals(random.Next(1,5));
                    isBoss = false;
                }
                xp = xp + (enemyHp / 3);
                xpBar.Value = xp;
                xpLabel.Content = Convert.ToString(xp);
                if (xp == levelUpXp)
                {
                    atk += 2;
                    atkLabel.Content = atk;
                    xp = 0;
                    levelUpXp += random.Next(1, 20);
                    levelXp++;
                    xpLabel.Content = Convert.ToString(xp);
                    xpBar.Maximum = levelUpXp;
                    xpBar.Value = xp;
                    levelXpLabel.Content = Convert.ToString(levelXp);
                }
                else if (xp > levelUpXp)
                {
                    atk += 2;
                    atkLabel.Content = atk;
                    additionXp = xp - levelUpXp;
                    xp = additionXp;
                    levelUpXp += random.Next(1, 20);
                    levelXp++;
                    xpLabel.Content = Convert.ToString(xp);
                    xpBar.Maximum = levelUpXp;
                    xpBar.Value = xp;
                    levelXpLabel.Content = Convert.ToString(levelXp);
                }
            }
            Properties.Settings.Default.atk = atk;
        }

        public void changelevel()
        {
            level++;
            if (level == 10)
            {
                level = 0;
                stage++;
                randResult = random.Next(1, 9);
                switch (randResult)
                {
                    case 1:
                        boss("Images/bossImages/boss1.png", "Куроро Люцифер");
                        break;
                    case 2:
                        boss("Images/bossImages/boss2.png", "Мадара Утиха");
                        break;
                    case 3:
                        boss("Images/bossImages/boss3.png", "Зереф Драгнил");
                        break;
                    case 4:
                        boss("Images/bossImages/boss4.png", "Донкихот Дофламинго");
                        break;
                    case 5:
                        boss("Images/bossImages/boss5.png", "Яхве Бах");
                        break;
                    case 6:
                        boss("Images/bossImages/Dio.png", "Дио Бранде");
                        break;
                    case 7:
                        boss("Images/bossImages/Frieza.png", "Фриза");
                        break;
                    case 8:
                        boss("Images/bossImages/Gilgamesh.png", "Гильгамеш");
                        break;
                    default:
                        boss("Images/bossImages/boss1.png", "Куроро Люцифер");
                        break;
                }
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
            OpenInventory();
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

        public void changeCharacter(Color color, string pathToImage, string characterName, int addDps)
        {
            charColor.Fill = new SolidColorBrush(color);
            charImage.Source = new BitmapImage(new Uri(pathToImage, UriKind.Relative));
            charName.Content = characterName;
            Properties.Settings.Default.charList.Add(characterName);
            dps += addDps;
        }

        public void changeCrystalCharacter(Color color, string pathToImage, string characterName, int addDps)
        {
            crystalSpinner.Visibility = Visibility.Visible;
            crystalCharColor.Fill = new SolidColorBrush(color);
            crystalCharImage.Source = new BitmapImage(new Uri(pathToImage, UriKind.Relative));
            crystalCharName.Content = characterName;
            Properties.Settings.Default.charList.Add(characterName);
            dps += addDps;
            dpsLabel.Content = dps;
            Properties.Settings.Default.dps = dps;
            Properties.Settings.Default.crystals = crystals;
            Properties.Settings.Default.Save();
            Inventory.Items.Refresh();
        }

        public void Special1()
        {
            crystalSpinner.Visibility = Visibility.Visible;
            crystalCharColor.Fill = new SolidColorBrush(Colors.Red);
            crystalCharImage.Source = new BitmapImage(new Uri("Images/specialHeroImages/koshena.jpg", UriKind.Relative));
            crystalCharName.Content = "Рухлять Оранжевая";
            Properties.Settings.Default.charList.Add("Рухлять Оранжевая");
            cps += 5;
            Inventory.Items.Refresh();
        }

        public void addChar(string query)
        {
            bool isExist = false;
            for (int i = 0; i < Inventory.Items.Count; i++)
            {
                var s = Inventory.Items[i].ToString();
                if (s.StartsWith(query))
                {
                    if (s == query)
                    {
                        Inventory.Items[i] = query + "x2";
                        isExist = true;
                        break;
                    }
                    else
                    {
                        // Escape your plain text before use with regex
                        var pattern = Regex.Escape(query);
                        // Check if s has this formnat: queryx2, queryx3, queryx4, ...
                        Match m = Regex.Match(s, "^" + pattern + @"x(\d+)$");
                        if (m.Success)
                        {
                            Inventory.Items[i] = query + "x" + (Int32.Parse(m.Groups[1].Value) + 1);
                            isExist = true;
                            break;
                        }
                    }
                }
            }
            if (!isExist) Inventory.Items.Add(query);
        }

        private void spin_Click(object sender, RoutedEventArgs e)
        {
            if (coins > spinCost)
            {
                coins -= spinCost;
                coinsLabel.Content = Convert.ToString(coins);
                randResult = random.Next(1, 50);
                switch (randResult)
                {
                    case 1:
                        charColor.Fill = new SolidColorBrush(Colors.LimeGreen);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero1.png", UriKind.Relative));
                        charName.Content = "Леорио";
                        Properties.Settings.Default.charList.Add("Леорио");
                        dps += 1;
                        break;
                    case 2:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero2.png", UriKind.Relative));
                        charName.Content = "Хисока";
                        Properties.Settings.Default.charList.Add("Хисока");
                        dps += 8;
                        break;
                    case 3:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero3.png", UriKind.Relative));
                        charName.Content = "Киллуа";
                        Properties.Settings.Default.charList.Add("Киллуа");
                        dps += 4;
                        break;
                    case 4:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero4.png", UriKind.Relative));
                        charName.Content = "Гон";
                        Properties.Settings.Default.charList.Add("Гон");
                        dps += 4;
                        break;
                    case 5:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero5.png", UriKind.Relative));
                        charName.Content = "Курапика";
                        Properties.Settings.Default.charList.Add("Курапика");
                        dps += 2;
                        break;
                    case 6:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero6.png", UriKind.Relative));
                        charName.Content = "Какаши";
                        Properties.Settings.Default.charList.Add("Какаши");
                        dps += 4;
                        break;
                    case 7:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero7.png", UriKind.Relative));
                        charName.Content = "Наруто";
                        Properties.Settings.Default.charList.Add("Наруто");
                        dps += 8;
                        break;
                    case 8:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero8.png", UriKind.Relative));
                        charName.Content = "Обито";
                        Properties.Settings.Default.charList.Add("Обито");
                        dps += 8;
                        break;
                    case 9:
                        charColor.Fill = new SolidColorBrush(Colors.LimeGreen);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero9.png", UriKind.Relative));
                        charName.Content = "Сакура";
                        Properties.Settings.Default.charList.Add("Сакура");
                        dps += 1;
                        break;
                    case 10:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero10.png", UriKind.Relative));
                        charName.Content = "Саске";
                        Properties.Settings.Default.charList.Add("Саске");
                        dps += 8;
                        break;
                    case 11:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero11.png", UriKind.Relative));
                        charName.Content = "Санджи";
                        Properties.Settings.Default.charList.Add("Санджи");
                        dps += 4;
                        break;
                    case 12:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero12.png", UriKind.Relative));
                        charName.Content = "Зоро";
                        Properties.Settings.Default.charList.Add("Зоро");
                        dps += 4;
                        break;
                    case 13:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero13.png", UriKind.Relative));
                        charName.Content = "Нами";
                        Properties.Settings.Default.charList.Add("Нами");
                        dps += 8;
                        break;
                    case 14:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero14.png", UriKind.Relative));
                        charName.Content = "Усопп";
                        Properties.Settings.Default.charList.Add("Усопп");
                        dps += 2;
                        break;
                    case 15:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero15.png", UriKind.Relative));
                        charName.Content = "Луффи";
                        Properties.Settings.Default.charList.Add("Луффи");
                        dps += 8;
                        break;
                    case 16:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero16.png", UriKind.Relative));
                        charName.Content = "Ичиго";
                        Properties.Settings.Default.charList.Add("Ичиго");
                        dps += 8;
                        break;
                    case 17:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero17.png", UriKind.Relative));
                        charName.Content = "Кенпачи";
                        Properties.Settings.Default.charList.Add("Кенпачи");
                        dps += 4;
                        break;
                    case 18:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero18.png", UriKind.Relative));
                        charName.Content = "Бьякуя";
                        Properties.Settings.Default.charList.Add("Бьякуя");
                        dps += 4;
                        break;
                    case 19:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero19.png", UriKind.Relative));
                        charName.Content = "Хицугая";
                        Properties.Settings.Default.charList.Add("Хицугая");
                        dps += 4;
                        break;
                    case 20:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero20.png", UriKind.Relative));
                        charName.Content = "Орихиме";
                        Properties.Settings.Default.charList.Add("Орихиме");
                        dps += 2;
                        break;
                    case 21:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero21.png", UriKind.Relative));
                        charName.Content = "Лелуш";
                        Properties.Settings.Default.charList.Add("Лелуш");
                        dps += 2;
                        break;
                    case 22:
                        charColor.Fill = new SolidColorBrush(Colors.LimeGreen);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero22.png", UriKind.Relative));
                        charName.Content = "Харухи";
                        Properties.Settings.Default.charList.Add("Харухи");
                        dps += 1;
                        break;
                    case 23:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero23.png", UriKind.Relative));
                        charName.Content = "Натсу";
                        Properties.Settings.Default.charList.Add("Натсу");
                        dps += 8;
                        break;
                    case 24:
                        charColor.Fill = new SolidColorBrush(Colors.LimeGreen);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero24.png", UriKind.Relative));
                        charName.Content = "Люси";
                        Properties.Settings.Default.charList.Add("Люси");
                        dps += 1;
                        break;
                    case 25:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero25.png", UriKind.Relative));
                        charName.Content = "Элиза";
                        Properties.Settings.Default.charList.Add("Элиза");
                        dps += 2;
                        break;
                    case 26:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero26.png", UriKind.Relative));
                        charName.Content = "Лаксус";
                        Properties.Settings.Default.charList.Add("Лаксус");
                        dps += 2;
                        break;
                    case 27:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero27.png", UriKind.Relative));
                        charName.Content = "Грэй";
                        Properties.Settings.Default.charList.Add("Грэй");
                        dps += 2;
                        break;
                    case 28:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero28.png", UriKind.Relative));
                        charName.Content = "Гатс";
                        Properties.Settings.Default.charList.Add("Гатс");
                        dps += 8;
                        break;
                    case 29:
                        changeCharacter(Colors.Orange, "Images/heroImages/Saber.png", "Сэйбер", 8);
                        break;
                    case 30:
                        changeCharacter(Colors.Purple, "Images/heroImages/Berserk.png", "Берсерк", 4);
                        break;
                    case 31:
                        changeCharacter(Colors.Purple, "Images/heroImages/Rider.png", "Райдер", 4);
                        break;
                    case 32:
                        changeCharacter(Colors.Blue, "Images/heroImages/Lancer.png", "Лансер", 2);
                        break;
                    case 33:
                        changeCharacter(Colors.Orange, "Images/heroImages/Shiro.png", "Широ", 8);
                        break;
                    case 34:
                        changeCharacter(Colors.Purple, "Images/heroImages/Giorno.png", "Джорно", 4);
                        break;
                    case 35:
                        changeCharacter(Colors.Purple, "Images/heroImages/Jonathan.png", "Джонатан", 4);
                        break;
                    case 36:
                        changeCharacter(Colors.Purple, "Images/heroImages/Josuke.png", "Джоске", 4);
                        break;
                    case 37:
                        changeCharacter(Colors.Blue, "Images/heroImages/Joseph.png", "Джозеф", 2);
                        break;
                    case 38:
                        changeCharacter(Colors.Orange, "Images/heroImages/Jotaro.png", "Джотаро", 8);
                        break;
                    case 39:
                        changeCharacter(Colors.Purple, "Images/heroImages/Gohan.png", "Гохан", 4);
                        break;
                    case 40:
                        changeCharacter(Colors.Orange, "Images/heroImages/Goku.png", "Гоку", 8);
                        break;
                    case 41:
                        changeCharacter(Colors.Blue, "Images/heroImages/Roshi.png", "Мастер Роши", 2);
                        break;
                    case 42:
                        changeCharacter(Colors.Blue, "Images/heroImages/Piccolo.png", "Пикколо", 2);
                        break;
                    case 43:
                        changeCharacter(Colors.Purple, "Images/heroImages/Vegeta.png", "Веджета", 4);
                        break;
                    case 44:
                        changeCharacter(Colors.Blue, "Images/heroImages/Kirito.png", "Кирито", 2);
                        break;
                    case 45:
                        changeCharacter(Colors.LimeGreen, "Images/heroImages/Onizuka.png", "Онидзука", 1);
                        break;
                    case 46:
                        changeCharacter(Colors.Purple, "Images/heroImages/OmaShu.png", "Сю", 4);
                        break;
                    case 47:
                        changeCharacter(Colors.Orange, "Images/heroImages/Puck.png", "✮Пак", 16);
                        break;
                    case 48:
                        changeCharacter(Colors.Purple, "Images/heroImages/Sakamoto.png", "✮Сакамото", 8);
                        break;
                    case 49:
                        changeCharacter(Colors.Purple, "Images/heroImages/LessRider.png", "✮Бесправный Ездок", 8);
                        break;
                    default:
                        break;
                }
                spinCost += 50;
                dpsLabel.Content = dps;
                spinCostLabel.Content = Convert.ToString(spinCost);
                Properties.Settings.Default.spinCost = spinCost;
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
            atk += value;
            atkLabel.Content = atk;
        }

        public void changeCoins(int value)
        {
            coins = coins + random.Next(1, value);
            coinsLabel.Content = Convert.ToString(coins);
        }

        public void addCoins(int value)
        {
            coins += value;
            coinsLabel.Content = coins;
        }

        public void removeCoins(int value)
        {
            coins -= value;
            coinsLabel.Content = coins;
        }

        public void addAtk(int value)
        {
            atk += value;
            atkLabel.Content = atk;
        }

        public void removeAtk(int value)
        {
            atk -= value;
            atkLabel.Content = atk;
        }

        public void changeEnemyHp(int value1, int value2)
        {
            enemyHpBar.Maximum = random.Next(value1, value2);
            enemyHpBar.Value = enemyHpBar.Maximum;
        }

        private void boss(string imagePath, string bossName)
        {
            enemyImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            enemyName.Content = bossName;
            enemyHpBar.Maximum = enemyHp * 2;
            enemyHpBar.Value = enemyHpBar.Maximum;
            isBoss = true;
        }

        private void changePic()
        {
            nextPic = random.Next(1, 10);
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
                    enemyName.Content = "Тёмный рыцарь";
                    break;
                case 4:
                    enemyImage.Source = new BitmapImage(new Uri("Images/enemyImages/enemy4.png", UriKind.Relative));
                    enemyName.Content = "Лучник";
                    break;
                case 5:
                    enemyImage.Source = new BitmapImage(new Uri("Images/enemyImages/enemy5.png", UriKind.Relative));
                    enemyName.Content = "Крестоносец";
                    break;
                case 6:
                    enemyImage.Source = new BitmapImage(new Uri("Images/enemyImages/enemy6.png", UriKind.Relative));
                    enemyName.Content = "Голем";
                    break;
                case 7:
                    enemyImage.Source = new BitmapImage(new Uri("Images/enemyImages/enemy7.png", UriKind.Relative));
                    enemyName.Content = "Огр";
                    break;
                case 8:
                    enemyImage.Source = new BitmapImage(new Uri("Images/enemyImages/enemy8.png", UriKind.Relative));
                    enemyName.Content = "Рыцарь";
                    break;
                case 9:
                    enemyImage.Source = new BitmapImage(new Uri("Images/enemyImages/enemy9.png", UriKind.Relative));
                    enemyName.Content = "Тролль";
                    break;
            }
        }

        public void OnTimer(object sender, EventArgs e)
        {
            enemyHpBar.Value = enemyHpBar.Value - dps;
            enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
            addCoins(cps);

            if (enemyHpBar.Value <= 0)
            {
                xp = xp + (enemyHp / 3);
                xpBar.Value = xp;
                xpLabel.Content = Convert.ToString(xp);
                if (xp == levelUpXp)
                {
                    atk += 2;
                    atkLabel.Content = atk;
                    xp = 0;
                    levelUpXp += random.Next(1, 20);
                    levelXp++;
                    xpLabel.Content = Convert.ToString(xp);
                    xpBar.Maximum = levelUpXp;
                    levelXpLabel.Content = Convert.ToString(levelXp);
                }
                else if (xp > levelUpXp)
                {
                    atk += 2;
                    atkLabel.Content = atk;
                    additionXp = xp - levelUpXp;
                    xp = additionXp;
                    levelUpXp += random.Next(1, 20);
                    levelXp++;
                    xpLabel.Content = Convert.ToString(xp);
                    xpBar.Maximum = levelUpXp;
                    xpBar.Value = xp;
                    levelXpLabel.Content = Convert.ToString(levelXp);
                }
                changelevel();
                if (isBoss)
                {
                    addCrystals(1);
                    isBoss = false;
                }

            }
            Properties.Settings.Default.atk = atk;
        }

        private void weapon1Buy_Click(object sender, RoutedEventArgs e)
        {
            if (coins >= 35)
            {
                addAtk(1);
                removeCoins(35);
                Properties.Settings.Default.atk = atk;
                Properties.Settings.Default.coins = coins;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void weapon2Buy_Click(object sender, RoutedEventArgs e)
        {
            if (coins >= 70)
            {
                addAtk(2);
                removeCoins(70);
                Properties.Settings.Default.atk = atk;
                Properties.Settings.Default.coins = coins;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void weapon3Buy_Click(object sender, RoutedEventArgs e)
        {
            if (coins >= 1024)
            {
                addAtk(40);
                removeCoins(1024);
                Properties.Settings.Default.atk = atk;
                Properties.Settings.Default.coins = coins;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            crystalSpinner.Visibility = Visibility.Hidden;
        }

        private void box1Spin_Click(object sender, RoutedEventArgs e)
        {
            if (crystals >= 5)
            {
                crystals -= 5;
                crystalsLabel.Content = crystals;
                randResult = random.Next(1, 19);
                switch (randResult)
                {
                    case 1:
                        changeCrystalCharacter(Colors.LimeGreen, "Images/heroFaceImages/LeorioFace.jpg", "Леорио", 2);
                        break;
                    case 2:
                        changeCrystalCharacter(Colors.LimeGreen, "Images/heroFaceImages/SakuraFace.jpg", "Сакура", 2);
                        break;
                    case 3:
                        changeCrystalCharacter(Colors.LimeGreen, "Images/heroFaceImages/HaruhiFace.jpg", "Харухи", 2);
                        break;
                    case 4:
                        changeCrystalCharacter(Colors.LimeGreen, "Images/heroFaceImages/LucyFace.jpg", "Люсси", 2);
                        break;
                    case 5:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/KurapikaFace.png", "Курапика", 4);
                        break;
                    case 6:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/NamiFace.jpg", "Нами", 4);
                        break;
                    case 7:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/UsoppFace.jpg", "Усопп", 4);
                        break;
                    case 8:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/OrihimeFace.jpg", "Орихиме", 4);
                        break;
                    case 9:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/LelouchFace.png", "Лелуш", 4);
                        break;
                    case 10:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/ErzaFace.jpg", "Эльза", 4);
                        break;
                    case 11:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/LaxusFace.png", "Лаксус", 4);
                        break;
                    case 12:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/GrayFace.png", "Грэй", 4);
                        break;
                    case 13:
                        changeCrystalCharacter(Colors.LimeGreen, "Images/heroFaceImages/OnizukaFace.jpg", "Онидзука", 2);
                        break;
                    case 14:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/RoshiFace.jpg", "Мастер Роши", 4);
                        break;
                    case 15:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/PiccoloFace.jpg", "Пикколо", 4);
                        break;
                    case 16:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/LancerFace.jpg", "Лансер", 4);
                        break;
                    case 17:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/JosephFace.png", "Джозеф", 4);
                        break;
                    case 18:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/KiritoFace.png", "Кирито", 4);
                        break;
                }
            }
            else
            {
                MessageBox.Show("У вас не хватает кристалов!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void box2Spin_Click(object sender, RoutedEventArgs e)
        {
            if (crystals >= 15)
            {
                crystals -= 15;
                crystalsLabel.Content = crystals;
                randResult = random.Next(1, 33);
                switch (randResult)
                {
                    case 1:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/KurapikaFace.png", "Курапика", 4);
                        break;
                    case 2:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/NamiFace.jpg", "Нами", 4);
                        break;
                    case 3:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/UsoppFace.jpg", "Усопп", 4);
                        break;
                    case 4:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/OrihimeFace.jpg", "Орихиме", 4);
                        break;
                    case 5:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/LelouchFace.png", "Лелуш", 4);
                        break;
                    case 6:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/ErzaFace.jpg", "Эльза", 4);
                        break;
                    case 7:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/LaxusFace.png", "Лаксус", 4);
                        break;
                    case 8:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/GrayFace.png", "Грэй", 4);
                        break;
                    case 9:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KilluaFace.png", "Киллуа", 8);
                        break;
                    case 10:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/GonFace.jpg", "Гон", 8);
                        break;
                    case 11:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KakashiFace.png", "Какаши", 8);
                        break;
                    case 12:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/SanjiFace.jpg", "Санджи", 8);
                        break;
                    case 13:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/ZoroFace.jpg", "Зоро", 8);
                        break;
                    case 14:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KenpachiFace.jpg", "Кенпачи", 8);
                        break;
                    case 15:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/ByakuyaFace.jpg", "Бьякуя", 8);
                        break;
                    case 16:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/HitsugayaFace.jpg", "Хицугая", 8);
                        break;
                    case 17:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/RoshiFace.jpg", "Мастер Роши", 4);
                        break;
                    case 18:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/PiccoloFace.jpg", "Пикколо", 4);
                        break;
                    case 19:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/LancerFace.jpg", "Лансер", 4);
                        break;
                    case 20:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/JosephFace.png", "Джозеф", 4);
                        break;
                    case 21:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/BerserkFace.jpg", "Берсерк", 8);
                        break;
                    case 22:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/GiornoFace.jpg", "Джорно", 8);
                        break;
                    case 23:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/GohanFace.png", "Гохан", 8);
                        break;
                    case 24:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/JonathanFace.jpg", "Джонатан", 8);
                        break;
                    case 25:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/JosukeFace.jpg", "Джоске", 8);
                        break;
                    case 26:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/GohanFace.jpg", "Гохан", 8);
                        break;
                    case 27:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/KiritoFace.png", "Кирито", 4);
                        break;
                    case 28:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/LessRiderFace.png", "✭Бесправный Ездок", 8);
                        break;
                    case 29:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/OmaShuFace.jpg", "Сю", 8);
                        break;
                    case 30:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/RiderFace.jpg", "Райдер", 8);
                        break;
                    case 31:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/SakamotoFace.jpg", "✭Сакамото", 8);
                        break;
                    case 32:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/VegetaFace.jpg", "Веджета", 8);
                        break;
                }
            }
            else
            {
                MessageBox.Show("У вас не хватает кристалов!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void box3Spin_Click(object sender, RoutedEventArgs e)
        {
            if (crystals >= 30)
            {
                crystals -= 30;
                crystalsLabel.Content = crystals;
                randResult = random.Next(1, 33);
                switch (randResult)
                {
                    case 1:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KilluaFace.png", "Киллуа", 8);
                        break;
                    case 2:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/GonFace.jpg", "Гон", 8);
                        break;
                    case 3:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KakashiFace.png", "Какаши", 8);
                        break;
                    case 4:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/SanjiFace.jpg", "Санджи", 8);
                        break;
                    case 5:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/ZoroFace.jpg", "Зоро", 8);
                        break;
                    case 6:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KenpachiFace.jpg", "Кенпачи", 8);
                        break;
                    case 7:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/ByakuyaFace.jpg", "Бьякуя", 8);
                        break;
                    case 8:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/HitsugayaFace.jpg", "Хицугая", 8);
                        break;
                    case 9:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/HisokaFace.png", "Хисока", 16);
                        break;
                    case 10:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/NarutoFace.jpg", "Наруто", 16);
                        break;
                    case 11:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/ObitoFace.jpg", "Обито", 16);
                        break;
                    case 12:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/SasukeFace.jpg", "Саске", 16);
                        break;
                    case 13:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/LuffyFace.png", "Луффи", 16);
                        break;
                    case 14:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/IchigoFace.jpg", "Ичиго", 16);
                        break;
                    case 15:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/NatsuFace.jpg", "Натсу", 16);
                        break;
                    case 16:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/GutsFace.jpg", "Гатс", 16);
                        break;
                    case 17:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/BerserkFace.jpg", "Берсерк", 8);
                        break;
                    case 18:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/GiornoFace.jpg", "Джорно", 8);
                        break;
                    case 19:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/GohanFace.png", "Гохан", 8);
                        break;
                    case 20:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/JonathanFace.jpg", "Джонатан", 8);
                        break;
                    case 21:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/JosukeFace.jpg", "Джоске", 8);
                        break;
                    case 22:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/GohanFace.jpg", "Гохан", 8);
                        break;
                    case 23:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/LessRiderFace.png", "✭Бесправный Ездок", 8);
                        break;
                    case 24:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/OmaShuFace.jpg", "Сю", 8);
                        break;
                    case 25:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/RiderFace.jpg", "Райдер", 8);
                        break;
                    case 26:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/SakamotoFace.jpg", "✭Сакамото", 8);
                        break;
                    case 27:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/VegetaFace.jpg", "Веджета", 8);
                        break;
                    case 28:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/GokuFace.png", "Гоку", 16);
                        break;
                    case 29:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/JotaroFace.jpg", "Джотаро", 16);
                        break;
                    case 30:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/PuckFace.jpg", "✭Пак", 16);
                        break;
                    case 31:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/SaberFace.png", "Сэйбер", 16);
                        break;
                    case 32:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/ShiroFace.jpg", "Широ", 16);
                        break;

                }
            }
            else
            {
                MessageBox.Show("У вас не хватает кристалов!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void box4Spin_Click(object sender, RoutedEventArgs e)
        {
            if (crystals >= 60)
            {
                crystals -= 60;
                crystalsLabel.Content = crystals;
                randResult = random.Next(1, 25);
                switch (randResult)
                {
                    case 1:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/HisokaFace.png", "Хисока", 16);
                        break;
                    case 2:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/NarutoFace.jpg", "Наруто", 16);
                        break;
                    case 3:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/ObitoFace.jpg", "Обито", 16);
                        break;
                    case 4:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/SasukeFace.jpg", "Саске", 16);
                        break;
                    case 5:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/LuffyFace.png", "Луффи", 16);
                        break;
                    case 6:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/IchigoFace.jpg", "Ичиго", 16);
                        break;
                    case 7:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/NatsuFace.jpg", "Натсу", 16);
                        break;
                    case 8:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/GutsFace.jpg", "Гатс", 16);
                        break;
                    case 9:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/KuroroFace.png", "֍Куроро", 30);
                        break;
                    case 10:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/MadaraFace.png", "֍Мадара", 30);
                        break;
                    case 11:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/ZerefFace.png", "֍Зереф", 30);
                        break;
                    case 12:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/DoflamingoFace.png", "֍Дофламинго", 30);
                        break;
                    case 13:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/YhwachFace.png", "֍Яхве", 30);
                        break;
                    case 14:
                        Special1();
                        break;
                    case 15:
                        changeCrystalCharacter(Colors.Red, "Images/specialHeroImages/kit.jpg", "Шэрая кошка", 50);
                        addAtk(30);
                        break;
                    case 16:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/GokuFace.png", "Гоку", 16);
                        break;
                    case 17:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/JotaroFace.jpg", "Джотаро", 16);
                        break;
                    case 18:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/PuckFace.jpg", "✭Пак", 16);
                        break;
                    case 19:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/SaberFace.png", "Сэйбер", 16);
                        break;
                    case 20:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/ShiroFace.jpg", "Широ", 16);
                        break;
                    case 21:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/Dio.png", "֍Дио", 30);
                        break;
                    case 22:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/Frieza.png", "֍Фриза", 30);
                        break;
                    case 23:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/Gilgamesh.png", "֍Гильгамеш", 30);
                        break;
                    case 24:
                        changeCrystalCharacter(Colors.Red, "Images/heroImages/Polzunok.png", "✭Ползунок", 50);
                        break;
                }
            }
            else
            {
                MessageBox.Show("У вас не хватает кристалов!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void InventoryClose_Click(object sender, RoutedEventArgs e)
        {
            CloseInventory();
        }

        public void CloseInventory()
        {
            Inventory.Visibility = Visibility.Hidden;
            InventoryClose.Visibility = Visibility.Hidden;
        }

        public void OpenInventory()
        {
            Inventory.Visibility = Visibility.Visible;
            InventoryClose.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = File.CreateText("stats"))
            {
                sw.WriteLine("LVL Персонажа: " + levelXp);
                sw.WriteLine("DPC: " + atk);
                sw.WriteLine("DPS: " + dps);
                sw.WriteLine("********************");
                sw.WriteLine("Level: " + level);
            }
            using (ZipFile zip = new ZipFile())
            {
                zip.Password = "1029384756";
                zip.AddFile("stats");
                zip.Save("stats.zip");
            }
            File.Delete("stats");
            MessageBox.Show("Зайдите в папку с игрой и отправьте файл ''stats.zip'' MoRset'у");
        }

        private void Inventory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((string)Inventory.SelectedItem == "Луффи")
            {
                MessageBox.Show("Имя: Монки Д. Луффи(Соломеная Шляпа)=Способность:Фрукт Gomu-Gomu(Резиновый Человек)=Аниме:Ван пис.", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Хисока")
            {
                MessageBox.Show("Имя: Хисока Моро=Способность:БанджиГам (Липкая тянучка)=Аниме:Хантер х Хантер", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Гон")
            {
                MessageBox.Show("Имя: Гон Фрикс=Способность:Камень Ножницы Бумага=Аниме:Хантер х Хантер", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Нами")
            {
                MessageBox.Show("Имя: Нами=Способность:Темпо=Аниме:Ван пис", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Зоро")
            {
                MessageBox.Show("Имя: Ророноа Зоро=Способность:Мечевые техники=Аниме:Ван пис", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Санджи")
            {
                MessageBox.Show("Имя: Винсмок Санджи=Способность:Дьявольская нога=Аниме:Ван пис", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Усопп")
            {
                MessageBox.Show("Имя: Усоппа=Способность:Гинго Пачинко=Аниме:Ван пис", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Ичиго")
            {
                MessageBox.Show("Имя: Куросаки Ичиго=Способность:Синигами,пустой,квинси=Аниме:Блич", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Натсу")
            {
                MessageBox.Show("Имя: Драгнил Натсу=Способность:Огненый убийца драконов=Аниме:Фэйри тэйл", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Орихиме")
            {
                MessageBox.Show("Имя: Иноуэ Орихиме=Способность:Сантен Кисюн=Аниме:Блич", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Леорио")
            {
                MessageBox.Show("Имя: Леорио Паладинайт=Способность:Телепорт-удар=Аниме:Хантер х Хантер", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Какаши")
            {
                MessageBox.Show("Имя: Хатаки Какаши=Способность:Мангёке Шаринган=Аниме:Наруто", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Наруто")
            {
                MessageBox.Show("Имя: Узумаки Наруто=Способность:Девятихвостый лис,сила рикудо=Аниме:Наруто", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Саске")
            {
                MessageBox.Show("Имя: Учиха Саске=Способность:Вечный мангёке шаринган,рикудо ринненган=Аниме:Наруто", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Сакура")
            {
                MessageBox.Show("Имя: Харуна Сакура=Способность:Сильный удар,техники лечения=Аниме:Наруто", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Обито")
            {
                MessageBox.Show("Имя: Учиха Обито=Способность:Мангёке шаринган,клетки хаширамы=Аниме:Наруто", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Хицугая")
            {
                MessageBox.Show("Имя: Тоширо Хицугая=Способность:Дайгурэн Хьёрэнмару=Аниме:Блич", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Харухи")
            {
                MessageBox.Show("Имя: Харухи Судзумия=Способность:Хватит повторять одну и ту же серию=Аниме:Меланхолия Судзумия Харухи", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Курапика")
            {
                MessageBox.Show("Имя: Курапика Курута=Способность:Святая цепь=Аниме:Хантер х Хантер", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Киллуа")
            {
                MessageBox.Show("Имя:  Киллуа Золдзик=Способность:Thunderbolt=Аниме:Хантер х Хантер", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Гатс")
            {
                MessageBox.Show("Имя: Гатс=Способность:Огромная нечеловеческая сила=Аниме:Берсерк", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Грэй")
            {
                MessageBox.Show("Имя: Фулбастер Грэй=Способность:Магия льда=Аниме:Фэйри Тэйл", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Эльза")
            {
                MessageBox.Show("Имя: Скарлет Эльза=Способность:Магия смены снаряжения=Аниме:Фэйри Тэйл", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Лаксус")
            {
                MessageBox.Show("Имя: Дрейар Лаксус=Способность:Лакрима убийцы драконов=Аниме:Фэйри Тэйл", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Люси")
            {
                MessageBox.Show("Имя: Хартфилия Люси=Способность:Заклинатель духов=Аниме:Фэйри ТэйлХвост феи", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Лелуш")
            {
                MessageBox.Show("Имя: Лелуш Британский=Способность:Гиас повиновения=Аниме:Код Гиас", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Бьякуя")
            {
                MessageBox.Show("Имя: Кучики Бьякуя=Способность:Сенбунзакура Кагеёси=Аниме:Блич", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Кенпачи")
            {
                MessageBox.Show("Имя: Зараки Кенпачи=Способность:Нозараши=Аниме:Блич", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Куроро")
            {
                MessageBox.Show("Имя: Куроро Люцифер=Способность:Скил Хантэр=Аниме:Хантер х Хантер", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Мадара")
            {
                MessageBox.Show("Имя: Учиха Мадара=Способность:Сила рикудо,вечный мангёке шаринган,ринненган=Аниме:Наруто", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Зереф")
            {
                MessageBox.Show("Имя: Зереф Драгнил=Способность:Проклятье,бессмертие=Аниме:Фэйри Тэйл", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Дофламинго")
            {
                MessageBox.Show("Имя: Докихот Дофламиго=Способность:Фрукт Ito-Ito(Нити)=Аниме:Ван пис", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Берсерк")
            {
                MessageBox.Show("Имя: Геракл(Берсерк)=Фантазм:12 подвигов Геракла=Аниме:Судьба", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Дио")
            {
                MessageBox.Show("Имя: Дио Брандо=Стэнд:Za Waradu(Остановка времени)=Аниме:ДжоДжо", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Фриза")
            {
                MessageBox.Show("Имя: Фриза=Способность:Золотой Фриза=Аниме:Драгон Болл", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Гильгамеш")
            {
                MessageBox.Show("Имя: Гильгамеш(Арчер)=Фантазм:Врата Вавилона=Аниме:Судьба", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Джорно")
            {
                MessageBox.Show("Имя: Джорно Джовани=Стэнд:Gold Experience(Создание живого)=Аниме:ДжоДжо", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Гохан")
            {
                MessageBox.Show("Имя: Сон Гохан=Способность:Супер Саян 2=Аниме:Драгон Болл", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Гоку")
            {
                MessageBox.Show("Имя: Сон Гоку=Способность:Супер Саян голубой=Аниме:Драгон Болл", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Джонатан")
            {
                MessageBox.Show("Имя: Джонатан Джостар=Способность:Хамон=Аниме:ДжоДжо", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Джозеф")
            {
                MessageBox.Show("Имя: Джозеф Джостар=Способность:Хамон=Аниме:ДжоДжо", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Джоске")
            {
                MessageBox.Show("Имя: Хигашиката Джоске=Стэнд:Crazy Diamond(восстоновление)=Аниме:ДжоДжо", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Джотаро")
            {
                MessageBox.Show("Имя: Джотаро Куджо=Стэнд:Star Platinum(Очень сильный стэнд)=Аниме:ДжоДжо", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Кирито")
            {
                MessageBox.Show("Имя: Киригая Кирито=Способность:Два Меча=Аниме:САО", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Лансер")
            {
                MessageBox.Show("Имя:  Диармайд Уа Дуибхне(Лансер)=Фанитазм:всёдостигающие копьё=Аниме:Судьба", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "✭Бесправный Ездок")
            {
                MessageBox.Show("Имя: Бесправный Ездок=Способность:Низкий порог боли=Аниме:Ван-панч ман", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Сю")
            {
                MessageBox.Show("Имя: Ома Сю=Способность:Вынимать пустоты других людей=Аниме:Корона Вины", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Онидзука")
            {
                MessageBox.Show("Имя: Эйкити Онидзука=Способность:Крутой учитель=Аниме:Крутой Учитель Онидзука", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Пикколо")
            {
                MessageBox.Show("Имя: Пикколо Даймао=Способность:быстрая регенерация=Аниме:Драгон Болл", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Райдер")
            {
                MessageBox.Show("Имя: Александр Македонский(Райдер)=Фантазм:Тысячная армия=Аниме:Судьба", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Мастер Роши")
            {
                MessageBox.Show("Имя: Мутэн Роши=Способность:Камехамеха,бессмертие=Аниме:Драгон Болл", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Сэйбер")
            {
                MessageBox.Show("Имя: Артурия Пендрагон=Фантазм:Экскалибур=Аниме:Судьба", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "✭Сакамото")
            {
                MessageBox.Show("Имя: Сакаиото=Способность:идеален=Аниме:Я - Сакамото, а что?", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Широ")
            {
                MessageBox.Show("Имя: Эмия Широ=Способность:Trace-on(создание вещей)=Аниме:Судьба", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "Веджета")
            {
                MessageBox.Show("Имя: Веджета=Способность:Супер Саян Голубой=Аниме:Драгон Болл", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "✭Пак")
            {
                MessageBox.Show("Имя: Пак=Способность:Превращаеться в огромное чудовище=Аниме:С нуля", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((string)Inventory.SelectedItem == "✭Ползунок")
            {
                MessageBox.Show("Имя: Ползунок=Способность:вылезает когда вы подходите к яме на булыжнике=Аниме:СИ ЭС ГОУ(Басни Димона)", "Характеристики", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void weapon4Buy_Click(object sender, RoutedEventArgs e)
        {
            if (coins >= 270)
            {
                addAtk(8);
                removeCoins(270);
                Properties.Settings.Default.atk = atk;
                Properties.Settings.Default.coins = coins;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void weapon5Buy_Click(object sender, RoutedEventArgs e)
        {
            if (coins >= 520)
            {
                addAtk(16);
                removeCoins(520);
                Properties.Settings.Default.atk = atk;
                Properties.Settings.Default.coins = coins;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void weapon6Buy_Click(object sender, RoutedEventArgs e)
        {
            if (coins >= 580)
            {
                addAtk(20);
                removeCoins(580);
                Properties.Settings.Default.atk = atk;
                Properties.Settings.Default.coins = coins;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void weapon7Buy_Click(object sender, RoutedEventArgs e)
        {
            if (coins >= 800)
            {
                addAtk(30);
                removeCoins(800);
                Properties.Settings.Default.atk = atk;
                Properties.Settings.Default.coins = coins;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void weapon8Buy_Click(object sender, RoutedEventArgs e)
        {
            if (coins >= 875)
            {
                addAtk(35);
                removeCoins(875);
                Properties.Settings.Default.atk = atk;
                Properties.Settings.Default.coins = coins;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void weapon9Buy_Click(object sender, RoutedEventArgs e)
        {
            if (coins >= 350)
            {
                addAtk(15);
                removeCoins(350);
                Properties.Settings.Default.atk = atk;
                Properties.Settings.Default.coins = coins;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}