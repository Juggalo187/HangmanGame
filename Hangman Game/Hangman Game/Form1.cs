using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;// added this library to access the internet

namespace Hangman_Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        HashSet<char> guessedLetters = new HashSet<char>();
        private static bool formInitialized = false;
        string word = "";
        int amount = 0;
        List<Label> labels = new List<Label>();// a new list for all the label(underscores)
        enum BodyParts // enumeration is used to store all the body parts
        {
            Head,
            Left_Eye,
            Right_Eye,
            Mouth,
            Right_Arm,
            Left_Arm,
            Body,
            Right_Leg,
            Left_Leg,
        }

        void DrawHangPost()
        {
            Graphics g = panel1.CreateGraphics();// for drawing on panel
            Pen p = new Pen(Color.Brown, 10);// use for creating the pn 
            g.DrawLine(p, new Point(130, 248), new Point(130, 10));// only vertical line,second # ends at the top of the panel
            g.DrawLine(p, new Point(135, 5), new Point(65, 5));// only horizontal line
            g.DrawLine(p, new Point(60, 0), new Point(60, 50));//vertical line
            /**
             * The next eight lines are for diffrent body parts that call the function "DrawBodypart()" to draw those bodyparts every time the user
             * enters a wrong entry. 
             */
            DrawBodyPart(BodyParts.Head);
            DrawBodyPart(BodyParts.Left_Eye);
            DrawBodyPart(BodyParts.Right_Eye);
            DrawBodyPart(BodyParts.Mouth);
            DrawBodyPart(BodyParts.Body);
            DrawBodyPart(BodyParts.Left_Arm);
            DrawBodyPart(BodyParts.Right_Arm);
            DrawBodyPart(BodyParts.Left_Leg);
            DrawBodyPart(BodyParts.Right_Leg);
            MessageBox.Show(GetRandomWord());//message box shows up with the random word
        }
        void DrawBodyPart(BodyParts bp)
        {
            Graphics g = panel1.CreateGraphics();// this allows us to access the graphics to draw the different body parts
            Pen p = new Pen(Color.Blue, 2);
            if (bp == BodyParts.Head)//draw head
            {
                g.DrawEllipse(p, 40, 50, 40, 40);
            }
            else if (bp == BodyParts.Left_Eye)//draw left eye
            {
                SolidBrush s = new SolidBrush(Color.Black);
                g.FillEllipse(s, 50, 60, 5, 5);

            }
            else if (bp == BodyParts.Right_Eye)// draw right eye
            {
                SolidBrush s = new SolidBrush(Color.Black);
                g.FillEllipse(s, 63, 60, 5, 5);
            }
            else if (bp == BodyParts.Mouth)// draw mouth
            {
                g.DrawArc(p, 50, 60, 20, 20, 45, 90);
            }
            else if (bp == BodyParts.Body)// draw body
            {
                g.DrawLine(p, new Point(60, 90), new Point(60, 170));
            }
            else if (bp == BodyParts.Left_Arm)//draw left arm
            {
                g.DrawLine(p, new Point(60, 100), new Point(30, 85));
            }
            else if (bp == BodyParts.Right_Arm)// draw right arm
            {
                g.DrawLine(p, new Point(60, 100), new Point(90, 85));/// the first point is always where you start from on the body
            }
            else if (bp == BodyParts.Left_Leg)// darw left leg
            {
                g.DrawLine(p, new Point(60, 170), new Point(30, 190));
            }
            else if (bp == BodyParts.Right_Leg)// draw right leg
            {
                g.DrawLine(p, new Point(60, 170), new Point(90, 190));
            }
        }

        void MakeLabels()
        {
          word = GetRandomWord();
          char[] chars = word.ToCharArray();
          int between = 330 / chars.Length - 1;//use minus 1 because last chracter is '\n' so we want to skip over that + between is spaces between underscores
          for (int i = 0; i < chars.Length; i++)// for loop used to do all the following features to add for every word so user can play over and over again
            {
                labels.Add(new Label());// for each character want to create a label to display it
                labels[i].Location = new Point((i * between) + 10, 80);// 80 to stay on the same y axis
                labels[i].Text = "_";// so underscores appear
                labels[i].Parent = groupBox2;// gets information from the parent
                labels[i].BringToFront();// so that the word appears ebven though there are words under it
                labels[i].CreateControl();// Button is created
            }
            label1.Text = "Word Length" +(chars.Length - 1).ToString() ;// this shows the label "Word Length" to show how many letters are in the word
        }
        string GetRandomWord()// this function gets all the random words from that url 
        {
            WebClient wc = new WebClient();
            string wordlist = wc.DownloadString("https://raw.githubusercontent.com/Juggalo187/HangmanGame/master/Hangman%20Game/Hangman%20Game/pocket.txt");// we get all words from this url
            string[] words = wordlist.Split('\n');
            Random ran = new Random();
            return words[ran.Next(0,words.Length-1)];
        }
        private void Form1_Shown(object sender, EventArgs e)
        {

            if (formInitialized)
                return;

            formInitialized = true;


            // Your "shown" logic here — like starting the game, picking the word, etc.

            /*
             * The next 5 lines are code for drawing the lamppost as soon as the program is run 
             */
            Graphics g = panel1.CreateGraphics();// for drawing on panel
            Pen p = new Pen(Color.Brown, 10);// for pen
            g.DrawLine(p, new Point(130, 248), new Point(130, 10));//draws vertical post
            g.DrawLine(p, new Point(135, 5), new Point(65, 5));// draws the horizontal post from the vertical post to where the hook is for the hangman
            g.DrawLine(p, new Point(60, 0), new Point(60, 50));//vertical hook
            MakeLabels();
            Console.ForegroundColor = ConsoleColor.Green;
            /* The next line of code wll display the instructions of the hangman game*/
            MessageBox.Show("Welcome to Hangman — with a twist! 🎉\n\nEnter a letter in the textbox and click 'Submit Letter' or press Enter to guess. Keep an eye on the 'Missed' label to track incorrect guesses. You only get 8 wrong guesses, so choose wisely!\n\nReady to play? Let’s go!");
        }


      
        private void button1_Click(object sender, EventArgs e)
        {
            Textboxenter();
        }

        public void Textboxenter()
        {

            if (textBox1.Text.Length >= 1)
            {
                char letter = textBox1.Text.ToLower().ToCharArray()[0];//what they enter will be converted to char
                if (!char.IsLetter(letter))// if the letter they entered is not an alphabet then an error message will appear
                {
                    MessageBox.Show("You can only submit letters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (guessedLetters.Contains(letter))
                {
                    MessageBox.Show($"You've already guessed the letter '{letter}'!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                guessedLetters.Add(letter); // Mark as guessed

                if (word.Contains(letter))// if letter entered matches the letter in the actual word then it will appear in one of the underscores
                {
                    char[] letters = word.ToCharArray();
                    for (int i = 0; i < letters.Length; i++)
                    {
                        if (letters[i] == letter)
                        {
                            labels[i].Text = letter.ToString();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("The letter you guessed is incorrect!");
                    /*
                     * This else condition is used if  letter entered does not match the letter in the actual word then the incorrect letter will appear
                     * in the missed label so that they know not to enter that letter again and if they enter 8 incorrect letters then the program will
                     * be exited from and 
                     */
                    label2.Text += " " + letter.ToString() + ",";
                    DrawBodyPart((BodyParts)amount);
                    amount++;
                    if (amount == 8)
                    {
                        MessageBox.Show($"You lost! The word was: {word}");
                        // Restart by refreshing the current form instead of closing it
                        this.Hide();
                        Form1 newForm = new Form1();
                        newForm.ShowDialog(); // Use ShowDialog to keep the app alive until this form finishes
                        this.Close();         // Now safely close the old form
                    }
                }
            }
            textBox1.Text = "";
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        /* this function was created so that enter can be pressed after filling in a word so we don't have to hit the "Submit Letter" every time*/ 
        {
            if (e.KeyCode==Keys.Enter)
            {
                Textboxenter();
            }
        }

    }
}

