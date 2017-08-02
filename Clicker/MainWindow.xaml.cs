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
                stage++;
                randResult = random.Next(1, 6);
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

        public void changeCharacter (Color color, string pathToImage, string characterName, int addDps)
        {
            charColor.Fill = new SolidColorBrush(color);
            charImage.Source = new BitmapImage(new Uri(pathToImage, UriKind.Relative));
            charName.Content = characterName;
            Properties.Settings.Default.charList.Add(characterName);
            dps += addDps;
        }

        public void changeCrystalCharacter (Color color, string pathToImage, string characterName, int addDps)
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
            cps++;
            Inventory.Items.Refresh();
        }

        private void spin_Click(object sender, RoutedEventArgs e)
        {
            if (coins > spinCost)
            {
                coins -= spinCost;
                coinsLabel.Content = Convert.ToString(coins);
                randResult = random.Next(1, 29);
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
                        charName.Content = "Люсси";
                        Properties.Settings.Default.charList.Add("Люсси");
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
                    default:
                        break;
                }
                spinCost += 10;
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
            } else
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
            } else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void weapon3Buy_Click(object sender, RoutedEventArgs e)
        {
            if (coins >= 140)
            {
                addAtk(4);
                removeCoins(140);
                Properties.Settings.Default.atk = atk;
                Properties.Settings.Default.coins = coins;
                Properties.Settings.Default.Save();
            } else
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
                randResult = random.Next(1, 13);
                switch (randResult)
                {
                    case 1:
                        changeCrystalCharacter(Colors.LimeGreen, "Images/heroFaceImages/LeorioFace.jpg", "Леорио", 1);
                        break;
                    case 2:
                        changeCrystalCharacter(Colors.LimeGreen, "Images/heroFaceImages/SakuraFace.jpg", "Сакура", 1);
                        break;
                    case 3:
                        changeCrystalCharacter(Colors.LimeGreen, "Images/heroFaceImages/HaruhiFace.jpg", "Харухи", 1);
                        break;
                    case 4:
                        changeCrystalCharacter(Colors.LimeGreen, "Images/heroFaceImages/LucyFace.jpg", "Люсси", 1);
                        break;
                    case 5:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/KurapikaFace.png", "Курапика", 2);
                        break;
                    case 6:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/NamiFace.jpg", "Нами", 2);
                        break;
                    case 7:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/UsoppFace.jpg", "Усопп", 2);
                        break;
                    case 8:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/OrihimeFace.jpg", "Орихиме", 2);
                        break;
                    case 9:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/LelouchFace.png", "Лелуш", 2);
                        break;
                    case 10:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/ErzaFace.jpg", "Эльза", 2);
                        break;
                    case 11:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/LaxusFace.png", "Лаксус", 2);
                        break;
                    case 12:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/GrayFace.png", "Грэй", 2);
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
                randResult = random.Next(1, 17);
                switch (randResult)
                {
                    case 1:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/KurapikaFace.png", "Курапика", 2);
                        break;
                    case 2:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/NamiFace.jpg", "Нами", 2);
                        break;
                    case 3:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/UsoppFace.jpg", "Усопп", 2);
                        break;
                    case 4:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/OrihimeFace.jpg", "Орихиме", 2);
                        break;
                    case 5:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/LelouchFace.png", "Лелуш", 2);
                        break;
                    case 6:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/ErzaFace.jpg", "Эльза", 2);
                        break;
                    case 7:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/LaxusFace.png", "Лаксус", 2);
                        break;
                    case 8:
                        changeCrystalCharacter(Colors.Blue, "Images/heroFaceImages/GrayFace.png", "Грэй", 2);
                        break;
                    case 9:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KilluaFace.png", "Киллуа", 4);
                        break;
                    case 10:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/GonFace.jpg", "Гон", 4);
                        break;
                    case 11:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KakashiFace.png", "Какаши", 4);
                        break;
                    case 12:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/SanjiFace.jpg", "Санджи", 4);
                        break;
                    case 13:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/ZoroFace.jpg", "Зоро", 4);
                        break;
                    case 14:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KenpachiFace.jpg", "Кенпачи", 4);
                        break;
                    case 15:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/ByakuyaFace.jpg", "Бьякуя", 4);
                        break;
                    case 16:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/HitsugayaFace.jpg", "Хицугая", 4);
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
                randResult = random.Next(1, 17);
                switch (randResult)
                {
                    case 1:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KilluaFace.png", "Киллуа", 4);
                        break;
                    case 2:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/GonFace.jpg", "Гон", 4);
                        break;
                    case 3:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KakashiFace.png", "Какаши", 4);
                        break;
                    case 4:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/SanjiFace.jpg", "Санджи", 4);
                        break;
                    case 5:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/ZoroFace.jpg", "Зоро", 4);
                        break;
                    case 6:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/KenpachiFace.jpg", "Кенпачи", 4);
                        break;
                    case 7:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/ByakuyaFace.jpg", "Бьякуя", 4);
                        break;
                    case 8:
                        changeCrystalCharacter(Colors.Purple, "Images/heroFaceImages/HitsugayaFace.jpg", "Хицугая", 4);
                        break;
                    case 9:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/HisokaFace.png", "Хисока", 8);
                        break;
                    case 10:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/NarutoFace.jpg", "Наруто", 8);
                        break;
                    case 11:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/ObitoFace.jpg", "Обито", 8);
                        break;
                    case 12:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/SasukeFace.jpg", "Саске", 8);
                        break;
                    case 13:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/LuffyFace.png", "Луффи", 8);
                        break;
                    case 14:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/IchigoFace.jpg", "Ичиго", 8);
                        break;
                    case 15:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/NatsuFace.jpg", "Натсу", 8);
                        break;
                    case 16:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/GutsFace.jpg", "Гатс", 8);
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
                randResult = random.Next(1, 17);
                switch (randResult)
                {
                    case 1:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/HisokaFace.png", "Хисока", 8);
                        break;
                    case 2:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/NarutoFace.jpg", "Наруто", 8);
                        break;
                    case 3:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/ObitoFace.jpg", "Обито", 8);
                        break;
                    case 4:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/SasukeFace.jpg", "Саске", 8);
                        break;
                    case 5:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/LuffyFace.png", "Луффи", 8);
                        break;
                    case 6:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/IchigoFace.jpg", "Ичиго", 8);
                        break;
                    case 7:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/NatsuFace.jpg", "Натсу", 8);
                        break;
                    case 8:
                        changeCrystalCharacter(Colors.Orange, "Images/heroFaceImages/GutsFace.jpg", "Гатс", 8);
                        break;
                    case 9:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/boss1.png", "֍Куроро Люцифер", 15);
                        break;
                    case 10:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/boss2.png", "֍Мадара Утиха", 15);
                        break;
                    case 11:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/boss3.png", "֍Зереф Драгнил", 15);
                        break;
                    case 12:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/boss4.png", "֍Донкихот Дофламинго", 15);
                        break;
                    case 13:
                        changeCrystalCharacter(Colors.Black, "Images/bossImages/boss5.png", "֍Яхве Бах", 15);
                        break;
                    case 14:
                        Special1();
                        break;
                    case 15:
                        changeCrystalCharacter(Colors.Red, "Images/specialHeroImages/kit.jpg", "Шэрая кошка", 0);
                        addAtk(30);
                        break;
                }
            } else
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
    }
}
