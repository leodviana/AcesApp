using System;
using System.Collections.Generic;
using AcesApp.Interfaces;
using Xamarin.Forms;

namespace AcesApp.Views
{
    public partial class RankingPage : ContentPage, ITabPageIcons
    {
        public RankingPage()
        {
            InitializeComponent();
        }

        public string GetIcon()
        {
            return "ranking";
        }

        public string GetSelectedIcon()
        {
            return "rankingblack";
        }
    }
}
