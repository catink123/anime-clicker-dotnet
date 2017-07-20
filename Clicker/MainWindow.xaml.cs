﻿using System;
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
            enemyHpBar.Value = Properties.Settings.Default.enemyHp;
            System.Threading.Thread.Sleep(10);
            changelevel();
        }

        private void Attack_Click(object sender, RoutedEventArgs e)
        {
            enemyHpBar.Value = enemyHpBar.Value - atk;
            enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
            if (enemyHpBar.Value <= 0)
            {
                changelevel();
                
            }
        }

        public void changelevel()
        {
            level++;
            xp = xp + (enemyHp / 3);
            xpLabel.Content = Convert.ToString(xp);
            if (level == 10)
            {
                boss("Images/bossImages/boss1.png", "Kuroro Lucifer");
                enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
            } else if (level == 9)
            {
                levelInfinity();
                enemyHp = Convert.ToInt32(enemyHpBar.Value);
                enemyHpLabel.Content = Convert.ToString(enemyHpBar.Value);
            } else
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
            enemyHp = Convert.ToInt32(enemyHpBar.Value);
        }

        public void changeEnemyHp(int value1, int value2)
        {
            enemyHpBar.Maximum = random.Next(value1, value2);
            enemyHpBar.Value = enemyHpBar.Maximum;
        }

        public void addCoins(int value)
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
            addCoins(100);
            System.Threading.Thread.Sleep(50);
            changePic();
        }

        public void boss(string imagePath, string bossName)
        {
            stage++;
            level = 0;
            enemyImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            enemyName.Content = bossName;
            enemyHpBar.Maximum = enemyHp * 2;
            enemyHpBar.Value = enemyHpBar.Maximum;
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
                        charName.Content = "Leorio";
                        charList.Add("Leorio");
                        break;
                    case 2:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero2.png", UriKind.Relative));
                        charName.Content = "Hisoka";
                        charList.Add("Hisoka");
                        break;
                    case 3:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero3.png", UriKind.Relative));
                        charName.Content = "Killoa";
                        charList.Add("Killoa");
                        break;
                    case 4:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero4.png", UriKind.Relative));
                        charName.Content = "Gon";
                        charList.Add("Gon");
                        break;
                    case 5:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero5.png", UriKind.Relative));
                        charName.Content = "Kurapika";
                        charList.Add("Kurapika");
                        break;
                    case 6:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero6.png", UriKind.Relative));
                        charName.Content = "Hatake Kakashi";
                        charList.Add("Hatake Kakashi");
                        break;
                    case 7:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero7.png", UriKind.Relative));
                        charName.Content = "Uzumaki Naruto";
                        charList.Add("Uzumaki Naruto");
                        break;
                    case 8:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero8.png", UriKind.Relative));
                        charName.Content = "Uchiha Obito";
                        charList.Add("Uchiha Obito");
                        break;
                    case 9:
                        charColor.Fill = new SolidColorBrush(Colors.LimeGreen);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero9.png", UriKind.Relative));
                        charName.Content = "Uchiha Sakura";
                        charList.Add("Uchiha Sakura");
                        break;
                    case 10:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero10.png", UriKind.Relative));
                        charName.Content = "Uchiha Saske";
                        charList.Add("Uchiha Saske");
                        break;
                    case 11:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero11.png", UriKind.Relative));
                        charName.Content = "Sanji";
                        charList.Add("Sanji");
                        break;
                    case 12:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero12.png", UriKind.Relative));
                        charName.Content = "Zoro";
                        charList.Add("Zoro");
                        break;
                    case 13:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero13.png", UriKind.Relative));
                        charName.Content = "Nami";
                        charList.Add("Nami");
                        break;
                    case 14:
                        charColor.Fill = new SolidColorBrush(Colors.Blue);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero14.png", UriKind.Relative));
                        charName.Content = "Usopp";
                        charList.Add("Usopp");
                        break;
                    case 15:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero15.png", UriKind.Relative));
                        charName.Content = "Monkey D. Luffy";
                        charList.Add("Monkey D. Luffy");
                        break;
                    case 16:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero16.png", UriKind.Relative));
                        charName.Content = "Kurosaki Ichigo";
                        charList.Add("Kurosaki Ichigo");
                        break;
                    case 17:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero17.png", UriKind.Relative));
                        charName.Content = "Zaraki Kenpachi";
                        charList.Add("Zaraki Kenpachi");
                        break;
                    case 18:
                        charColor.Fill = new SolidColorBrush(Colors.Purple);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero18.png", UriKind.Relative));
                        charName.Content = "Byakuya Kuchiki";
                        charList.Add("Byakuya Kuchiki");
                        break;
                    case 19:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero19.png", UriKind.Relative));
                        charName.Content = "Toshiro Hitsugaya";
                        charList.Add("Toshiro Hitsugaya");
                        break;
                    case 20:
                        charColor.Fill = new SolidColorBrush(Colors.Orange);
                        charImage.Source = new BitmapImage(new Uri("Images/heroImages/hero20.png", UriKind.Relative));
                        charName.Content = "Inoue Orihime";
                        charList.Add("Inoue Orihime");
                        break;
                    default:
                        break;
                        spinCost += 10;
                        spinCostLabel.Content = Convert.ToString(spinCost);
                        Properties.Settings.Default.spinCost = spinCost;
                        Properties.Settings.Default.Save();
                        Inventory.Items.Add(charList);
                }
            } else
            {
                MessageBox.Show("У вас не хватает денег!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }
    }
}
