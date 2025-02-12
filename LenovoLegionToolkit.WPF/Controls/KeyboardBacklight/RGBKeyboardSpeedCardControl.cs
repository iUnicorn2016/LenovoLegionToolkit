﻿using LenovoLegionToolkit.Lib;
using LenovoLegionToolkit.WPF.Resources;
using Wpf.Ui.Common;

namespace LenovoLegionToolkit.WPF.Controls.KeyboardBacklight
{
    public class RGBKeyboardSpeedCardControl : AbstractComboBoxKeyboardBacklightCardControl<RBGKeyboardSpeed>
    {
        public RGBKeyboardSpeedCardControl()
        {
            Icon = SymbolRegular.Keyboard24;
            Title = Resource.RGBKeyboardSpeedCardControl_Title;
        }
    }
}
