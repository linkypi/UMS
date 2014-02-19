using System;
using System.Windows.Input;
using Telerik.Windows;


namespace UserMS
{
    public class NotifyMessage
    {
        private readonly string _skinName;
        private readonly string _headerText;
        private readonly string _bodyText;
        private readonly string _pageTitle;
        private readonly MouseButtonEventHandler _xButton_MouseLeftButtonDown;
        private readonly MouseEventHandler _xButton_MouseMove;
        private readonly MouseEventHandler _xButton_MouseLeave;
        private readonly RadRoutedEventHandler _tabCloseMenu_ItemClick;

        private readonly Action _clickAction;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skinName">弹出框链接页面地址</param>
        /// <param name="headerText"></param>
        /// <param name="bodyText">弹出框主题内容</param>
        /// <param name="pageTitle">菜单的名称</param>
        /// <param name="clickAction"></param>
        public NotifyMessage(string skinName, string headerText, string bodyText, string pageTitle, 
            MouseButtonEventHandler xButton_MouseLeftButtonDown,
            MouseEventHandler xButton_MouseMove,
            MouseEventHandler xButton_MouseLeave,
            RadRoutedEventHandler tabCloseMenu_ItemClick,
            Action clickAction)
        {
            _skinName       = skinName;
            _headerText     = headerText;
            _bodyText       = bodyText;
            _pageTitle      = pageTitle;
            _xButton_MouseLeftButtonDown = xButton_MouseLeftButtonDown;
            _xButton_MouseMove = xButton_MouseMove;
            _xButton_MouseLeave = xButton_MouseLeave;
            _tabCloseMenu_ItemClick = tabCloseMenu_ItemClick;
            _clickAction    = clickAction;
        }
        public MouseButtonEventHandler XButton_MouseLeftButtonDown
        {
            get { return _xButton_MouseLeftButtonDown; }
        }
        public MouseEventHandler XButton_MouseMove
        {
            get { return _xButton_MouseMove; }
        }
        public MouseEventHandler XButton_MouseLeave
        {
            get { return _xButton_MouseLeave; }
        }
        public RadRoutedEventHandler TabCloseMenu_ItemClick
        {
            get { return _tabCloseMenu_ItemClick; }
        }
        public string PageTitle
        {
            get { return _pageTitle; }
        }
        public string SkinName
        {
            get { return _skinName; }
        }
        
        public string HeaderText
        {
            get { return _headerText; }
        }

        public string BodyText
        {
            get { return _bodyText; }
        }

        public Action OnClick
        {
            get { return _clickAction; }
        }
    }
}
