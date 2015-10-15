// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using SharpDX;

namespace Project
{
    /// <summary>
    /// A page with options that can be edited to edit game variables.
    /// </summary>
    // Options Page
    public sealed partial class Options
    {
        private MainPage parent;
        public float playerSpeed = 0.5f;
        public float playerAcceleration = 1.4f;
        public bool powerups = true;
        public float difficulty = 1;

        public Options(MainPage parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private void changeSpeed(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            playerSpeed = (float)e.NewValue;
        }

        private void changeAcceleration(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            playerAcceleration = (float)e.NewValue;
        }

        private void changePowerups(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if ((int)e.NewValue == 1)
            {
                powerups = true;
            } else
            {
                powerups = false;
            }
        }

        private void changeDifficulty(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            difficulty = (float)e.NewValue;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            parent.options = this;
            parent.Children.Add(parent.mainMenu);
            parent.Children.Remove(this);
        }
    }
}
