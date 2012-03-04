using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

namespace ListBoxTest
{
    public class TestCollectionViewModel
    {
        #region Static

        #region Properties

        /// <summary>
        /// Gets or sets the seed for generating chars
        /// </summary>
        private static int Seed { get; set; }
        
        /// <summary>
        /// Gets a dictionary of characters to use
        /// </summary>
        private static Dictionary<int, char> Chars
        {
            get
            {
                if (_chars == null)
                {
                    _chars = new Dictionary<int, char>();
                    _chars.Add(0, 'a');
                    _chars.Add(1, 'b');
                    _chars.Add(2, 'c');
                    _chars.Add(3, 'd');
                    _chars.Add(4, 'e');
                    _chars.Add(5, 'f');
                    _chars.Add(6, 'g');
                    _chars.Add(7, 'h');
                    _chars.Add(8, 'i');
                    _chars.Add(9, 'j');
                    _chars.Add(10, 'k');
                    _chars.Add(11, 'l');
                    _chars.Add(12, 'm');
                    _chars.Add(13, 'n');
                    _chars.Add(14, 'o');
                    _chars.Add(15, 'p');
                    _chars.Add(16, 'q');
                    _chars.Add(17, 'r');
                    _chars.Add(18, 's');
                    _chars.Add(19, 't');
                    _chars.Add(20, 'u');
                    _chars.Add(21, 'v');
                    _chars.Add(22, 'w');
                    _chars.Add(23, 'x');
                    _chars.Add(24, 'y');
                    _chars.Add(25, 'z');
                    _chars.Add(26, ' ');
                }
                return _chars;
            }
        }
        static Dictionary<int, char> _chars;

        #endregion

        #region Methods

        /// <summary>
        /// Gets a new test collection view model with a number of items with a specified length
        /// </summary>
        /// <param name="numItems"></param>
        /// <param name="itemLength"></param>
        /// <returns></returns>
        public static TestCollectionViewModel Get(int numItems, int itemLength)
        {
            TestCollectionViewModel testVM = new TestCollectionViewModel();

            for (int i = 0; i < numItems; i++)
                testVM.Items.Add(new TestItemViewModel() { Name = GetNewItem(itemLength) });

            return testVM;
        }

        /// <summary>
        /// Gets a new item of the specified length
        /// </summary>
        /// <param name="itemLength"></param>
        /// <returns></returns>
        private static string GetNewItem(int itemLength)
        {
            StringBuilder item = new StringBuilder();
            for (int i = 0; i < itemLength; i++)
                item.Append(GetCharacter());

            return item.ToString();
        }

        /// <summary>
        /// Gets a single random character
        /// </summary>
        /// <returns></returns>
        private static char GetCharacter()
        {
            char c = Chars[new Random(Seed++).Next(26)];

            int upper = new Random().Next(1);
            if (upper > 0)
                return char.ToUpper(c);
            else
                return c;
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets a collection of test view model items
        /// </summary>
        public ObservableCollection<TestItemViewModel> Items
        {
            get
            {
                if (_items == null)
                    _items = new ObservableCollection<TestItemViewModel>();
                return _items;
            }
        }
        ObservableCollection<TestItemViewModel> _items;

        #endregion
    }
}
