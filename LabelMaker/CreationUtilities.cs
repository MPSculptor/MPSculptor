﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;

using Microsoft.VisualBasic.FileIO;

namespace CreationUtilities

{

    //Handles all of the text manipulation routines

    public class TextOperations
    {
        public static string getHexColour(string sentColour)
        {

            //CONVERT AN INTEGER COLOUR INTO A HEX COLOUR
            //sent colour as RGB single number
            string gotHexColour = "";

            //split single colour number into 3 components
            int numberColour = (int.Parse(sentColour));
            int blue = (numberColour / 256 / 256);
            int greenred = numberColour - (blue * 256 * 256);
            int green = (greenred / 256);
            int red = (greenred - (green * 256));

            //Console.WriteLine("colours - " + red + " , " + green + " , " + blue);

            //convert integer to Hex value
            // value of 00 can convert to "0", so check and change
            string redString = red.ToString("x");
            if (redString.Equals("0"))
            {
                redString = "00";
            }

            string greenString = green.ToString("x");
            if (greenString.Equals("0"))
            {
                greenString = "00";
            }

            string blueString = blue.ToString("x");
            if (blueString.Equals("0"))
            {
                blueString = "00";
            }

            //add into single Hex value
            gotHexColour = "#" + redString + greenString + blueString;
            //Console.WriteLine(gotHexColour);

            return gotHexColour;
        }




        public static string[] SplitText(string toSplit, int pieces)
        {
            string decisionText;
            //string partialText = "";
            int finalCount = 0;
            int finalPieces = pieces;
            int splits;
            string[] splitString = new string[2];

            //remove leading and trailiing spaces and asign
            decisionText = toSplit.Trim();

            //test that there are enough gaps to split into
            int testSpaces = TextOperations.CountGaps(decisionText);
            if ((testSpaces + 1) < pieces)
            {
                finalPieces = testSpaces + 1;
            }
            splits = finalPieces;
            string[] returnText = new string[(finalPieces)];


            for (int count = 0; count < (finalPieces - 1); count++)
            {

                int textLength = decisionText.Length;
                int textPiece = (textLength / splits);
                int textPositionStart = textPiece;


                char testCharPlus;
                char testCharMinus;
                bool state = false;
                bool testPlus = false;
                bool testMinus = false;

                testSpaces = TextOperations.CountGaps(decisionText);
                if ((testSpaces + 1) == splits)
                {
                    textPositionStart = 1;
                    //break;
                }

                //set after test for gaps
                int textPositionPlus = textPositionStart;
                int textPositionMinus = textPositionStart;
                int splitPosition = textPositionStart;
                int splitPositionPlus = splitPosition;
                int splitPositionMinus = splitPosition;


                while (state == false)
                {
                    testCharPlus = decisionText[textPositionPlus];
                    testCharMinus = decisionText[textPositionMinus];

                    //test upstring of position
                    if (testCharPlus == ' ')
                    {
                        splitPositionPlus = textPositionPlus;
                        state = true;
                        testPlus = true;
                    }
                    //test downstring of position
                    if (testCharMinus == ' ')
                    {
                        splitPositionMinus = textPositionMinus;
                        state = true;
                        testMinus = true;
                    }
                    textPositionPlus++;
                    if (textPositionPlus > (decisionText.Length - 1))
                    {
                        textPositionPlus = decisionText.Length;
                    }
                    textPositionMinus--;
                    if (textPositionMinus < 0)
                    {
                        textPositionMinus = 0;
                    }

                }

                if (testPlus == true)
                {
                    splitPosition = splitPositionPlus;
                }
                if (testMinus == true)
                {
                    splitPosition = splitPositionMinus;
                }

                //split Text in two
                splitString = TextOperations.SplitTextAtPoint(decisionText, splitPosition);
                //set return text segment
                returnText[count] = splitString[0].Trim();

                //trim of segment ready to go again
                decisionText = splitString[1].Trim();

                //set index for final segment assignment
                finalCount = count + 1;

                //reduce segements as we go
                splits--;
            }

            //set last segment
            returnText[finalCount] = decisionText;


            return returnText;
        }

        public static int CountGaps(string gapText)
        {

            int gaps = 0;
            bool space = false;

            for (int count = 0; count < gapText.Length; count++)
            {

                if (gapText[count] == ' ')
                {
                    if (space == false)
                    {
                        gaps++;
                        space = true;
                    }
                }
                else
                {
                    space = false;
                }
            }

            return gaps;

        }

        public static string[] SplitTextAtPoint(string toSplit, int splitPosition)
        {

            string[] returnText = new string[2];

            returnText[0] = toSplit.Substring(0, splitPosition);
            returnText[1] = toSplit.Substring(splitPosition);

            return returnText;
        }

    }

    //  **THIS IS A TEMPORARY CLASS **
    // Its purpose is to collect data for one label from a couple of temporary text files
    // replace by database reader in time



    public class dataReader
    {
        public static string[] readQueue( string[] sentData, string[] sentName, string[] moreData)
        {
            string[] queueData = new string[36];

            queueData[0] = sentName[0]; //Full name
            queueData[1] = moreData[2]; //qty
            queueData[2] = moreData[3]; // price
            queueData[3] = sentData[9];
            queueData[4] = moreData[4]; // Customer Name
            queueData[5] = sentData[11];
            queueData[6] = sentData[8];
            queueData[7] = sentData[6];
            queueData[8] = moreData[0];
            queueData[9] = moreData[6]; // Font Name
            queueData[10] = moreData[7]; // Font Colour
            queueData[11] = moreData[8]; // Bold
            queueData[12] = moreData[9]; // Italic
            queueData[13] = moreData[10]; // Border Colour
            queueData[14] = moreData[11]; // Back colour
            queueData[15] = sentData[19];
            queueData[16] = sentName[1]; 
            queueData[17] = sentName[2];
            queueData[18] = sentName[3];
            queueData[19] = moreData[1]; // AGM picture to use
            queueData[20] = sentData[12];
            queueData[21] = sentData[13];
            queueData[22] = sentData[14];
            queueData[23] = sentData[15];
            queueData[24] = "Order No. #" + moreData[5]; //Order Number
            queueData[25] = "Shipping Name";
            queueData[26] = "First Name";
            queueData[27] = "Last Name";
            queueData[28] = "Address Line 1";
            queueData[29] = "Address Line 2";
            queueData[30] = "Address City";
            queueData[31] = "Address State";
            queueData[32] = "Postcode";
            queueData[33] = "Notes";



            return queueData;
        }

        public static string[] readFile(string fileName, char separator)
        {
            try
            {
                string path = fileName;
                // Open the file to read from.
                string readInString = File.ReadAllText(path);
                //LabelMaker.Form1.Controls.richTextBox1.Text = outputString;

                //find out divisions and set string array to right size
                int pipeCount = 0;
                for (int i = 0; i < readInString.Length; i++)
                {
                    //if (readInString[i] == '|')
                    if (readInString[i] == separator)
                    {
                        pipeCount++;
                    }
                }
                string[] outputString = new string[pipeCount];

                //Add text from between each pipe char.to subsequent string lines

                int counter = 0;
                string addString = "";

                for (int i = 0; i < readInString.Length; i++)
                {
                    if (readInString[i] != separator)
                    {
                        addString = addString + readInString[i];
                    }
                    else
                    {
                        outputString[counter] = addString;
                        addString = "";
                        counter++;
                        //System.out.println(counter + " - " + outputString[counter]);
                    }
                }

                return outputString;

            }
            catch (IOException)
            {
                 MessageBox.Show("Failed to read file - ");
                string[] outputString = new string[1];
                return outputString;
            }


        }

       

        public static string[] csvReaderHeader(string path)
        {
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                
                // Read the row with the column names
                string[] headers = csvParser.ReadFields();
                return headers;
            }
        }

    }

    /// @author Martin
    /// This Routine handles the creation of a label
    /// which is printed with Graphics onto a panel Container
    /// </summary>

    public class BackupService
    {
        private readonly string _connectionString;
        private readonly string _backupFolderFullPath;
        private readonly string[] _systemDatabaseNames = { "master", "tempdb", "model", "msdb" };

        public BackupService(string connectionString, string backupFolderFullPath)
        {
            _connectionString = connectionString;
            _backupFolderFullPath = backupFolderFullPath;
        }

        public void BackupAllUserDatabases()
        {
            foreach (string databaseName in GetAllUserDatabases())
            {
                BackupDatabase(databaseName);
            }
        }

        public void BackupDatabase(string databaseName)
        {
            string filePath = BuildBackupPathWithFilename(databaseName);

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = String.Format("BACKUP DATABASE [{0}] TO DISK='{1}'", databaseName, filePath);

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private IEnumerable<string> GetAllUserDatabases()
        {
            var databases = new List<String>();

            DataTable databasesTable;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                databasesTable = connection.GetSchema("Databases");

                connection.Close();
            }

            foreach (DataRow row in databasesTable.Rows)
            {
                string databaseName = row["database_name"].ToString();

                if (_systemDatabaseNames.Contains(databaseName))
                    continue;

                databases.Add(databaseName);
            }

            return databases;
        }

        private string BuildBackupPathWithFilename(string databaseName)
        {
            string filename = string.Format("{0}-{1}.bak", databaseName, DateTime.Now.ToString("yyyy-MM-dd"));

            return Path.Combine(_backupFolderFullPath, filename);
        }
    }
}
