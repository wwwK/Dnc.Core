﻿using DDnc.WPF.Ui;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Dnc.WPF.Ui
{
    [ValueConversion(typeof(decimal), typeof(string))]
    public class CNYConverter
        : AbstractValueConverter<CNYConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cny = (decimal)value;

            return $"￥{cny.ToString("#.00")}";
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0m;

            if (decimal.TryParse(value.ToString(), out var cny))
                return cny;

            return 0m;
        }
    }
}
