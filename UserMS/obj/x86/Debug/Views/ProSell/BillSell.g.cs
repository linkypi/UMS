﻿#pragma checksum "..\..\..\..\..\Views\ProSell\BillSell.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F08B84761EE1C682F3FCD627FB7B5044"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18063
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Telerik.Charting;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Animation;
using Telerik.Windows.Controls.BulletGraph;
using Telerik.Windows.Controls.Carousel;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Controls.Charting;
using Telerik.Windows.Controls.Data.PropertyGrid;
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.Gauge;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.HeatMap;
using Telerik.Windows.Controls.Legend;
using Telerik.Windows.Controls.Map;
using Telerik.Windows.Controls.Primitives;
using Telerik.Windows.Controls.RibbonView;
using Telerik.Windows.Controls.Sparklines;
using Telerik.Windows.Controls.TimeBar;
using Telerik.Windows.Controls.Timeline;
using Telerik.Windows.Controls.TransitionEffects;
using Telerik.Windows.Controls.TreeListView;
using Telerik.Windows.Controls.TreeMap;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.Data;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;
using Telerik.Windows.Input.Touch;
using Telerik.Windows.Shapes;
using UserMS;
using UserMS.API;


namespace UserMS {
    
    
    /// <summary>
    /// BillSell
    /// </summary>
    public partial class BillSell : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal UserMS.BillSell Page;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadBusyIndicator busy;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadMenuItem Back;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadMenuItem New;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadMenuItem Del;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadMenuItem Save;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadMenuItem Next;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView dataGrid;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadButton HallName;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox IMEI;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadButton IMEISearch;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BillName;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.WrapPanel MainPanel;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox MobileIMEI;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadButton MobileIMEISearch;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox MobileName;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ModelClass;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel StackPanel1;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FieldLabel1;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BillField1;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel StackPanel2;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FieldLabel2;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BillField2;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel StackPanel3;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FieldLabel3;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BillField3;
        
        #line default
        #line hidden
        
        
        #line 107 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel StackPanel4;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FieldLabel4;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BillField4;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel StackPanel5;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FieldLabel5;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BillField5;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel StackPanel6;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FieldLabel6;
        
        #line default
        #line hidden
        
        
        #line 131 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BillField6;
        
        #line default
        #line hidden
        
        
        #line 134 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel StackPanel7;
        
        #line default
        #line hidden
        
        
        #line 138 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FieldLabel7;
        
        #line default
        #line hidden
        
        
        #line 140 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BillField7;
        
        #line default
        #line hidden
        
        
        #line 143 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel StackPanel8;
        
        #line default
        #line hidden
        
        
        #line 147 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FieldLabel8;
        
        #line default
        #line hidden
        
        
        #line 149 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BillField8;
        
        #line default
        #line hidden
        
        
        #line 152 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel StackPanel9;
        
        #line default
        #line hidden
        
        
        #line 156 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FieldLabel9;
        
        #line default
        #line hidden
        
        
        #line 158 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BillField9;
        
        #line default
        #line hidden
        
        
        #line 161 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel StackPanel10;
        
        #line default
        #line hidden
        
        
        #line 165 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label FieldLabel10;
        
        #line default
        #line hidden
        
        
        #line 167 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox BillField10;
        
        #line default
        #line hidden
        
        
        #line 175 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button _Add;
        
        #line default
        #line hidden
        
        
        #line 176 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button _Cancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/UserMS;component/views/prosell/billsell.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.Page = ((UserMS.BillSell)(target));
            return;
            case 2:
            this.busy = ((Telerik.Windows.Controls.RadBusyIndicator)(target));
            return;
            case 3:
            this.LayoutRoot = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 4:
            this.Back = ((Telerik.Windows.Controls.RadMenuItem)(target));
            
            #line 14 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
            this.Back.Click += new Telerik.Windows.RadRoutedEventHandler(this.Back_OnClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.New = ((Telerik.Windows.Controls.RadMenuItem)(target));
            
            #line 15 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
            this.New.Click += new Telerik.Windows.RadRoutedEventHandler(this.New_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Del = ((Telerik.Windows.Controls.RadMenuItem)(target));
            
            #line 16 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
            this.Del.Click += new Telerik.Windows.RadRoutedEventHandler(this.Del_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Save = ((Telerik.Windows.Controls.RadMenuItem)(target));
            
            #line 17 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
            this.Save.Click += new Telerik.Windows.RadRoutedEventHandler(this.Save_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Next = ((Telerik.Windows.Controls.RadMenuItem)(target));
            
            #line 18 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
            this.Next.Click += new Telerik.Windows.RadRoutedEventHandler(this.Next_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.dataGrid = ((Telerik.Windows.Controls.RadGridView)(target));
            return;
            case 10:
            this.HallName = ((Telerik.Windows.Controls.RadButton)(target));
            return;
            case 11:
            this.IMEI = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            this.IMEISearch = ((Telerik.Windows.Controls.RadButton)(target));
            
            #line 44 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
            this.IMEISearch.Click += new System.Windows.RoutedEventHandler(this.IMEISearch_OnClick);
            
            #line default
            #line hidden
            return;
            case 13:
            this.BillName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 14:
            this.MainPanel = ((System.Windows.Controls.WrapPanel)(target));
            return;
            case 15:
            this.MobileIMEI = ((System.Windows.Controls.TextBox)(target));
            return;
            case 16:
            this.MobileIMEISearch = ((Telerik.Windows.Controls.RadButton)(target));
            
            #line 64 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
            this.MobileIMEISearch.Click += new System.Windows.RoutedEventHandler(this.MobileIMEISearch_Onclick);
            
            #line default
            #line hidden
            return;
            case 17:
            this.MobileName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 18:
            this.ModelClass = ((System.Windows.Controls.TextBox)(target));
            return;
            case 19:
            this.StackPanel1 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 20:
            this.FieldLabel1 = ((System.Windows.Controls.Label)(target));
            return;
            case 21:
            this.BillField1 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 22:
            this.StackPanel2 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 23:
            this.FieldLabel2 = ((System.Windows.Controls.Label)(target));
            return;
            case 24:
            this.BillField2 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 25:
            this.StackPanel3 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 26:
            this.FieldLabel3 = ((System.Windows.Controls.Label)(target));
            return;
            case 27:
            this.BillField3 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 28:
            this.StackPanel4 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 29:
            this.FieldLabel4 = ((System.Windows.Controls.Label)(target));
            return;
            case 30:
            this.BillField4 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 31:
            this.StackPanel5 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 32:
            this.FieldLabel5 = ((System.Windows.Controls.Label)(target));
            return;
            case 33:
            this.BillField5 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 34:
            this.StackPanel6 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 35:
            this.FieldLabel6 = ((System.Windows.Controls.Label)(target));
            return;
            case 36:
            this.BillField6 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 37:
            this.StackPanel7 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 38:
            this.FieldLabel7 = ((System.Windows.Controls.Label)(target));
            return;
            case 39:
            this.BillField7 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 40:
            this.StackPanel8 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 41:
            this.FieldLabel8 = ((System.Windows.Controls.Label)(target));
            return;
            case 42:
            this.BillField8 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 43:
            this.StackPanel9 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 44:
            this.FieldLabel9 = ((System.Windows.Controls.Label)(target));
            return;
            case 45:
            this.BillField9 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 46:
            this.StackPanel10 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 47:
            this.FieldLabel10 = ((System.Windows.Controls.Label)(target));
            return;
            case 48:
            this.BillField10 = ((System.Windows.Controls.TextBox)(target));
            return;
            case 49:
            this._Add = ((System.Windows.Controls.Button)(target));
            
            #line 175 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
            this._Add.Click += new System.Windows.RoutedEventHandler(this._Add_OnClick);
            
            #line default
            #line hidden
            return;
            case 50:
            this._Cancel = ((System.Windows.Controls.Button)(target));
            
            #line 176 "..\..\..\..\..\Views\ProSell\BillSell.xaml"
            this._Cancel.Click += new System.Windows.RoutedEventHandler(this._Cancel_OnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

