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
using System.Windows.Shapes;

namespace WPF_project_Cafe
{
    /// <summary>
    /// Sub_cafe.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Sub_cafe : Window
    {
        MainWindow mw = new MainWindow();
        Variable variable = new Variable();
        DBSqlite DB = new DBSqlite();
        public Sub_cafe(Variable variable)
        {
            this.variable = variable;
            InitializeComponent();
            Load();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            variable.btClick = 0;
            this.Close();
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            variable.btClick = 1;
            this.Close();
        }
        public void Load()
        {
            sub_cafe_border.Background = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + @"\Image_sub_cafe\background_img3.jpg")));
            menu_img.Background = GlobalVar.btn_select_img;
            menu_sticker_img.Background = GlobalVar.btn_select_sticker_img;
        }
    }
}