using System;
using System.Collections.Generic;
using AcesApp.Interfaces;
using Xamarin.Forms;

namespace AcesApp.Views
{
    public partial class AulasProfessorPage2 : ContentPage, ITabPageIcons
    {
        public AulasProfessorPage2()
        {
            InitializeComponent();
        }
        public string GetIcon()
        {
            return "calendar";
        }

        public string GetSelectedIcon()
        {
            return "calendarblack";
        }
    }
}
