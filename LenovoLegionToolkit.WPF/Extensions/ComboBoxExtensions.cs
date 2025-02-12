﻿using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Controls
{
    public static class ComboBoxExtensions
    {
        public static void SetItems<T>(this ComboBox comboBox, IEnumerable<T> items, T selectedItem, Func<T, string>? displayValueConverter)
        {
            var boxedItems = items.Select(v => new ComboBoxItem<T>(v, displayValueConverter));
            var selectedBoxedItem = boxedItems.FirstOrDefault(bv => EqualityComparer<T>.Default.Equals(bv.Value, selectedItem));

            comboBox.Items.Clear();
            comboBox.Items.AddRange(boxedItems);
            comboBox.SelectedValue = selectedBoxedItem;
        }
        public static void SelectItem<T>(this ComboBox comboBox, T item) where T : struct
        {
            var boxedItems = comboBox.Items.OfType<ComboBoxItem<T>>().Select(item => item.Value).ToArray();
            comboBox.SelectedIndex = Array.IndexOf(boxedItems, item);
        }

        public static void ClearItems(this ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.SelectedValue = null;
        }

        public static bool TryGetSelectedItem<T>(this ComboBox comboBox, out T? value)
        {
            if (comboBox.SelectedItem is ComboBoxItem<T> selectedBoxedItem)
            {
                value = selectedBoxedItem.Value;
                return true;
            }

            value = default;
            return false;
        }

        public static T? GetNewValue<T>(this SelectionChangedEventArgs args) where T : struct
        {
            var items = args.AddedItems;
            if (items.Count < 1 || items[0] is not ComboBoxItem<T> item)
                return null;
            return item.Value;
        }

        public static T? GetOldValue<T>(this SelectionChangedEventArgs args) where T : struct
        {
            var items = args.RemovedItems;
            if (items.Count < 1 || items[0] is not ComboBoxItem<T> item)
                return null;
            return item.Value;
        }

        public static bool TryGetOldValue<T>(this SelectionChangedEventArgs args, out T? value)
        {
            var newValue = args.RemovedItems.Cast<ComboBoxItem<T>>().FirstOrDefault();
            if (newValue is null)
            {
                value = default;
                return false;
            }

            value = newValue.Value;
            return true;
        }

        private class ComboBoxItem<T>
        {
            public static bool operator ==(ComboBoxItem<T> left, ComboBoxItem<T> right) => left.Equals(right);

            public static bool operator !=(ComboBoxItem<T> left, ComboBoxItem<T> right) => !(left == right);

            public T Value { get; }
            private readonly Func<T, string>? _displayString;

            public ComboBoxItem(T value, Func<T, string>? displayString)
            {
                Value = value;
                _displayString = displayString;
            }

            public override bool Equals(object? obj) => obj is ComboBoxItem<T> item && EqualityComparer<T>.Default.Equals(Value, item.Value);

            public override int GetHashCode() => HashCode.Combine(Value);

            public override string ToString() => _displayString?.Invoke(Value) ?? Value?.ToString() ?? "";
        }
    }
}
