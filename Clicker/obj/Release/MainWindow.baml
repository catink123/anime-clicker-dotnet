   M S B A M L   `   `   `  ���� H  DAnime Clicker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ClickerClicker      Clicker.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:Clicker       � Title$  Anime Clicker��$	��480��$	��860�� � 
ResizeMode$ NoResize=�
���   �.��� .+�� $��	0,0,346,0q�����  �� ImageSource$- 'Images/backgroundImages/background1.jpg�� j� Stretch$ UniformToFill=�.+��� -    ��Name$ 
background��$
��Left=�$	��451��$	��Top=�$	��508��$-��'Images/backgroundImages/background1.jpg��$��UniformToFill=�� -    �Name$ 
enemyHpBar��$��Center=�$��22��$��Center=�$	��285�� � Value$ 10Q�
���  �� � Maximum$ 10Q����	#FF780000$��	0,308,0,0q��� -    ��Name$ 	enemyName��$��Орк��$��Center=�$��Bottom=�
�������$��Segoe Script)�$��24(����	#59000000$	��287��$��42��$��Center=�$
��Bold"�$��112,0,109,-2q��� -   $ 
enemyImage��$	��351��$��	0,-71,0,0q�$#��Images/enemyImages/enemy1.png��	 J� RenderTransformOrigin$	 0.5,0.5&�$��Center=�$��Center=�z�T� 
 T� Y$
 0Q�� -   $ xpBar��$��Right=�$��15��$��0q�$	��Top=�$��70��$ 20Q��� $	��XP:��$��Right=�$��0,0,75,0q�$	��Top=�
�������$��Segoe UI Black)�$��22(����	#59000000�� -   $ xpLabel��$��0��$��Right=�$��0,15,0,0q�$	��Top=����	#FF06B025$��Segoe UI Black)����	#59000000$��24��$��70��$��Center=��� $
��ATK:��$��Right=�$��	0,78,75,0q�$	��Top=�
�������$��Segoe UI Black)�$��22(����	#59000000�� $
��LVL:��$��Right=�$��	0,39,75,0q�$	��Top=�
�������$��Segoe UI Black)�$��22(����	#59000000�� -   $ atkLabel��$��1��$��Right=�$��0,78,0,0q�$	��Top=����	#FF0097FF$��Segoe UI Black)�$��20(����	#59000000$��39��$��70��$��Center=�$��Center=��� -   $ 
coinsLabel��$��1��$��Right=�$��Bottom=����	#FFFFF300$��Segoe UI Black)�$��20(����	#59000000$��39��$��70��$��Center=�$��Center=��� -	   $ crystalsLabel��$��1��$��Right=�$��0,0,0,39q�$��Bottom=�
�������$��Segoe UI Black)�$��20(����	#59000000$��39��$��70��$��Center=�$��Center=��� -
    ��Name$ enemyAttack��$�� �����	#00000000���	#00000000
�������$��24(�$
��Bold"�$��Italic$�$	��372��$	��351��$��Center=�$��Center=�$��	0,-71,0,0q�$	 0.5,0.5&� � Triggers :�  :� RoutedEvent$ ButtonBase.Click��.[��� .����  g� AutoReverse . .��W�  �� 
TargetName$ 	enemyName�� �� TargetProperty$8 2(UIElement.RenderTransform).(TranslateTransform.Y)�	 W� To$	 -25X� g� Duration$ 0:0:0.30D� W� EasingFunction 0  *System.Windows.Media.Animation.QuarticEase  7  1System.Windows.Media.Animation.EasingFunctionBase   
EasingMode$ EaseIn=�����  �� 
TargetType" ��.���  �� Property � �� �� Value �	#7F000000��  � �� ��  �� 
TargetType" ��.R��� #��z��._��� $��Center=�$��Center=� �� Triggers P�  P� Property �@J�IsMouseOver P� Value . . ���  � ��
 �   ��� $��Center=�$��64��$��	0,331,0,0q�$��Center=�$	��288��$&�� Images/miscImages/enemyHpBar.png���� -   $ enemyHpLabel��$��
Loading...��$��Center=�$��Center=�
�������$��Segoe UI Black)�$��20(����	#00000000$��38��$	��292��$��Center=�$��Center=�$��	0,304,0,0q��� -   $ levelXpLabel��$��0��$��Right=�$��0,39,0,0q�$	��Top=����	#FF06B025$��Segoe UI Black)�$��20(����	#59000000$��39��$��70��$��Center=�$��Center=��� -   $ dpsLabel��$��1��$��Right=�$��	0,117,0,0q�$	��Top=�
���  ��$��Segoe UI Black)�$��20(����	#59000000$��39��$��70��$��Center=�$��Center=��� $
��DPS:��$��Right=�$��
0,117,75,0q�$	��Top=�
�������$��Segoe UI Black)�$��22(����	#59000000�� $��Right=�$��41��$��	0,0,69,38q�$��Bottom=�$��42��$$��Images/miscImages/crystals.png���� $��Right=�$��37��$��0,0,71,0q�$��Bottom=�$��37��$!��Images/miscImages/coins.png���� $
��Left=�$��65��$	��Top=�$��63��$1��+Images/miscImages/xernya_kakaya_nibudz'.png���� -   $��Button��$
��Left=�$��65��$	��Top=�$��63�����	#00DDDDDD���	#00707070���� " ��.���  � �� �	#7F000000��  � �� �� " ��.R��� #��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ���  � ��
 �   ��� $��Right=�$	��451��$��0q�$	��Top=�$	��346��.@��� $)��#Рулетка персонажей��.��� ���	#FFE5E5E5.+��� $
��Left=�$	��403��$	��Top=�$	��340��$
��Fill=�$ ��Images/miscImages/shop.png���� $	��200��$��0,54,0,0q�
���   �$	��Top=�$��Center=�$	��200����
� .���  � Color$ 	#FF6A6A6A�� � Offset$ 1Q�� $ 	#FFB8B8B8���� -    ��Name$ 	charColor�����	#FF919191$��55��$��	0,199,0,0q�
���   �$	��Top=�$��Center=�$	��200���� -   $ charName��$&�� Нажмите на кнопку��$��Center=�$��	0,199,0,0q�$	��Top=�$��55��$	��200��$��18(�$��MV Boli)�
�������� $��Center=�$	��145��$��0,54,0,0q�$	��Top=�$	��200��.+��� -   $ 	charImage��$#��Images/miscImages/spinner.gif���� -   $
 spin��$��Крутить��$
��Left=�$��
70,271,0,0q�$	��Top=�$	��200��$��30��$��20(��� $��	Цена:��$
��Left=�$��	70,0,0,76q�$��Bottom=�$��45��$��Right=��� -   $ spinCostLabel��$	��100��$
��Left=�$��
115,0,0,76q�$��Bottom=�$	��155���� $��(coins)��$
��Left=�$��	70,0,0,49q�$��Bottom=�$��45��$��Center=�$��27��$��9(��� -   $ InventoryToggle��$��Инвентарь��$
��Left=�$��238,343,0,0q�$	��Top=�$��80��$��27���� $!��Магазин оружия��.��� ���	#FFE5E5E5.+��� $��Center=�$	��408��$��Center=�$	��346��$
��Fill=�$ ��Images/miscImages/shop.png���� $
��Left=�$	��339��$��	20,33,0,0q�$	��Top=�$	��304��.��� $	��Top=�$	��287��$	��828��.+��� -   $ 
weapon1Buy��$��Купить��$
��Left=�$��	1,140,0,0q�$	��Top=�$	��128�����	#FF00FF23�� $
��Left=�$	��128��$	��Top=�$	��128��$
��Fill=�$%��Images/weaponImages/weapon1.png��$��1,12,0,0q��� $��Цена: 35; +1 к DPC��$
��Left=�$��	1,160,0,0q�$	��Top=�$	��128���� -   $ 
weapon2Buy��$��Купить��$
��Left=�$��154,140,0,0q�$	��Top=�$	��128�����	#FF00FF23�� $��Цена: 70; +2 к DPC��$
��Left=�$��154,160,0,0q�$	��Top=�$	��128���� $
��Left=�$	��128��$	��Top=�$	��128��$
��Fill=�$%��Images/weaponImages/weapon2.png��$��
154,12,0,0q��� -   $ 
weapon3Buy��$��Купить��$
��Left=�$��	2,295,0,0q�$	��Top=�$	��128�����	#FFFF8B00
��������� $ ��Цена: 1024; +40 к DPC��$
��Left=�$��	2,315,0,0q�$	��Top=�$	��128��$��24���� $
��Left=�$��91��$	��Top=�$	��127��$%��Images/weaponImages/weapon3.png��$��	2,203,0,0q��� -   $ 
weapon4Buy��$��Купить��$
��Left=�$��154,295,0,0q�$	��Top=�$	��128��
����  �
��������� $��Цена: 270; +8 к DPC��$
��Left=�$��154,315,0,0q�$	��Top=�$	��128��$��24���� $
��Left=�$��84��$	��Top=�$	��127��$&�� Images/weaponImages/Scissors.png��$��154,210,0,0q��� -   $ 
weapon5Buy��$��Купить��$
��Left=�$��	2,450,0,0q�$	��Top=�$	��128�����	#FF9700FF
��������� $��Цена: 520; +16 к DPC��$
��Left=�$��	2,470,0,0q�$	��Top=�$	��128��$��24���� $
��Left=�$	��105��$	��Top=�$	��128��$#��Images/weaponImages/Jacal.png��$��	2,344,0,0q��� -   $ 
weapon6Buy��$��Купить��$
��Left=�$��153,450,0,0q�$	��Top=�$	��128�����	#FF9700FF
��������� $��Цена: 580; +20 к DPC��$
��Left=�$��153,470,0,0q�$	��Top=�$	��128��$��24���� $
��Left=�$	��105��$	��Top=�$��90��$*��$Images/weaponImages/DragonSlayer.png��$��172,344,0,0q��� -   $ 
weapon7Buy��$��Купить��$
��Left=�$��	2,605,0,0q�$	��Top=�$	��128�����	#FFFF8B00
��������� $��Цена: 800; +30 к DPC��$
��Left=�$��	2,625,0,0q�$	��Top=�$	��128��$��24���� $
��Left=�$	��105��$	��Top=�$��83��$*��$Images/weaponImages/KiritoSwords.png��$��
22,499,0,0q��� -   $ 
weapon8Buy��$��Купить��$
��Left=�$��153,605,0,0q�$	��Top=�$	��128�����	#FFFF8B00
��������� $��Цена: 875; +35 к DPC��$
��Left=�$��153,625,0,0q�$	��Top=�$	��128���� $
��Left=�$	��116��$	��Top=�$	��128��$&�� Images/weaponImages/Durandal.png��$��153,492,0,0q��� -   $ 
weapon9Buy��$��Купить��$
��Left=�$��	2,782,0,0q�$	��Top=�$	��128��
����  �
��������� $��Цена: 350; +15 к DPC��$
��Left=�$��	2,802,0,0q�$	��Top=�$	��128���� $
��Left=�$	��128��$	��Top=�$	��128��$*��$Images/weaponImages/GintokiSword.png��$��	2,654,0,0q��� $'��!Магазин Кристалов��$��
-3,-2,-1,0q�.��� ���	#FFE5E5E5.+��� $��Цена: 15��$
��Left=�$��179,184,0,0q�$	��Top=�$	��137��$��Center=��� $
��Left=�$	��406��$	��Top=�$	��343��$
��Fill=�$ ��Images/miscImages/shop.png��$��	0,0,-3,-3q��� $��Цена: 5��$
��Left=�$��
26,184,0,0q�$	��Top=�$	��137��$��Center=��� -   $ box1Spin��$��Купить��$
��Left=�$��
26,164,0,0q�$	��Top=�$	��137��
��� � ��� $
��Left=�$	��116��$��	39,48,0,0q�$	��Top=�$	��124��$ ��Images/miscImages/box1.png���� -   $ box2Spin��$��Купить��$
��Left=�$��179,164,0,0q�$	��Top=�$	��137��
����  �
��������� $
��Left=�$	��129��$��
179,35,0,0q�$	��Top=�$	��137��$ ��Images/miscImages/box2.png���� -    $ box4Spin��$��Купить��$
��Left=�$��179,324,0,0q�$	��Top=�$	��137�����	#FFFF9600�� $
��Left=�$	��114��$��192,210,0,0q�$	��Top=�$	��119��$ ��Images/miscImages/box4.png���� $
��Left=�$	��114��$��
39,210,0,0q�$	��Top=�$	��116��$ ��Images/miscImages/box3.png���� -!   $ box3Spin��$��Купить��$
��Left=�$��
26,324,0,0q�$	��Top=�$	��137�����	#FF9600FF
�������� -"    �Name$ crystalSpinner�����	#00000000$��60,72,58,108q�$y�Hidden=�.+��� 
���   �$
��Left=�$	��223��
�������$	��Top=�$	��222��  �� RadiusY$  11��! �� RadiusX$! 11���� $	��200��$��
10,10,12,0q�
���   �$	��Top=�$��Center=�$	��200����
� .��� $ 	#FF6A6A6A��$ 1Q�� $ 	#FFB8B8B8���� -#   $ crystalCharColor�����	#FF919191$��55��$��10,155,12,0q�
���   �$	��Top=�$��Center=�$	��200���� -$   $ crystalCharName��$&�� Нажмите на кнопку��$��Center=�$��10,155,12,0q�$	��Top=�$��54��$	��200��$��18(�$��MV Boli)�
�������� $��Center=�$	��143��$��
10,11,12,0q�$	��Top=�$	��200��.+��� -%   $ crystalCharImage��$#��Images/miscImages/spinner.gif���� -&   $ button��$��Закрыть��$
��Left=�$	��Top=�$	��222��$��13��$��	0,210,0,0q�$��Microsoft Sans Serif)�$��9(����	#00000000���	#00000000
��������� $��Цена: 60��$
��Left=�$��179,344,0,0q�$	��Top=�$	��137��$��Center=��� $��Цена: 30��$
��Left=�$��
26,344,0,0q�$	��Top=�$	��137��$��Center=��� -'   $ InventoryToggle_Copy��$��Инвентарь��$
��Left=�$��128,370,0,0q�$	��Top=�$��80��$��27���� -(   " ��Name$" 	Inventory��$��Right=�$	��399��$��Bottom=�$	��346��$y�Hidden=��� -)   $ InventoryClose��$��Закрыть��$��Right=�$��18��$��0,46,0,0q�$	��Top=�$��56��$y�Hidden=�