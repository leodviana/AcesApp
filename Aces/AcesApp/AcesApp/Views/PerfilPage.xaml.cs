﻿using AcesApp.Interfaces;
using Xamarin.Forms;

namespace AcesApp.Views
{
    public partial class PerfilPage : ContentPage, ITabPageIcons
    {
        public PerfilPage()
        {
            InitializeComponent();
        }

        public string GetIcon()
        {
            return "Email36";
        }

        public string GetSelectedIcon()
        {
            return "Email36";
        }
    }
}
