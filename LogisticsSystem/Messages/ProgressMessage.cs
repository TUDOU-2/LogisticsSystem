using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Messages
{
    public class ProgressMessage
    {
        public bool IsVisible { get; }

        public ProgressMessage(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}
