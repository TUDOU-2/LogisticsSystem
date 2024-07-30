using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LogisticsSystem.Behaviors
{
    public class CloseExpanderBehavior : Behavior<StackPanel>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
        }

        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;
            if (source != null)
            {
                ClearRadioButtonSelections();
            }
        }

        // 清除所有RadioButton的选中状态
        private void ClearRadioButtonSelections()
        {
            foreach (var radioButton in FindAllRadioButtons(AssociatedObject))
            {
                radioButton.IsChecked = false;
            }
        }

        // 递归查找所有的RadioButton
        private IEnumerable<RadioButton> FindAllRadioButtons(DependencyObject obj)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is RadioButton radioButton)
                {
                    yield return radioButton;
                }
                else if (child is FrameworkElement fe)
                {
                    foreach (var rb in FindAllRadioButtons(child))
                    {
                        yield return rb;
                    }
                }
            }
        }
    }
}
