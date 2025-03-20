using LegoPartsCatalogApp.Models;
using LegoPartsCatalogClassLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xaml;

namespace LegoPartsCatalogApp
{
    public class ColorQueryViewModel
    {
        private ColorQueryModel _model;

        public IReadOnlyCollection<ColorGroupQueryViewModel> ColorGroups
        { get; private set; }

        public ColorQueryViewModel(ColorQueryModel model)
        {
            _model = model;

            List<ColorGroupQueryViewModel> colorGroups =
                new List<ColorGroupQueryViewModel>();

            foreach (var colorGroup in model.ColorGroups)
                colorGroups.Add(new ColorGroupQueryViewModel(colorGroup));

            ColorGroups = colorGroups;

            return;
        }

        public void MakeQuery()
        {
            foreach (var colorGroup in ColorGroups)
                colorGroup.MakeQuery();
            _model.MakeQuery();

            return;
        }
    }

    public class ColorGroupQueryViewModel// : INotifyPropertyChanged
    {
        private ColorGroupQueryModel _model;

        public string GroupName => _model.GroupName;

        public IReadOnlyCollection<SingleColorQueryViewModel> GroupValues
        { get; private set; }

        public bool IsSelected { get; set; }

        public ColorGroupQueryViewModel(ColorGroupQueryModel model)
        {
            _model = model;

            List<SingleColorQueryViewModel> colors = 
                new List<SingleColorQueryViewModel>();

            foreach (var color in model.GetColors())
                colors.Add(new SingleColorQueryViewModel(color));

            GroupValues = colors;

            return;
        }

        public void MakeQuery()
        {
            if (IsSelected) 
                _model.MakeQuery();
            else 
                _model.MakeQuery(GroupValues
                    .Where(c => c.IsSelected)
                    .Select(c => c.Color)
                    );

            return;
        }

        /*
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        */
    }

    public class SingleColorQueryViewModel
    {
        public ColorInfo Color;

        public string Name => Color.Name;

        public string RGB => Color.RGB;

        public bool IsSelected { get; set; }

        public SingleColorQueryViewModel(ColorInfo color)
        {
            Color = color;
            IsSelected = false;

            return;
        }
    }
}
