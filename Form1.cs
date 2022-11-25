using System.Diagnostics.Metrics;
using System.Drawing.Text;

namespace NotWordle
{
    public partial class Form1 : Form
    {
        public Button[,] buttonsArray = new Button[6, 5];
        public List<String> wordList;
        public string theWord;
        public int rowPos = 0;
        public int colPos = 0;
        public Form1()
        {
            
            InitializeComponent();
            InitGame();
        }


        // Iniitalize game by generating random word from list and creating buttons
        public void InitGame()
        {
            theWord = GenerateWord("wordle-answers-alphabetical.txt");
            this.Text = "Legally Not Wordle";
            this.BackColor = Color.FromArgb(18, 18, 19);
            this.Size = new Size(385, 460);
            InitButtons();
        }

        // Create all buttons and add them to 2D array buttonsArray
        public void InitButtons()
        {
            
            int buttonX = 30;
            int buttonY = 30;

            for (int row = 0; row < buttonsArray.GetLength(0); row++)
            {
                for (int col = 0; col < buttonsArray.GetLength(1); col++)
                {

                    // Creating new button
                    Button newButton = new Button();
                    newButton.Size = new Size(50, 50);
                    newButton.Name = $"Button{row+col}";
                    newButton.Text = " ";
                    newButton.BackColor = Color.FromArgb(58, 58, 60);
                    newButton.ForeColor = Color.FromArgb(255, 255, 255);
                    newButton.FlatAppearance.BorderSize = 0;
                    newButton.Font = new Font("Helvetica", 15f, FontStyle.Bold);
                    newButton.FlatStyle = FlatStyle.Flat;
                    newButton.Location = new Point(buttonX, buttonY);

                    // Adding button to 2D array and form
                    buttonsArray[row, col] = newButton;
                    Controls.Add(newButton);

                    
                    buttonX += 65; // increase x position
                }
                buttonX = 30; // reset x position
                buttonY += 65; // increase y position
            }
        }

        // Generate random word from word text file
        public string GenerateWord(string fileLocation)
        {
            StreamReader sr = new StreamReader(fileLocation);
            wordList = new List<string>();

            // Read all words into file into list of strings
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                wordList.Add(line);
            }
            
            // Pick a random word in word list
            Random rnd = new Random();
            string randomWord = wordList[rnd.Next(0, wordList.Count)];
            return randomWord;
        }

        // User input detection
        private void Form1_Keyup(object sender, KeyEventArgs e)
        {
            string letter = e.KeyCode.ToString();

            // Check if input key was backspace
            if (letter == "Back")
            {
                if (colPos < 4)
                {
                    
                    if (colPos == 0)
                    {
                        buttonsArray[rowPos, colPos].Text = " ";
                    }
                    else
                    {
                        buttonsArray[rowPos, colPos - 1].Text = " ";
                        colPos--;
                    }
                    
                }
                else
                {
                    
                    if (buttonsArray[rowPos, 4].Text == " ")
                    {
                        buttonsArray[rowPos, colPos - 1].Text = " ";
                        colPos--;
                    }
                    else
                    {
                        buttonsArray[rowPos, colPos].Text = " ";
                    }
                    
                }
                
                
            } 
            // Check if input letter was enter key
            else if (letter == "Return") {
                // create string word from button letters
                string currentGuess = "";
                for (int colCount = 0; colCount < buttonsArray.GetLength(1); colCount++)
                {
                    currentGuess = currentGuess + buttonsArray[rowPos, colCount].Text;
                }
                CheckWord(currentGuess);
            } 
            // If input letter was not backspace or enter and the letter is one character
            else if (letter.Length == 1) {
                buttonsArray[rowPos, colPos].Text = letter;
                if (colPos < buttonsArray.GetLength(1) - 1)
                {
                    colPos++;
                    return;
                }
            }

        }

        // Method to check if word is correct or valid word to check letters
        public void CheckWord(string guessWord)
        {

            if (guessWord.Equals(theWord.ToUpper()))
            {
                for (int colNum = 0; colNum < buttonsArray.GetLength(1); colNum++)
                {
                    buttonsArray[rowPos, colNum].BackColor = Color.FromArgb(83, 141, 78);
                }
                MessageBox.Show("YOU WIN");
            }
            else
            {
                if (CheckIfInList(guessWord))
                {
                    if (rowPos < buttonsArray.GetLength(0)-1)
                    {
                        CheckLetters(guessWord);
                        rowPos++;
                        colPos = 0;
                    }
                    else
                    {
                        CheckLetters(guessWord);
                        MessageBox.Show($"The word was: {theWord.ToUpper()}");
                    }


                }
                
            }

            
        }

        // Method to check if input string is valid and in text file of valid words
        public bool CheckIfInList(string word)
        {
            for (int i = 0; i < wordList.Count; i++)
            {
                if (word.Equals(wordList[i].ToUpper())){
                    return true;
                }
            }
            return false;
        }

        // Check the letters of gussed word and color buttons accordingly
        public void CheckLetters(string guessWord)
        {
            // Color tile yellow
            for (int i = 0; i < guessWord.Length; i++)
            {
                for (int j = 0; j < guessWord.Length; j++)
                {
                    if (guessWord[i].ToString() == theWord[j].ToString().ToUpper())
                    {
                        buttonsArray[rowPos, i].BackColor = Color.FromArgb(181, 159, 59);
                    }
                }
            }

            // Color tile green
            for (int i = 0; i < guessWord.Length; i++)
            {
                if (guessWord[i].ToString() == theWord[i].ToString().ToUpper())
                {
                    buttonsArray[rowPos, i].BackColor = Color.FromArgb(83, 141, 78);
                }
            }

            
        }

        // Restart button
        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        // Exit button
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}