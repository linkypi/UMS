   M S B A M L   `   `   `  ����
  UserMS"    UserMS.DemoReport.ReportTest   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      j fTelerik.Windows.Controls.Data, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Documents.Core, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7w sTelerik.Windows.Controls.DataVisualization, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p	 lTelerik.Windows.Controls.RibbonView, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n
 jTelerik.Windows.Controls.GridView, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Input, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7r nTelerik.Windows.Controls.DataServices, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p lTelerik.Windows.Controls.Navigation, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n jTelerik.Windows.Controls.Charting, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7b ^Telerik.Windows.Maths, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7k gTelerik.Windows.Controls.Chart, Version=2014.1.224.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Wtelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
         5      gtoolkitAhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit    	 
         5      -   5          N� Title$  ReportTest Page��5      .��5      ]� -   5         ]�Name$ 
LayoutRoot��5      .+�5   
   7  1Telerik.Windows.Controls.RadDataServiceDataSource  -   5          Name$ 
datasource��5   ,   &   Telerik.Windows.Controls.RadMenu  5   
   $	��Top=�6   $	��Top=�62   .@�5      *  $Telerik.Windows.Controls.RadMenuItem  -   5          Name$ Export��5   "   $��导出��62   6>   5      �� 5   
   $	��Top=�6   $	��200��6,   .��5      �� $	��Top=�6    �� Orientation$ 
Horizontal=�6$   .+�5      �� $
��Wrap=�6   $��筛选工具��6#   $��0,0,10,0q�6/   $��Center=�6A   ,  &Telerik.Windows.Controls.RadDataFilter  5      -   5          Name$ 
dataFilter��5      $	��Top=�6b   $��
0,0,-440,0q�6z      Source 6/   �� *
DataView+ �� ElementName$ 
datasource��6z   5       5!      +  %Telerik.Windows.Controls.RadDataPager  5"   
   -   5          Name$ radDataPager��5"      	   DisplayMode$		 All=�5$      
   IsTotalItemCountFixed
 . $��Bottom=�5%   !   $��Bottom=�6b   $
��Auto��6z      Source 5#      �� *
DataView+$ 
datasource��5%   z   * 
 $Telerik.Windows.Controls.RadGridView  5&      -   5          Name$
 Grid��5&      ; 
 5Telerik.Windows.Controls.GridView.GridViewDataControl   
IsReadOnly .    RowIndicatorVisibility$ 	Collapsed=�5*         CanUserFreezeColumns .  $	��Top=�5+   !   *  $Telerik.Windows.Controls.DataControl   ItemsSource 5'      �� *
DataView+$ 
datasource��   IsBusy 5(      �� *IsBusy+$ 
datasource��5+   !   5-      5.      