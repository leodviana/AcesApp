using System;
using System.Collections.Generic;
using AcesApp.Interfaces;
using Xamarin.Forms;

namespace AcesApp.Views
{
    public partial class ModificacoesPage : ContentPage,  ITabPageIcons
    {
        public ModificacoesPage()
        {
            InitializeComponent();
        }

        public string GetIcon()
        {
            return "mudancaverde";
        }

        public string GetSelectedIcon()
        {
            return "mudanca";
        }
    }
}
