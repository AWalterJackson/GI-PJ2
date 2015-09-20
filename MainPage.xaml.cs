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

namespace PirateGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        private readonly LabGame game;

        public MainPage()
        {
            InitializeComponent();
            game = new LabGame(this);
            this.score.Visibility = Visibility.Collapsed;
            this.menu.Visibility = Visibility.Collapsed;
            this.menu.IsEnabled = false;
            this.tips.Visibility = Visibility.Collapsed;
            this.tips.Text = "It's space invaders... don't get murdered.";
        }

        public void UpdateScore(int score)
        {
            this.score.Text = score.ToString();
        }

        private void startButtonPressed(object sender, RoutedEventArgs e)
        {
            this.Start.Visibility = Visibility.Collapsed;
            this.score.Visibility = Visibility.Visible;
            this.Start.IsEnabled = false;
            this.Difficulty.Visibility = Visibility.Collapsed;
            this.Difficulty.IsEnabled = false;
            this.Instructions.Visibility = Visibility.Collapsed;
            this.Instructions.IsEnabled = false;
            game.Run(this);
            game.started = true;
        }

        private void Difficulty_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            game.difficulty = (int)this.Difficulty.Value + 1;
            game.difficulty = game.difficulty / 5;
            if (game.difficulty == 0)
            {
                game.difficulty = 1;
            }
        }

        private void showInstructions(object sender, RoutedEventArgs e)
        {
            this.Start.Visibility = Visibility.Collapsed;
            this.Start.IsEnabled = false;
            this.Difficulty.Visibility = Visibility.Collapsed;
            this.Difficulty.IsEnabled = false;
            this.menu.Visibility = Visibility.Visible;
            this.menu.IsEnabled = true;
            this.tips.Visibility = Visibility.Visible;
            this.Instructions.Visibility = Visibility.Collapsed;
            this.Instructions.IsEnabled = false;
        }

        private void mainMenu(object sender, RoutedEventArgs e)
        {
            this.menu.Visibility = Visibility.Collapsed;
            this.menu.IsEnabled = false;
            this.Start.Visibility = Visibility.Visible;
            this.Start.IsEnabled = true;
            this.Difficulty.Visibility = Visibility.Visible;
            this.Difficulty.IsEnabled = true;
            this.tips.Visibility = Visibility.Collapsed;
            this.Instructions.Visibility = Visibility.Visible;
            this.Instructions.IsEnabled = true;
        }
    }
}
