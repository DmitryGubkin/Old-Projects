
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members

        /// <summary>
        /// Array of ceils
        /// </summary>
        private MarkType[] _mResults;

        /// <summary>
        /// if True - X, False is O
        /// </summary>
        private bool _mPlayer;

        private bool _mGameEnded;
      

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            
            InitializeComponent();
            NewGame();
        }

        #endregion

        private void NewGame()
        {
            
            _mResults = new MarkType[9];

            for (int i = 0; i < 9; i++)
            {
                _mResults[i] = MarkType.Free;
            }
            _mPlayer = true;

            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = string.Empty;
                button.Background =new SolidColorBrush(Color.FromRgb(20, 189, 172));
                button.Foreground = new SolidColorBrush(Color.FromRgb(84, 84, 84));
              
               
            });

            _mGameEnded = false;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_mGameEnded)
            {
                NewGame();
                return;
            }

            var button = (Button) sender;

            var collum = Grid.GetColumn(button);
            var row = Grid.GetRow(button);

            var index = collum + (row*3); //  index of kliked element

            if (_mResults[index] != MarkType.Free)
                return;

            _mResults[index] = _mPlayer ? MarkType.Cross : MarkType.Nought;
            button.Content = _mPlayer ? "X" : "O";


            if (!_mPlayer)
                button.Foreground = new SolidColorBrush(Color.FromRgb(242, 235, 211));

            _mPlayer ^= true;// inver player


            CheckWinner();

        }

        private void CheckWinner()
        {
            //Horizontal check
            for (int i = 0; i < 7; i += 3)
            {
                if (_mResults[i] != MarkType.Free &&
                    (_mResults[i] & _mResults[i + 1] & _mResults[i + 2]) == _mResults[i])
                {
                    SetWinColor(new []{i,(i+1), (i+2)});
                   // MessageBox.Show(_mResults[i].ToString() + " Wins");
                    _mGameEnded = true;
                }
            }


            //Vertical Check
            for (int i = 0; i < 3; i ++)
            {
                if (_mResults[i] != MarkType.Free &&
                    (_mResults[i] & _mResults[i + 3] & _mResults[i + 6]) == _mResults[i])
                {
                    SetWinColor(new[] { i, (i + 3), (i + 6) });
                   // MessageBox.Show(_mResults[i].ToString() + " Wins");
                    _mGameEnded = true;
                }
            }


            //Diagonal Check

            for (int i = 2; i > -1; i -= 2)
            {
                int mlt = 1;
                if (i == 0)
                    mlt = 2;

                if (_mResults[i] != MarkType.Free &&
                    (_mResults[i] & _mResults[i + 2*mlt] & _mResults[i + 4*mlt]) == _mResults[i])
                {
                    SetWinColor(new[] { i, (i + 2*mlt), (i + 4*mlt) });
                   // MessageBox.Show(_mResults[i].ToString() + " Wins");
                    _mGameEnded = true;

                }


            }

          
                if (!_mResults.Any(res => res == MarkType.Free) &!_mGameEnded)
                {
                    _mGameEnded = true;
                    Container.Children.Cast<Button>().ToList().ForEach(button =>
                    {
                        button.Background = new SolidColorBrush(Color.FromArgb(255, 252, 217, 168));

                    });
                }
                GC.Collect();
        }

        private void SetWinColor( int[] arr)
        {
            foreach (int t in arr)
            {
                Container.Children.Cast<Button>().ToList()[t].Background = new SolidColorBrush(Color.FromArgb(255, 136, 179, 197));
            }
        }
    }
}
