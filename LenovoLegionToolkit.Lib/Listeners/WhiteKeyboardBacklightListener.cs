﻿using System;
using System.Management;
using System.Threading.Tasks;
using LenovoLegionToolkit.Lib.Extensions;
using LenovoLegionToolkit.Lib.Features;
using LenovoLegionToolkit.Lib.Utils;

namespace LenovoLegionToolkit.Lib.Listeners
{
    public class WhiteKeyboardBacklightListener : AbstractWMIListener<WhiteKeyboardBacklightChanged>
    {
        private readonly WhiteKeyboardBacklightFeature _feature;
        public WhiteKeyboardBacklightListener(WhiteKeyboardBacklightFeature feature)
            : base("ROOT\\WMI", "LENOVO_LIGHTING_EVENT")
        {
            _feature = feature ?? throw new ArgumentNullException(nameof(feature));
        }

        protected override WhiteKeyboardBacklightChanged GetValue(PropertyDataCollection properties) => default;

        protected override async Task OnChangedAsync(WhiteKeyboardBacklightChanged value)
        {
            try
            {
                var state = await _feature.GetStateAsync().ConfigureAwait(false);
                MessagingCenter.Publish(new Notification(NotificationType.WhiteKeyboardBacklight, NotificationDuration.Short, state.GetDisplayName()));
            }
            catch (Exception ex)
            {
                if (Log.Instance.IsTraceEnabled)
                    Log.Instance.Trace($"Failed to get white backlight state.", ex);
            }
        }
    }
}
