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
using System.IO;

namespace Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    // TASK 4: Instructions Page
    public sealed partial class Highscores
    {
        private MainPage parent;
        public Highscores(MainPage parent)
        {
            InitializeComponent();
            this.parent = parent;

            "Read highscores list, sort them and edit the txtHighscores attribute to display the highscores";

            string line;
            System.IO.StringReader sr = new System.IO.StringReader("highscore.txt");
            while ((line = sr.ReadLine()) != null)
            {
                System.Diagnostics.Debug.WriteLine(line);
            }

            var _Folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            _Folder = await _Folder.GetFolderAsync("MyFolder");

            File.Exists(path)

                //WHY DOES FILE IO IN WINDOWS STORE APPS HAVE TO BE SO OVERCOMPLICATED?? I can't be bothered with this.

            // acquire file
            var _File = await _Folder.GetFileAsync("MyFile.txt");
            Assert.IsNotNull(_File, "Acquire File");

            // write content
            var _WriteThis = "Hello World";
            await Windows.Storage.FileIO.WriteTextAsync(_File, _WriteThis);

            // read content
            var _ReadThis = await Windows.Storage.FileIO.ReadTextAsync(_File);
            Assert.AreEqual(_WriteThis, _ReadThis, "Contents correct");
        }

        public async void ReadFile()
        {
            // settings
            var path = @"MyFolder\MyFile.txt";
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;


            // acquire file
            var file = await folder.GetFileAsync(path);
            var readFile = await Windows.Storage.FileIO.ReadLinesAsync(file);
            foreach (var line in readFile)
            {
                System.Diagnostics.Debug.WriteLine("" + line.Split(';')[0]);
            }
        }


        private void GoBack(object sender, RoutedEventArgs e)
        {
            parent.Children.Add(parent.mainMenu);
            parent.Children.Remove(this);
        }
    }
}
