using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace Umini
{
    class CustomControl
    {
    }

    public class MaterialButton : Button
    {
        public static DependencyProperty KindProperty;

        static MaterialButton()
        {
            KindProperty = DependencyProperty.Register("Kind", typeof(PackIconKind), typeof(MaterialButton));
        }

        public PackIconKind Kind
        {
            get { return (PackIconKind)base.GetValue(KindProperty); }
            set { base.SetValue(KindProperty, value); }
        }
    }

}
